using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Database;
using ToolkitCore.Models;
using ToolkitCore.Utilities;
using Verse;

namespace ToolkitCore.Controllers
{
    [StaticConstructorOnStartup]
    public static class ChatCommandController
    {
        static List<ToolkitChatCommandWrapper> toolkitChatCommandWrappers = new List<ToolkitChatCommandWrapper>();
        static ChatCommandController()
        {
            LoadCommandMethodSettings();
            LoadNewCommandMethods();
        }
        public static ToolkitChatCommandWrapper GetChatCommand(string commandText)
        {
            string baseCommand = CommandFilter.Parse(commandText).FirstOrDefault();

            if (baseCommand == null)
            {
                return null;
            }

            return toolkitChatCommandWrappers.FirstOrDefault(
                c => c.CommandText.EqualsIgnoreCase(baseCommand)
            );
        }

        public static List<ToolkitChatCommandWrapper> GetAllChatCommands()
        {
            return toolkitChatCommandWrappers;
        }

        public static ToolkitChatCommand GetChatCommandDef(string commandText)
        {
            return DefDatabase<ToolkitChatCommand>.AllDefsListForReading.FirstOrDefault(
                c => c.commandText.EqualsIgnoreCase(commandText)
            );
        }
        
        static void LoadCommandMethodSettings()
        {
            DatabaseController.LoadObject(
                    "CommandMethods",
                    LoadedModManager.GetMod<ToolkitCore>(),
                    out CommandMethodWrapper loadedCommands
                );
            if (loadedCommands == null) return;
            foreach (ToolkitChatCommandWrapper loadedCommand in loadedCommands.Commands)
            {
                ToolkitChatCommand def = DefDatabase<ToolkitChatCommand>.AllDefsListForReading.FirstOrDefault(
                    c => c.defName.EqualsIgnoreCase(loadedCommand.DefName));
                if (def == null)
                {
                    continue;
                }
                loadedCommand.DefName = def.defName;
                loadedCommand.Def = def;
                toolkitChatCommandWrappers.Add(loadedCommand);
            }
        }

        static void LoadNewCommandMethods()
        {
            foreach(ToolkitChatCommand toolkitChatCommand in DefDatabase<ToolkitChatCommand>.AllDefs)
            {
                ToolkitChatCommandWrapper wrapper =
                    toolkitChatCommandWrappers.Find((c) => c.DefName == toolkitChatCommand.defName);
                if (wrapper == null)
                {
                    toolkitChatCommandWrappers.Add(new ToolkitChatCommandWrapper()
                    {
                        Def = toolkitChatCommand,
                        DefName = toolkitChatCommand.defName,
                        CommandText = toolkitChatCommand.commandText,
                        Enabled = toolkitChatCommand.enabled,
                        PermissionRole = toolkitChatCommand.permissionRole
                    });
                }
                else
                {
                    Log.Message(wrapper.CommandText);
                }
            }
        }

        public static void SaveCommandMethodSettings()
        {
            DatabaseController.SaveObject(
                    new CommandMethodWrapper()
                    {
                        Commands = toolkitChatCommandWrappers
                    },
                        "CommandMethods",
                        LoadedModManager.GetMod<ToolkitCore>()
                ); ;
        }

        public static void ResetCommandMethodToDefaultSettings()
        {
            toolkitChatCommandWrappers = new List<ToolkitChatCommandWrapper>();
            foreach (ToolkitChatCommand def in DefDatabase<ToolkitChatCommand>.AllDefsListForReading)
            {
                toolkitChatCommandWrappers.Add(new ToolkitChatCommandWrapper()
                {
                    Def = def,
                    DefName = def.defName,
                    CommandText = def.commandText,
                    Enabled = def.enabled,
                    PermissionRole = def.permissionRole
                });
            }
        }
    }
    
    [Serializable]
    public class CommandMethodWrapper
    {
        public IEnumerable<ToolkitChatCommandWrapper> Commands { get; set; }
    }
}
