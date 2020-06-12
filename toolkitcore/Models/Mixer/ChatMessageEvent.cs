using System;
using System.Collections.Generic;
using System.Linq;
using ToolkitCore.Interfaces;
using static ToolkitCore.Models.Services;

namespace ToolkitCore.Models.Mixer
{
    [Serializable]
    public class ChatMessageEvent : MixerEvent, IMessage
    {
        public MixerMessageData data { get; set; }

        public string Message()
        {
            return data.message.message.First().text;
        }

        public Service Service()
        {
            return Services.Service.Mixer;
        }

        public string Username()
        {
            return data.user_name;
        }

        public int UserId()
        {
            return data.user_id;
        }

        public bool Whisper()
        {
            return false;
        }

        public bool IsModerator()
        {
            return data.user_roles.Contains("Mod");
        }

        public bool IsBroadcaster()
        {
            return data.user_roles.Contains("Owner");
        }
    }

    [Serializable]
    public class MixerMessageData
    {
        public int channel { get; set; }

        public string id { get; set; }

        public string user_name { get; set; }

        public int user_id { get; set; }

        public List<string> user_roles { get; set; }

        public int user_level { get; set; }

        public string user_avatar { get; set; }

        public MixerMessage message { get; set; }
    }

    [Serializable]
    public class MixerMessage
    {
        public List<RawMixerMessage> message { get; set; }
    }

    [Serializable]
    public class RawMixerMessage
    {
        public string type { get; set; }
        
        public string data { get; set; }

        public string text { get; set; }
    }
}
