using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace rim_twitch
{
    public class RimTwitch : Mod
    {
        RimTwitchSettings settings;

        public RimTwitch(ModContentPack content) : base(content)
        {
            settings = GetSettings<RimTwitchSettings>();
        }

        public override string SettingsCategory() => "RimTwitch";

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
            if (RimTwitchSettings.connectOnGameStartup)
                ThreadWorker.StartThread();
        }
    }
}
