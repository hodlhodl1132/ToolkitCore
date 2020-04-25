using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Interfaces;
using ToolkitCore.Models;
using ToolkitCore.Windows;
using UnityEngine;
using Verse;

namespace ToolkitCore
{
    public class AddonMenu : IAddonMenu
    {
        List<FloatMenuOption> IAddonMenu.MenuOptions() => new List<FloatMenuOption>
        {
            new FloatMenuOption("Settings", delegate ()
            {
                Window_ModSettings window = new Window_ModSettings(LoadedModManager.GetMod<ToolkitCore>());
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }),
            new FloatMenuOption("Message Log", delegate()
            {
                Window_MessageLog window = new Window_MessageLog();
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }),
            new FloatMenuOption("Help", delegate()
            {
                Application.OpenURL("https://github.com/hodldeeznuts/ToolkitCore/wiki");
            })
        };
    }
}
