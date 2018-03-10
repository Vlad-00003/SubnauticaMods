using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using System.Reflection;

namespace MultiQMod
{
    internal class PickupableStoragePatch
    {
        private static readonly MethodInfo OnHandClick =
            typeof(PickupableStorage).GetMethod("OnHandClick", BindingFlags.Public | BindingFlags.Instance);
        private static readonly MethodInfo OnHandHover =
            typeof(PickupableStorage).GetMethod("OnHandHover", BindingFlags.Public | BindingFlags.Instance);

        private static readonly MethodInfo OnHandClickPatched =
            typeof(PickupableStoragePatch).GetMethod("ClickPrefix", BindingFlags.Static | BindingFlags.Public);
        private static readonly MethodInfo OnHandHoverPatched =
            typeof(PickupableStoragePatch).GetMethod("HoverPrefix", BindingFlags.Static | BindingFlags.Public);
        internal static void Patch(HarmonyInstance harmony)
        {
            if (OnHandClick == null)
                Utils.Log("Unable to patch PickupableStorage.OnHandClick() - Method not found!");
            else
                harmony.Patch(OnHandClick, new HarmonyMethod(OnHandClickPatched), null);
            if (OnHandHover == null)
                Utils.Log("Unable to patch PickupableStorage.OnHandHover() - Method not found!");
            else
                harmony.Patch(OnHandHover, new HarmonyMethod(OnHandHoverPatched), null);
        }

        public static bool ClickPrefix(PickupableStorage __instance, GUIHand hand)
        {
            __instance.pickupable.OnHandClick(hand);
            return false;
        }
        public static bool HoverPrefix(PickupableStorage __instance, GUIHand hand)
        {
            __instance.pickupable.OnHandHover(hand);
            return false;
        }
    }
}
