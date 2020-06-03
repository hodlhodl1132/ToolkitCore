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

        public bool Whisper()
        {
            return false;
        }
    }

    [Serializable]
    public class MixerMessageData
    {
        public int channel { get; set; }

        public string id { get; set; }

        public string user_name { get; set; }

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
