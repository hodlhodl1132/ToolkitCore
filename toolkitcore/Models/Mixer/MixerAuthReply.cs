using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolkitCore.Models.Mixer
{
    [Serializable]
    public class MixerAuthReply : MixerReply
    {
        public MixerAuthReplyData data { get; set; }
    }

    [Serializable]
    public class MixerAuthReplyData
    {
        public bool authenticated { get; set; }

        public List<string> roles { get; set; }
    }
}
