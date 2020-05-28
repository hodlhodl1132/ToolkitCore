using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ToolkitCore.HarmonyPatches
{
    [StaticConstructorOnStartup]
    static class SavePatch
    {
        static SavePatch()
        {
            Harmony harmony = new Harmony("com.rimworld.mod.hodlhodl.toolkit.core");

            Harmony.DEBUG = true;

            harmony.Patch(
                    original: AccessTools.Method(
                            type: typeof(GameDataSaveLoader),
                            name: "SaveGame"),
                    postfix: new HarmonyMethod(typeof(SavePatch), nameof(SaveGame_PostFix))
                );

            harmony.Patch(
                original: AccessTools.Method(
                        type: typeof(GameDataSaveLoader),
                        name: "LoadGame",
                        new Type[] { typeof(string) }),
                postfix: new HarmonyMethod(typeof(SavePatch), nameof(LoadGame_PostFix))
            );
        }

        static void SaveGame_PostFix()
        {
            Database.DatabaseController.SaveToolkit();
            ToolkitData.globalDatabase.Write();
        }

        static void LoadGame_PostFix()
        {
            Log.Message("Running ToolkitCore loadgame_postfix");
            Database.DatabaseController.LoadToolkit();
        }
    }
}
