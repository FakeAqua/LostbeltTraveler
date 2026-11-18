using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using Terraria;

namespace LostbeltTraveler.Projs.Executor.Firearm
{
    public class ExsanguinationHomingBullet : LostbeltProjs
    {
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public override EnumDamageClass Category => EnumDamageClass.Executor;
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(16);
        }
        public override void ExSD()
        {
            Projectile.SetupImmnuity(-1);
            Projectile.width = Projectile.height = 16;
            Projectile.MaxUpdates = 5;
            Projectile.noEnchantmentVisuals = true;
        }
        public override void ProjAI()
        {
            if (Projectile.GetTargetSafe(out NPC target, searchDistance: 450))
            {

            }
        }
        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
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
