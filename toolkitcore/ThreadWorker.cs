using System.Collections.Generic;
using System.Threading;
using TwitchLib.Client.Models;
using Verse;

namespace ToolkitCore
{
    public static class ThreadWorker
    {
        public static bool runThread = true;
        public static bool stayConnected = false;
        public static bool sendDebugMSG = false;
        public static bool complete;

        static List<Thread> threads = new List<Thread>();

        public static void StartThread()
        {
            if (!runThread) return;

            if (threads.Count > 0)
            {
                foreach (Thread thread in threads)
                {
                    thread.Abort();
                    Log.Warning("Old Thread from ToolkitCore Aborted - If this message appears multiple times at once alert mod author");
                    if (!thread.IsAlive)
                    {
                        threads = threads.FindAll((s) => s != thread);
                    }
                }
            }

            if (ToolkitCoreSettings.connectOnGameStartup) stayConnected = true;

            Thread clientThread = new Thread((wrapper) => new TwitchWrapper().Initialize(new ConnectionCredentials(ToolkitCoreSettings.bot_username, ToolkitCoreSettings.oauth_token)))
            {
                Name = "Client BG Worker",
                IsBackground = true
            };

            threads.Add(clientThread);

            clientThread.Start();

            if (threads.Count > 1)
            {
                Log.Error("Multiple threads, report to mod author and restart game if any issues arise.");
            }
        }

    }
}
