using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Interfaces;
using Verse;

namespace ToolkitCore.Models.Mixer
{
    [Serializable]
    public class ChatCommandEvent : ChatMessageEvent, ICommand
    {
        public string Command()
        {
            return Parameters().First().Replace("!", "");
        }

        public List<string> Parameters()
        {
            return Message().Split(' ').ToList();
        }
    }
}
