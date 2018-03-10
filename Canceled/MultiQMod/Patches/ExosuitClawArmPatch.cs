using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using System.Reflection;
using System.Reflection.Emit;

namespace MultiQMod
{
    internal class ExosuitClawArmPatch
    {
        internal static void Patch(HarmonyInstance harmony)
        {
            //hatmony.patch(originalmethod,prefix,postfix,il);
            //private const BindingFlags flags =
            //BindingFlags.Public |
            //BindingFlags.NonPublic |
            //BindingFlags.Static |
            //BindingFlags.Instance |
            //BindingFlags.DeclaredOnly;
            var OnHitOriginal = typeof(ExosuitClawArm).GetMethod("OnHit", BindingFlags.Public | BindingFlags.Instance);
            var OnHitTranspiler = typeof(ExosuitClawArmPatch).GetMethod("Transpiler",
                BindingFlags.Public | BindingFlags.Static);
            harmony.Patch(OnHitOriginal,null,null,new HarmonyMethod(OnHitTranspiler));
        }

        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original,
            IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            int found = -1;
            for (int i = 0; i < codes.Count; i++)
            {
                if (i + 4 >= codes.Count)
                    break;
                if (codes[i].opcode == OpCodes.Ldloc_S &&
                    codes[i+1].opcode == OpCodes.Ldc_R4 && (float)codes[i+1].operand == 50f &&
                    codes[i + 2].opcode == OpCodes.Ldloc_1 &&
                    codes[i + 3].opcode == OpCodes.Ldc_I4_0 &&
                    codes[i+4].opcode == OpCodes.Ldnull)
                {
                    found = i + 1;
                    break;
                }
            }
            if (found == -1)
            {
                Utils.Log("Unable to patch IL in ExosuitClawArm.OnHit!");
            }
            else
            {
                codes[found].operand = 100f;
            }
            return codes.AsEnumerable();
        }
    }
}
