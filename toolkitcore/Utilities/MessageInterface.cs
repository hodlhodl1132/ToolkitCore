using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Controllers;
using ToolkitCore.Interfaces;
using ToolkitCore.Models;
using Verse;
using static ToolkitCore.Models.Services;

namespace ToolkitCore.Utilities
{
    public class MessageInterface : MessageInterfaceBase
    {
        public MessageInterface(Game game)
        {

        }

        public override void ParseMessage(IMessage message)
        {
            if (ViewerController.ViewerExists(message.Service(), message.Username()))
            {
                ViewerController.GetViewer(message.Service(), message.Username()).UpdateViewerFromMessage(message);
            }
            else
            {
                ViewerController.CreateViewer(message.Service(), message.Username(), message.UserId()).UpdateViewerFromMessage(message);
            }

            MessageLogger.LogMessage(message);
        }
    }
}
