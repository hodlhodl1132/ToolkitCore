using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolkitCore.Utilities
{
    public static class ActiveViewers
    {
        static List<string> identifiers = new List<string>();
        static Dictionary<string, DateTime> lastActiveAt = new Dictionary<string, DateTime>();

        public static void TryAddViewer(string Username)
        {
            if (!identifiers.Contains(Username))
            {
                identifiers.Add(Username);
            }

            lastActiveAt[Username] = DateTime.Now;
        }

        public static void TryRemoveViewer(string Username)
        {
            if (identifiers.Contains(Username))
            {
                identifiers.Remove(Username);
            }
        }
    }
}
