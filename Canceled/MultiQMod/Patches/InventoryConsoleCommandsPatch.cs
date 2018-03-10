using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using System.Reflection;

namespace MultiQMod
{
    internal class InventoryConsoleCommandsPatch
    {
        private static readonly MethodInfo Awake =
            typeof(InventoryConsoleCommands).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly MethodInfo AwakePatch =
            typeof(InventoryConsoleCommandsPatch).GetMethod("Postfix", BindingFlags.Static | BindingFlags.Public);
        internal static void Patch(HarmonyInstance harmony)
        {
            if (Awake == null)
            {
                Utils.Log("Unable to patch InventoryConsoleCommands.Awake() - Method not found!");
                return;
            }
            harmony.Patch(Awake,null,new HarmonyMethod(AwakePatch));
        }

        public static void Postfix(InventoryConsoleCommands __instance)
        {
            DevConsole.RegisterConsoleCommand(__instance, "resizestorage");;
        }
    }
}
