using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Models;
using TwitchLib.Client.Models;

namespace ToolkitCore.CommandMethods
{
    public class HelloWorld : CommandMethod
    {
        public HelloWorld(ToolkitChatCommand command) : base(command)
        {
            
        }

        public override bool CanExecute(ChatMessage message)
        {
            if (!base.CanExecute(message)) return false;

            if (true) return true;
        }

        public override void Execute(ChatCommand chatCommand)
        {
            MessageQueue.messageQueue.Enqueue("Hello World!");
        }
    }
}
