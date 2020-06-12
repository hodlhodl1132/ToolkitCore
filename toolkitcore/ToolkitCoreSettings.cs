using System;
using ToolkitCore.Utilities;
using UnityEngine;
using Verse;

namespace ToolkitCore
{
    public class ToolkitCoreSettings : ModSettings
    {
        public static bool allowWhispers = true;
        public static bool forceWhispers = false;
        public static bool sendMessageToChatOnStartup = true;

        // Twitch
        public static string twitchChannelUsername = "";
        public static string twitchBotUsername = "";
        public static string twitchOauthToken = "";
        public static bool twitchConnectOnStartup = false;

        // Mixer
        public static string mixerAccessToken = "";
        public static string mixerRefreshToken = "";
        public static string mixerUsername = "";
        public static bool mixerConnectOnStartup = false;

        internal static string mixerClientID = "405b869d155f40d8bde397cd1d25695206b3633b9e1bb4fa";

        public void DoWindowContents(Rect inRect)
        {
            Rect helpButton = new Rect(inRect.width - 120f, verticalSpacing, 90f, verticalHeight);
            if (Widgets.ButtonText(helpButton, "Help"))
            {
                Application.OpenURL("https://github.com/hodldeeznuts/ToolkitCore/wiki/Twitch-Chat-Connection");
            }

            Rect channelDetails = new Rect(0f, verticalSpacing, inRect.width / 2f, 64f);
            Widgets.Label(channelDetails, TCText.BigText("Channel Details"));

            float sectionVertical = channelDetails.y + (verticalSpacing * 2f);

            Rect label = new Rect(0f, sectionVertical, 200f, verticalHeight);
            Widgets.Label(label, "Channel:");

            label.y += verticalSpacing;

            Widgets.Label(label, "Bot Username:");

            label.y += verticalSpacing;

            Widgets.Label(label, "OAuth Token:");

            Rect input = new Rect(200f, sectionVertical, 200f, verticalHeight);

            twitchChannelUsername = Widgets.TextField(input, twitchChannelUsername);

            input.y += verticalSpacing;

            twitchBotUsername = Widgets.TextField(input, twitchBotUsername);

            if (twitchChannelUsername != "")
            {
                Rect copyUsername = new Rect(input.x + input.width + 10f, input.y, 210f, verticalHeight);

                if (Widgets.ButtonText(copyUsername, "Same as Channel"))
                {
                    twitchBotUsername = twitchChannelUsername;
                }
            }

            input.y += verticalSpacing;

            Rect oauthToggle = new Rect(input.x + input.width + 10f, input.y, 60f, verticalHeight);

            if (showOauth)
            {
                twitchOauthToken = Widgets.TextField(input, twitchOauthToken);

                if (Widgets.ButtonText(oauthToggle, "Hide")) showOauth = !showOauth;
            }
            else
            {
                Widgets.Label(input, new string('*', Math.Min(twitchOauthToken.Length, 16)));

                if (Widgets.ButtonText(oauthToggle, "Show")) showOauth = !showOauth;
            }

            Rect newToken = new Rect(oauthToggle.x + oauthToggle.width + 10f, input.y, 140f, verticalHeight);

            if (Widgets.ButtonText(newToken, "New OAuth Token")) Application.OpenURL("https://www.twitchapps.com/tmi/");

            input.y += verticalSpacing;

            if (Widgets.ButtonText(input, "Paste from Clipboard")) twitchOauthToken = GUIUtility.systemCopyBuffer;

            // Connection

            Rect connectionDetails = new Rect(0f, input.y + (verticalSpacing * 2), inRect.width / 2f, 64f);
            Widgets.Label(connectionDetails, TCText.BigText("Connection"));

            sectionVertical = connectionDetails.y + (verticalSpacing * 2f);

            label.y = sectionVertical;
            input.y = sectionVertical;

            Widgets.Label(label, "Status:");

            Rect connectionButton = new Rect(input.x + input.width + WidgetRow.LabelGap, input.y, input.width, verticalHeight);

            if (TwitchWrapper.Client != null && TwitchWrapper.Client.IsConnected)
            {
                Widgets.Label(input, TCText.ColoredText("Connected", Color.green));

                if (Widgets.ButtonText(connectionButton, "Disconnect")) TwitchWrapper.Client.Disconnect();
            }
            else
            {
                Widgets.Label(input, TCText.ColoredText("Not Connected", Color.red));

                if (Widgets.ButtonText(connectionButton, "Connect")) TwitchWrapper.StartAsync();
            }

            label.y += verticalSpacing;

            Widgets.Label(label, "Auto Connect on Startup:");

            input.y = label.y;

            Widgets.Checkbox(input.position, ref twitchConnectOnStartup);

            label.y += verticalSpacing;

            Widgets.Label(label, "Allow Viewers to Whisper:");

            input.y = label.y;

            Widgets.Checkbox(input.position, ref allowWhispers);

            label.y += verticalSpacing;

            Widgets.Label(label, "Force Viewers to Whisper:");

            input.y = label.y;

            Widgets.Checkbox(input.position, ref forceWhispers);

            label.y += verticalSpacing;

            Widgets.Label(label, "Send Connection Message:");

            input.y = label.y;

            Widgets.Checkbox(input.position, ref sendMessageToChatOnStartup);
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref allowWhispers, "allowWhispers", true);
            Scribe_Values.Look(ref sendMessageToChatOnStartup, "sendMessageToChatOnStartup", true);
            Scribe_Values.Look(ref forceWhispers, "forceWhispers", false);

            //Global
            Scribe_Values.Look(ref twitchChannelUsername, "twitchChannelUsername", "");
            Scribe_Values.Look(ref twitchBotUsername, "twitchBotUsername", "");
            Scribe_Values.Look(ref twitchOauthToken, "twitchOauthToken", "");
            Scribe_Values.Look(ref twitchConnectOnStartup, "twitchConnectOnStartup", false);

            //Twitch


            //Mixer
            Scribe_Values.Look(ref mixerAccessToken, "mixerAccessToken");
            Scribe_Values.Look(ref mixerRefreshToken, "mixerRefreshToken");
            Scribe_Values.Look(ref mixerUsername, "mixerUsername");
            Scribe_Values.Look(ref mixerConnectOnStartup, "mixerAutoConnect", false);
        }

        bool showOauth = false;

        static readonly float verticalHeight = 32f;
        static readonly float verticalSpacing = 40f;
    }
}
