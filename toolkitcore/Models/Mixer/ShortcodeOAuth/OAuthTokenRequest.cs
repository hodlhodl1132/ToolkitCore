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
        public OAuthTokenRequest(bool refresh = false, string code = "")
        {
            client_id = ToolkitCoreSettings.mixerClientID;
            if (!refresh)
            {
                grant_type = "authorization_code";
            }
            else
            {
                grant_type = "refresh_token";
                refresh_token = ToolkitCoreSettings.mixerRefreshToken;
            }
                
            this.code = code;
        }

        public string client_id { get; set; }

        public string code { get; set; }

        public string grant_type { get; set; }

        public string refresh_token { get; set; }
    }
}
