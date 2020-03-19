using UnityEngine;
using Verse;

namespace ToolkitCore
{
    public class ToolkitCoreSettings : ModSettings
    {
        public static string channel_username = "";
        public static string bot_username = "";
        public static string oauth_token = "";
        public static bool hideOauth = true;

        public static bool connectOnGameStartup = false;        

        public void DoWindowContents(Rect rect)
        {
            Listing_Standard ls = new Listing_Standard();
            ls.Begin(rect);

            ls.Label("It is recommended to have a separate account for bot responses. You can use the your twitch channel account as the bot. The Oauth Token must come from the account set for the Bot. The Bot account should have mod status to limit issues.");

            channel_username = ls.TextEntryLabeled("Twitch Channel Username: ", channel_username);

            bot_username = ls.TextEntryLabeled("Bot Username: ", bot_username);

            if (hideOauth)
            {
                ls.Label("<color=red>WARNING</color>: Do not show your Oauth Token on Stream");

                if (ls.ButtonTextLabeled("Only show Oauth if this screen is not visible to stream", "Show Oauth"))
                {
                    hideOauth = false;
                }
            }
            else
            {
                oauth_token = ls.TextEntryLabeled("Oauth Token:", oauth_token);

                if (ls.ButtonTextLabeled("Need a new Oauth Token?", "Oauth Token"))
                {
                    Application.OpenURL("https://www.twitchapps.com/tmi/");
                }

                if (ls.ButtonTextLabeled("Hide My Oauth Token", "Hide Oauth"))
                {
                    hideOauth = true;
                }
            }

            ls.CheckboxLabeled("Auto Connect Client on Startup", ref connectOnGameStartup, "Allow the Twitch Client to connect immediately upon entering the main menu");

            if (channel_username != "" && bot_username != "" && oauth_token != "")
            {
                if (ThreadWorker.stayConnected && TwitchWrapper.Client != null && TwitchWrapper.Client.IsConnected)
                {
                    if (ls.ButtonTextLabeled("Disconnect Client", "Disconnect"))
                    {
                        ThreadWorker.stayConnected = false;
                    }
                }
                else
                {
                    if (ls.ButtonTextLabeled("Connect Client", "Connect"))
                    {
                        ThreadWorker.stayConnected = true;
                        ToolkitCoreSettings.connectOnGameStartup = true;
                    }
                }
            }

            if (Prefs.DevMode)
            {
                if (ThreadWorker.runThread)
                {
                    if (ls.ButtonTextLabeled("Stop Thread", "Stop"))
                    {
                        ThreadWorker.runThread = false;
                    }
                }
                else
                {
                    if (ls.ButtonTextLabeled("Start Thread", "Start"))
                    {
                        ThreadWorker.runThread = true;
                        ThreadWorker.StartThread();
                    }
                }

                ls.Label($"Stay Connected: {ThreadWorker.stayConnected} - Run Thread: {ThreadWorker.runThread} - Debug MSG: {ThreadWorker.sendDebugMSG}");

                if (TwitchWrapper.Client != null)
                    ls.Label($"Initialized: {TwitchWrapper.Client.IsInitialized} - Connected: {TwitchWrapper.Client.IsConnected}");

                if (!ThreadWorker.sendDebugMSG && ls.ButtonTextLabeled("Send DEBUG Message", "Send"))
                {
                    ThreadWorker.sendDebugMSG = true;
                }
            }

            ls.End();
        }

        public override void ExposeData()
        {
            Scribe_Values.Look<string>(ref channel_username, "channel_username", "", true);
            Scribe_Values.Look<string>(ref bot_username, "bot_username", "", true);
            Scribe_Values.Look<string>(ref oauth_token, "oauth_token", "", true);
            Scribe_Values.Look<bool>(ref connectOnGameStartup, "connectOnGameStartup", true);
        }
    }
}