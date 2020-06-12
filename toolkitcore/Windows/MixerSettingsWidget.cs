using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Utilities;
using UnityEngine;
using Verse;

namespace ToolkitCore.Windows
{
    public static class MixerSettingsWidget
    {
        public static void OnGUI(Rect inRect)
        {
            Color cCache = GUI.color;
            float midpoint = inRect.width / 2f;
            Rect leftColumn = new Rect(0f, 0f, midpoint - 10f, inRect.height);
            Rect rightColumn = new Rect(midpoint + 10f, 0f, midpoint - 10f, inRect.height);
            Rect rightColumnInner = new Rect(0f, 0f, rightColumn.width, rightColumn.height);
            Rect separator = new Rect(midpoint - 0.5f, 0f, 1f, inRect.height);
            
            GUI.color = new Color(0.22f, 0.22f, 0.22f, 0.75f);
            Widgets.DrawLineVertical(separator.x, separator.y, separator.height);
            GUI.color = cCache;

            GUI.BeginGroup(leftColumn);
            DrawLeftColumn(leftColumn);
            GUI.EndGroup();

            GUI.BeginGroup(rightColumn);
            DrawRightColumn(rightColumnInner);
            GUI.EndGroup();
        }

        private static void DrawLeftColumn(Rect columnRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(columnRect);
            
            (Rect channelLabel, Rect channelField) = listing.GetRect(Text.LineHeight).ToForm(0.7f);
            Widgets.Label(channelLabel, "Username:");
            ToolkitCoreSettings.mixerUsername = Widgets.TextField(channelField, ToolkitCoreSettings.mixerUsername);

            (Rect _, Rect tknBtn) = listing.GetRect(Text.LineHeight).ToForm(0.7f);

            bool isRefreshing = !ToolkitCoreSettings.mixerRefreshToken.NullOrEmpty();
            if (isRefreshing && Widgets.ButtonText(tknBtn, "Refresh OAuth Token"))
            {
                // TODO: Try to refresh the token. If it fails, it should take the user through the shortcode wizard.
            }
            else if (!isRefreshing && Widgets.ButtonText(tknBtn, "New OAuth Token"))
            {
                Dialog_MixerAuthWizard wizard = new Dialog_MixerAuthWizard();
                
                Find.WindowStack.TryRemove(wizard.GetType());
                Find.WindowStack.Add(wizard);
            }
            
            listing.End();
        }

        private static void DrawRightColumn(Rect columnRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(columnRect);
            
            (Rect statusLabel, Rect statusField) = listing.GetRect(Text.LineHeight).ToForm();
            Widgets.Label(statusLabel, "Status:");

            string statusText = MixerWrapper.Connected() ? TCText.ColoredText("Connected", ColorLibrary.BrightGreen) : TCText.ColoredText("Disconnected", Color.red);
            SettingsHelper.DrawLabelAnchored(statusField, statusText, TextAnchor.MiddleCenter);

            (Rect _, Rect connBtn) = listing.GetRect(Text.LineHeight).ToForm();

            if (MixerWrapper.Connected())
            {
                if (Widgets.ButtonText(connBtn, "Disconnect"))
                {
                    MixerWrapper.Disconnect();
                }
            }
            else
            {
                if (Widgets.ButtonText(connBtn, "Connect"))
                {
                    MixerWrapper.InitializeClient();
                }
            }
            
            listing.End();
        }
    }
}
