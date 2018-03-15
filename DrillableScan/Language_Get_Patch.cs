using Harmony;
using System;

namespace DrillableScan
{
    [HarmonyPatch(typeof(Language), "Get")]
    class Language_Get_Patch
    {
        static bool Prefix(Language __instance, ref string __result, string key)
        {
            if (key.IndexOf("drillable", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                if (__instance.TryGet(key, out __result))
                {
                    __result += " (Drillable)";
                    return false;
                }
            }
            return true;
        }
    }
}
