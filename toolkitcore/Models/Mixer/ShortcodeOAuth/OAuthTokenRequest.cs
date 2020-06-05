using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolkitCore.Models.Mixer.ShortcodeOAuth
{
    [Serializable]
    public class OAuthTokenRequest
    {
        public OAuthTokenRequest(string code)
        {
            client_id = ToolkitCoreSettings.mixerClientID;
            grant_type = "authorization_code";
            this.code = code;
        }

        public string client_id { get; set; }

        public string code { get; set; }

        public string grant_type { get; set; }
    }
}
