using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Models;
using Verse;

namespace ToolkitCore.Models
{
    public class ToolkitChatCommand : Def, IExposable
    {
        public string commandText;

        public bool enabled;

        public Type commandClass;

        public bool requiresMod;

        public bool requiresBroadcaster;

        public bool TryExecute(ChatCommand chatCommand)
        {
            try
            {
                CommandMethod method = (CommandMethod)Activator.CreateInstance(commandClass, this);

                if (!method.CanExecute(chatCommand.ChatMessage)) return false;

                method.Execute(chatCommand);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }

            return true;
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref commandText, "commandText", "helloworld");
            Scribe_Values.Look(ref enabled, "enabled", true);
            Scribe_Values.Look(ref commandClass, "commandClass", typeof(CommandMethod));
            Scribe_Values.Look(ref requiresMod, "requiresMod", false);
            Scribe_Values.Look(ref requiresBroadcaster, "requiresBroadcaster", false);
        }
    }
}
