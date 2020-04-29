using RimWorld;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Models;

namespace ToolkitCore.Utilities
{
    public static class ViewerTracker
    {
        static Dictionary<string, DateTime> viewersLastActiveTime = new Dictionary<string, DateTime>();

        public static void UpdateViewer(string Username)
        {
            if (Username == null)
            {
                throw new ArgumentNullException(Username);
            }

            if (viewersLastActiveTime.ContainsKey(Username))
            {
                viewersLastActiveTime[Username] = DateTime.Now;
            }
            else
            {
                viewersLastActiveTime.Add(Username, DateTime.Now);
            }
        }

        public static int MinutesSinceLastActive(Viewer viewer)
        {
            if (viewer == null || !viewersLastActiveTime.ContainsKey(viewer.Username))
            {
                throw new Exception("Cannot provide Minutes since viewer was last active since viewer has not been tracker.");
            }

            TimeSpan timeSpan = DateTime.Now - viewersLastActiveTime[viewer.Username];

            return timeSpan.Minutes;
        }
    }
}
