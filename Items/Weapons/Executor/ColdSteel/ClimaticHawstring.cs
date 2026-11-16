using LostbeltTraveler.Core.NetCode;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Projs.Executor.ColdSteel;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Executor.ColdSteel
{
    public class ClimaticHawstring : ExecutorWeaponClass
    {
        public override int ExecutionProgress => 40;
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.ColdSteel;
        public override void ExSD()
        {
            Item.damage = 50;
            Item.useTime = Item.useAnimation = 20;
            Item.knockBack = 5f;
            Item.SetUpRarityPrice(ItemRarityID.LightRed);
            Item.SetUpNoUseGraphicItem(true, false);
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ProjectileType<ClimaticHawstringProj>();
            Item.shootSpeed = 12f;

        }
        public override bool CanShoot(Player player) => false;
        public override void HoldItem(Player player)
        {
            if (!player.IsOwnerSide())
                return;
            if (player.GetExecutionSrike())
            {
                player.Lostbelt().climaticHawstringLaserCounter = 30;
                player.RemoveExecutionProgress(Type);
            }
            if (player.HasProj(Item.shoot))
                return;
            int damage = (int)player.GetTotalDamage<ExecutorDamageClass>().ApplyTo(Item.damage);
            Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, Item.shoot, damage, Item.knockBack, player.whoAmI, ai0: 9);

        }
    }
}
