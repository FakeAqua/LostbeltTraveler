using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Projs.Executor.Thrown;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Executor.Thrown
{
    public class AngryBomb : ExecutorWeaponClass
    {
        public override int ExecutionProgress => 12;
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.Throw;
        public override void ExSD()
        {
            Item.damage = 42;
            Item.shootSpeed = 20;
            Item.useTime = Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.SetUpRarityPrice(ItemRarityID.Blue);
            Item.SetUpNoUseGraphicItem();
            Item.knockBack = 2f;
            Item.shoot = ProjectileType<AngryBombProj>();
            Item.UseSound = LostbeltSoundAssets.Misc_KnifeToss with { Pitch = 0.5f, Variants = [3], Volume = 0.75f };
            Item.Lostbelt().ExecutionProj = ProjectileType<AngryBombExecution>();
        }
    }
}
