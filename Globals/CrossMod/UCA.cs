using ContinentOfJourney.Items;
using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Globals.Configs;
using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.CrossMod
{
    public class UCACrossModSupportProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override void SetStaticDefaults()
        {
            if (!LostbeltConfigServer.Instance.CrossModSupport)
                return;
            if (LostbeltTraveler.CrossMod_UCA is null)
                return;
            int heavySwing = LostbeltTraveler.CrossMod_UCA.Find<ModProjectile>("StormRulerHeldHeavySwing").Type;
            int normalSwing = LostbeltTraveler.CrossMod_UCA.Find<ModProjectile>("StormRulerHeldSwingProj").Type;
            int kingofStorm = LostbeltTraveler.CrossMod_UCA.Find<ModProjectile>("StormRulerHeld_KingofStorm").Type;
            int Lunge = LostbeltTraveler.CrossMod_UCA.Find<ModProjectile>("StormRulerHeldProj_Lunge").Type;
            int LungeKingofStorm = LostbeltTraveler.CrossMod_UCA.Find<ModProjectile>("StormRulerHeldProj_Lunge_KingofStorm").Type;
            ScarletItemIDSets.GiantKiller[heavySwing] = true;
            ScarletItemIDSets.GiantKiller[normalSwing] = true;
            ScarletItemIDSets.GiantKiller[kingofStorm] = true;
            ScarletItemIDSets.GiantKiller[Lunge] = true;
            ScarletItemIDSets.GiantKiller[LungeKingofStorm] = true;
        }
    }
    public class UCACrossModSupport : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override void SetStaticDefaults()
        {
            if (!LostbeltConfigServer.Instance.CrossModSupport)
                return;

            if (LostbeltTraveler.CrossMod_UCA is null)
                return;
            int carnage = LostbeltTraveler.CrossMod_UCA.Find<ModItem>("CarnageRay").Type;
            LostbeltList.ShinyRarityItemDictionary.Add(carnage, Globals.Database.Enums.ShinyRarityType.ScarletRed);
            int night = LostbeltTraveler.CrossMod_UCA.Find<ModItem>("NightsRayAlt").Type;
            int shadow = LostbeltTraveler.CrossMod_UCA.Find<ModItem>("ShadowBoltStaffAlt").Type;
            LostbeltList.ShinyRarityItemDictionary.Add(night, Globals.Database.Enums.ShinyRarityType.ForeverNight);
            LostbeltList.ShinyRarityItemDictionary.Add(shadow, Globals.Database.Enums.ShinyRarityType.ForeverNight);
            int vivid = LostbeltTraveler.CrossMod_UCA.Find<ModItem>("VividClarityAlt").Type;
            int element = LostbeltTraveler.CrossMod_UCA.Find<ModItem>("ElementRayAlt").Type;
            int sword = LostbeltTraveler.CrossMod_UCA.Find<ModItem>("StormRulerAlt").Type;
            LostbeltList.ShinyRarityItemDictionary.Add(vivid, Globals.Database.Enums.ShinyRarityType.FateWhite);
            LostbeltList.ShinyRarityItemDictionary.Add(element, Globals.Database.Enums.ShinyRarityType.FateWhite);
            LostbeltList.ShinyRarityItemDictionary.Add(sword, Globals.Database.Enums.ShinyRarityType.FateWhite);
            ScarletItemIDSets.GiantKiller[sword] = true;
            int terra = LostbeltTraveler.CrossMod_UCA.Find<ModItem>("TerraRay").Type;
            LostbeltList.ShinyRarityItemDictionary.Add(terra, Globals.Database.Enums.ShinyRarityType.Life);
            int plasma = LostbeltTraveler.CrossMod_UCA.Find<ModItem>("PlasmaRodAlt").Type;
            int soul = LostbeltTraveler.CrossMod_UCA.Find<ModItem>("SoulPiercerAlt").Type;
            LostbeltList.ShinyRarityItemDictionary.Add(plasma, Globals.Database.Enums.ShinyRarityType.Nebula);
            LostbeltList.ShinyRarityItemDictionary.Add(soul, Globals.Database.Enums.ShinyRarityType.Nebula);

        }
    }
    public class UCACrossModSupportSystem : ModSystem
    {
        public override void PostAddRecipes()
        {
            if (!LostbeltConfigServer.Instance.CrossModSupport)
                return;

            if (LostbeltTraveler.CrossMod_UCA is null)
                return;
            if (LostbeltTraveler.CrossMod_Calamity is not null)
                return;
            int carnageRay = LostbeltTraveler.CrossMod_UCA.Find<ModItem>("CarnageRay").Type;
            int nightRay = LostbeltTraveler.CrossMod_UCA.Find<ModItem>("NightsRayAlt").Type;
            int terraRay = LostbeltTraveler.CrossMod_UCA.Find<ModItem>("TerraRay").Type;
            int shadowbolt = LostbeltTraveler.CrossMod_UCA.Find<ModItem>("ShadowBoltStaffAlt").Type;
            //元素射线
            int elementalRay = LostbeltTraveler.CrossMod_UCA.Find<ModItem>("ElementRayAlt").Type;
            //灵魂穿透者
            int soulPiercer = LostbeltTraveler.CrossMod_UCA.Find<ModItem>("SoulPiercerAlt").Type;
            //耀界之光
            int vividClarity = LostbeltTraveler.CrossMod_UCA.Find<ModItem>("VividClarityAlt").Type;
            //风暴管束者
            int stormBlade = LostbeltTraveler.CrossMod_UCA.Find<ModItem>("StormRulerAlt").Type;
            int[] ucaWeapons = [carnageRay, nightRay, terraRay, shadowbolt, elementalRay, soulPiercer, vividClarity, stormBlade];
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];
                if (recipe.HasResult(terraRay))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(shadowbolt))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(elementalRay) && recipe.HasTile(TileID.LunarCraftingStation))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(vividClarity) && recipe.HasTile(TileID.LunarCraftingStation))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(soulPiercer) && recipe.HasTile(TileID.LunarCraftingStation))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(stormBlade) && recipe.HasTile(TileID.LunarCraftingStation))
                {
                    recipe.DisableRecipe();
                }

            }
            Recipe.Create(stormBlade).
                AddIngredient<TornadoScythe>().
                AddIngredient<SoulofBlight>(5).
                AddIngredient<DeepBar>(5).
                AddTile(TileID.MythrilAnvil).
                Register();

            Recipe.Create(terraRay).
                AddIngredient(nightRay).
                AddIngredient(ItemID.BrokenHeroSword).
                AddTile(TileID.MythrilAnvil).
                Register();

            Recipe.Create(terraRay).
                AddIngredient(carnageRay).
                AddIngredient(ItemID.BrokenHeroSword).
                AddTile(TileID.MythrilAnvil).
                Register();

            Recipe.Create(elementalRay).
                AddIngredient(terraRay).
                AddIngredient<UniversalCube>().
                AddTile(TileID.LunarCraftingStation).
                Register();

            Recipe.Create(shadowbolt).
                AddIngredient(ItemID.ShadowbeamStaff).
                AddIngredient<CubistBar>(5).
                AddIngredient<LivingBar>(5).
                AddIngredient<EternalBar>(5).
                AddTile(FinalAnvilTile).
                Register();

            Recipe.Create(soulPiercer).
                AddIngredient(ItemID.ShadowbeamStaff).
                AddIngredient<EssenceofTime>(5).
                AddIngredient<EssenceofLife>(5).
                AddIngredient<EssenceofMatter>(5).
                AddTile(FinalAnvilTile).
                Register();

            Recipe.Create(vividClarity)
                .AddIngredient(elementalRay)
                .AddIngredient(shadowbolt)
                .AddIngredient(soulPiercer)
                .AddIngredient<CrownofSilveryLight>(15)
                .AddTile(FinalAnvilTile)
                .Register();
        }
    }
}
