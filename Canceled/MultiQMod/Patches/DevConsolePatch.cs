using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Xml.Serialization;
using Harmony;

namespace MultiQMod
{
    internal class DevConsolePatch
    {
        private static readonly MethodInfo HasUsedConsoleOriginal = 
            typeof(DevConsole).GetMethod("HasUsedConsole",
            BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        private static readonly MethodInfo HasUsedConsolePatched = 
            typeof(DevConsolePatch).GetMethod("HasUsedConsolePrefix",
            BindingFlags.Public | BindingFlags.Static);
        internal static void Patch(HarmonyInstance harmony)
        {
            if (HasUsedConsoleOriginal == null)
            {
                Utils.Log("Unable to patch DevConsole.HasUsedConsoleOriginal - Method not found!");
            }
            else
            {
                harmony.Patch(HasUsedConsoleOriginal, new HarmonyMethod(HasUsedConsolePatched), null);
            }
        }

        public static bool HasUsedConsolePrefix(ref bool __result)
        {
            __result = false;
            return false;
        }
    }
}
