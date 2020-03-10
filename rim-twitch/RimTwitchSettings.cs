using UnityEngine;
using Verse;

namespace rim_twitch
{
    public class RimTwitchSettings : ModSettings
    {
        public static string channel_username = "";
        public static string oauth_token = "";
        public static bool hideOauth = true;

        public static bool connectOnGameStartup = false;        

        public void DoWindowContents(Rect rect)
        {
            Listing_Standard ls = new Listing_Standard();
            ls.Begin(rect);

            channel_username = ls.TextEntryLabeled("Twitch Channel Username: ", channel_username);

            if (hideOauth)
            {
                ls.Label("<color=red>WARNING</color>: Do not show your Oauth Token on Stream");

                if (ls.ButtonTextLabeled("I acknowledge that showing my Oauth Token on stream is risky.", "Show Oauth"))
                {
                    hideOauth = false;
                }
            }
            else
            {
                oauth_token = ls.TextEntryLabeled("Oauth Token:", oauth_token);

                if (ls.ButtonTextLabeled("Hide My Oauth Token from Stream", "Hide Oauth"))
                {
                    hideOauth = true;
                }
            }

            ls.CheckboxLabeled("Auto Connect Client on Startup", ref connectOnGameStartup, "Allow the Twitch Client to connect immediately upon entering the main menu");

            if (channel_username != "" && oauth_token != "")
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

                if (ThreadWorker.stayConnected)
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
                    }
                }

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
            Scribe_Values.Look<string>(ref oauth_token, "oauth_token", "", true);
            Scribe_Values.Look<bool>(ref connectOnGameStartup, "connectOnGameStartup", true);
        }
    }
}