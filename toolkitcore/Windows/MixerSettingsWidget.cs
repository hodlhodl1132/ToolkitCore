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

            listing.Gap(8f);
            (Rect tokenLabel, Rect tokenField) = listing.GetRect(Text.LineHeight).ToForm(0.7f);
            Widgets.Label(tokenLabel, "OAuth Token:");

            if (showAccessToken)
            {
                ToolkitCoreSettings.mixerAccessToken = Widgets.TextField(tokenField, ToolkitCoreSettings.mixerAccessToken);
            }
            else
            {
                Widgets.Label(tokenField, new string('*', Math.Min(ToolkitCoreSettings.mixerAccessToken.Length, 16)));
            }

            SettingsHelper.DrawShowButton(tokenField, ref showAccessToken);
            
            (Rect _, Rect tknBtn) = listing.GetRect(Text.LineHeight).ToForm(0.7f);
            (Rect _, Rect pasteBtn) = listing.GetRect(Text.LineHeight).ToForm(0.7f);

            if (Widgets.ButtonText(tknBtn, "New OAuth Token"))
            {
                Application.OpenURL("https://mixertokengenerator.com/?code=GwzpS4SGkHJLEzZS&state=frontend%7Cbmsxd25aWnphRWZRMTlFK09SdWl3dz09%7C23%2C25%2C34");
            }

            if (Widgets.ButtonText(pasteBtn, "Paste from Clipboard"))
            {
                ToolkitCoreSettings.mixerAccessToken = GUIUtility.systemCopyBuffer;
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

            if (MixerWrapper.Connected() && Widgets.ButtonText(connBtn, "Reconnect"))
            {
                
            }
            else if (Widgets.ButtonText(connBtn, "Connect"))
            {
                MixerWrapper.InitializeClient();
            }
            
            listing.End();
        }

        private static bool showAccessToken;
    }
}
