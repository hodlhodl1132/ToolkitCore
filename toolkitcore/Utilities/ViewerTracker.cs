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

        public static void UpdateViewer(Viewer viewer)
        {
            if (viewer == null)
            {
                throw new ArgumentNullException("viewer is null");
            }

            if (viewersLastActiveTime.ContainsKey(viewer.UserId))
            {
                viewersLastActiveTime[viewer.Username] = DateTime.Now;
            }
            else
            {
                viewersLastActiveTime.Add(viewer.UserId, DateTime.Now);
            }
        }

        public static int MinutesSinceLastActive(Viewer viewer)
        {
            if (viewer == null || !viewersLastActiveTime.ContainsKey(viewer.UserId))
            {
                throw new Exception("Cannot provide Minutes since viewer was last active since viewer has not been tracker.");
            }

            TimeSpan timeSpan = DateTime.Now - viewersLastActiveTime[viewer.UserId];

            return timeSpan.Minutes;
        }

        public static bool ViewerIsBeingTracker(Viewer viewer)
        {
            return viewersLastActiveTime.ContainsKey(viewer.UserId);
        }
    }
}
