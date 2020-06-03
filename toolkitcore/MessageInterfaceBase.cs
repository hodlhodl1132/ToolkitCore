using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Interfaces;
using Verse;

namespace ToolkitCore
{
    public abstract class MessageInterfaceBase : GameComponent
    {
        public abstract void ParseMessage(IMessage message);
    }
}
