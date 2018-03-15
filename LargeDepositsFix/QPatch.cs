using Harmony;
using System;
using System.Reflection;

namespace LargeDepositsFix
{
    public class QPatch
    {
        public static void Patch()
        {
            try
            {
                HarmonyInstance harmony = HarmonyInstance.Create("com.drillablescan.mod");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
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
