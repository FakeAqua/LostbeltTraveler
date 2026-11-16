using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using Terraria;
using Terraria.ModLoader;

namespace LostbeltTraveler.Projs.Executor.Caster
{
    public class MythrilStaffProj : LostbeltProjs
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }

        public override EnumDamageClass Category => EnumDamageClass.Executor;
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public int TileBounce = 0;
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(20);
        }
        public override void ExSD()
        {
            Projectile.SetupImmnuity(2);
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.MaxUpdates = 3;
            Projectile.penetrate = 2;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            TileBounce++;
            Projectile.BounceOnTile(oldVelocity);
            return TileBounce > 2;
        }
        public override void ProjAI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
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
