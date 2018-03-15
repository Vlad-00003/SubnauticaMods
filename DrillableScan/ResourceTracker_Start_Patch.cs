using Harmony;
using System.Linq;

namespace DrillableScan
{
    [HarmonyPatch(typeof(ResourceTracker), "Start")]
    class ResourceTracker_Start_Patch
    {
        static bool Prefix(ResourceTracker __instance)
        {
            if (Enumerable.Range(70, 15).Contains((int)CraftData.GetTechType(__instance.gameObject)))
            {
                __instance.overrideTechType = TechType.None;
            }
            return true;
        }
    }
}
