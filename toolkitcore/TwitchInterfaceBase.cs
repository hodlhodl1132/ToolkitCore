using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using Verse;

namespace ToolkitCore
{
    public abstract class TwitchInterfaceBase : GameComponent
    {
        public abstract void ParseMessage(ITwitchCommand twitchCommand);
    }
}
