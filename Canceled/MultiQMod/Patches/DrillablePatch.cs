using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;

namespace MultiQMod
{
    internal class DrillablePatch
    {
        private static readonly ConstructorInfo ConstructorOriginal = typeof(Drillable).GetConstructor(new Type[0]);
        private static readonly MethodInfo ConstructorPatched =
            typeof(DrillablePatch).GetMethod("CtorPostfix", BindingFlags.Public | BindingFlags.Static);
        private static readonly FieldInfo minResourcesToSpawn = 
            typeof(Drillable).GetField("minResourcesToSpawn", BindingFlags.Public | BindingFlags.Instance);
        private static readonly FieldInfo maxResourcesToSpawn = 
            typeof(Drillable).GetField("maxResourcesToSpawn", BindingFlags.Public | BindingFlags.Instance);
        private static readonly FieldInfo kChanceToSpawnResources = 
            typeof(Drillable).GetField("kChanceToSpawnResources", BindingFlags.Public | BindingFlags.Instance);
        internal static void Patch(HarmonyInstance harmony)
        {
            harmony.Patch(ConstructorOriginal,null,new HarmonyMethod(ConstructorPatched));
        }

        public static void CtorPostfix(Drillable __instance)
        {
            if (minResourcesToSpawn == null)
                Utils.Log("Unable to resolve minResourcesToSpawn.");
            if (maxResourcesToSpawn == null)
                Utils.Log("Unable to resolve maxResourcesToSpawn.");
            if (kChanceToSpawnResources == null)
                Utils.Log("Unable to resolve kChanceToSpawnResources.");

            minResourcesToSpawn?.SetValue(__instance, 2);
            maxResourcesToSpawn?.SetValue(__instance, 5);
            kChanceToSpawnResources?.SetValue(__instance, 2.0f);
        }
    }
}
