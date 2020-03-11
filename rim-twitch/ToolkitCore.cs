using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ToolkitCore
{
    public class ToolkitCore : Mod
    {
        ToolkitCoreSettings settings;

        public ToolkitCore(ModContentPack content) : base(content)
        {
            settings = GetSettings<ToolkitCoreSettings>();
        }

        public override string SettingsCategory() => "ToolkitCore";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            settings.DoWindowContents(inRect);
        }
    }

    [StaticConstructorOnStartup]
    public static class Startup
    {
        static Startup()
        {
            ThreadWorker.StartThread();
        }
    }
}
