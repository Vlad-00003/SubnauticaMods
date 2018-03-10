using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MultiQMod
{
    internal class TemplateQmod
    {
        private static readonly MethodInfo MethodOne =
            typeof(PlaceTool).GetMethod("RandomMethod",BindingFlags.Instance | BindingFlags.Public);

        private static readonly MethodInfo MethodOnePatched =
            typeof(TemplateQmod).GetMethod("Prefix", BindingFlags.Static | BindingFlags.Static);
        internal static void Patch(HarmonyInstance harmony)
        {
            if (MethodOne == null)
            {
                Utils.Log("Unable to patch PlaceTool.RandomMethod() - Method not found.");
            }
            else
            {
                Utils.Log($"Patching \"{MethodOne}\" with \"{MethodOnePatched}\"");
                harmony.Patch(MethodOne,new HarmonyMethod(MethodOnePatched), null);
            }
        }

        public static bool Prefix(TemplateQmod __instance, ref bool __result, string RandomVarThatWontBeChanged)
        {
            //here we can modify result, but not var as it isn't a ref.
            //returning true allow the func body to be executed. False - deny it and takes the result we provided
            return true;
        }

        public static void Postfix(TemplateQmod __instance, bool __result, string RandomVarThatWontBeChanged)
        {
            //Here we can modify nothing, as non of the args(and result) are ref.
        }
        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original,
            IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            return codes.AsEnumerable();
        }
    }
}
