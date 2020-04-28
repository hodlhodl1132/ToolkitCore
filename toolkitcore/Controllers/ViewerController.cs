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
        public static Viewer CreateViewer(string Username)
        {
            if (ViewerExists(Username))
            {
                throw new Exception("Viewer already exists");
            }

            Viewer viewer = new Viewer(Username);

            Viewers.All.Add(viewer);

            return viewer;
        }

        public static Viewer GetViewer(string Username)
        {
            Viewer viewer = Viewers.All.Find(vwr => vwr.Username == Username);

            return viewer;
        }

        public static bool ViewerExists(string Username)
        {
            return Viewers.All.Find((x) => x.Username == Username) != null;
        }
    }
}
