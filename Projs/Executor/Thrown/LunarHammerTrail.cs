using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Projs.Executor.Thrown
{
    public class LunarHammerTrail : LostbeltProjs
    {
        public override EnumDamageClass Category => EnumDamageClass.Executor;
        public override string Texture => GetVanillaAssetPath(VanillaAsset.Projectile, ProjectileID.MoonlordArrowTrail);
        public override Vector2 TileHitbox => new Vector2(10, 10);
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(20);
        }
        public override void ExSD()
        {
            Projectile.MaxUpdates = 3;
            Projectile.width = Projectile.height = 20;
            Projectile.SetupImmnuity(10);
            Projectile.penetrate = 2;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
        }
        public override void ProjAI()
        {
            base.ProjAI();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}
