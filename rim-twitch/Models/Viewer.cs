using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Controllers;
using ToolkitCore.Database;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Models;
using UnityEngine;
using Verse;

namespace ToolkitCore.Models
{
    public static class Viewers
    {
        public static List<Viewer> All
        {
            get
            {
                return ToolkitData.globalDatabase.viewers;
            }
        }
    }

    public class Viewer : IExposable
    {
        public string Username;

        public string DisplayName;

        public string UserId;

        public bool IsBroadcaster;

        public bool IsBot;

        public bool IsModerator;

        public bool IsSubscriber;

        public List<KeyValuePair<string, string>> Badges;

        public UserType UserType;

        public Viewer()
        {

        }

        public Viewer(string username)
        {
            this.Username = username;
        }

        public Viewer(string Username = null, string DisplayName = null, string UserId = null, bool IsBroadcaster = false, bool IsBot = false, bool IsModerator = false, bool IsSubscriber = false)
        {
            this.Username = Username;
            this.DisplayName = DisplayName;
            this.UserId = UserId;
            this.IsBroadcaster = IsBroadcaster;
            this.IsBot = IsBot;
            this.IsModerator = IsModerator;
            this.IsSubscriber = IsSubscriber;
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref Username, "Username");
            Scribe_Values.Look(ref DisplayName, "DisplayName");
            Scribe_Values.Look(ref UserId, "UserId");
            Scribe_Values.Look(ref IsBroadcaster, "IsBroadcaster");
            Scribe_Values.Look(ref IsBot, "IsBot");
            Scribe_Values.Look(ref IsModerator, "IsModerator");
            Scribe_Values.Look(ref IsSubscriber, "IsSubscriber");
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this, true);
        }

        public void UpdateViewerFromMessage(ChatMessage chatMessage)
        {
            this.DisplayName = chatMessage.DisplayName;
            this.UserId = chatMessage.UserId;
            this.IsBroadcaster = chatMessage.IsBroadcaster;
            this.IsBot = chatMessage.IsMe;
            this.IsModerator = chatMessage.IsModerator;
            this.IsSubscriber = chatMessage.IsSubscriber;
            this.Badges = chatMessage.Badges;
            this.UserType = chatMessage.UserType;

            CheckIfNewViewer();
        }

        

        void CheckIfNewViewer()
        {
            lock (Viewers.All)
            {
                lock (Viewers.All)
                {
                    if (!Viewers.All.Contains(this)) Viewers.All.Add(this);
                }
            }
        }
    }
}
