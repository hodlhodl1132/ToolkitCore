using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Controllers;
using ToolkitCore.Models;
using ToolkitCore.Windows.Utility;
using UnityEngine;
using Verse;

namespace ToolkitCore.Windows
{
    public class Window_Commands : Window
    {
        public Window_Commands()
        {
            this.doCloseButton = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Rect columnHeaders = new Rect(0f, 0f, inRect.width, 26f);
            DrawHeaderRow(columnHeaders);

            Rect commandsWrapper = new Rect(0f, columnHeaders.height + 5f, inRect.width, 450f);
            DrawRows(commandsWrapper);
        }
        void DrawHeaderRow(Rect columnHeaders)
        {
            Rect headerButton = new Rect(columnHeaders.x, columnHeaders.y, 120f, 30f);

            if (Widgets.ButtonText(headerButton, resetButtonText))
            {
                ToggleResetDefaultWarning();
            }
        }

        Vector2 scrollPosition;

        void DrawRows(Rect commandsWrapper)
        {
            GUI.BeginGroup(commandsWrapper);
            Rect viewRect = new Rect(0f, 0f, commandsWrapper.width - 16f, Text.LineHeight * 40f);

            Widgets.BeginScrollView(commandsWrapper, ref scrollPosition, viewRect);

            GUI.BeginGroup(viewRect);
            float rowHeight = 36f;
            Rect row = new Rect(0, 0f, viewRect.width - 16f, rowHeight);

            int x = 0;
            
            foreach (ToolkitChatCommandWrapper command in ChatCommandController.GetAllChatCommands())
            {
                if (x % 2 == 1) { Widgets.DrawHighlight(row); }
                x += 1;
                GUI.BeginGroup(row);
                Text.Anchor = TextAnchor.MiddleLeft;
                Rect labelText = new Rect(WidgetRow.LabelGap, 0f, 120f, rowHeight);
                Widgets.Label(labelText, command.Def.defName.CapitalizeFirst());
                labelText.x += labelText.width + WidgetRow.DefaultGap;
                if (Widgets.ButtonText(labelText, command.CommandText, false))
                {
                    Dialog_TextEntry textEntry = new Dialog_TextEntry(delegate (string commandText)
                    {
                        command.CommandText = commandText;
                    }, $"Renaming {command.Def.defName.CapitalizeFirst()}");
                    Find.WindowStack.TryRemove(textEntry.GetType());
                    Find.WindowStack.Add(textEntry);
                }
                labelText.x += labelText.width + WidgetRow.DefaultGap;
                Widgets.Label(labelText, command.PermissionRole.ToString());
                if (Widgets.ButtonText(labelText, command.PermissionRole.ToString(), false))
                {
                    CreateRoleContextMenu(command.CommandText);
                }
                bool enabled = command.Enabled;
                Widgets.Checkbox(new Vector2(labelText.x + labelText.width + WidgetRow.DefaultGap, 6f), ref enabled);
                command.Enabled = enabled;
                Text.Anchor = TextAnchor.UpperLeft;
                GUI.EndGroup();
                row.y += rowHeight;
            }
            GUI.EndGroup();

            Widgets.EndScrollView();
            GUI.EndGroup();
        }

        void CreateRoleContextMenu(string commandText)
        {
            List<FloatMenuOption> options = new List<FloatMenuOption>();
            foreach (Role role in Enum.GetValues(typeof(Role)))
            {
                options.Add(new FloatMenuOption(role.ToString(), delegate () 
                {
                    ToolkitChatCommandWrapper command = ChatCommandController.GetChatCommand(commandText);
                    command.PermissionRole = role;
                }));
            }
            
            Find.WindowStack.Add(new FloatMenu(options));
        }

        void ToggleResetDefaultWarning()
        {
            if (!resetWarning)
            {
                resetButtonText = "Are you sure?";
                resetWarning = true;
                return;
            }
            resetButtonText = "Reset All";
            resetWarning = false;
            ChatCommandController.ResetCommandMethodToDefaultSettings();
        }

        public override void Close(bool doCloseSound = true)
        {
            base.Close(doCloseSound);
            Log.Message("Saving CommandMethod Settings");
            ChatCommandController.SaveCommandMethodSettings();
        }

        public override Vector2 InitialSize => new Vector2(600f, 600f);

        bool resetWarning = false;
        string resetButtonText = "Reset All";
    }
}
