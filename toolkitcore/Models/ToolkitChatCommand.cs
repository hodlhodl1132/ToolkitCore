using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Interfaces;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using Verse;

namespace ToolkitCore.Models
{
    [Serializable]
    public class ToolkitChatCommand : Def
    {
        public string commandText;

        public bool enabled;

        public Type commandClass;

        public Role permissionRole;

        public bool TryExecute(ICommand command)
        {
            try
            {
                CommandMethod method = (CommandMethod)Activator.CreateInstance(commandClass, this);

                if (!method.CanExecute(command)) return false;

                method.Execute(command);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }

            return true;
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ToolkitChatCommandWrapper
    {
        [JsonProperty]
        public string CommandText { get; set; }
        [JsonProperty]
        public bool Enabled { get; set; }
        [JsonProperty]
        public Role PermissionRole { get; set; }
        [JsonProperty]
        public string DefName { get; set; }
        public ToolkitChatCommand Def { get; set; }

        public bool TryExecute(ICommand command)
        {
            return this.Def.TryExecute(command);
        }
    }
}
