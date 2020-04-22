using TwitchLib.Client.Models.Interfaces;
using Verse;

namespace ToolkitCore
{
    public abstract class TwitchInterfaceBase : GameComponent
    {
        public abstract void ParseMessage(ITwitchMessage twitchMessage);
    }
}
