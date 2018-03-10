using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using System.Reflection;

namespace MultiQMod
{
    internal class SteamEconomyPatch
    {
        private static readonly MethodInfo UnlockItems =
            typeof(SteamEconomy).GetMethod("UnlockItems", BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly MethodInfo UnlockItemsPatch =
            typeof(SteamEconomyPatch).GetMethod("Prefix", BindingFlags.Static | BindingFlags.Public);
        internal static void Patch(HarmonyInstance harmony)
        {
            if (UnlockItems == null)
            {
                Utils.Log("Unable to patch SteamEconomy.UnclockItems - Method not found!");
                return;
            }
            harmony.Patch(UnlockItems,new HarmonyMethod(UnlockItemsPatch), null);
        }

        public static bool Prefix(SteamEconomy __instance)
        {
            KnownTech.Add(TechType.SpecialHullPlate, false);
            KnownTech.Add(TechType.DevTestItem, false);
            KnownTech.Add(TechType.BikemanHullPlate, false);
            KnownTech.Add(TechType.EatMyDictionHullPlate, false);
            KnownTech.Add(TechType.DioramaHullPlate, false);
            KnownTech.Add(TechType.MarkiplierHullPlate, false);
            KnownTech.Add(TechType.MuyskermHullPlate, false);
            KnownTech.Add(TechType.LordMinionHullPlate, false);
            KnownTech.Add(TechType.JackSepticEyeHullPlate, false);
            KnownTech.Add(TechType.IGPHullPlate, false);
            KnownTech.Add(TechType.GilathissHullPlate, false);
            KnownTech.Add(TechType.Marki1, false);
            KnownTech.Add(TechType.Marki2, false);
            KnownTech.Add(TechType.JackSepticEye, false);
            return false;
        }
    }
}
