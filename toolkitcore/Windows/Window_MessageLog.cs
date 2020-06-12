using NAudio.Mixer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Interfaces;
using ToolkitCore.Models;
using ToolkitCore.Utilities;
using TwitchLib.Client.Models;
using UnityEngine;
using Verse;

namespace ToolkitCore.Windows
{
    public class Window_MessageLog : Window
    {
        static Texture2D mixerIcon = ContentFinder<Texture2D>.Get("UI/Icons/mixerIcon");
        static Texture2D twitchIcon = ContentFinder<Texture2D>.Get("UI/Icons/twitchIcon");

        public Window_MessageLog()
        {
            doCloseButton = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            GameFont original = Text.Font;
            Text.Font = GameFont.Small;

            Rect serviceIcon = new Rect(0, 0, 24, 24);
            Rect messageBox = new Rect(30, 0, 500, 0);

            foreach (IMessage message in MessageLogger.MessageLog)
            {
                float rowHeight = Text.CalcHeight(message.Message(), messageBox.width);
                GUI.DrawTexture(serviceIcon, message.Service() == Services.Service.Twitch ? twitchIcon : mixerIcon);

                messageBox.height = rowHeight;

                Widgets.Label(messageBox, $"{message.Username()}: {message.Message()}");

                serviceIcon.y += rowHeight;
                messageBox.y += rowHeight;
            }

            Text.Font = original;
        }

        public override Vector2 InitialSize => new Vector2(600, UI.screenHeight - 100f);
    }
}
