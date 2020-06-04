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

        public bool IsModerator()
        {
            if (TwitchMessage.ChatMessage != null)
            {
                return TwitchMessage.ChatMessage.IsModerator;
            }

            return false;
        }

        public bool IsBroadcaster()
        {
            if (TwitchMessage.ChatMessage != null)
            {
                return TwitchMessage.ChatMessage.IsBroadcaster;
            }

            return false;
        }

        public bool whisper { get; set; }

        public ITwitchMessage TwitchMessage { get; set; }
    }
}
