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
                    Find.WindowStack.Add(new FloatMenu(addon.GetAddonMenu().MenuOptions));
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

            //Rect label = new Rect(0, 0, 400f, 32f);
            //Widgets.Label(label, "Toolkit Quick Settings");

            //Rect settingsButton = new Rect(0, 32f, 300f, 32f);
            //if (Widgets.ButtonText(settingsButton, "ToolkitCore Settings"))
            //{
            //    Window_ModSettings window = new Window_ModSettings(LoadedModManager.GetMod<ToolkitCore>());
            //    Find.WindowStack.TryRemove(window.GetType());
            //    Find.WindowStack.Add(window);
            //}

            //// If client is not setup do not render connection details
            //if (TwitchWrapper.Client == null) return;

            //Rect connectionLabel = new Rect(500f, 0f, 200f, 32f);
            //Widgets.Label(connectionLabel, TwitchWrapper.Client.IsConnected ? TCText.ColoredText("Connected", Color.green) : TCText.ColoredText("Not Connected", Color.red));

            //Rect messageLogLabel = new Rect(500f, 32f, 200f, 32f);
            //Widgets.Label(messageLogLabel, "Message Log");

            //Rect messageBox = new Rect(500f, 64f, 300f, 180f);

            //Rect whisperLogLabel = new Rect(500f, 232f, 200f, 32f);
            //Widgets.Label(whisperLogLabel, "Whisper Log");


            //Widgets.TextArea(whisperBox, whisperBoxText, true);
        }

        public override Vector2 RequestedTabSize => new Vector2(800, 400);
    }
}
