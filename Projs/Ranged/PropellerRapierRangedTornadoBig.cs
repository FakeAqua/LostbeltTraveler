using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Projs.Ranged
{
    public class PropellerRapierRangedTornadoBig : LostbeltProjs
    {
        public override string Texture => GetVanillaAssetPath(VanillaAsset.Projectile, ProjectileID.WeatherPainShot);
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(4);
        }
        public override void ExSD()
        {
            base.ExSD();
        }
        public override void OnFirstFrame()
        {
            base.OnFirstFrame();
        }
        public override void ProjAI()
        {
            base.ProjAI();
        }
        public override bool? CanDamage()
        {
            return base.CanDamage();
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
