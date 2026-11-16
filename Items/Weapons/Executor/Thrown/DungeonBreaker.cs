using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Projs.Executor.Thrown;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Executor.Thrown
{
    public class DungeonBreaker : ExecutorWeaponClass
    {
        public override float ExecutionStrikeDamageMult => 0.5f;
        public override int ExecutionProgress => 8;
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.Throw;
        public override void ExSD()
        {
            Item.width = Item.height = 66;
            Item.damage = 40;
            Item.knockBack = 8f;
            Item.shootSpeed = 19f;
            Item.useTime = Item.useAnimation = 38;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = LostbeltSoundAssets.Misc_KnifeToss with { PitchVariance=.1f, Pitch = -0.4f, Variants = [2] };
            Item.shoot = ProjectileType<DungeonBreakerProj>();
            Item.SetUpRarityPrice(ItemRarityID.Orange);
            Item.SetUpNoUseGraphicItem();
            Item.Lostbelt().ExecutionProj = ProjectileType<DungeonBreakerExecution>();
        }
        public static void PlayPipes(Player owner)
        {
            for (int i = 0; i < 2; i++)
            {
                ScarletSound(LostbeltSoundAssets.Pipes, owner.Center, 1f, 0);
            }
        }
    }
}
