using LostbeltTraveler.Core.NetCode;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Projs.Executor.Firearm;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Executor.Firearm
{
    public class CaptainDeadBeard : ExecutorWeaponClass
    {
        public override int ExecutionProgress => 26;
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.Firearm;
        public bool SniperMode = false;
        public override void ExSSD()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Supporter);
            ScarletItemIDSets.ForceToTacticalExecute[Type] = true;
        }
        public override void ExSD()
        {
            Item.damage = 25;
            Item.shootSpeed = 19;
            Item.SetUpRarityPrice(ItemRarityID.Orange);
            Item.SetUpNoUseGraphicItem(true);
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = null;
            Item.knockBack = 7f;
            Item.useTime = Item.useAnimation = 24;
            Item.shoot = ProjectileType<CaptainDeadBeardHeldProj>();
            Item.Lostbelt().ItemBelongTo = EnumItemOwner.Supporter;
            Item.Lostbelt().OwnerName = "梦山之主";
        }
        public override bool CanShoot(Player player)
        {
            return false;
        }
        public override void HoldItem(Player player)
        {
            if (!player.IsOwnerSide())
                return;
            if (player.HasProj(Item.shoot) || player.HasProj<CaptainDeadBeardSniperProj>())
                return;
            if (!SniperMode)
            {
                int projDamage = (int)player.GetTotalDamage<ExecutorDamageClass>().ApplyTo(Item.damage);
                Projectile proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, Item.shoot, projDamage, Item.knockBack, player.whoAmI);
                proj.originalDamage = projDamage;
                proj.Lostbelt().HasExecutionMechanic = true;
                proj.netUpdate = true;
            }
            else
            {
                int projDamage = (int)player.GetTotalDamage<ExecutorDamageClass>().ApplyTo(Item.damage);
                Projectile proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, ProjectileType<CaptainDeadBeardSniperProj>(), projDamage, Item.knockBack, player.whoAmI);
                proj.originalDamage = projDamage;
                proj.Lostbelt().HasExecutionMechanic = true;
                proj.netUpdate = true;

            }
        }
    }
}
