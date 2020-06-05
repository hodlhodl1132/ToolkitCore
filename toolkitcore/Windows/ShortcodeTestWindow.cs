using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Models.Mixer.ShortcodeOAuth;
using UnityEngine;
using Verse;

namespace ToolkitCore.Windows
{
    public class ShortcodeTestWindow : Window
    {
        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            switch (step)
            {
                case ShortcodeStep.needcode:

                    if (!gettingShortcode)
                    {
                        if (listing.ButtonText("GetShortCode"))
                        {
                            gettingShortcode = true;
                            _ = GetShortcode();
                        }
                    }
                    else
                    {
                        listing.Label("Awaiting Response");
                    }
                    
                    break;

                case ShortcodeStep.entercode:

                    if (needToEnterCode)
                    {
                        listing.Label("Please go to mixer.com/go and enter code: " + code);
                        if (listing.ButtonText("Done"))
                        {
                            needToEnterCode = false;
                            _ = CheckShortcode();
                        }
                    }
                    else
                    {
                        listing.Label("Checking Completion");
                    }

                    break;
                case ShortcodeStep.verifying:
                    listing.Label("Fetching Access Token");
                    break;

                case ShortcodeStep.finished:
                    listing.Label("Access Token Fetched, You may Close this Window now.");
                    break;
            }

            listing.End();
        }

        async Task GetShortcode()
        {
            bool task = await ShortcodeUtilities.GetShortcode();
            if (task)
            {
                step = ShortcodeStep.entercode;
                code = ShortcodeUtilities.OAuthShortcodeResponse.code;
                needToEnterCode = true;
            }
        }

        async Task CheckShortcode()
        {
            bool task = await ShortcodeUtilities.CheckShortcode();
            if (task)
            {
                step = ShortcodeStep.verifying;
                _ = GetOAuthToken();
            }
        }

        async Task GetOAuthToken()
        {
            bool task = await ShortcodeUtilities.GetOAuthToken();
            if (task)
            {
                ToolkitCoreSettings.mixerAccessToken = ShortcodeUtilities.OAuthTokenResponse.access_token;
                ToolkitCoreSettings.mixerRefreshToken = ShortcodeUtilities.OAuthTokenResponse.refresh_token;
                step = ShortcodeStep.finished;
            }
        }

        bool gettingShortcode = false;
        bool needToEnterCode = false;

        string code = string.Empty;

        ShortcodeStep step { get; set; } = ShortcodeStep.needcode;

        public enum ShortcodeStep
        {
            needcode,
            entercode,
            verifying,
            finished
        }
    }
}
