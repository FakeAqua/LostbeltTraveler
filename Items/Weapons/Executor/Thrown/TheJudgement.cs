using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Projs.Executor.Thrown;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Executor.Thrown
{
    public class TheJudgement : ExecutorWeaponClass
    {
        public override int ExecutionProgress => 30;
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.Throw;
        public override void ExSSD()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Hallowed);
        }

        public override void ExSD()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ProjectileType<TheJudgementProj>();
            Item.knockBack = 8f;
            Item.width = Item.height = 58;
            Item.damage = 50;
            Item.useTime = 13;
            Item.useAnimation = 13;
            Item.shootSpeed = 18f;
            Item.SetUpRarityPrice(ItemRarityID.LightRed);
            Item.SetUpNoUseGraphicItem();
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ThePunishment>().
                AddIngredient(ItemID.ChlorophyteBar, 10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
