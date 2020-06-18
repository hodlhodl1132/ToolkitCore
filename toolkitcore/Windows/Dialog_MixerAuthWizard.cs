using System.Net;
using System.Threading.Tasks;
using ToolkitCore.Models.Mixer.ShortcodeOAuth;
using ToolkitCore.Utilities;
using UnityEngine;
using Verse;

namespace ToolkitCore.Windows
{
    public class Dialog_MixerAuthWizard : Window
    {
        private string _currentCode = string.Empty;
        private Pages _currentPage = Pages.ShortCode;
        private FlowSteps _currentStep = FlowSteps.Begin;
        private int _globalTimer = -1;

        public Dialog_MixerAuthWizard()
        {
            forcePause = true;
            closeOnClickedOutside = false;
            closeOnCancel = false;
            closeOnAccept = false;
        }

        public override void DoWindowContents(Rect inRect)
        {
            GameFont original = Text.Font;
            Text.Font = GameFont.Small;
            Rect titleRect = new Rect(0f, 0f, inRect.width, Text.LineHeight * 2);
            Rect buttonRect = new Rect(0f, inRect.height - Text.LineHeight, inRect.width, Text.LineHeight);
            Rect contentRect = new Rect(
                0f,
                titleRect.x + titleRect.height + 5f,
                inRect.width,
                inRect.height - titleRect.height - buttonRect.height - 10f
            );
            Rect buttonInnerRect = new Rect(0f, 0f, buttonRect.width, buttonRect.height);
            Rect contentInnerRect = new Rect(0f, 0f, contentRect.width, contentRect.height);

            GUI.BeginGroup(titleRect);
            DrawTitleFor(titleRect, _currentPage);
            GUI.EndGroup();

            GUI.BeginGroup(buttonRect);
            DrawCurrentButtons(buttonInnerRect);
            GUI.EndGroup();

            GUI.BeginGroup(contentRect);
            DrawCurrentPage(contentInnerRect);
            GUI.EndGroup();
            Text.Font = original;
        }

        private static void DrawTitleFor(Rect inRect, Pages page)
        {
            string title;

            switch (page)
            {
                case Pages.ShortCode:
                    title = "Code Flow";
                    break;
                case Pages.AuthCode:
                    title = "Verifying Code Status";
                    break;
                case Pages.Denied:
                    title = "Access Denied";
                    break;
                case Pages.TimedOut:
                    title = "Code Expired";
                    break;
                default:
                    title = "Unknown Page";
                    break;
            }

            SettingsHelper.DrawBigLabelAnchored(inRect, title, TextAnchor.MiddleCenter);
        }

        private void DrawCurrentButtons(Rect inRect)
        {
            Rect btnRect = new Rect(
                inRect.width,
                inRect.y,
                Text.CalcSize($@"Copy ""{_currentCode}""").x + 16f,
                Text.LineHeight
            );
            btnRect = btnRect.ShiftLeft(0f);

            switch (_currentPage)
            {
                case Pages.TimedOut:
                case Pages.Denied:
                case Pages.AuthCode:
                    if (Widgets.ButtonText(btnRect, "Done"))
                    {
                        Close();
                    }

                    if (_currentStep != FlowSteps.Done && _currentPage != Pages.AuthCode)
                    {
                        return;
                    }

                    bool isConnected = MixerWrapper.Connected();

                    btnRect = btnRect.ShiftLeft();
                    if (!isConnected && Widgets.ButtonText(btnRect, "Connect"))
                    {
                        MixerWrapper.InitializeClient();
                    }
                    else if (isConnected)
                    {
                        SettingsHelper.DrawLabelAnchored(
                            btnRect,
                            TCText.ColoredText("Connected", Color.green),
                            TextAnchor.MiddleRight
                        );
                    }

                    break;
                case Pages.ShortCode:
                    if (Widgets.ButtonText(btnRect, $@"Copy ""{_currentCode}"""))
                    {
                        GUIUtility.systemCopyBuffer = _currentCode;
                    }

                    if (Widgets.ButtonText(btnRect.ShiftLeft(), "Open Browser"))
                    {
                        Application.OpenURL($"https://mixer.com/go?code={_currentCode}");
                    }

                    break;
            }
        }

        private async Task GetShortCode()
        {
            _currentStep = FlowSteps.GeneratingCode;
            bool task = await ShortcodeUtilities.GetShortcode();

            if (!task)
            {
                _currentStep = FlowSteps.Begin;
                Log.Error("Failed to retrieve short code!");
                return;
            }

            _currentStep = FlowSteps.CodeGenerated;
            _currentCode = ShortcodeUtilities.OAuthShortcodeResponse.code;
            _currentStep = FlowSteps.HasCode;
        }

        private async Task CheckShortcodeLoop()
        {
            while (_globalTimer > 0)
            {
                int delay = Mathf.Min(13, _globalTimer);
                await Task.Delay(delay * 1000);
                _globalTimer -= delay;

                try
                {
                    bool success = await ShortcodeUtilities.CheckShortcode();

                    if (success)
                    {
                        _currentStep = FlowSteps.AccessGranted;
                        _currentPage = Pages.AuthCode;
                        _globalTimer = 0;
                        await GetOAuthToken();
                    }
                }
                catch (WebException e)
                {
                    if (!(e.Response is HttpWebResponse response))
                    {
                        Log.Error("Response received wasn't a http response!");
                        return;
                    }

                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.NoContent:
                        case HttpStatusCode.Forbidden:
                            _currentPage = Pages.Denied;
                            _currentStep = FlowSteps.AccessDenied;
                            _globalTimer = -1;
                            break;
                        case HttpStatusCode.NotFound:
                            _currentPage = Pages.TimedOut;
                            _currentStep = FlowSteps.CodeExpired;
                            _globalTimer = -1;
                            break;
                        default:
                            _currentPage = Pages.Denied;
                            _currentStep = FlowSteps.AccessDenied;
                            _globalTimer = -1;
                            break;
                    }
                }
            }
        }

        private async Task GetOAuthToken()
        {
            _currentStep = FlowSteps.CodeExchange;
            bool task = await ShortcodeUtilities.GetOAuthToken();

            if (!task)
            {
                Log.Error("Couldn't get oauth token! Was the auth code invalid?");
                return;
            }

            ToolkitCoreSettings.mixerAccessToken = ShortcodeUtilities.OAuthTokenResponse.access_token;
            ToolkitCoreSettings.mixerRefreshToken = ShortcodeUtilities.OAuthTokenResponse.refresh_token;
            _currentStep = FlowSteps.Done;
        }

        public override void PreOpen()
        {
            base.PreOpen();

            if (_currentCode.NullOrEmpty())
            {
                Task shortCode = GetShortCode();
                Task postShortcode = shortCode.ContinueWith(
                    t =>
                    {
                        if (!t.IsCompletedSuccessfully)
                        {
                            return;
                        }

                        _currentPage = Pages.ShortCode;
                        _currentStep = FlowSteps.WaitingOnUser;
                        _globalTimer = ShortcodeUtilities.OAuthShortcodeResponse.expires_in;
                    }
                );

                postShortcode.ContinueWith(async t => await CheckShortcodeLoop());
            }
            else
            {
                _currentPage = Pages.ShortCode;
                _currentStep = FlowSteps.WaitingOnUser;
            }
        }

        private void DrawCurrentPage(Rect inRect)
        {
            switch (_currentPage)
            {
                case Pages.ShortCode:
                    Widgets.Label(
                        inRect.TopHalf(),
                        @"Welcome to the Mixer auth wizard! To connect ToolkitCore to Mixer, you'll first have to click the ""Open Browser"" button. Once the web page loads, you'll then have to click ""Approve"". <b>This window will update once you're done.</b>"
                    );
                    Widgets.Label(
                        inRect.BottomHalf(),
                        $@"If button does not work for you, you can go to <b>https://mixer.com/go</b>, then enter the code <b>{_currentCode}</b>."
                    );
                    return;
                case Pages.AuthCode:
                    switch (_currentStep)
                    {
                        case FlowSteps.AccessGranted:
                        case FlowSteps.CodeExchange:
                            Widgets.Label(inRect, "Getting token...");
                            break;
                        case FlowSteps.Done:
                            Widgets.Label(inRect, "Access granted! ToolkitCore can now connect to Mixer.");
                            break;
                    }

                    return;
                case Pages.TimedOut:
                    Widgets.Label(
                        inRect,
                        "The code has expired. You'll have to restart this process in order to connect to Mixer."
                    );
                    break;
                case Pages.Denied:
                    Widgets.Label(
                        inRect,
                        "ToolkitCore was denied access to your Mixer account. ToolkitCore won't be able to connect to Mixer without approval."
                    );
                    break;
                default:
                    return;
            }
        }

        private enum Pages
        {
            ShortCode, AuthCode, Denied,
            TimedOut
        }

        private enum FlowSteps
        {
            Begin, GeneratingCode, CodeGenerated,
            HasCode, WaitingOnUser, AccessGranted,
            CodeExchange, Done, AccessDenied,
            CodeExpired
        }
    }
}
