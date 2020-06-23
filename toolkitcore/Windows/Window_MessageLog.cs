using ToolkitCore.Interfaces;
using ToolkitCore.Models;
using ToolkitCore.Utilities;
using UnityEngine;
using Verse;

namespace ToolkitCore.Windows
{
    [StaticConstructorOnStartup]
    public class Window_MessageLog : Window
    {
        static Texture2D mixerIcon;
        static Texture2D twitchIcon;

        private Vector2 _scrollPos = Vector2.zero;
        private float _longestNameCache;

        static Window_MessageLog()
        {
            mixerIcon = ContentFinder<Texture2D>.Get("UI/Icons/mixerIcon");
            twitchIcon = ContentFinder<Texture2D>.Get("UI/Icons/twitchIcon");
        }

        public Window_MessageLog()
        {
            doCloseButton = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard(GameFont.Small);
            Rect contentRect = inRect.ContractedBy(30f);
            Rect contentView = new Rect(0f, 0f, contentRect.width - 16f, 0f);

            listing.Begin(inRect);
            listing.BeginScrollView(contentRect, ref _scrollPos, ref contentView);

            for (int index = 0; index < MessageLogger.MessageLog.Count; index++)
            {
                IMessage message = MessageLogger.MessageLog[index];
                float usernameWidth = Text.CalcSize(message.Username()).x;

                if (usernameWidth > _longestNameCache)
                {
                    _longestNameCache = usernameWidth;
                }

                Rect usernameRect = new Rect(0f, 0f, _longestNameCache, Text.LineHeight);
                Rect iconRect = new Rect(_longestNameCache + 11f, 0f, 22f, usernameRect.height);
                Rect messageRect = new Rect(
                    iconRect.x + iconRect.width + 10f,
                    0f,
                    contentView.width - (usernameRect.width + iconRect.width + 20f),
                    0f
                );

                messageRect.height = Text.CalcHeight(message.Message(), messageRect.width);

                Texture2D icon;
                Window_Services.Tab tab;
                Rect lineRect = listing.GetRect(messageRect.height);

                usernameRect.y = lineRect.y;
                iconRect.y = lineRect.y;
                messageRect.y = lineRect.y;

                contentView.height += messageRect.height;

                if (!lineRect.IsRegionVisible(contentView, _scrollPos))
                {
                    continue;
                }

                if (index % 2 == 0)
                {
                    Widgets.DrawLightHighlight(lineRect);
                }

                switch (message.Service())
                {
                    case Services.Service.Mixer:
                        icon = mixerIcon;
                        tab = Window_Services.Tab.Mixer;
                        break;
                    case Services.Service.Twitch:
                        icon = twitchIcon;
                        tab = Window_Services.Tab.Twitch;
                        break;
                    default:
                        icon = Texture2D.normalTexture;
                        tab = Window_Services.Tab.Global;
                        break;
                }

                GUI.DrawTexture(iconRect.ContractedBy(4f), icon);
                Widgets.DrawHighlightIfMouseover(iconRect);
                if (Widgets.ButtonInvisible(iconRect))
                {
                    Window_Services service = new Window_Services(tab);

                    Find.WindowStack.TryRemove(service);
                    Find.WindowStack.Add(service);
                }

                SettingsHelper.DrawLabelAnchored(usernameRect, message.Username(), TextAnchor.MiddleRight);
                Widgets.Label(messageRect, message.Message());

                contentView.height += 5f;
            }

            listing.End();
            listing.EndScrollView(ref contentView);
        }

        public override Vector2 InitialSize => new Vector2(600, UI.screenHeight - 100f);
    }
}
