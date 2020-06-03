using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolkitCore.Models.Mixer
{
    [Serializable]
    public class AuthMethod : ChatMethod
    {
        public AuthMethod(int channelId, int userId, string authKey)
        {
            this.method = "auth";
            this.id = 0;

            this.arguments = new List<object>()
            {
                channelId,
                userId,
                authKey
            };
        }
    }
}
