using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ToolkitCore.Windows
{
    public class Window_ModSettings : Window
    {
        public Window_ModSettings(Mod mod)
        {
            this.Mod = mod;
            this.doCloseButton = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Mod.DoSettingsWindowContents(inRect);
        }

        public override Vector2 InitialSize => new Vector2(900f, 700f);

        public override void PostClose()
        {
            Mod.WriteSettings();
        }

        public Mod Mod { get; }
    }
}
