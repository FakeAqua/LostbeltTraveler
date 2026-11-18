using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Projs.Executor.Thrown
{
    public class LunarHammerBoom : LostbeltProjs
    {
        public override string Texture => GetVanillaAssetPath(VanillaAsset.Projectile, ProjectileID.LunarFlare);
        public override EnumDamageClass Category => EnumDamageClass.Executor;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 7;
        }
        public override void ExSD()
        {
            Projectile.width = Projectile.height = 100;
            Projectile.SetupImmnuity(10);
            Projectile.penetrate = 1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 21;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override void ProjAI()
        {
            Projectile.AddFrames(1, Main.projFrames[Type]);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}
