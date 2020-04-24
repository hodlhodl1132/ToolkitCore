using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Models;
using Verse;

namespace ToolkitCore.Utilities
{
    [StaticConstructorOnStartup]
    public static class AddonRegistry
    {
        public static List<ToolkitAddon> ToolkitAddons { get; set; }

        static AddonRegistry()
        {
            ToolkitAddons = DefDatabase<ToolkitAddon>.AllDefs.ToList();
        }
    }
}
