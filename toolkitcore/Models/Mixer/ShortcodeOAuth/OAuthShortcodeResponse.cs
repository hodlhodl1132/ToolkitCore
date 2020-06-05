using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolkitCore.Models.Mixer.ShortcodeOAuth
{
    [Serializable]
    public class OAuthShortcodeResponse
    {
        public string code { get; set; }

        public int expires_in { get; set; }

        public string handle { get; set; }
    }
}
