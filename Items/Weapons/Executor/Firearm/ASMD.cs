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
    public class ASMD : ExecutorWeaponClass
    {
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.Firearm;
        public override int ExecutionProgress => 24;
        public static int ExecutionIceBlockCount = 7;
        public override void ExSSD()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Frost);
            ScarletItemIDSets.IsHeldProjItem[Type] = true;
        }
        public override void ExSD()
        {
            Item.damage = 410;
            Item.useTime = Item.useAnimation = 48;
            Item.SetUpRarityPrice(ItemRarityID.Red);
            Item.SetUpNoUseGraphicItem(true);
            Item.Lostbelt().drawBuffIconAndDetail = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ProjectileType<ASMDHeldProj>();
            Item.Lostbelt().ExecutionProj = ProjectileType<ASMDExecutionBullet>();
            Item.shootSpeed = 18f;
            Item.knockBack = 3;
        }
        public override bool CanShoot(Player player)
        {
            return false;
        }
        public override void HoldItem(Player player)
        {
            if (!player.IsOwnerSide())
                return;
            if (player.HasProj<ASMDHeldProj>(out int projID))
                return;
            int projDamage = (int)player.GetTotalDamage<ExecutorDamageClass>().ApplyTo(Item.damage);
            Projectile proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, projID, 0, Item.knockBack, player.whoAmI);
            proj.originalDamage = projDamage;
            proj.Lostbelt().HasExecutionMechanic = true;
            proj.netUpdate = true;
        }
    }
}
