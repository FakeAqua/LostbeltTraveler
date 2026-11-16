using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Projs.Executor.Misc;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Executor.Misc
{
    public class ArcticGuanDao : ExecutorWeaponClass
    {
        public override int ExecutionProgress => 9;
        public override void ExSSD()
        {
            ScarletItemIDSets.ForceToTacticalExecute[Type] = true;
            ScarletItemIDSets.IsHeldProjItem[Type] = true;
        }
        public override void ExSD()
        {
            Item.SetUpNoUseGraphicItem(true);
            Item.SetUpRarityPrice(ItemRarityID.Blue);
            Item.Lostbelt().drawBuffIconAndDetail = true;
            Item.damage = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = Item.useAnimation = 25;
            Item.shoot = ProjectileType<ArcticGuanDaoHeldProj>();
            Item.shootSpeed = 7;
            Item.UseSound = null;
            Item.knockBack = 6.7f;
        }
        public override bool CanShoot(Player player)
        {
            return !player.HasProj(Item.shoot);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            bool exe = player.GetExecutionSrike();
            Vector2 dir = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            proj.Lostbelt().HasExecutionMechanic = true;
            proj.Lostbelt().ExecutionStrike = exe;
            ((ArcticGuanDaoHeldProj)proj.ModProjectile).BeginTargetRotation = dir.ToRotation();
            ((ArcticGuanDaoHeldProj)proj.ModProjectile).Flip = Main.rand.NextBool();
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.IceBlock, 5).
                AddIngredient(ItemID.Shiverthorn, 1).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}
