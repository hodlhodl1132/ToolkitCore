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
        public ToolkitChatCommand command;

        public CommandMethod(ToolkitChatCommand command)
        {
            this.command = command;
        }

        public virtual bool CanExecute(ITwitchCommand twitchCommand)
        {
            // If command not enabled
            if (!command.enabled) return false;

            // If command requires broadcaster status and message not from broadcaster
            if (command.requiresBroadcaster && twitchCommand.ChatMessage != null && !twitchCommand.ChatMessage.IsBroadcaster) return false;

            // If command requires moderator status and message not from broadcaster or moderator
            if (command.requiresMod && twitchCommand.ChatMessage != null && (!twitchCommand.ChatMessage.IsBroadcaster && !twitchCommand.ChatMessage.IsModerator)) return false;

            return true;
        }

        public virtual void Execute(ITwitchCommand twitchCommand)
        {

        }

        public virtual bool CanExecute(ICommand command)
        {
            return false;
        }

        public virtual void Execute(ICommand command)
        {

        }
    }
}
