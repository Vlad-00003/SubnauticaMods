using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;

namespace WaterFilterOverflow
{
    public class QPatch
    {
        private static void Patch()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("com.waterfilteroverflow.mod");
        }
    }
}
