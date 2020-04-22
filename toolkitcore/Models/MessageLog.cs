using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Models;

namespace ToolkitCore.Models
{
    public static class MessageLog
    {
        static readonly int chatMessageQueueLength = 10;
        static readonly int whisperMessageQueueLength = 5;

        public static List<ChatMessage> LastChatMessages { get; } = new List<ChatMessage>(chatMessageQueueLength);
        public static List<WhisperMessage> LastWhisperMessages { get; } = new List<WhisperMessage>(whisperMessageQueueLength);

        public static void LogMessage(ChatMessage chatMessage)
        {
            if (LastChatMessages.Count >= chatMessageQueueLength - 1)
            {
                LastChatMessages.RemoveAt(0);
            }

            LastChatMessages.Add(chatMessage);
        }

        public static void LogMessage(WhisperMessage whisperMessage)
        {
            if (LastWhisperMessages.Count >= whisperMessageQueueLength - 1)
            {
                LastWhisperMessages.RemoveAt(0);
            }

            LastWhisperMessages.Add(whisperMessage);
        }
    }
}
