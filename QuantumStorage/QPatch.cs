using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SMLHelper;
using SMLHelper.Patchers;
using UnityEngine;
using Logger = Utilites.Logger.Logger;
using Object = UnityEngine.Object;

namespace QuantumStorage
{
    public class QPatch
    {
        public static void Patch()
        {
            Logger.ClearCustomLog();
            Logger.Debug("Loading start...");

            var ab = AssetBundle.LoadFromFile(
                $"./QMods/{Assembly.GetExecutingAssembly().GetName().Name}/testing.assets");
            Logger.Debug("Bundle name: " + ab.name);

            var cube = ab.LoadAsset<GameObject>("Cube");
            Logger.Debug("Cube name: " + cube.name);

            Utility.AddBasicComponents(ref cube, "Cube");
            Logger.Debug("BasicComponents added!");

            var techType = TechTypePatcher.AddTechType("Cube", "Cube", "Just a funny cube");
            Logger.Debug("TechType created!");

            var constructable = cube.AddComponent<Constructable>();
            constructable.allowedInBase = true;
            constructable.allowedOnGround = true;
            constructable.allowedInSub = true;
            constructable.allowedOnCeiling = false;
            constructable.allowedOnConstructables = true;
            constructable.allowedOnWall = true;
            constructable.allowedOutside = true;
            constructable.techType = techType;
            var model = cube.FindChild("Sphere");
            constructable.model = model;
            var modelname = model?.name ?? "NULL";
            Logger.Debug($"Constructable added! Model: {modelname}");

            var techTag = cube.AddComponent<TechTag>();
            techTag.type = techType;
            Logger.Debug("techTag added!");

            var rb = cube.GetComponent<Rigidbody>();
            Object.DestroyImmediate(rb);
            Logger.Debug("Rigitbody killed!");

            CustomPrefabHandler.customPrefabs.Add(new CustomPrefab("Cube", "Submarine/Build/Cube",cube,techType));
            Logger.Debug("Customprefab added!");

            var techData = new TechDataHelper
            {
                _ingredients = new List<IngredientHelper>()
                {
                    new IngredientHelper(TechType.Titanium, 1)
                },
                _techType = techType
            };
            Logger.Debug("techData added!");

            CraftDataPatcher.customTechData.Add(techType,techData);
            Logger.Debug("techData added to custom list!");

            CraftDataPatcher.customBuildables.Add(techType);
            Logger.Debug("techType added to custom Buildables!");

            var groups = typeof(CraftData).GetField("groups", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .GetValue(null) as Dictionary<TechGroup, Dictionary<TechCategory, List<TechType>>>;
            groups[TechGroup.InteriorModules][TechCategory.InteriorModule].Add(techType);
            Logger.Debug("techType added to the groups!");
        }
    }
}
