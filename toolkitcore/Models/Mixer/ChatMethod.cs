using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolkitCore.Models.Mixer
{
    public class ChatMethod
    {
        public string type { get; set; } = "method";

        public string method { get; set; }

        public virtual List<object> arguments { get; set; }

        public int id { get; set; }
    }
}
