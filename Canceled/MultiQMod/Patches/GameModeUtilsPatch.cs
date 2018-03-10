using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using System.Reflection;

namespace MultiQMod
{
    internal class GameModeUtilsPatch
    {
        private static readonly MethodInfo AllowsAchievements =
            typeof(GameModeUtils).GetMethod("AllowsAchievements", BindingFlags.Public | BindingFlags.Static);

        private static readonly MethodInfo AllowsAchievementsPatched =
            typeof(GameModeUtilsPatch).GetMethod("Prefix", BindingFlags.Public | BindingFlags.Static);
        internal static void Patch(HarmonyInstance harmony)
        {
            if (AllowsAchievements == null)
            {
                Utils.Log("Unable to patch GameModeUtils.AllowsAchievements - Method not found!");
            }
            else
            {
                harmony.Patch(AllowsAchievements, new HarmonyMethod(AllowsAchievementsPatched), null);
            }
        }

        public static bool Prefix(ref bool __result)
        {
            __result = true;
            return false;
        }
    }
}
