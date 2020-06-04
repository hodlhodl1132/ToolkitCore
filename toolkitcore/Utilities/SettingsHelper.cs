using System;
using UnityEngine;
using Verse;

namespace ToolkitCore.Utilities
{
    [StaticConstructorOnStartup]
    public static class SettingsHelper
    {
        private static Texture2D VisibleIcon;
        private static Texture2D HiddenIcon;

        static SettingsHelper()
        {
            VisibleIcon = ContentFinder<Texture2D>.Get("UI/Icons/Visible");
            HiddenIcon = ContentFinder<Texture2D>.Get("UI/Icons/Hidden");
        }
        
        public static bool DrawClearButton(Rect canvas)
        {
            Rect region = new Rect(canvas.x + canvas.width - 16f, canvas.y, 16f, canvas.height);
            Widgets.ButtonText(region, "×", false);

            bool clicked = Mouse.IsOver(region) && Event.current.type == EventType.Used && Input.GetMouseButtonDown(0);

            if (!clicked)
            {
                return false;
            }

            GUI.FocusControl(null);
            return true;
        }
        
        public static bool DrawDoneButton(Rect canvas)
        {
            Rect region = new Rect(canvas.x + canvas.width - 16f, canvas.y, 16f, canvas.height);
            Widgets.ButtonText(region, "✔", false);

            bool clicked = Mouse.IsOver(region) && Event.current.type == EventType.Used && Input.GetMouseButtonDown(0);

            if (!clicked)
            {
                return false;
            }

            GUI.FocusControl(null);
            return true;
        }

        public static void DrawShowButton(Rect canvas, ref bool state)
        {
            Rect region = new Rect(canvas.x + canvas.width - 16f, canvas.y, 16f, canvas.height);
            Widgets.ButtonImageFitted(region, state ? HiddenIcon : VisibleIcon);

            bool clicked = Mouse.IsOver(region) && Event.current.type == EventType.Used && Input.GetMouseButtonDown(0);

            if (!clicked)
            {
                return;
            }

            state = !state;
            GUI.FocusControl(null);
        }

        public static bool WasRightClicked(this Rect region)
        {
            if (!Mouse.IsOver(region))
            {
                return false;
            }

            Event current = Event.current;
            bool was = current.button == 1;

            switch (current.type)
            {
                case EventType.Used when was:
                case EventType.MouseDown when was:
                    current.Use();
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsRegionVisible(this Rect region, Rect scrollView, Vector2 scrollPos)
        {
            return (region.y >= scrollPos.y || region.y + region.height - 1f >= scrollPos.y) && region.y <= scrollPos.y + scrollView.height;
        }

        public static void DrawColored(this Texture2D t, Rect region, Color color)
        {
            Color old = GUI.color;

            GUI.color = color;
            GUI.DrawTexture(region, t);
            GUI.color = old;
        }

        public static void DrawLabelAnchored(Rect region, string text, TextAnchor anchor)
        {
            TextAnchor cache = Text.Anchor;

            Text.Anchor = anchor;
            Widgets.Label(region, text);
            Text.Anchor = cache;
        }

        public static void DrawBigLabelAnchored(Rect region, string text, TextAnchor anchor)
        {
            GameFont cache = Text.Font;

            Text.Font = GameFont.Medium;
            DrawLabelAnchored(region, text, anchor);
            Text.Font = cache;
        }

        public static Tuple<Rect, Rect> ToForm(this Rect region, float factor = 0.8f)
        {
            Rect left = new Rect(region.x, region.y, region.width * factor - 2f, region.height);

            return new Tuple<Rect, Rect>(
                left,
                new Rect(left.x + left.width + 2f, left.y, region.width - left.width - 2f, left.height)
            );
        }
    }
}
