using System;
using System.Collections.Generic;
using RimWorld;
using ToolkitCore.Utilities;
using UnityEngine;
using Verse;

namespace ToolkitCore.Windows
{
    public class Window_Services : Window
    {
        internal Window_Services(Tab defaultTab = Tab.Global)
        {
            doCloseButton = true;
            tab = defaultTab;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Color cCache = GUI.color;

            Rect headerRect = new Rect(0f, 0f, inRect.width, 35f);
            Rect tabBarRect = new Rect(0f, 50f, inRect.width, 35f);
            Rect contentRect = new Rect(
                0f,
                tabBarRect.y + tabBarRect.height,
                inRect.width,
                inRect.height - (tabBarRect.y + tabBarRect.height)
            );
            Rect innerContentRect = new Rect(0f, 0f, contentRect.width - 20f, contentRect.height - 20f - 55f);

            SettingsHelper.DrawBigLabelAnchored(headerRect, "Streaming Service Settings", TextAnchor.MiddleCenter);

            GUI.color = Color.grey;
            Widgets.DrawLightHighlight(tabBarRect);
            Widgets.DrawLightHighlight(contentRect);
            GUI.color = cCache;

            float suggestedWidth = inRect.width / tabsList.Count;
            float offset = 0f;
            foreach (TabWidget tabWidget in tabsList)
            {
                float adjustedWidth = Text.CalcSize(tabWidget.Label).x + 20f;

                Rect tabRect = new Rect(
                    offset,
                    tabBarRect.y,
                    Mathf.Max(suggestedWidth, adjustedWidth),
                    tabBarRect.height
                );

                Rect statusRect = new Rect(tabRect.x + 8f, tabRect.y + (tabRect.height / 2f) - 8f, 16f, 16f);

                if (tabWidget.IsSelected())
                {
                    Widgets.DrawLightHighlight(tabRect);
                }

                SettingsHelper.DrawLabelAnchored(tabRect, tabWidget.Label, TextAnchor.MiddleCenter);

                if (tabWidget.GetStatus != null)
                {
                    tabWidget.DrawStatusIcon(statusRect);

                    if (Widgets.ButtonInvisible(statusRect))
                    {
                        tabWidget.OnStatusClick();
                    }
                }

                if (Widgets.ButtonInvisible(tabRect))
                {
                    tabWidget.OnClick();
                }

                offset += tabRect.width;
            }

            Widgets.DrawLightHighlight(contentRect);

            GUI.BeginGroup(contentRect.ContractedBy(10f));
            switch (tab)
            {
                case Tab.Global:
                    GlobalSettingsWidget.OnGUI(innerContentRect);
                    break;
                case Tab.Twitch:
                    TwitchSettingsWidget.OnGUI(innerContentRect);
                    break;
                case Tab.Mixer:
                    MixerSettingsWidget.OnGUI(innerContentRect);
                    break;
            }

            GUI.EndGroup();
        }

        public override void PreOpen()
        {
            base.PreOpen();

            if (!tabsList.NullOrEmpty())
            {
                return;
            }

            tabsList.Add(
                new TabWidget {Label = "Global", OnClick = () => tab = Tab.Global, IsSelected = () => tab == Tab.Global}
            );
            tabsList.Add(
                new TabWidget
                {
                    Label = "Twitch",
                    OnClick = () => tab = Tab.Twitch,
                    IsSelected = () => tab == Tab.Twitch,
                    GetStatus = () => TwitchWrapper.Client?.IsConnected ?? false,
                    OnStatusClick = () =>
                    {
                        if (TwitchWrapper.Client?.IsConnected ?? false)
                        {
                            TwitchWrapper.StartAsync();
                        }
                        else
                        {
                            TwitchWrapper.Client?.Disconnect();
                            TwitchWrapper.StartAsync();
                        }
                    }
                }
            );
            tabsList.Add(
                new TabWidget
                {
                    Label = "Mixer",
                    OnClick = () => tab = Tab.Mixer,
                    IsSelected = () => tab == Tab.Mixer,
                    GetStatus = MixerWrapper.Connected,
                    OnStatusClick = () =>
                    {
                        if (MixerWrapper.Connected())
                        {

                        }
                        else
                        {
                            MixerWrapper.InitializeClient();
                        }
                    }
                }
            );
        }

        public override void PostClose()
        {
            LoadedModManager.GetMod<ToolkitCore>().GetSettings<ToolkitCoreSettings>().Write();

            tabsList.Clear();
        }

        public override Vector2 InitialSize => new Vector2(1024f, UI.screenHeight - 200f);

        protected override float Margin => 0f;

        private Tab tab = Tab.Global;

        private List<TabWidget> tabsList = new List<TabWidget>();

        internal enum Tab { Global, Twitch, Mixer }

        internal class TabWidget
        {
            public string Label { get; set; }
            public Action OnClick { get; set; }
            public Func<bool> GetStatus { get; set; }
            public Action OnStatusClick { get; set; }
            public Func<bool> IsSelected { get; set; }

            public void DrawStatusIcon(Rect region)
            {
                bool status = GetStatus();
                string statusText = status ? "Connected" : "Disconnected";
                statusText += "\n\nClick to ";
                statusText += (status ? "reconnect" : "connect") + ".";
                
                Texture2D.whiteTexture.DrawColored(region, status ? ColorLibrary.BrightGreen : Color.red);
                TooltipHandler.TipRegion(region, statusText);
            }
        }
    }
}
