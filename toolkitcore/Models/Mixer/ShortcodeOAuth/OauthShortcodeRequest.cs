using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolkitCore.Models.Mixer.ShortcodeOAuth
{
    [Serializable]
    public class OAuthShortcodeRequest
    {
        public OAuthShortcodeRequest()
        {
            client_id = ToolkitCoreSettings.mixerClientID;
            scope = "chat:chat chat:connect chat:whisper";
        }

        public string client_id { get; set; }

        public string scope { get; set; }
    }
}
