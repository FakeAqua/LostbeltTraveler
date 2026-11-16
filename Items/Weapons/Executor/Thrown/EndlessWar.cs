using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Items.Materials;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Executor.Thrown
{
    public class EndlessWar : ExecutorWeaponClass
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }

        public override float ExecutionStrikeDamageMult => 1.0f;
        public override int ExecutionProgress => 15;
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.Throw;
        public override void ExSSD()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Matter);
            ScarletItemIDSets.GrantsBoosterAfterSon[Type] = true;
        }
        public override void ExSD()
        {
            Item.width = Item.height = 120;
            Item.damage = 1547;
            Item.useTime = Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.knockBack = 12f;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.SetUpRarityPrice(ItemRarityID.Red);
            Item.Lostbelt().NotFinished = true;
            Item.shootSpeed = 16;
        }
        public override void ExModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ExModifyTooltips(tooltips);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BinaryStars>().
                AddIngredient<DeathTolls>().
                AddIngredient<GaiaStriker>().
                AddIngredient<CrownofSilveryLight>(15).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
