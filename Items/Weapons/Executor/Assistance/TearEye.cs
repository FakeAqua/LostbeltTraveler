using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Projs.Executor.Assistance;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Executor.Assistance
{
    public class TearEye : ExecutorWeaponClass
    {
        public override int ExecutionProgress => 20;
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.Assistance;
        public override void ExSD()
        {
            Item.SetUpNoUseGraphicItem(true);
            Item.SetUpRarityPrice(ItemRarityID.Orange);
            Item.Lostbelt().drawBuffIconAndDetail = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = Item.useAnimation = 36;
            Item.knockBack = 2;
            Item.UseSound = SoundID.Item152;
            Item.damage = 40;
            Item.shootSpeed = 12f;
            Item.shoot = ProjectileType<TearEyeProj>();
            Item.Lostbelt().ExecutionProj = ProjectileType<TearEyeMark>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            proj.Lostbelt().HasExecutionMechanic = true;
            GhostKnife.QuickSpawnMark(Type, Item.Lostbelt().ExecutionProj, player, source, position);

            return false;
        }


    }
}
