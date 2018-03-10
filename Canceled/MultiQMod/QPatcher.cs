using Oculus.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using Harmony;

namespace MultiQMod
{
    public class QPatch
    {
        public static void Patch()
        {
            try
            {
                HarmonyInstance harmony = HarmonyInstance.Create("MultiQMod.mod");
                BaseSpotLightPatch.Patch(harmony);
                CoffeVendingMachinePatch.Patch(harmony);
                DevConsolePatch.Patch(harmony);
                DrillablePatch.Patch(harmony);
                ExosuitPatch.Patch(harmony);
                ExosuitClawArmPatch.Patch(harmony);
                ExosuitDrillArmPatch.Patch(harmony);
                ExosuitGrapplingArmPatch.Patch(harmony);
                GameModeUtilsPatch.Patch(harmony);
                GravspherePatch.Patch(harmony);
                InventoryPatch.Patch(harmony);
                InventoryConsoleCommandsPatch.Patch(harmony);
                //ToDo - Fix get_mapScale patch does nothing. https://github.com/pardeike/Harmony/issues/70
                MapRoomFunctionalityPatch.Patch(harmony);
                PickupableStoragePatch.Patch(harmony);
                PlayerPatch.Patch(harmony);
                SteamEconomyPatch.Patch(harmony);
            }
            catch (Exception e)
            {
                Utils.Log($"Patcher error! {e.GetType()}:\n{e.Message}\n{e.StackTrace}\n{e.InnerException?.GetType()}\n{e.InnerException?.Message}");
            }
        }
    }
}
