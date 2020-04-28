using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Models;
using ToolkitCore.Utilities;
using TwitchLib.Client.Models;
using UnityEngine;
using Verse;

namespace ToolkitCore.Windows
{
    public class Window_MessageLog : Window
    {
        public Window_MessageLog()
        {
            doCloseButton = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            listing.Label("Twitch Message Log");

            listing.ColumnWidth = inRect.width * 0.30f;

            if (TwitchWrapper.Client != null)
            {
                bool connected = TwitchWrapper.Client.IsConnected;

                listing.Label( connected ? TCText.ColoredText("Connected", Color.green) : TCText.ColoredText("Not Connected", Color.red));

                if (listing.ButtonText(connected ? "Disconnect" : "Connect"))
                {
                    TwitchWrapper.Client.Disconnect();
                }
            }
            else
            {
                if (listing.ButtonText("Reset Twitch Client"))
                {
                    TwitchWrapper.StartAsync();
                }
            }

            listing.End();

            float columnWidth = inRect.width * 0.49f;

            Rect headerOne = new Rect(0, 100f, columnWidth, 32f);
            Widgets.Label(headerOne, "Message Log");

            Rect messageBoxOne = new Rect(0, 132f, columnWidth, 200f);

            string messageBoxText = "";
            foreach (ChatMessage chatMessage in MessageLog.LastChatMessages)
            {
                messageBoxText += $"{chatMessage.DisplayName}: {chatMessage.Message}\n";
            }

            Widgets.TextArea(messageBoxOne, messageBoxText, true);

            Rect headerTwo = new Rect(headerOne);
            headerTwo.x += columnWidth + 10f;
            Widgets.Label(headerTwo, "Whisper Log");

            Rect messageBoxTwo = new Rect(messageBoxOne);
            messageBoxTwo.x += messageBoxTwo.width + 10f;

            string whisperBoxText = "";
            foreach (WhisperMessage whisperMessage in MessageLog.LastWhisperMessages)
            {
                whisperBoxText += $"{whisperMessage.DisplayName}: {whisperMessage.Message}\n";
            }

            Widgets.TextArea(messageBoxTwo, whisperBoxText, true);
        }
    }
}
