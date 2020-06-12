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
    public static class TwitchSettingsWidget
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
            Widgets.Label(channelLabel, "Channel:");
            ToolkitCoreSettings.channel_username = Widgets.TextField(channelField, ToolkitCoreSettings.channel_username);

            (Rect _, Rect copyBtn) = listing.GetRect(Text.LineHeight).ToForm(0.7f);

            Widgets.DrawHighlightIfMouseover(copyBtn);
            SettingsHelper.DrawLabelAnchored(copyBtn, "<b>↓</b>", TextAnchor.MiddleCenter);
            TooltipHandler.TipRegion(copyBtn, "Copy channel to bot username.");
            
            if (Widgets.ButtonInvisible(copyBtn))
            {
                ToolkitCoreSettings.bot_username = ToolkitCoreSettings.channel_username;
            }

            (Rect botLabel, Rect botField) = listing.GetRect(Text.LineHeight).ToForm(0.7f);
            Widgets.Label(botLabel, "Bot Username:");
            ToolkitCoreSettings.bot_username = Widgets.TextField(botField, ToolkitCoreSettings.bot_username);

            listing.Gap(8f);
            (Rect tokenLabel, Rect tokenField) = listing.GetRect(Text.LineHeight).ToForm(0.7f);
            Widgets.Label(tokenLabel, "OAuth Token:");

            if (showToken)
            {
                ToolkitCoreSettings.oauth_token = Widgets.TextField(tokenField, ToolkitCoreSettings.oauth_token);
            }
            else
            {
                Widgets.Label(tokenField, new string('*', Math.Min(ToolkitCoreSettings.oauth_token.Length, 16)));
            }

            SettingsHelper.DrawShowButton(tokenField, ref showToken);
            listing.Gap();

            (Rect _, Rect tmiBtn) = listing.GetRect(Text.LineHeight).ToForm(0.7f);
            (Rect _, Rect pasteBtn) = listing.GetRect(Text.LineHeight).ToForm(0.7f);

            if (Widgets.ButtonText(tmiBtn, "New OAuth Token"))
            {
                Application.OpenURL("https://www.twitchapps.com/tmi/");
            }

            if (Widgets.ButtonText(pasteBtn, "Paste from Clipboard"))
            {
                ToolkitCoreSettings.oauth_token = GUIUtility.systemCopyBuffer;
            }

            listing.End();
        }

        private static void DrawRightColumn(Rect columnRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(columnRect);
            
            (Rect statusLabel, Rect statusField) = listing.GetRect(Text.LineHeight).ToForm();
            Widgets.Label(statusLabel, "Status:");
            
            string statusText = TwitchWrapper.Client?.IsConnected ?? false ? TCText.ColoredText("Connected", ColorLibrary.BrightGreen) : TCText.ColoredText("Disconnected", Color.red);
            SettingsHelper.DrawLabelAnchored(statusField, statusText, TextAnchor.MiddleCenter);
            
            (Rect _, Rect connBtn) = listing.GetRect(Text.LineHeight).ToForm();

            if ((TwitchWrapper.Client?.IsConnected ?? false) && Widgets.ButtonText(connBtn, "Reconnect"))
            {
                TwitchWrapper.Client.Disconnect();
                TwitchWrapper.StartAsync();
            }
            else if (Widgets.ButtonText(connBtn, "Connect"))
            {
                TwitchWrapper.StartAsync();
            }
            
            (Rect autoLabel, Rect autoField) = listing.GetRect(Text.LineHeight).ToForm(0.85f);
            Widgets.Label(autoLabel, "Auto connect on startup?");
            Widgets.Checkbox(autoField.x + autoField.width - 24f, autoField.y, ref ToolkitCoreSettings.connectOnGameStartup);
            
            listing.End();
        }

        private static bool showToken;
    }
}
