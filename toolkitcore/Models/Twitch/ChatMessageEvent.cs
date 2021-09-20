using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Interfaces;
using TwitchLib.Client.Models.Interfaces;
using Verse;
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

        public int UserId()
        {
            if (TwitchMessage.ChatMessage != null)
            {
                if (int.TryParse(TwitchMessage.ChatMessage.UserId, out int userId))
                {
                    return userId;
                }
            }
            else
            {
                if (int.TryParse(TwitchMessage.ChatMessage.UserId, out int userId))
                {
                    return userId;
                }
            }

            Log.Warning($"Failed to find userId for twitch user {Username()}");

            return default;
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

        public bool IsSubscriber()
        {
            if (TwitchMessage.ChatMessage != null)
            {
                return TwitchMessage.ChatMessage.IsSubscriber;
            }
            return false;
        }

        public bool IsVIP()
        {
            if (TwitchMessage.ChatMessage != null)
            {
                return TwitchMessage.ChatMessage.IsVip;
            }
            return false;
        }

        public bool whisper { get; set; }

        public ITwitchMessage TwitchMessage { get; set; }
    }
}
