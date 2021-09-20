using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Interfaces;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using Verse;

namespace ToolkitCore.Models
{
    public class CommandMethod
    {
        public ToolkitChatCommand ToolkitChatCommand;

        public CommandMethod(ToolkitChatCommand command)
        {
            this.ToolkitChatCommand = command;
        }

        public virtual bool CanExecute(ICommand command)
        {
            // If command not enabled
            if (!ToolkitChatCommand.enabled) return false;
            Log.Message("Command enabled");

            return true;
        }

        public virtual void Execute(ICommand command)
        {

        }
    }
}
