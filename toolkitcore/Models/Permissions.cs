using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Utilities;
using Verse;

namespace ToolkitCore.Models
{
    [StaticConstructorOnStartup]
    public static class PermissionController
    {
        public static PermissionsWrapper Permissions;

        public static Dictionary<string, Permission> allPermissions;

        static PermissionController()
        {
            populatePermissions();
            debugAllPermissions();
        }

        static void populatePermissions()
        {
            allPermissions = new Dictionary<string, Permission>();

            foreach (ToolkitAddon addon in AddonRegistry.ToolkitAddons)
            {
                PermissionsWrapper wrapper = addon.GetPermissionsWrapper();
                
                foreach (Permission permission in wrapper.Permissions)
                {
                    allPermissions.Add($"{wrapper.Namespace}.{permission.Identifier}", permission);
                }
            }
        }

        static void debugAllPermissions()
        {
            if (!ToolkitCore.DEBUG) return;

            foreach (KeyValuePair<string, Permission> keyValuePair in allPermissions)
            {
                Log.Message($"Registered Permission: {keyValuePair.Key} - {keyValuePair.Value.Role}");
            }
        }
    }

    public abstract class PermissionsWrapper
    {
        public abstract List<Permission> Permissions { get; }

        public abstract string Namespace { get; }
    }

    public class Permission
    {
        public string Identifier;

        public Role Role;

        public Permission(string identifier, Role role)
        {
            this.Identifier = identifier;
            this.Role = role;
        }
    }

    public enum Role
    {
        Broadcaster = 1,
        Mod = 2,
        VIP = 3,
        Subscriber = 4,
        Viewer = 5
    }
}
