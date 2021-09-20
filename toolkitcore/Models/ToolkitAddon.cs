using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Interfaces;
using Verse;

namespace ToolkitCore.Models
{
    public class ToolkitAddon : Def
    {
        public ToolkitAddon()
        {

        }

        public Type menuClass = typeof(IAddonMenu);

        public Type permissionsClass = typeof(PermissionsWrapper);

        public IAddonMenu GetAddonMenu()
        {
            return Activator.CreateInstance(menuClass) as IAddonMenu;
        }

        public PermissionsWrapper GetPermissionsWrapper()
        {
            return Activator.CreateInstance(permissionsClass) as PermissionsWrapper;
        }
    }
}
