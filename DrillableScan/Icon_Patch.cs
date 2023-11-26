using HarmonyLib;
using System;

namespace DrillableScan
{
    [HarmonyPatch(typeof(uGUI_MapRoomResourceNode), "SetTechType")]
    class MapRoomResourceNode_SetTechType_Patch
    {
        static void Postfix(uGUI_MapRoomResourceNode __instance, ref TechType techType)
        {
            switch (techType)
            {
                case TechType.DrillableSalt:
                  __instance.icon.sprite = SpriteManager.GetWithNoDefault(TechType.Salt);
                  __instance.icon.enabled = true;
                  break;
                case TechType.DrillableQuartz:
                  __instance.icon.sprite = SpriteManager.GetWithNoDefault(TechType.Quartz);
                  __instance.icon.enabled = true;
                  break;
                case TechType.DrillableCopper:
                  __instance.icon.sprite = SpriteManager.GetWithNoDefault(TechType.Copper);
                  __instance.icon.enabled = true;
                  break;
                case TechType.DrillableTitanium:
                  __instance.icon.sprite = SpriteManager.GetWithNoDefault(TechType.Titanium);
                  __instance.icon.enabled = true;
                  break;
                case TechType.DrillableLead:
                  __instance.icon.sprite = SpriteManager.GetWithNoDefault(TechType.Lead);
                  __instance.icon.enabled = true;
                  break;
                case TechType.DrillableSilver:
                  __instance.icon.sprite = SpriteManager.GetWithNoDefault(TechType.Silver);
                  __instance.icon.enabled = true;
                  break;
                case TechType.DrillableDiamond:
                  __instance.icon.sprite = SpriteManager.GetWithNoDefault(TechType.Diamond);
                  __instance.icon.enabled = true;
                  break;
                case TechType.DrillableGold:
                  __instance.icon.sprite = SpriteManager.GetWithNoDefault(TechType.Gold);
                  __instance.icon.enabled = true;
                  break;
                case TechType.DrillableMagnetite:
                  __instance.icon.sprite = SpriteManager.GetWithNoDefault(TechType.Magnetite);
                  __instance.icon.enabled = true;
                  break;
                case TechType.DrillableLithium:
                  __instance.icon.sprite = SpriteManager.GetWithNoDefault(TechType.Lithium);
                  __instance.icon.enabled = true;
                  break;
                case TechType.DrillableMercury:
                  __instance.icon.sprite = SpriteManager.GetWithNoDefault(TechType.MercuryOre);
                  __instance.icon.enabled = true;
                  break;
                case TechType.DrillableUranium: 
                  __instance.icon.sprite = SpriteManager.GetWithNoDefault(TechType.Uranium);
                  __instance.icon.enabled = true;
                  break; 
                case TechType.DrillableNickel:
                  __instance.icon.sprite = SpriteManager.GetWithNoDefault(TechType.Nickel);
                  __instance.icon.enabled = true;
                  break;
                case TechType.DrillableSulphur:
                  __instance.icon.sprite = SpriteManager.GetWithNoDefault(TechType.Sulphur);
                  __instance.icon.enabled = true;
                  break;
                case TechType.DrillableKyanite:
                  __instance.icon.sprite = SpriteManager.GetWithNoDefault(TechType.Kyanite);
                  __instance.icon.enabled = true;
                  break;
                case TechType.DrillableAluminiumOxide:
                  __instance.icon.sprite = SpriteManager.GetWithNoDefault(TechType.AluminumOxide);
                  __instance.icon.enabled = true;
                  break;
                default:
                  break;
            }
        }
    }
}
