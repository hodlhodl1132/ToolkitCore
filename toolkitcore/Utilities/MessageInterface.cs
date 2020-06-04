using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Interfaces;
using ToolkitCore.Models;
using Verse;
using static ToolkitCore.Models.Services;

namespace ToolkitCore.Utilities
{
    public class MessageInterface : MessageInterfaceBase
    {
        public MessageInterface(Game game)
        {

        }

        public override void ParseMessage(IMessage message)
        {
            Log.Message($"{message.Username()}: {message.Message()}");
        }
    }
}
