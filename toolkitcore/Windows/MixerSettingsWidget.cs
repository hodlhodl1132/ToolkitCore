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
            Rect label = new Rect(0f, 0f, 200f, 24f);
            Rect value = new Rect(204f, 0f, 200f, 24f);

            Widgets.Label(label, "Mixer Username:");
            ToolkitCoreSettings.mixerUsername = Widgets.TextField(value, ToolkitCoreSettings.mixerUsername);

            label.y += label.height;
            value.y += value.height;

            Widgets.Label(label, "Mixer AuthKey:");
            ToolkitCoreSettings.mixerAccessToken = Widgets.TextField(value, ToolkitCoreSettings.mixerAccessToken);

            Rect newAuthKey = new Rect(value);
            newAuthKey.x += 4f + newAuthKey.width;

            if (Widgets.ButtonText(newAuthKey, "New Auth Key"))
            {
                Application.OpenURL("https://mixertokengenerator.com/?code=GwzpS4SGkHJLEzZS&state=frontend%7Cbmsxd25aWnphRWZRMTlFK09SdWl3dz09%7C23%2C25%2C34");
            }

            label.y += label.height;
            value.y += value.height;

            string statusLabel = MixerWrapper.Connected() ? TCText.ColoredText("Connected", ColorLibrary.BrightGreen) : TCText.ColoredText("Disconnected", ColorLibrary.BrickRed);

            Widgets.Label(label, "Status:");
            Widgets.Label(value, statusLabel);

            Rect connectionButton = new Rect(newAuthKey);
            connectionButton.y += connectionButton.height;

            if (MixerWrapper.Connected() && Widgets.ButtonText(connectionButton, "Reconnect"))
            {

            }
            else if (Widgets.ButtonText(connectionButton, "Connect"))
            {
                MixerWrapper.InitializeClient();
            }
        }
    }
}
