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

        public override void ExposeData()
        {
            // Global
            Scribe_Values.Look(ref allowWhispers, "allowWhispers", true);
            Scribe_Values.Look(ref sendMessageToChatOnStartup, "sendMessageToChatOnStartup", true);
            Scribe_Values.Look(ref forceWhispers, "forceWhispers", false);

            // Twitch
            Scribe_Values.Look(ref twitchChannelUsername, "twitchChannelUsername", "");
            Scribe_Values.Look(ref twitchBotUsername, "twitchBotUsername", "");
            Scribe_Values.Look(ref twitchOauthToken, "twitchOauthToken", "");
            Scribe_Values.Look(ref twitchConnectOnStartup, "twitchConnectOnStartup", false);

            // Mixer
            Scribe_Values.Look(ref mixerAccessToken, "mixerAccessToken");
            Scribe_Values.Look(ref mixerRefreshToken, "mixerRefreshToken");
            Scribe_Values.Look(ref mixerUsername, "mixerUsername");
            Scribe_Values.Look(ref mixerConnectOnStartup, "mixerAutoConnect", false);
        }
    }
}
