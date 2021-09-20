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
        }
    }
}
