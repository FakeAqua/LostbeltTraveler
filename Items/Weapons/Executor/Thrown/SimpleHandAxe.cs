using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Globals.Instances.Items;
using LostbeltTraveler.Projs.Executor.Thrown;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Executor.Thrown
{
    public class SimpleHandAxe : ExecutorWeaponClass
    {
        public override int ExecutionProgress => 15;
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.Throw;
        public override void ExSD()
        {
            Item.SetUpNoUseGraphicItem();
            Item.SetUpRarityPrice(ItemRarityID.Blue);
            Item.shootSpeed = 44;
            Item.useTime = Item.useAnimation = 20;
            Item.shoot = ProjectileType<SimpleHandAxeProj>();
            Item.Lostbelt().ExecutionProj = ProjectileType<SimpleHandAxeExecution>();
            //音效在射弹初始化时进行
            Item.UseSound = null;
            Item.damage = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(LostbeltRecipeGroup.AnyGoldBar, 5).
                AddIngredient(ItemID.Diamond, 2).
                AddIngredient(ItemID.FallenStar, 1).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
