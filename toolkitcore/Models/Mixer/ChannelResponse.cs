using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolkitCore.Models.Mixer
{
    [Serializable]
    public class ChannelResponse
    {
        #pragma warning disable IDE1006 // Naming Styles
        public int id { get; set; }

        public int userId { get; set; }
        #pragma warning restore IDE1006 // Naming Styles
    }
}
