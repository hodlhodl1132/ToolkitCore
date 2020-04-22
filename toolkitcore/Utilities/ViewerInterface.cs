using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Controllers;
using ToolkitCore.Models;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using Verse;

namespace ToolkitCore.Utilities
{
    public class ViewerInterface : TwitchInterfaceBase
    {
        public ViewerInterface(Game game)
        {

        }

        public override void ParseMessage(ITwitchCommand twitchCommand)
        {
            if (twitchCommand == null) return;

            Viewer viewer = ViewerController.GetViewer(twitchCommand.Username, true);

            if (viewer != null && twitchCommand.ChatMessage != null)
            {
                viewer.UpdateViewerFromMessage(twitchCommand.ChatMessage);
            }
        }
    }
}
