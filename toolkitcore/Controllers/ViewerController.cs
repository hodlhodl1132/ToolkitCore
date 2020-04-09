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
        public static Viewer GetViewer(string Username, out bool viewerExists, bool softCheck = false)
        {
            // If doing soft check, do not create a new viewer if no viewer is found

            Viewer viewer = Viewers.All.Find(vwr => vwr.Username == Username);

            viewerExists = viewer != null;

            if (!viewerExists && softCheck)
            {
                return null;
            }

            return viewer ?? new Viewer(Username);
        }
    }
}
