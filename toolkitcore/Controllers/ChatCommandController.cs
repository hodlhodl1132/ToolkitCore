using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Models;
using ToolkitCore.Utilities;
using Verse;

namespace ToolkitCore.Controllers
{
    public static class ChatCommandController
    {
        public static ToolkitChatCommand GetChatCommand(string commandText)
        {
            string baseCommand = CommandFilter.Parse(commandText).FirstOrDefault();

            if (baseCommand == null)
            {
                return null;
            }

            return DefDatabase<ToolkitChatCommand>.AllDefsListForReading.FirstOrDefault(
                c => c.commandText.EqualsIgnoreCase(baseCommand)
            );
        }  
    }
}
