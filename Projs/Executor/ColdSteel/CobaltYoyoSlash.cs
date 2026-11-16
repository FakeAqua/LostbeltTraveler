using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Projs.Executor.ColdSteel
{
    public class CobaltYoyoSlash : LostbeltProjs
    {
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public override EnumDamageClass Category => EnumDamageClass.Executor;
        public override void ExSD()
        {
            Projectile.MaxUpdates = 3;
            Projectile.SetupImmnuity(10 * Projectile.MaxUpdates);
            Projectile.penetrate = 4;
            Projectile.timeLeft = 600;
            Projectile.noEnchantmentVisuals = true;
        }
        public override void ProjAI()
        {
            if (Projectile.Lostbelt().CurStoredTarget.IsLegal())
            {
                Projectile.timeLeft = 2;
                Projectile.Center = Projectile.Lostbelt().CurStoredTarget.Center;
            }
            else
                Projectile.Kill();
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.Lostbelt().CurStoredTarget.IsLegal() && target.Equals(Projectile.Lostbelt().CurStoredTarget))
                return null;
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 35; i++)
            {
                ECSParticle.SmokeParticle(target.Center, RandVelTwoPi(1f, 12f), RandLerpColor(Color.SkyBlue, Color.LightSkyBlue), 25, RandRotTwoPi, .75f, Projectile.scale * Main.rand.NextFloat(.9f, 1.15f) * .24f, Main.rand.NextBool(), BlendState.Additive);
            }
            for (int i = 0; i < 35; i++)
            {
                ECSParticle.ShinyCrossStarECS(target.Center, RandVelTwoPi(1f, 4.5f), RandLerpColor(Color.SkyBlue, Color.LightSkyBlue), 25, 1, Projectile.scale * Main.rand.NextFloat(.90f, 1.05f) * .75f, .2f);
            }
            ScarletSound(SoundID.DD2_GoblinBomb, Projectile.Center, volume: .75f, pitch: .6f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = LostbeltModAssets.Particle_HRShinyOrbSmall.Value;
            Vector2 pos = Projectile.Center - Main.screenPosition;
            float scale = Projectile.scale * 0.62f;
            SB.EnterShaderArea();
            SB.FastDraw(tex, pos, Color.RoyalBlue, 0, tex.Size() / 2f, scale, 0);
            SB.FastDraw(tex, pos, Color.LightSkyBlue, 0, tex.Size() / 2f, scale * .95f, 0);
            SB.EndShaderArea();
            return false;
        }
    }
}
