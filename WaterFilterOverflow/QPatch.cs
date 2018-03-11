using System;
using System.Collections.Generic;
using System.Reflection;
using Harmony;
using JetBrains.Annotations;
using Utilites;
using Utilites.Config;

namespace WaterFilterOverflow
{
    public class QPatch
    {
        [UsedImplicitly]
        public static void Patch()
        {
            Logger.ClearCustomLog(Assembly.GetExecutingAssembly().GetName().Name);
            FileLog.Reset();
            Logger.Debug("Starting....");
            try
            {
                //HarmonyInstance harmony = HarmonyInstance.Create("com.waterfilteroverflow.mod");
                //harmony.PatchAll(Assembly.GetExecutingAssembly());
                var config =
                    new ConfigFile(Assembly.GetExecutingAssembly().GetName().Name, "config");
                config["Option one"]= "Value one";
                config["Option two"] = 12;
                config["Third"] = new List<string>() { "cool", "Awsome!" };
                config["Fourth"] = 0.7f;
                config["Fith"] = new Dictionary<string,int>()
                {
                    ["First key"] = 42
                };
                Logger.Info(config.Get<string>("Option one"));
                Logger.Error(config.Get<string>("Option two"));
                Logger.Warning(string.Join("\n",config.Get<List<string>>("Third").ToArray()));
                Logger.Debug(config.Get<float>("Fourth").ToString("P"));
                var dict = config.Get<Dictionary<string, int>>("Fith");
                Logger.Log("Logging dictionary from the config");
                foreach (var i in dict)
                {
                    Logger.Log($"Key: {i.Key} | Value {i.Value}");
                }
                config.Save();
            }
            catch (Exception e)
            {
                e.Log(LogType.Harmony);
            }
        }
    }
}
