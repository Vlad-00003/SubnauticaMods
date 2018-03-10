using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Permissions;

namespace MultiQMod
{
    internal class PlayerPatch
    {
        private static readonly MethodInfo Awake =
            typeof(Player).GetMethod("Awake");

        private static readonly MethodInfo AwakePatch =
            typeof(PlayerPatch).GetMethod("AwakeTranspiler", BindingFlags.Static | BindingFlags.Public);

        private static readonly MethodInfo UnlockItems =
            typeof(SteamEconomy).GetMethod("UnlockItems", BindingFlags.Instance | BindingFlags.NonPublic);

        internal static void Patch(HarmonyInstance harmony)
        {
            if (Awake == null)
            {
                Utils.Log("Unable to patch Player.Awake - Method not found");
            }
            else
            {
                if (UnlockItems == null)
                {
                    Utils.Log("Unable to patch Player.Awake - Method SteamEconomy.UnlockItems not found");
                    return;
                }
                harmony.Patch(Awake,null,null,new HarmonyMethod(AwakePatch));
            }
        }

        public static IEnumerable<CodeInstruction> AwakeTranspiler(MethodBase original,
            IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            var ret = codes[codes.Count - 1];
            codes.Remove(ret);
            codes.Add(new CodeInstruction(OpCodes.Ldarg_0));
            codes.Add(new CodeInstruction(OpCodes.Call, UnlockItems));
            codes.Add(ret);
            return codes.AsEnumerable();
        }
    }
}
