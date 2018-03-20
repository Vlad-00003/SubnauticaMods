using SMLHelper.Patchers;
using System.Collections.Generic;
using Utilites.Config;
using Utilites.Logger;

namespace UnpackTitaniumIngots
{
    public class QPatch
    {
        private static readonly ConfigFile Config = new ConfigFile("config");
        private static int _x = 2;
        private static int _y = 2;
        public static void Patch()
        {
            Config.Load();
            var configChanged = Config.TryGet(ref _x, "Scrap metal size", "x")
                                | Config.TryGet(ref _y, "Scrap metal size", "y");
            if (_x <= 0)
            {
                _x = 2;
                Config["Scrap metal size", "x"] = _x;
                Logger.Error("Size of the item can't be less or equal of 0! X was set to 2", LogType.Custom | LogType.Console);
                configChanged = true;
            }
            if (_y <= 0)
            {
                _y = 2;
                Config["Scrap metal size", "y"] = _y;
                Logger.Error("Size of the item can't be less or equal of 0! Y was set to 2", LogType.Custom | LogType.Console);
                configChanged = true;
            }
            if(configChanged)
                Config.Save();
            var techData = new TechDataHelper
            {
                _craftAmount = 2,
                _ingredients = new List<IngredientHelper>()
                {
                    new IngredientHelper(TechType.TitaniumIngot, 1)
                },
                _linkedItems = new List<TechType>()
                {
                    TechType.Titanium,
                    TechType.Titanium
                },
                _techType = TechType.ScrapMetal
            };
            CraftDataPatcher.customTechData.Add(TechType.ScrapMetal, techData);
            CraftTreePatcher.customCraftNodes.Add("Resources/BasicMaterials/ScrapMetal", TechType.ScrapMetal);
            CraftDataPatcher.customItemSizes[TechType.ScrapMetal] = new Vector2int(_x,_y);
            TechTypePatcher.UnlockOnStart.Add(TechType.ScrapMetal);
        }
    }
}
