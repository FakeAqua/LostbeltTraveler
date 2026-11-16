using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Projs.Executor.ColdSteel;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Executor.ColdSteel
{
    public class CobaltYoyo : ExecutorWeaponClass
    {
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.ColdSteel;
        public override int ExecutionProgress => 12;
        public override void ExSD()
        {
            Item.damage = 64;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = Item.useAnimation = 40;
            Item.SetUpRarityPrice(ItemRarityID.LightRed);
            Item.SetUpNoUseGraphicItem(true);
            Item.shoot = ProjectileType<CobaltYoyoProj>();
            Item.shootSpeed = 12f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.WoodYoyo).
                AddIngredient(ItemID.CobaltBar, 8).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
