using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using System.Reflection;
using System.Reflection.Emit;

namespace MultiQMod
{
    internal class GravspherePatch
    {
        private static readonly MethodInfo OnTriggerEnter =
            typeof(Gravsphere).GetMethod("OnTriggerEnter", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly MethodInfo OnTriggerEnterPatched =
            typeof(GravspherePatch).GetMethod("OnTriggerEnterTranspiler", BindingFlags.Public | BindingFlags.Static);

        private static readonly MethodInfo ApplyGravitation =
            typeof(Gravsphere).GetMethod("ApplyGravitation", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly MethodInfo ApplyGravitationPatched =
            typeof(GravspherePatch).GetMethod("ApplyGravitationTranspiler", BindingFlags.Public | BindingFlags.Static);
        internal static void Patch(HarmonyInstance harmony)
        {
            if (OnTriggerEnter == null)
            {
                Utils.Log("Unable to patch Gravsphere.OnTriggerEnter - Method not found");
            }
            else
            {
                harmony.Patch(OnTriggerEnter,null,null,new HarmonyMethod(OnTriggerEnterPatched));
            }
            if (ApplyGravitation == null)
            {
                Utils.Log("Unable to patch Gravsphere.ApplyGravitation - Method not found");
            }
            else
            {
                harmony.Patch(ApplyGravitation,null,null,new HarmonyMethod(ApplyGravitationPatched));
            }
        }

        public static IEnumerable<CodeInstruction> OnTriggerEnterTranspiler(MethodBase original,
            IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            int found = -1;
            for (int i = 0; i < codes.Count; i++)
            {
                if (i + 4 >= codes.Count)
                    break;
                sbyte? f = (codes[i + 3].operand as sbyte?);
                if (codes[i].opcode == OpCodes.Ldarg_0 &&
                    codes[i + 1].opcode == OpCodes.Ldfld &&
                    (codes[i + 1].operand as FieldInfo)?.Name == "attractableList" &&
                    codes[i + 2].opcode == OpCodes.Callvirt &&
                    (codes[i + 2].operand as MethodInfo)?.Name == "get_Count" &&
                    codes[i + 3].opcode == OpCodes.Ldc_I4_S &&
                    f.HasValue && f.Value == 12 &&
                    codes[i + 4].opcode == OpCodes.Bge

                )
                {
                    found = i + 3;
                    break;
                }
            }
            if (found == -1)
            {
                Utils.Log("Unable to patch Gravsphere.OnTriggerEnter - IL code not found");
            }
            else
            {
                codes[found].operand = 24;
            }
            return codes.AsEnumerable();
        }

        public static IEnumerable<CodeInstruction> ApplyGravitationTranspiler(MethodBase original,
            IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            int found = -1;
            for (int i = 0; i < codes.Count; i++)
            {
                if (i + 4 >= codes.Count)
                    break;
                var a = (codes[i+2].operand as float?);
                var b = (codes[i+3].operand as float?);
                if (codes[i].opcode == OpCodes.Call &&
                    (codes[i].operand as MethodInfo)?.Name == "Pow" &&
                    codes[i+1].opcode == OpCodes.Mul &&
                    codes[i+2].opcode == OpCodes.Ldc_R4 &&
                    a.HasValue && a.Value == 0.7f &&
                    codes[i+3].opcode == OpCodes.Ldc_R4 &&
                    b.HasValue && b.Value == 15f &&
                    codes[i+4].opcode == OpCodes.Call &&
                    (codes[i+4].operand as MethodInfo)?.Name == "Clamp"
                )
                {
                    found = i + 3;
                    break;
                }
            }
            if (found == -1)
            {
                Utils.Log("Unable to patch Gravsphere.ApplyGravitation - IL not found");
            }
            else
            {
                codes[found].operand = 30f;
            }
            //Utils.Log($"[50]: {codes[50]}\n{codes[50].opcode == OpCodes.Call} \"{(codes[50].operand as MethodInfo)?.Name}\"");
            //Utils.Log($"[51]: {codes[51]}\n{codes[51].opcode == OpCodes.Mul}");
            //Utils.Log($"[52]: {codes[52]}\n{codes[52].opcode == OpCodes.Ldc_R4} {a.HasValue} | {resa}");
            //Utils.Log($"[53]: {codes[53]}\n{codes[53].opcode == OpCodes.Ldc_R4} {b.HasValue} | {resb}");
            //Utils.Log($"[54]: {codes[54]}\n{codes[54].opcode == OpCodes.Call} \"{(codes[54].operand as MethodInfo)?.Name}\"");
            return codes.AsEnumerable();
        }
    }
}
