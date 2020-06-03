using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolkitCore.Models.Mixer
{
    public class MsgMethod : ChatMethod
    {
        public MsgMethod(string message)
        {
            this.id = 2;
            this.arguments = new List<object>()
            {
                message
            };
            this.method = "msg";
        }
    }
}
