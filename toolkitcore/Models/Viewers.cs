using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Database;
using ToolkitCore.Interfaces;
using ToolkitCore.Models.Mixer;
using ToolkitCore.Models.Twitch;
using Verse;
using static ToolkitCore.Models.Services;

namespace ToolkitCore.Models
{
    [StaticConstructorOnStartup]
    public static class ViewerManager
    {
        static ViewerManager()
        {
            if (DatabaseController.LoadObject<ViewerList>("Viewers", LoadedModManager.GetMod<ToolkitCore>(), out ViewerList viewers))
            {
                ViewerList.Instance = viewers;
            }
            else
            {
                ViewerList.Instance = new ViewerList();
            }
        }

        public static void SaveViewers()
        {
            DatabaseController.SaveObject(ViewerList.Instance, "Viewers", LoadedModManager.GetMod<ToolkitCore>());
        }
    }

    [Serializable]
    public class ViewerList
    {
        public static ViewerList Instance { get; set; }

        public List<Viewer> All { get; set; }
    }

    [Serializable]
    public class Viewer
    {
        public Viewer()
        {
            ViewerList.Instance.All.Add(this);
        }

        public Service Service { get; set; }

        public string Username { get; set; }

        public long LastSeenAt { get; set; }

        public void UpdateLastSeenAt()
        {
            LastSeenAt = DateTime.Now.ToFileTime();
        }

        public DateTime LastSeen()
        {
            return DateTime.FromFileTime(LastSeenAt);
        }

        public void UpdateViewerFromMessage(IMessage message)
        {
            this.Service = message.Service();

            this.Username = message.Username();

            this.UpdateLastSeenAt();
        }
    }
}
