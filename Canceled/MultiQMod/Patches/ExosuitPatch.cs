using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using System.Reflection;

namespace MultiQMod
{
    internal class ExosuitPatch
    {
        private static readonly FieldInfo crushDepths = typeof(Exosuit).GetField("crushDepths", 
            BindingFlags.Instance | BindingFlags .NonPublic | BindingFlags.Static);

        private static readonly FieldInfo thrustConsumption = 
            typeof(Exosuit).GetField("thrustConsumption", BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly MethodInfo UpdateStorageSize =
            typeof(Exosuit).GetMethod("UpdateStorageSize", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly ConstructorInfo Ctor = typeof(Exosuit).GetConstructor(new Type[0]);

        private static readonly MethodInfo UpdateStorageSizePatched =
            typeof(ExosuitPatch).GetMethod("UpdateStoragePrefix", BindingFlags.Public | BindingFlags.Static);

        private static readonly MethodInfo CtorPatched =
            typeof(ExosuitPatch).GetMethod("CtorPostfix", BindingFlags.Public | BindingFlags.Static);
        internal static void Patch(HarmonyInstance harmony)
        {
            if (UpdateStorageSize == null)
            {
                Utils.Log($"Unable to patch Exosuit.UpdateStorageSize - Method not found!");
            }
            else
            {
                harmony.Patch(UpdateStorageSize, new HarmonyMethod(UpdateStorageSizePatched), null);
            }
            if (Ctor == null)
            {
                Utils.Log("Unable to patch Exosuit.Ctor - Method not found!");
            }
            else
            {
                if (thrustConsumption == null)
                {
                    Utils.Log("Unable to patch Exosuit.ctor - Field \"thrustConsumption\" not found!");
                }
                else
                {
                    harmony.Patch(Ctor, null, new HarmonyMethod(CtorPatched));
                }
            }
            if (crushDepths == null)
            {
                Utils.Log("Unable to change Exosuit.crushDepths - filed not found!");
            }
            else
            {
                if (((Dictionary<TechType, float>) crushDepths.GetValue(null)).Count != 6)
                {
                    Utils.Log("Unable to patch Exosuit.crushDepths - dictionary changed!");
                }
                else
                {
                    crushDepths.SetValue(null,new Dictionary<TechType, float>()
                    {
                        [TechType.SeamothReinforcementModule]=1600f,
                        [TechType.VehicleHullModule1]=150f,
                        [TechType.VehicleHullModule2]=400f,
                        [TechType.VehicleHullModule3]=800f,
                        [TechType.ExoHullModule1]=400f,
                        [TechType.ExoHullModule2]=800f
                    });
                }
            }
        }

        public static bool UpdateStoragePrefix(Exosuit __instance)
        {
            int height = 10;
            int width = 8;
            __instance.storageContainer.Resize(width,height);
            return false;
        }

        public static void CtorPostfix(Exosuit __instance)
        {
            thrustConsumption.SetValue(__instance,0.05f);
            //Here we can modify nothing, as non of the args(and result) are ref.
        }
    }
}
