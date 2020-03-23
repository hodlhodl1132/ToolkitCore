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