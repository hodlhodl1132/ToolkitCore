using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolkitCore
{
    public static class MessageQueue
    {
        public static ConcurrentQueue<string> messageQueue = new ConcurrentQueue<string>();
    }
}
