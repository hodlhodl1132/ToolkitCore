using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Models;
using Verse;

namespace ToolkitCore.Controllers
{
    public static class ViewerController
    {
        public static Viewer GetViewer(string Username, bool createIfNotExists = false)
        {
            Viewer viewer = Viewers.All.Find(vwr => vwr.Username == Username);

            if (viewer == null && createIfNotExists)
            {
                viewer = new Viewer(Username);
            }

            return viewer;
        }

        public static bool ViewerExists(string Username)
        {
            return Viewers.All.Find((x) => x.Username == Username) != null;
        }
    }
}
