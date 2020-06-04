using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Interfaces;
using ToolkitCore.Models;
using ToolkitCore.Utilities;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace ToolkitCore.CommandMethods
{
    public class HelloWorld : CommandMethod
    {
        public HelloWorld(ToolkitChatCommand command) : base(command)
        {
            
        }

        public override bool CanExecute(ICommand command)
        {
            return base.CanExecute(command);
        }

        public override void Execute(ICommand command)
        {
            MessageSender.SendMessage("Hello World!", command.Service());
        }
    }
}
