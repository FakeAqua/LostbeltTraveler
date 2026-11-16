using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Projs.Executor.ColdSteel;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Executor.ColdSteel
{
    public class PureDagger : ExecutorWeaponClass
    {
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.ColdSteel;
        public static int DefenseAdd = 4;
        public override void ExSSD()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateWhite);
        }
        public override void ExSD()
        {
            Item.damage = 36;
            Item.useTime = Item.useAnimation = 19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 16f;
            Item.knockBack = 5;
            Item.shoot = ProjectileType<PureDaggerProj>();
            Item.SetUpRarityPrice(ItemRarityID.Green);
            Item.Lostbelt().drawBuffIconAndDetail = true;
            Item.SetUpNoUseGraphicItem(true);
            Item.master = true;
        }
        public override bool CanShoot(Player player)
        {
            return !player.HasProj(Item.shoot) && !player.HasProj<PureDaggerExecution>();
            //return !player.HasProj<EndlessWarSmasher>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            bool exe = player.GetExecutionSrike();
            Vector2 dir = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            proj.Lostbelt().HasExecutionMechanic = true;
            proj.Lostbelt().ExecutionStrike = exe;
            ((PureDaggerProj)proj.ModProjectile).BeginTargetRotation = dir.ToRotation();
            ((PureDaggerProj)proj.ModProjectile).Flip = Main.rand.NextBool();
            return false;
        }
    }
}
