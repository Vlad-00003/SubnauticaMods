using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using System.Reflection;
using System.Reflection.Emit;

namespace MultiQMod
{
    internal class ExosuitGrapplingArmPatch
    {
        private static readonly MethodInfo OnUseDown =
            typeof(ExosuitGrapplingArm).GetMethod("IExosuitArm.OnUseDown",
                BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly MethodInfo OnUseDownPatch =
                typeof(ExosuitGrapplingArmPatch).GetMethod("OnUseDownTranspiler",
                    BindingFlags.Static | BindingFlags.Public);

        private static readonly MethodInfo FixedUpdate =
            typeof(ExosuitGrapplingArm).GetMethod("FixedUpdate");

        private static readonly MethodInfo FixedUpdatePatch =
            typeof(ExosuitGrapplingArmPatch).GetMethod("FixedUpdateTranspiler",
                BindingFlags.Static | BindingFlags.Public);
        internal static void Patch(HarmonyInstance harmony)
        {
            if (OnUseDown == null)
            {
                Utils.Log("Unable to patch ExosuitGrapplingArm.OnUseDown - Method not found");
            }
            else
            {
                harmony.Patch(OnUseDown, null, null, new HarmonyMethod(OnUseDownPatch));
            }
            if (FixedUpdate == null)
            {
                Utils.Log("Unable to patch ExosuitGrapplingArm.FixedUpdate - Method not found");
            }
            else
            {
                harmony.Patch(FixedUpdate, null, null, new HarmonyMethod(FixedUpdatePatch));
            }
        }
        public static IEnumerable<CodeInstruction> OnUseDownTranspiler(MethodBase original,
            IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            int found = -1;
            for (int i = 0; i < codes.Count; i++)
            {
                if (i + 3 >= codes.Count)
                    break;
                var f = (codes[i + 2].operand as float?);
                if (codes[i].opcode == OpCodes.Ldarg_0 &&
                    codes[i + 1].opcode == OpCodes.Ldfld &&
                    codes[i + 2].opcode == OpCodes.Ldc_R4 &&
                    f.HasValue && f.Value == 35f &&
                    codes[i + 3].opcode == OpCodes.Callvirt &&
                    (codes[i+3].operand as MethodInfo)?.Name == "LaunchHook")
                {
                    found = i + 2;
                    break;
                }
            }
            if (found == -1)
            {
                Utils.Log("Unable to patch ExosuitGrapplingArm.IExosuitArm.OnUseDown - IL not found!");
            }
            else
            {
                codes[found].operand = 70f;
            }
            return codes.AsEnumerable();
        }
        public static IEnumerable<CodeInstruction> FixedUpdateTranspiler(MethodBase original,
            IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            int found = -1;
            int found2 = -1;
            for (int i = 0; i < codes.Count; i++)
            {
                if (i + 4 >= codes.Count)
                    break;
                var f = (codes[i + 2].operand as float?);
                if (codes[i].opcode == OpCodes.Callvirt &&
                    (codes[i].operand as MethodInfo)?.Name == "GetComponent" &&
                    codes[i + 1].opcode == OpCodes.Ldloc_2 &&
                    codes[i + 2].opcode == OpCodes.Ldc_R4 &&
                    f.HasValue && f.Value == 15f &&
                    codes[i + 3].opcode == OpCodes.Call &&
                    (codes[i + 3].operand as MethodInfo)?.Name == "op_Multiply" &&
                    codes[i + 4].opcode == OpCodes.Ldc_I4_5 &&
                    codes[i+5].opcode == OpCodes.Callvirt &&
                    (codes[i+5].operand as MethodInfo)?.Name == "AddForce"
                )
                {
                    found = i + 2;
                }
                var f1 = (codes[84].operand as float?);
                if (
                    codes[i].opcode == OpCodes.Call &&
                    (codes[i].operand as MethodInfo)?.Name == "get_magnitude" &&
                    codes[i + 1].opcode == OpCodes.Ldc_R4 &&
                    f1.HasValue && f1.Value == 35f &&
                    codes[i + 2].opcode == OpCodes.Ble_Un &&
                    codes[i + 2].operand is Label &&
                    codes[i + 3].opcode == OpCodes.Ldarg_0 &&
                    codes[i + 4].opcode == OpCodes.Call &&
                    (codes[i + 4].operand as MethodInfo)?.Name == "ResetHook"
                )
                {
                    found2 = i + 1;
                }
            }
            if (found == -1)
            {
                Utils.Log("Failed to patch ExosuitGrapplingArm.FixedUpdate - IL not found!\nLine: \"componentInParent.GetComponent<Rigidbody>().AddForce(a * 15f, ForceMode.Acceleration);\"");
            }
            else
            {
                codes[found].operand = 30f;
            }
            if (found2 == -1)
            {
                Utils.Log(
                    "Failed to patch ExosuitGrapplingArm.FixedUpdate - IL not found!\nLine: \"if ((this.hook.transform.position - this.front.position).magnitude > 35f)\"");
            }
            else
            {
                codes[found2].operand = 70f;
            }
            return codes.AsEnumerable();
        }
    }
}
