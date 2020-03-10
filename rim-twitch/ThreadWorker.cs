using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwitchLib.Client.Models;
using Verse;

namespace rim_twitch
{
    public static class ThreadWorker
    {
        public static bool runThread = true;
        public static bool stayConnected = true;
        public static bool sendDebugMSG = false;
        public static bool complete;

        public static void StartThread()
        {
            if (!runThread) return;

            Thread clientThread = new Thread((wrapper) => new TwitchWrapper().Initialize(new ConnectionCredentials(RimTwitchSettings.channel_username, RimTwitchSettings.oauth_token)))
            {
                Name = "Client BG Worker",
                IsBackground = true
            };

            clientThread.Start();
        }
    }
}
