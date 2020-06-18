using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ToolkitCore.Database;
using ToolkitCore.Windows;
using UnityEngine;
using Verse;

namespace ToolkitCore
{
    public class ToolkitCore : Mod
    {
        public static ToolkitCoreSettings settings;

        public ToolkitCore(ModContentPack content) : base(content)
        {
            GetSettings<ToolkitCoreSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            // Ugly hack to make settings open custom window instead of repeatably calling this method
            Find.WindowStack.TryRemove(typeof(Dialog_ModSettings));
            Find.WindowStack.TryRemove(typeof(Dialog_Options));
            Window_Services window = new Window_Services();
            Find.WindowStack.TryRemove(window.GetType());
            Find.WindowStack.Add(window);
            Find.WindowStack.TryRemove(typeof(Window_ModSettings));
        }

        public override string SettingsCategory()
        {
            return "ToolkitCore";
        }
    }

    [StaticConstructorOnStartup]
    public static class Startup
    {
        static Startup()
        {
            if (ToolkitCoreSettings.twitchConnectOnStartup)
            {
                TwitchWrapper.StartAsync();
            }

            if (ToolkitCoreSettings.mixerConnectOnStartup)
            {
                MixerWrapper.InitializeClient();
            }
        }
    }
}
