using System.Collections.Generic;

namespace ToolkitCore.Interfaces
{
    public interface ICommand : IMessage
    {
        string Command();

        List<string> Parameters();
    }
}
