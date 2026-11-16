using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Projs.Executor.Thrown;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Executor.Thrown
{
    public class LunarHammer : ExecutorWeaponClass
    {
        public override int ExecutionProgress => 15;
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.Throw;
        public override void ExSD()
        {
            Item.damage = 541;
            Item.SetUpNoUseGraphicItem(false);
            Item.SetUpRarityPrice(ItemRarityID.Red);
            Item.useTime = Item.useAnimation = 30;
            Item.knockBack = 1.5f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shoot = ProjectileType<LunarHammerProj>();
            Item.UseSound = LostbeltSoundAssets.Blunt_Swing;
            Item.shootSpeed = 16;

        }
    }
}
