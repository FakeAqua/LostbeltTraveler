using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Projs.Executor.Assistance;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Executor.Assistance
{
    public class FishronKnife : ExecutorWeaponClass
    {
        public override int ExecutionProgress => 20;
        public static float DamageBuff = .20f;
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.Assistance;
        public override void ExSSD()
        {
            base.ExSSD();
        }
        public override void ExSD()
        {
            Item.damage = 100;
            Item.knockBack = 4;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = Item.useAnimation = 25;
            Item.shootSpeed = 16f;
            Item.shoot = ProjectileType<FishronKnifeProj>();
            Item.UseSound = SoundID.Item64;
            Item.SetUpRarityPrice(ItemRarityID.Orange);
            Item.SetUpNoUseGraphicItem();
            Item.Lostbelt().ForceAutomaticExecution = true;
            Item.Lostbelt().ExecutionProj = ProjectileType<FishronKnifeMark>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            proj.Lostbelt().HasExecutionMechanic = true;
            GhostKnife.QuickSpawnMark(Type, Item.Lostbelt().ExecutionProj, player, source, position);
            return false;
        }
        public override void AddRecipes()
        {
            base.AddRecipes();
        }
    }
}
