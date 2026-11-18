using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.PixelatedRender;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Graphics.Particles;
using Terraria;

namespace LostbeltTraveler.Projs.Executor.Thrown
{
    public class LunarHammerMissle : LostbeltProjs, IPixelatedRenderer
    {
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public float OverallScale = 0;
        public ref float Timer => ref Projectile.localAI[0];
        public override Vector2 TileHitbox => new Vector2(30, 30);
        public override void SetStaticDefaults()

        {
            Projectile.ToTrailSetting(30);
        }
        public override void ExSD()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.MaxUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }
        public override void ProjAI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            float timeToMaxSize = 15f * Projectile.MaxUpdates;
            float aniProgress = Utils.GetLerpValue(0, timeToMaxSize, Timer, true);
            OverallScale = Lerp(0f, 1f, EaseOutCubic(aniProgress));
            Timer++;
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Main.rand.NextBool(7))
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePos(16), Projectile.velocity / 2f, Color.Aquamarine, 45, 1, .65f, .2f);

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            float particleOffsetRandRot = Projectile.rotation;
            float ringScale = Projectile.scale * 1.2f;
            for (float i = 0; i < 1f; i += .2f)
            {
                new NoiseShockRing(Projectile.Center, Vector2.Zero, Color.Lerp(Color.Aquamarine, Color.LightSkyBlue, i), 30, 1, .9f * Lerp(0.95f, 1f, i) * Projectile.localAI[1], -1, Vector2.Zero, false).Spawn();
                ECSParticle.CrossGlow(Projectile.Center, Color.Aquamarine, 40, i, 0.1f);
            }
            for (int i = 0; i < 15; i++)
            {
                Vector2 args = (TwoPi / 16f * i).ToRotationVector2();
                Vector2 particlePos = Projectile.Center + args * 10f;
                ECSParticle.ShinyCrossStarECS(particlePos, RandVelTwoPi(1.5f, 9.4f) * 1.1f, Color.Aquamarine, 45, 1, .95f, .2f);

            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Vector2 args2 = (TwoPi / 4f * j + particleOffsetRandRot).ToRotationVector2();
                    ECSParticle.LightntingGlow(Projectile.Center.ToRandCirclePos(3), args2 * 7.277f * Main.rand.NextFloat(), Color.Aquamarine, 40, Main.rand.NextFloat(), 0.75f * 1 * Main.rand.NextFloat(.9f, 1.1f), 4);
                }
            }
            for (int i = 0; i < 20; i++)
            {
                ECSParticle.TurbulenceShinyOrb(Projectile.Center.ToRandCirclePos(60), 1.9f, Color.Aquamarine, 60, 1, .1f);
            }
            Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<LunarHammerBoom>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
        }
        public ScarletDrawLayer LayerToRenderTo => ScarletDrawLayer.BeforePlayer;
        public BlendState BlendState => BlendState.Additive;

        public void RenderPixelated(SpriteBatch sb)
        {
            if (Projectile.IsOutScreen())
                return;
            if (!Projectile.Lostbelt().FirstFrame)
                return;
            PixelatedRenderManager.BeginDrawProj = true;

            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            Texture2D line = LostbeltModAssets.Particle_SharpTear;

            Vector2 offset2 = Projectile.velocity.ToSafeNormalize() * 0f;
            for (int i = 0; i < Projectile.oldRot.Length - 2; i++)
            {
                float progress = 1 - (float)i / Projectile.oldRot.Length;
                Color col = Color.Lerp(Color.Aqua, Color.DeepSkyBlue * 1f, progress) * 1f;
                Vector2 lineScale = new Vector2(0.45f * OverallScale, 2.1f * progress);
                float rot = Projectile.oldRot[i] + PiOver2;
                Vector2 AfterImagePos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                Vector2 offset = Main.rand.NextVector2Circular(2.5f, 2f + (2f * (1f - progress))).RotatedBy(Projectile.velocity.ToRotation()) - offset2;
                Vector2 innerScale = new Vector2(0.45f * OverallScale, 2.1f * progress);
                SB.FastDraw(line, AfterImagePos + offset, col * 0.85f * progress * 1f,
                    rot, line.Size() / 2f, lineScale * 1.25f * 1.5f);
                SB.FastDraw(line, AfterImagePos + offset, Color.White * 0.85f * progress * 1f,
                   rot, line.Size() / 2f, innerScale * 1.5f);
            }
            LostbeltMethods.EndShaderAreaPixel();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.IsOutScreen())
                return false;
            if (!Projectile.Lostbelt().FirstFrame)
                return false;
            PixelatedRenderManager.BeginDrawProj = true;
            return false;
        }
    }
}
