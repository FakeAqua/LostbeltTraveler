using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Projs.Executor.Misc;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Executor.Misc
{
    public class TheGreatDipper : ExecutorWeaponClass
    {
        public override int ExecutionProgress => 7;
        public override void ExSSD()
        {
            ScarletItemIDSets.NoGeneralExecutionProgressDraw[Type] = true;
            ScarletItemIDSets.ForceToCustomExecute[Type] = true;
            ScarletItemIDSets.IsHeldProjItem[Type] = true;
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Donator);
        }
        public override void ExSD()
        {
            Item.damage = 1180;
            Item.useTime = Item.useAnimation = 28;
            Item.knockBack = 4.5f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 16f;
            Item.shoot = ProjectileType<TheGreatDipperHeldProj>();
            Item.SetUpRarityPrice(ItemRarityID.Red);
            Item.SetUpNoUseGraphicItem(true);
            Item.Lostbelt().OwnerName = "冰川咲";
            Item.Lostbelt().ItemBelongTo = EnumItemOwner.Donator;
            Item.master = true;

        }
        public override bool CanShoot(Player player)
        {
            return !player.HasProj(Item.shoot) && !player.HasProj<TheGreatDipperGalaxyHelper>();
        }
        public override bool CanUseItem(Player player)
        {
            return player.Lostbelt().emblemGalaxy;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            bool exe = player.GetExecutionSrike();
            Vector2 dir = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            proj.originalDamage = damage;
            proj.Lostbelt().HasExecutionMechanic = true;
            proj.Lostbelt().ExecutionStrike = exe;
            ((TheGreatDipperHeldProj)proj.ModProjectile).BeginTargetRotation = dir.ToRotation();
            ((TheGreatDipperHeldProj)proj.ModProjectile).Flip = Main.rand.NextBool();

            return false;
        }
        public override void HoldItem(Player player)
        {
            player.Lostbelt().tacticalExecution = !player.Lostbelt().theGreatDipperBuff;
            bool noSevenStar = player.HasProj<TheGreatDipperDipper>() && player.HasProj<TheGreatDipperDipperStar>();
            if (noSevenStar || player.whoAmI != Main.myPlayer)
                return;
            if (!player.Lostbelt().emblemGalaxy)
                return;

            int dmg = (int)player.GetTotalDamage<ExecutorDamageClass>().ApplyTo(Item.damage);
            Projectile proj = Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.MountedCenter, Vector2.Zero, ProjectileType<TheGreatDipperDipper>(), 0, 0, player.whoAmI);
            proj.originalDamage = dmg;
            proj.Center = player.MountedCenter - player.direction * 120f * Vector2.UnitX;
        }
    }
}
