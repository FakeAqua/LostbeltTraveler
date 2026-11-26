using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Items.Weapons.Magic;
using LostbeltTraveler.Projs.Executor.Assistance;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Executor.Assistance
{
    public class GrassKnife : ExecutorWeaponClass
    {
        public override int ExecutionProgress => 20;
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.Assistance;
        public static int PrehardmodeDamage = 25;
        public static int HardmodeDamage = 50;
        public static int PostWoSDamage = 100;
        public override void ExSSD()
        {
            base.ExSSD();
        }
        public override void ExSD()
        {
            Item.damage = 54;
            Item.knockBack = 4;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = Item.useAnimation = 25;
            Item.shootSpeed = 16f;
            Item.shoot = ProjectileType<GrassKnifeProj>();
            Item.UseSound = SoundID.Item1;
            Item.SetUpRarityPrice(ItemRarityID.Orange);
            Item.SetUpNoUseGraphicItem();
            Item.Lostbelt().ForceAutomaticExecution = true;
            Item.Lostbelt().ExecutionProj = ProjectileType<GrassKnifeMark>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 randomVelocity = velocity.ToSafeNormalize().RotatedByRandom(ToRadians(12.5f)) * Main.rand.NextFloat(0.88f, 1.12f);
            Projectile proj = Projectile.NewProjectileDirect(source, position, randomVelocity * Item.shootSpeed, type, damage, knockback, player.whoAmI);
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
