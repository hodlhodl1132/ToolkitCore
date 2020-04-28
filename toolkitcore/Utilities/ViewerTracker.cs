using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
