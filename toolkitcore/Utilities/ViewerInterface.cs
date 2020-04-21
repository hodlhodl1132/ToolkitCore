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

        public override void ParseMessage(ChatMessage chatMessage)
        {
            if (chatMessage == null) return;

            ViewerController.GetViewer(chatMessage.Username, out bool viewerExists).UpdateViewerFromMessage(chatMessage);
        }

        public override void ParseWhisper(WhisperMessage whisperMessage)
        {
            
        }
    }
}
