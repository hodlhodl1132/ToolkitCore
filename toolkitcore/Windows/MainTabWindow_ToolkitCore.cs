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

            listing.ColumnWidth = inRect.width * 0.45f;

            listing.Label("Toolkit Quick Menu");

            foreach (ToolkitAddon addon in AddonRegistry.ToolkitAddons)
            {
                if (listing.ButtonText(addon.LabelCap))
                {
                    Find.WindowStack.Add(new FloatMenu(addon.GetAddonMenu().MenuOptions()));
                }
            }

            if (TwitchWrapper.Client != null)
            {
                listing.NewColumn();

                listing.Label(TwitchWrapper.Client.IsConnected ? TCText.ColoredText("Connected", Color.green) : TCText.ColoredText("Not Connected", Color.red));

                listing.Label("Chat Log");

                string messageBoxText = "";
                foreach (ChatMessage chatMessage in MessageLog.LastChatMessages)
                {
                    messageBoxText += $"{chatMessage.DisplayName}: {chatMessage.Message}\n";
                }

                listing.TextEntry(messageBoxText, 7);

                listing.Label("Whisper Log");

                string whisperBoxText = "";
                foreach (WhisperMessage whisperMessage in MessageLog.LastWhisperMessages)
                {
                    whisperBoxText += $"{whisperMessage.DisplayName}: {whisperMessage.Message}\n";
                }

                listing.TextEntry(whisperBoxText, 3);
            }

            listing.End();
        }

        public override Vector2 RequestedTabSize => new Vector2(800, 400);
    }
}
