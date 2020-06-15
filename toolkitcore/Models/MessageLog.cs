using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Controllers;
using ToolkitCore.Interfaces;
using ToolkitCore.Utilities;
using TwitchLib.Client.Models;
using Verse;

namespace ToolkitCore.Models
{
    public static class MessageLogger
    {
        static readonly int LogLength = 20;

        public static List<IMessage> MessageLog { get; set; } = new List<IMessage>();

        public static void LogMessage(IMessage message)
        {
            if (MessageLog.Count + 1 > LogLength)
            {
                MessageLog.RemoveAt(0);
            }

            MessageLog.Add(message);
        }
    }
}
