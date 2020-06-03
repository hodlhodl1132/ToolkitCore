using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Interfaces;
using Verse;

namespace ToolkitCore.Utilities
{
    public class CommandInterface : CommandInterfaceBase
    {
        public CommandInterface(Game game)
        {

        }

        public override void ParseCommand(ICommand command)
        {
            Log.Message($"{command.Username()}: {command.Message()} - Command: {command.Command()}");
        }
    }
}
