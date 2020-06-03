using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ToolkitCore.Windows
{
    public class Window_Services : Window
    {
        internal Window_Services(Tab defaultTab = Tab.Global)
        {
            this.doCloseButton = true;
            tab = defaultTab;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Rect rect = new Rect(0f, 0f, inRect.width, 35f);
            Text.Font = GameFont.Medium;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(rect, "Streaming Service Settings");
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;

            tabsList.Clear();
            tabsList.Add(new TabRecord("Global", delegate ()
            {
                this.tab = Tab.Global;
            }, this.tab == Tab.Global));
            tabsList.Add(new TabRecord("Twitch", delegate ()
            {
                this.tab = Tab.Twitch;
            }, this.tab == Tab.Twitch));
            tabsList.Add(new TabRecord("Mixer", delegate ()
            {
                this.tab = Tab.Mixer;
            }, this.tab == Tab.Mixer));
            inRect.yMin += 119f;
            Widgets.DrawMenuSection(inRect);
            TabDrawer.DrawTabs(inRect, tabsList, 300f);
            tabsList.Clear();

            Rect rect2 = new Rect(inRect);
            rect2.y += 10f;
            rect2.x += 10f;
            GUI.BeginGroup(rect2);
            Rect inRect2 = rect2;

            switch (tab)
            {
                case Tab.Global:
                    GlobalSettingsWidget.OnGUI(inRect2);
                    break;
                case Tab.Twitch:
                    TwitchSettingsWidget.OnGUI(inRect2);
                    break;
                case Tab.Mixer:
                    MixerSettingsWidget.OnGUI(inRect2);
                    break;
            }
            GUI.EndGroup();
        }

        public override void PostClose()
        {
            LoadedModManager.GetMod<ToolkitCore>().GetSettings<ToolkitCoreSettings>().Write();
        }

        public override Vector2 InitialSize => new Vector2(1024f, (float)UI.screenHeight - 200f);

        protected override float Margin => 0f;

        private Tab tab = Tab.Global;

        private static List<TabRecord> tabsList = new List<TabRecord>();

        internal enum Tab
        {
            Global,
            Twitch,
            Mixer
        }
    }
}
