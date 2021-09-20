using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Models;
using Verse;
using static ToolkitCore.Models.Services;

namespace ToolkitCore.Utilities
{
    public static class MessageSender
    {
        public static bool SendMessage(string message, Service service, bool whisper = false)
        {
            switch (service)
            {
                case Service.Twitch:

                    if (!TwitchWrapper.Client.IsConnected)
                    {
                        Log.Error("Tried to send message to twitch but not connected to twitch. Message: " + message);
                        return false;
                    }

                    TwitchWrapper.SendChatMessage(message);

                    return true;
            }

            Log.Error("Tried to send message but failed. Message: " + message);

            return false;
        }
    }
}
