using System.Collections.Generic;
using System.Linq;
using ToolkitCore.Interfaces;
using Verse;

namespace ToolkitCore.Models.Twitch
{
    public class ChatCommandEvent : ChatMessageEvent, ICommand
    {
        public string Command()
        {
            return Parameters().First().Replace("!", "");
        }

        public List<string> Parameters()
        {
            return TwitchMessage.ChatMessage.Message.Split(' ').ToList();
        }
    }
}
