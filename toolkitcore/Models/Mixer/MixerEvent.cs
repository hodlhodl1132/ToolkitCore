using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolkitCore.Models.Mixer
{
    [Serializable]
    public class MixerEvent
    {
        public string type { get; set; }

        public string Event { get; set; }
    }
}
