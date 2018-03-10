using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using System.Reflection;
using System.Reflection.Emit;

namespace MultiQMod
{
    internal class InventoryPatch
    {
        private static readonly FieldInfo _container =
            typeof(Inventory).GetField("_container", BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly MethodInfo Awake =
            typeof(Inventory).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly MethodInfo AwakePostfix =
            typeof(InventoryPatch).GetMethod("Postfix", BindingFlags.Public | BindingFlags.Static);
        internal static void Patch(HarmonyInstance harmony)
        {
            if (Awake == null)
            {
                Utils.Log("Unable to patch Inventory.Awake - Method not found!");
            }
            else
            {
                if (_container == null)
                {
                    Utils.Log("Error in the patch Inventory.Awake - can't get the _container field.");
                    return;
                }
                harmony.Patch(Awake,null,new HarmonyMethod(AwakePostfix));
            }
        }

        public static void Postfix(Inventory __instance)
        {
            var container = _container.GetValue(__instance) as ItemsContainer;
            if (container == null)
            {
                Utils.Log("Error in the patch Inventory.Awake - can't get the _container value.");
                return;
            }
            container.Resize(8,10);
        }
    }
}
