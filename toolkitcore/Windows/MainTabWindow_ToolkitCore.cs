using RimWorld;
using System.Collections.Generic;
using ToolkitCore.Models;
using ToolkitCore.Utilities;
using TwitchLib.Client.Models;
using UnityEngine;
using Verse;

namespace ToolkitCore.Windows
{
    public class MainTabWindow_ToolkitCore : MainTabWindow
    {

        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            listing.Label("Toolkit Quick Menu");

            foreach (ToolkitAddon addon in AddonRegistry.ToolkitAddons)
            {
                if (listing.ButtonText(addon.LabelCap))
                {
                    Find.WindowStack.Add(new FloatMenu(addon.GetAddonMenu().MenuOptions()));
                }
            }

            listing.End();
        }

        public override Vector2 RequestedTabSize => new Vector2(300f, 100f + (AddonRegistry.ToolkitAddons.Count * 32f) );

        public override MainTabWindowAnchor Anchor => MainTabWindowAnchor.Right;
    }
}
