using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Models;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace ToolkitCore.CommandMethods
{
    public class HelloWorld : CommandMethod
    {
        public HelloWorld(ToolkitChatCommand command) : base(command)
        {
            
        }

        public override bool CanExecute(ITwitchCommand twitchCommand)
        {
            if (!base.CanExecute(twitchCommand)) return false;

            return true;
        }

        public override void Execute(ITwitchCommand twitchCommand)
        {
            TwitchWrapper.SendChatMessage("Hello World!");
        }
    }
}
