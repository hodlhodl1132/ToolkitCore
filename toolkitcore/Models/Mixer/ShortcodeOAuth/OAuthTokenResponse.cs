using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolkitCore.Models.Mixer.ShortcodeOAuth
{
    [Serializable]
    public class OAuthTokenResponse
    {
        public string access_token { get; set; }

        public int expire_in { get; set; }

        public string refresh_token { get; set; }

        public string token_type { get; set; }
    }
}
