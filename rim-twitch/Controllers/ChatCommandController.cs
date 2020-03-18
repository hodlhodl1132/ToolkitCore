using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Models;
using Verse;

namespace ToolkitCore.Controllers
{
    public static class ChatCommandController
    {
        public static ToolkitChatCommand GetChatCommand(string commandText)
        {
            return DefDatabase<ToolkitChatCommand>.AllDefs.ToList().Find(cc => string.Equals(cc.commandText, commandText, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
