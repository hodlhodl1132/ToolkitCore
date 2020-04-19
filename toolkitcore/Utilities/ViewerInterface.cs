using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Controllers;
using ToolkitCore.Models;
using TwitchLib.Client.Models;
using Verse;

namespace ToolkitCore.Utilities
{
    public class ViewerInterface : TwitchInterfaceBase
    {
        public ViewerInterface(Game game)
        {

        }

        public override void ParseCommand(ChatMessage msg)
        {
            ViewerController.GetViewer(msg.Username, out bool viewerExists).UpdateViewerFromMessage(msg);
        }
    }
}
