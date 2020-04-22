using RimWorld;
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
            Rect label = new Rect(0, 0, 400f, 32f);
            Widgets.Label(label, "Toolkit Quick Settings");

            Rect settingsButton = new Rect(0, 32f, 300f, 32f);
            if (Widgets.ButtonText(settingsButton, "ToolkitCore Settings"))
            {
                Window_ModSettings window = new Window_ModSettings(LoadedModManager.GetMod<ToolkitCore>());
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }

            // If initial setup has not been completed
            if (ToolkitCoreSettings.channel_username == "")
            {
                Rect setupWarning = new Rect(0, 80f, 300f, 64f);
                Widgets.Label(setupWarning, "ToolkitCore has not been setup completely in mod settings.");

                return;
            }


            // If client is not setup do not render connection details
            if (TwitchWrapper.Client == null) return;

            Rect connectionLabel = new Rect(500f, 0f, 200f, 32f);
            Widgets.Label(connectionLabel, TwitchWrapper.Client.IsConnected ? TCText.ColoredText("Connected", Color.green) : TCText.ColoredText("Not Connected", Color.red));

            Rect messageLogLabel = new Rect(500f, 32f, 200f, 32f);
            Widgets.Label(messageLogLabel, "Message Log");

            Rect messageBox = new Rect(500f, 64f, 300f, 180f);
            string messageBoxText = "";
            foreach (ChatMessage chatMessage in MessageLog.LastChatMessages)
            {
                messageBoxText += $"{chatMessage.DisplayName}: {chatMessage.Message}\n";
            }
            Widgets.TextArea(messageBox, messageBoxText, true);

            Rect whisperLogLabel = new Rect(500f, 232f, 200f, 32f);
            Widgets.Label(whisperLogLabel, "Whisper Log");

            Rect whisperBox = new Rect(500f, 264f, 300f, 80f);
            string whisperBoxText = "";
            foreach (WhisperMessage whisperMessage in MessageLog.LastWhisperMessages)
            {
                whisperBoxText += $"{whisperMessage.DisplayName}: {whisperMessage.Message}\n";
            }
            Widgets.TextArea(whisperBox, whisperBoxText, true);
        }

        public override Vector2 RequestedTabSize => new Vector2(800, 400);
    }
}
