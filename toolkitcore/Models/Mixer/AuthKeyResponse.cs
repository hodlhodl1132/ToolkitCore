using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolkitCore.Models.Mixer
{
    [Serializable]
    public class AuthKeyResponse
    {
        #pragma warning disable IDE1006 // Naming Styles
        public string authkey { get; set; }

        public List<string> endpoints { get; set; }

        public List<string> permissions { get; set; }
        #pragma warning restore IDE1006 // Naming Styles
    }
}
