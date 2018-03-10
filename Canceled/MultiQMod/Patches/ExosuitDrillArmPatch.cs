using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using System.Reflection;
using System.Reflection.Emit;

namespace MultiQMod
{
    internal class ExosuitDrillArmPatch
    {
        private static readonly MethodInfo OnHitPatch =
            typeof(ExosuitDrillArmPatch).GetMethod("Transpiler", BindingFlags.Public | BindingFlags.Static);

        private static readonly MethodInfo OnHit =
            typeof(ExosuitDrillArm).GetMethod("OnHit");
        internal static void Patch(HarmonyInstance harmony)
        {
            harmony.Patch(OnHit,null,null,new HarmonyMethod(OnHitPatch));
        }
        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original,
            IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            int found = -1;
            for (int i = 0; i < codes.Count; i++)
            {
                if (i + 5 >= codes.Count)
                    break;
                var t = (codes[i + 3].operand as sbyte?);
                var f = (codes[i + 1].operand as float?);
                if (codes[i].opcode == OpCodes.Ldloc_S &&
                    codes[i + 1].opcode == OpCodes.Ldc_R4 && f.HasValue && f.Value == 4f &&
                    codes[i + 2].opcode == OpCodes.Ldloc_1 &&
                    codes[i + 3].opcode == OpCodes.Ldc_I4_S && t.HasValue && t.Value == 9 &&
                    codes[i + 4].opcode == OpCodes.Ldnull &&
                    codes[i + 5].opcode == OpCodes.Callvirt &&
                    (codes[i + 5].operand as MethodInfo)?.Name == "TakeDamage"
                )
                {
                    found = i + 1;
                    break;
                }
            }

            if (found == -1)
            {
                Utils.Log("Unable to patch IL in ExosuitDrillArm.OnHit!");
            }
            else
            {
                codes[found].operand = 16f;
            }
            return codes.AsEnumerable();
        }
    }
}
