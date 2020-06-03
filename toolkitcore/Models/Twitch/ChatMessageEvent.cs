using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Interfaces;
using TwitchLib.Client.Models.Interfaces;
using static ToolkitCore.Models.Services;

namespace ToolkitCore.Models.Twitch
{
    public class ChatMessageEvent : IMessage
    {
        public string Message()
        {
            return TwitchMessage.Message;
        }

        public Service Service()
        {
            return Services.Service.Twitch;
        }

        public string Username()
        {
            return TwitchMessage.Username;
        }

        public bool Whisper()
        {
            return whisper;
        }

        public bool whisper { get; set; }

        public ITwitchMessage TwitchMessage { get; set; }
    }
}
