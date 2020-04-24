using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Models;
using ToolkitCore.Windows;
using Verse;

namespace ToolkitCore
{
    public class AddonMenu
    {
        public virtual List<FloatMenuOption> MenuOptions { get; set; }

        public AddonMenu()
        {
            MenuOptions = CreateMenuOptions();
        }

        static List<FloatMenuOption> CreateMenuOptions() => new List<FloatMenuOption>
            {
                new FloatMenuOption("Settings", delegate ()
                {
                    Window_ModSettings window = new Window_ModSettings(LoadedModManager.GetMod<ToolkitCore>());
                    Find.WindowStack.TryRemove(window.GetType());
                    Find.WindowStack.Add(window);
                })
            };
    }
}
