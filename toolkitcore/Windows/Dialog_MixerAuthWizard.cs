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
        private Pages _currentPage = Pages.Explanation;
        private FlowSteps _currentStep = FlowSteps.Begin;
        private int _currentTimer = -1;
        private const int PollInterval = 8;  // Seconds

        public Dialog_MixerAuthWizard()
        {
            forcePause = true;
            closeOnClickedOutside = false;
            closeOnCancel = false;
            closeOnAccept = false;
        }

        public override void DoWindowContents(Rect inRect)
        {
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
        }

        private static void DrawTitleFor(Rect inRect, Pages page)
        {
            string title;

            switch (page)
            {
                case Pages.Explanation:
                    title = "Mixer Token Wizard";
                    break;
                case Pages.ShortCode:
                    title = "Code Flow";
                    break;
                case Pages.AuthCode:
                    title = "Verifying Code Status";
                    break;
                default:
                    title = "Unknown Page";
                    break;
            }

            SettingsHelper.DrawBigLabelAnchored(inRect, title, TextAnchor.MiddleCenter);
        }

        private void DrawCurrentButtons(Rect inRect)
        {
            Rect btnTemplateRect = new Rect(
                inRect.width,
                inRect.y,
                Text.CalcSize($@"Copy ""{_currentCode}""").x + 16f,
                Text.LineHeight
            );
            btnTemplateRect.x -= btnTemplateRect.width;

            if (Widgets.ButtonText(btnTemplateRect, _currentPage == Pages.AuthCode ? "Done" : "Cancel"))
            {
                Close();
            }

            if (_currentPage == Pages.AuthCode)
            {
                if (_currentStep == FlowSteps.Done)
                {
                    bool isConnected = MixerWrapper.Connected();
                    
                    btnTemplateRect.x -= btnTemplateRect.width + 5f;
                    if (!isConnected && Widgets.ButtonText(btnTemplateRect, "Connect"))
                    {
                        MixerWrapper.InitializeClient();
                    }
                    else if (isConnected)
                    {
                        SettingsHelper.DrawLabelAnchored(btnTemplateRect, TCText.ColoredText("Connected", Color.green), TextAnchor.MiddleRight);
                    }
                }
                
                if (_currentStep == FlowSteps.Done || _currentStep == FlowSteps.CodeExchange)
                {
                    return;
                }

                btnTemplateRect.x -= btnTemplateRect.width + 5f;
                if (Widgets.ButtonText(btnTemplateRect, $@"Copy ""{_currentCode}"""))
                {
                    GUIUtility.systemCopyBuffer = _currentCode;
                }

                return;
            }

            if (Widgets.ButtonText(btnTemplateRect, "Cancel"))
            {
                Close();
            }

            btnTemplateRect.x -= btnTemplateRect.width + 5f;
            if (Widgets.ButtonText(btnTemplateRect, "Next"))
            {
                GoToNextPage();
            }

            if (_currentCode.NullOrEmpty())
            {
                return;
            }

            btnTemplateRect.x -= btnTemplateRect.width + 5f;
            if (Widgets.ButtonText(btnTemplateRect, $@"Copy ""{_currentCode}"""))
            {
                GUIUtility.systemCopyBuffer = _currentCode;
            }
        }

        private void GoToNextPage()
        {
            switch (_currentPage)
            {
                case Pages.Explanation:
                    if (_currentCode.NullOrEmpty())
                    {
                        Task _ = GetShortCode();
                    }

                    _currentPage = Pages.ShortCode;
                    _currentStep = FlowSteps.WaitingOnUser;
                    break;
                case Pages.ShortCode:
                    if (!_currentCode.NullOrEmpty())
                    {
                        Task _ = CheckShortcode();
                    }

                    _currentPage = Pages.AuthCode;
                    break;
                case Pages.AuthCode:
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

            Application.OpenURL($"https://mixer.com/go?code={_currentCode}");
        }

        private async Task CheckShortcode()
        {
            _currentStep = FlowSteps.HasCode;
            bool task = await ShortcodeUtilities.CheckShortcode();

            if (!task)
            {
                Log.Error("Couldn't obtain shortcode! Was it submitted?");

                _currentTimer = PollInterval;

                while (_currentTimer > 0)
                {
                    await Task.Delay(1000);
                    _currentTimer -= 1;
                }
                
                Task _ = CheckShortcode();
                return;
            }

            _currentStep = FlowSteps.AccessGranted;
            Task __ = GetOAuthToken();
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

        private void DrawCurrentPage(Rect inRect)
        {
            switch (_currentPage)
            {
                case Pages.Explanation:
                    Widgets.Label(
                        inRect,
                        @"In the next page you'll be prompted with a code. Your browser will automatically open to a page on Mixer's site you'll need to enter this code. Once you're done there, you'll need to click the ""Next"" button so the mod can continue the authentication sequence."
                    );
                    return;
                case Pages.ShortCode:
                    Widgets.Label(inRect, $@"Enter <b>{_currentCode}</b> at mixer.com/go");
                    return;
                case Pages.AuthCode:
                    switch (_currentStep)
                    {
                        case FlowSteps.HasCode:
                        case FlowSteps.WaitingOnUser:
                            Widgets.Label(
                                inRect,
                                $@"You must enter the code ""{_currentCode}"" at mixer.com/go before you can continue. Checking again in {_currentTimer:N0} seconds..."
                            );
                            break;
                        case FlowSteps.AccessGranted:
                        case FlowSteps.CodeExchange:
                            Widgets.Label(inRect, "Getting token...");
                            break;
                        case FlowSteps.Done:
                            Widgets.Label(inRect, "Access granted! ToolkitCore can now connect to Mixer.");
                            break;
                    }

                    return;
                default:
                    return;
            }
        }

        private enum Pages { Explanation, ShortCode, AuthCode }

        private enum FlowSteps
        {
            Begin, GeneratingCode, CodeGenerated,
            HasCode, WaitingOnUser, AccessGranted,
            CodeExchange, Done
        }
    }
}
