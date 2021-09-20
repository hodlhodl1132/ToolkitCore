using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Database;
using ToolkitCore.Interfaces;
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
            if (DatabaseController.LoadObject("Viewers", LoadedModManager.GetMod<ToolkitCore>(), out ViewerList viewers))
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
            if (ViewerList.Instance != null)
            {
                ViewerList.Instance.All.Add(this);
            }
        }

        public Service Service { get; set; }

        public string Username { get; set; }

        public long LastSeenAt { get; set; }

        public int UserId { get; set; }

        public List<string> Permissions { get; set; }

        public bool HasPermission(string permission)
        {
            return Permissions.Contains(permission);
        }

        public void AddPermission(string permission)
        {
            Permissions.Add(permission);
        }

        public void RemovePermission(string permission)
        {
            Permissions.Remove(permission);
        }

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
