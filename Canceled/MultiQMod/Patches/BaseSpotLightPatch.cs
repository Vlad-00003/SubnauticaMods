using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Harmony;

namespace MultiQMod
{
    internal class BaseSpotLightPatch
    {
        private static readonly FieldInfo powerPerSecond = 
            typeof(BaseSpotLight).GetField("powerPerSecond", BindingFlags.NonPublic | BindingFlags.Static);
        private static readonly FieldInfo updateInterval = 
            typeof(BaseSpotLight).GetField("updateInterval", BindingFlags.NonPublic | BindingFlags.Static);
        internal static void Patch(HarmonyInstance harmony)
        {
            //as long as these are the static fields that never change it's values, and they are in the static methods -
            //there is no need for the "patch" after all...
            if(powerPerSecond == null)
            {
                Utils.Log("BaseSpotLight.powerPerSecond is null!");
            }
            if(updateInterval == null)
            {
                Utils.Log("BaseSpotLight.updateInterval is null!");
            }
            powerPerSecond?.SetValue(null, 0.05f);
            updateInterval?.SetValue(null, 20f);
        }
    }
}
