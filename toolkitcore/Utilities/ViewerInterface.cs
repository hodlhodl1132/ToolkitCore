using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Controllers;
using ToolkitCore.Models;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using TwitchLib.Client.Models.Interfaces;
using Verse;

namespace ToolkitCore.Utilities
{
    public class ViewerInterface : TwitchInterfaceBase
    {
        public ViewerInterface(Game game)
        {

        }

        public override void ParseMessage(ITwitchMessage twitchCommand)
        {
            if (twitchCommand == null) return;

            Viewer viewer;

            if (ViewerController.ViewerExists(twitchCommand.Username))
            {
                viewer = ViewerController.GetViewer(twitchCommand.Username);
            }
            else
            {
                viewer = ViewerController.CreateViewer(twitchCommand.Username);
            }

            if (viewer != null && twitchCommand.ChatMessage != null)
            {
                viewer.UpdateViewerFromMessage(twitchCommand.ChatMessage);
            }
        }
    }
}
