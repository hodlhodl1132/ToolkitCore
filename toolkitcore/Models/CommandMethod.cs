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

        //public virtual bool CanExecute(ITwitchCommand twitchCommand)
        //{
        //    // If command not enabled
        //    if (!ToolkitChatCommand.enabled) return false;

        //    // If command requires broadcaster status and message not from broadcaster
        //    if (ToolkitChatCommand.requiresBroadcaster && twitchCommand.ChatMessage != null && !twitchCommand.ChatMessage.IsBroadcaster) return false;

        //    // If command requires moderator status and message not from broadcaster or moderator
        //    if (ToolkitChatCommand.requiresMod && twitchCommand.ChatMessage != null && (!twitchCommand.ChatMessage.IsBroadcaster && !twitchCommand.ChatMessage.IsModerator)) return false;

        //    return true;
        //}

        //public virtual void Execute(ITwitchCommand twitchCommand)
        //{

        //}

        public virtual bool CanExecute(ICommand command)
        {
            // If command not enabled
            if (!ToolkitChatCommand.enabled) return false;
            Log.Message("Command enabled");

            // If command requires broadcaster status and message not from broadcaster
            if (ToolkitChatCommand.requiresBroadcaster && !command.IsBroadcaster()) return false;
            Log.Message("Broadcaster checked");

            // If command requires moderator status and message not from broadcaster or moderator
            if (ToolkitChatCommand.requiresMod  && (!command.IsBroadcaster() && !command.IsModerator())) return false;
            Log.Message("Mod Checked");

            return true;
        }

        public virtual void Execute(ICommand command)
        {

        }
    }
}
