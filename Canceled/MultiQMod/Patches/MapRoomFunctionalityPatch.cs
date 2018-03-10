using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using System.Reflection;

namespace MultiQMod
{
    internal class MapRoomFunctionalityPatch
    {
        private static readonly MethodInfo GetMapScale =
            typeof(MapRoomFunctionality).GetMethod("get_mapScale", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly MethodInfo GetScanRange =
            typeof(MapRoomFunctionality).GetMethod("GetScanRange", BindingFlags.Public | BindingFlags.Instance);

        private static readonly MethodInfo GetScanInterval =
            typeof(MapRoomFunctionality).GetMethod("GetScanInterval", BindingFlags.Public | BindingFlags.Instance);

        private static readonly MethodInfo GetMapScalePatched =
            typeof(MapRoomFunctionalityPatch).GetMethod("MapScalePostfix", BindingFlags.Static | BindingFlags.Public);

        private static readonly MethodInfo GetScanRangePatched =
            typeof(MapRoomFunctionalityPatch).GetMethod("ScanRangePrefix", BindingFlags.Static | BindingFlags.Public);

        private static readonly MethodInfo GetScanIntervalPatched =
            typeof(MapRoomFunctionalityPatch).GetMethod("ScanIntervalPrefix", BindingFlags.Static | BindingFlags.Public);
        
        internal static void Patch(HarmonyInstance harmony)
        {
            if(GetMapScale == null)
                Utils.Log("Unable to patch MapRoomFunctionality.get_mapScale - Method not found!");
            else
                harmony.Patch(GetMapScale,null, new HarmonyMethod(GetMapScalePatched));
            if (GetScanRange == null)
                Utils.Log("Unable to patch MapRoomFunctionality.GetScanRange() - Method not found!");
            else
                harmony.Patch(GetScanRange, new HarmonyMethod(GetScanRangePatched), null);
            if (GetScanInterval == null)
                Utils.Log("Unable to patch MapRoomFunctionality.GetScanInterval() - Method not found!");
            else
                harmony.Patch(GetScanInterval, new HarmonyMethod(GetScanIntervalPatched), null);
        }

        public static void MapScalePostfix(MapRoomFunctionality __instance, ref float __result)
        {
            __result = __instance.hologramRadius / 1000f;
            Utils.Log($"get_MapScale: {__instance}\n{__instance.hologramRadius} / {1000f} = {__result}");
        }
        public static bool ScanRangePrefix(MapRoomFunctionality __instance, ref float __result)
        {
            __result = 1000f;
            return false;
        }
        public static bool ScanIntervalPrefix(MapRoomFunctionality __instance, ref float __result)
        {
            __result = 1f;
            return false;
        }
    }
}
