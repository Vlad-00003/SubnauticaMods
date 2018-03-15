using System;
using System.Reflection;
using Harmony;
using JetBrains.Annotations;
using Utilites.Config;
using Utilites.Logger;

namespace WaterFilterOverflow
{
    public class QPatch
    {
        [UsedImplicitly]
        public static void Patch()
        {
            Logger.ClearCustomLog(Assembly.GetExecutingAssembly().GetName().Name);
            try
            {
                //HarmonyInstance harmony = HarmonyInstance.Create("com.waterfilteroverflow.mod");
                //harmony.PatchAll(Assembly.GetExecutingAssembly());
                var config = new ConfigFile(Assembly.GetExecutingAssembly().GetName().Name, "config");
                config.Load();
                string first = "Fifffff";
                config.TryGet(ref first, "Step 1", "Step 2", "Step 3", "First option");
                Logger.Log(first);
                config.Save();
            }
            catch (Exception e)
            {
                e.Log();
            }
        }
    }
}
