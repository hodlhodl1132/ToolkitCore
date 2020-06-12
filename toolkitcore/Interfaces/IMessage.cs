using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ToolkitCore.Models.Services;

namespace ToolkitCore.Interfaces
{
    public interface IMessage
    {
        string Message();

        string Username();

        Service Service();

        int UserId();

        bool Whisper();

        bool IsBroadcaster();

        bool IsModerator();
    }
}
