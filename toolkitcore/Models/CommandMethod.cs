using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Models;
using Verse;

namespace ToolkitCore.Models
{
    public class CommandMethod
    {
        public ToolkitChatCommand command;

        public CommandMethod(ToolkitChatCommand command)
        {
            this.command = command;
        }

        public virtual bool CanExecute(ChatCommand chatCommand)
        {
            var message = chatCommand.ChatMessage;

            // If command not enabled
            if (!command.enabled) return false;

            // If command requires broadcaster status and message not from broadcaster
            if (command.requiresBroadcaster && !message.IsBroadcaster) return false;

            // If command requires moderator status and message not from broadcaster or moderator
            if (command.requiresMod && (!message.IsBroadcaster || !message.IsModerator)) return false;

            return true;
        }

        public virtual void Execute(ChatCommand chatCommand)
        {

        }
    }
}
