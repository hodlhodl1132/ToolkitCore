using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Models;

namespace ToolkitCore
{
    public class ToolkitCorePermissions : Models.PermissionsWrapper
    {
        public override List<Permission> Permissions => new List<Permission>();

        public override string Namespace => "toolkitcore";
    }
}
