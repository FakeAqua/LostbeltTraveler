using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.PixelatedRender;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Items.Weapons.Executor.Firearm;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Projs.Executor.Firearm
{
    public class CaptainDeadBeardBullet:LostbeltProjs,IPixelatedRenderer
    {
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public float OverallScale = 0;
        public ref float Timer => ref Projectile.localAI[0];
        public override Vector2 TileHitbox => new Vector2(30, 30);
        public override void SetStaticDefaults()

        {
            Projectile.ToTrailSetting(10);
        }
        public override void ExSD()
        {
            Projectile.width = 18;
            Projectile.height = 18;
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
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center.ToRandCirclePos(4), DustID.GoldCoin);
                d.velocity = Projectile.velocity / 4f;
                d.noGravity = true;
                d.scale = Main.rand.NextFloat(.75f, 1.15f);
            }

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            Projectile.AddExecutionTimeImmediate<CaptainDeadBeard>();
            for (int i = 0; i < 16; i++)
            {
                Vector2 pos = Projectile.Center.ToRandCirclePos(3);
                Vector2 vel = Projectile.SafeDir().ToRandVelocity(ToRadians(24f), 0.3f, 8f) * 1f;
                int lifeTime = Main.rand.Next(30, 70);
                float rot = RandRotTwoPi;
                ECSParticle.SmokeParticle(pos, -vel, RandLerpColor(Color.Goldenrod, Color.Gold), lifeTime, rot, 1, 0.25f, Main.rand.NextBool(), BlendState.Additive);
            }
            for (int i = 0; i < 18; i++)
            {
                Vector2 pos = Projectile.Center.ToRandCirclePos(3);
                Vector2 vel = Projectile.SafeDir().ToRandVelocity(ToRadians(24f), 0.3f, 8f) * 1f;
                int lifeTime = Main.rand.Next(30, 70);
                float rot = RandRotTwoPi;
                ECSParticle.ShinyCrossStarECS(pos, -vel, RandLerpColor(Color.LightGoldenrodYellow, Color.Goldenrod), lifeTime, 1, .4f, .2f);

            }
        }
        public ScarletDrawLayer LayerToRenderTo => ScarletDrawLayer.BeforePlayer;
        public BlendState BlendState => BlendState.Additive;

        public void RenderPixelated(SpriteBatch sb)
        {
            if (Projectile.IsOutScreen())
                return;
            if (!Projectile.Lostbelt().FirstFrame)
                return;
            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            Texture2D line = LostbeltModAssets.Particle_SharpTear;
            Vector2 offset2 = Projectile.velocity.ToSafeNormalize() * 0f;
            for (int i = 0; i < Projectile.oldRot.Length; i++)
            {
                float progress = 1 - (float)i / Projectile.oldRot.Length;
                Color col = Color.Lerp(Color.LightGoldenrodYellow, Color.DarkGoldenrod* 1f, progress) * 1f;
                Vector2 lineScale = new Vector2(0.45f * OverallScale*progress, .9f * progress*progress);
                float rot = Projectile.oldRot[i] + PiOver2;
                Vector2 AfterImagePos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                Vector2 offset = Main.rand.NextVector2Circular(2.5f, 2f + (2f * (1f - progress))).RotatedBy(Projectile.velocity.ToRotation()) - offset2;
                Vector2 innerScale = new Vector2(0.45f * OverallScale*progress, .9f * progress*progress);
                SB.FastDraw(line, AfterImagePos + offset, col * 0.85f * progress * 1f,
                    rot, line.Size() / 2f, lineScale * 1f * 1.5f);
                SB.FastDraw(line, AfterImagePos + offset, Color.White * 0.85f * progress * 1f,
                   rot, line.Size() / 2f, innerScale * 1f);
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
