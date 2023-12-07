using BepInEx;
using HarmonyLib;
using System;
using System.Reflection;

namespace DrillableScan
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class MyPlugin : BaseUnityPlugin
    {
        public void Awake()
        {
            try
            {
                var harmony = new Harmony(PluginInfo.PLUGIN_GUID);
                harmony.PatchAll();
            }
            catch (Exception e)
            {
                Console.WriteLine($"DrillableScan patch failed!\n{FormatException(e)}");
            }
        }
        private static string FormatException(Exception e)
        {
            if (e == null)
                return string.Empty;
            return $"\"Exception: {e.GetType()}\"\n\tMessage: {e.Message}\n\tStacktrace: {e.StackTrace}\n" +
                   FormatException(e.InnerException);
        }
    }
}
