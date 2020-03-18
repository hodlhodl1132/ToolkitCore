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
        public static Viewer GetViewer(string Username)
        {
            return Viewers.All.Find(vwr => vwr.Username == Username) ?? new Viewer(Username);
        }
    }
}
