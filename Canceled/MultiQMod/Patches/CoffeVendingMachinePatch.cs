using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;

namespace MultiQMod
{
    class CoffeVendingMachinePatch
    {
        private static readonly MethodInfo OnHoverOriginal = typeof(CoffeeVendingMachine).GetMethod("OnHover");
        private static readonly MethodInfo OnHoverPatched =
            typeof(CoffeVendingMachinePatch).GetMethod("OnHoverPrefix", BindingFlags.Public | BindingFlags.Static);
        private static readonly MethodInfo OnMachineUseOriginal = typeof(CoffeeVendingMachine).GetMethod("OnMachineUse");
        private static readonly MethodInfo OnMachineUsePatched =
            typeof(CoffeVendingMachinePatch).GetMethod("OnMachineUsePrefix", BindingFlags.Public | BindingFlags.Static);

        private static readonly FieldInfo powerRelayField = typeof(CoffeeVendingMachine).GetField("powerRelay",
            BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly FieldInfo enableElectonicsTimeField = 
            typeof(PowerRelay).GetField("enableElectonicsTime", 
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        internal static void Patch(HarmonyInstance harmony)
        {
            harmony.Patch(OnHoverOriginal,new HarmonyMethod(OnHoverPatched),null);
            harmony.Patch(OnMachineUseOriginal,new HarmonyMethod(OnMachineUsePatched),null);
        }
        public static bool OnHoverPrefix(CoffeeVendingMachine __instance, ref HandTargetEventData eventData)
        {
            HandReticle.main.SetInteractText("Выключить освещение");
            HandReticle.main.SetIcon(HandReticle.IconType.Interact, 1f);
            return false;
        }

        public static bool OnMachineUsePrefix(CoffeeVendingMachine __instance, ref HandTargetEventData eventData)
        {
            if (powerRelayField == null)
            {
                Utils.Log("Can't get powerRelay field from the CoffeVendingMachine");
                Utils.ToScreen("Can't get powerRelay field from the CoffeVendingMachine");
                return true;
            }
            var powerRelay = powerRelayField.GetValue(__instance) as PowerRelay;
            if (powerRelay == null)
            {
                Utils.Log($"powerRelay is null!");
                Utils.ToScreen("powerRelay is null!");
                return true;
            }
            if (enableElectonicsTimeField == null)
            {
                Utils.Log("enableElectonicsTimeField is null!");
                return true;
            }
            if (!powerRelay.IsPowered())
            {
                enableElectonicsTimeField.SetValue(powerRelay, 1f);
            }
            if (!powerRelay.IsPowered())
            {
                return false;
            }
            powerRelay.DisableElectronicsForTime(9999999f);
            return false;
        }
    }
}
