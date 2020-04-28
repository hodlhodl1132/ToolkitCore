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
        public static void CreateViewer(string Username)
        {
            if (ViewerExists(Username))
            {
                throw new Exception("Viewer already exists");
            }

            Viewers.All.Add(new Viewer(Username));
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
