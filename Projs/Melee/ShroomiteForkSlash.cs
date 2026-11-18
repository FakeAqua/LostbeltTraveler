using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Globals.Classes;
using Terraria;
using Terraria.ModLoader;

namespace LostbeltTraveler.Projs.Melee
{
    public class ShroomiteForkSlash : LostbeltProjs
    {
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public ref float Timer => ref Projectile.ai[0];
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void ExSD()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.SetupImmnuity(5);
            Projectile.penetrate = 13;
        }
        public float LerpProgress => Utils.GetLerpValue(0, 15, Timer, true);
        public override void ProjAI()
        {
            Timer++;
            NPC curTar = Projectile.Lostbelt().CurStoredTarget;
            if (curTar.IsLegal())
                Projectile.Center = curTar.Center;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (LerpProgress >= 1)
            {

                NPC curTar = Projectile.Lostbelt().CurStoredTarget;
                if (curTar.IsLegal() && target.Equals(curTar))
                    return null;
            }
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            float rot = RandRotTwoPi;
            float scale = 1.2f;
            ScarletSound(LostbeltSoundAssets.TheSevenStar_Hit, Projectile.Center, 0.8f, 1, 0.3f, 0.1f);
            ECSParticle.Stain(target.Center, Vector2.Zero, Color.RoyalBlue, 60, 1, rot, scale, new Vector2(0.35f, 1.2f), squashSpeedX: 1.1f, squashSpeedY: .92f);
            ECSParticle.Stain(target.Center, Vector2.Zero, Color.White, 60, 1, rot, scale * .75f, new Vector2(0.35f, 1.2f), squashSpeedX: 1.1f, squashSpeedY: .92f);
            for (int i = 0; i < 2; i++)
            {
                ECSParticle.ShinyCrossStarECS(target.Center + rot.ToRotationVector2() * Main.rand.NextFloat(-1f, 1.01f) * 0f, rot.ToRotationVector2().ToRandVelocity(0, -5.2f, 5.6f),
                    RandLerpColor(Color.RoyalBlue, Color.LightSkyBlue), 40, 1, Main.rand.NextFloat(.9f, 1.1f) * .8f, .2f);
            }

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D orb = LostbeltModAssets.Particle_HRShinyOrb.Value;
            SB.EnterShaderArea();
            Vector2 pos = Projectile.Center - Main.screenPosition;
            Vector2 scale = new Vector2(Lerp(0.1f, 1f, EaseInCubic(LerpProgress)), Lerp(.1f, 1f, LerpProgress)) * .1f;
            SB.FastDraw(orb, pos, Color.RoyalBlue, 0, orb.Size() / 2f, scale, 0, 0);
            SB.FastDraw(orb, pos, Color.White, 0, orb.Size() / 2f, scale * .9f, 0, 0);
            SB.EndShaderArea();
            return false;
        }
    }
}
