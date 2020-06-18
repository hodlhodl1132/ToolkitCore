using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ToolkitCore.Database;
using UnityEngine;
using Verse;

namespace ToolkitCore
{
    public class ToolkitCore : Mod
    {
        public static ToolkitCoreSettings settings;

        public ToolkitCore(ModContentPack content) : base(content)
        {
            settings = GetSettings<ToolkitCoreSettings>();
        }
    }

    public class ToolkitData : Mod
    {
        public static GlobalDatabase globalDatabase;

        public ToolkitData(ModContentPack content) : base(content)
        {
            globalDatabase = GetSettings<GlobalDatabase>();
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
