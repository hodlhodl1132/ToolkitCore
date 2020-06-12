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
    public class CommandInterface : CommandInterfaceBase
    {
        public CommandInterface(Game game)
        {

        }

        public override void ParseCommand(ICommand command)
        {
            Log.Message($"{command.Username()}: {command.Message()} - Command: {command.Command()}");

            ToolkitChatCommand toolkitChatCommand = ChatCommandController.GetChatCommand(command.Command());

            if (toolkitChatCommand != null)
            {
                toolkitChatCommand.TryExecute(command);
            }

            if (ViewerController.ViewerExists(command.Service(), command.Username()))
            {
                ViewerController.GetViewer(command.Service(), command.Username()).UpdateViewerFromMessage(command);
            }
            else
            {
                ViewerController.CreateViewer(command.Service(), command.Username(), command.UserId()).UpdateViewerFromMessage(command);
            }

            MessageLogger.LogCommand(command);
        }
    }
}
