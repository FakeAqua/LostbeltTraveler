using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Globals.Classes;
using Terraria;

namespace LostbeltTraveler.Items.Weapons.Requirement
{
    public class TechThunder : LostbeltProjs
    {
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(12);
        }
        public override void ExSD()
        {
            Projectile.SetupImmnuity(-1);
            Projectile.extraUpdates = 25;
            Projectile.penetrate = 1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;

        }
        public override void ProjAI()
        {
            ECSParticle.HRShinyOrb(Projectile.Center, Projectile.velocity * 0f, RandLerpColor(Color.SkyBlue, Color.LightSkyBlue), 40, .85f, 0.075f * 1.2f, 0.25f);
            ECSParticle.HighResolutionThunder(Projectile.Center.ToRandCirclePosEdge(2), Projectile.SafeDir() * 0, Color.SkyBlue, 40, 1, Projectile.velocity.ToRotation() + PiOver2, Projectile.scale * .15f * 2, 3);
            if (Projectile.Lostbelt().CurStoredTarget.IsLegal())
            {
                Projectile.HomingTarget(Projectile.Lostbelt().CurStoredTarget.Center, -1, 5f, 0f);

            }
            else if (Projectile.GetTargetSafe(out NPC target))
            {
                Projectile.Lostbelt().CurStoredTarget = target;

            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return base.OnTileCollide(oldVelocity);
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 8; i++)
            {
                float args = TwoPi / 8f * i;
                ECSParticle.HighResolutionThunder(Projectile.Center.ToRandCirclePosEdge(16), Vector2.Zero, Color.SkyBlue, 45, 1, args, Projectile.scale * .5f, 3);
            }
            float ringScale = Projectile.scale * 1.2f;
            ECSParticle.Ring(Projectile.Center, Vector2.Zero, Color.SkyBlue, 70, 1, 0, ringScale, .2f, 1);
            ECSParticle.Ring(Projectile.Center, Vector2.Zero, Color.LightSkyBlue, 70, 1, 0, ringScale * .85f, .2f, 1);
            ECSParticle.Ring(Projectile.Center, Vector2.Zero, Color.LightSkyBlue, 70, 1, 0, ringScale * .65f, .2f, 1);
            for (int i = 0; i < 32; i++)
            {
                ECSParticle.TurbulenceShinyOrb(Projectile.Center.ToRandCirclePos(60), 1.2f, RandLerpColor(Color.SkyBlue, Color.LightSkyBlue), 45, 1, Projectile.scale * Main.rand.NextFloat(.9f, 1.1f) * .1f, RandRotTwoPi, .25f);
            }
            ECSParticle.ShinyCrossStarSmall(Projectile.Center, Vector2.Zero, Color.SkyBlue, 45, 1, ringScale, 0.02f);
            ECSParticle.ShinyCrossStarSmall(Projectile.Center, Vector2.Zero, Color.White, 45, 1, ringScale * .85f, 0.02f);

            base.OnKill(timeLeft);
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
