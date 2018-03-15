using Harmony;
using UnityEngine;

namespace LargeDepositsFix
{
    [HarmonyPatch(typeof(Drillable), "DestroySelf")]
    class Drillable_DestroySelf_Patch
    {
        static bool Prefix(Drillable __instance)
        {
            __instance.SendMessage("OnBreakResource",null, SendMessageOptions.DontRequireReceiver);
            return true;
        }
    }
}
