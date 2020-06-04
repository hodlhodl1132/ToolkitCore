using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Database;
using ToolkitCore.Models.Mixer;
using ToolkitCore.Models.Twitch;
using Verse;
using static ToolkitCore.Models.Services;

namespace ToolkitCore.Models
{
    [StaticConstructorOnStartup]
    public static class AllViewers
    {
        static AllViewers()
        {
            if (DatabaseController.LoadObject<NewViewers>("Viewers", LoadedModManager.GetMod<ToolkitCore>(), out NewViewers viewers))
            {
                All = viewers;
            }
            else
            {
                All = new NewViewers();
            }
        }

        public static void SaveViewers()
        {
            DatabaseController.SaveObject(All, "Viewers", LoadedModManager.GetMod<ToolkitCore>());
        }

        public static NewViewers All { get; }
    }

    [Serializable]
    public class NewViewers
    {
        public List<TwitchViewer> TwitchViewers { get; set; }

        public List<MixerViewer> MixerViewers { get; set; }

        public TwitchViewer GetTwitchViewer(string username)
        {
            return TwitchViewers.Find((viewer) => viewer.Username.ToLower() == username.ToLower());
        }

        public MixerViewer GetMixerViewer(string username)
        {
            return MixerViewers.Find((viewer) => viewer.Username.ToLower() == username.ToLower());
        }
    }

    [Serializable]
    public class NewViewer
    {
        public Service Service { get; set; }

        public string Username { get; set; }

        public long LastSeenAt { get; set; }

        public void UpdateLastSeenAt()
        {
            LastSeenAt = DateTime.Now.ToFileTime();
        }
    }
}
