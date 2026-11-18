using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.PixelatedRender;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Graphics.Particles;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Projs.Executor.Firearm
{
    public class CaptainDeadBeardCannonRodeoSmall : LostbeltProjs, IPixelatedRenderer
    {
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(16);
        }
        public override void ExSD()
        {
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.noEnchantmentVisuals = true;
            Projectile.MaxUpdates = 5;
            Projectile.penetrate = 1;
            Projectile.SetupImmnuity(-1);
        }
        public override void OnFirstFrame()
        {
            base.OnFirstFrame();
        }
        public override void ProjAI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.IsOutScreen())
                return;
            if (Main.rand.NextBool(3))
                ECSParticle.SmokeParticle(Projectile.Center.ToRandCirclePos(16), Projectile.velocity / 6f,
                    RandLerpColor(Color.OrangeRed, Color.Orange), 45, RandRotTwoPi, 1, Main.rand.NextFloat(.9f, 1.1f) * .34f, false, blendstate: BlendState.Additive);

            if (Main.rand.NextBool(6))
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center.ToRandCirclePos(14), DustID.OrangeTorch);
                d.velocity = Projectile.velocity / 4f;
                d.scale = 1f;
                d.noGravity = true;
            }
            if (Main.rand.NextBool(5))
            {
                Vector2 pos = Projectile.Center.ToRandCirclePosEdge(14);
                ECSParticle.TurbulenceShinyOrb(pos, 1.5f, RandLerpColor(Color.Orange, Color.OrangeRed), 45, 1, .1f, glowMult: .4f);
                ECSParticle.ShrinkParticle(pos, Projectile.velocity / 4f, RandLerpColor(Color.Orange, Color.OrangeRed), 45, .41f, Projectile.velocity.ToRotation(), .6f, 1);
            }

        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            int dustCount = 10;
            for (int i = 0; i < dustCount; ++i)
            {
                Vector2 dir = Projectile.SafeDirByRot();
                Vector2 pos = Projectile.Center + Main.rand.NextVector2CircularEdge(10f, 10f) + dir * Main.rand.NextFloat(10f);
                new ShinyCrossStar(pos, RandVelTwoPi(2f, 6f), RandLerpColor(Color.Orange, Color.OrangeRed), 45, RandRotTwoPi, RandZeroToOne, Projectile.scale, 0.5f).Spawn();
            }
            for (int i = 0; i < 5; i++)
            {
                Color Firecolor = RandLerpColor(Color.Black, Color.DarkOrange);
                //同样，这里需要提供速度
                new Fire(Projectile.Center + Projectile.SafeDirByRot() * Main.rand.NextFloat(10f), Vector2.UnitX.RotatedByRandom(TwoPi) * Main.rand.NextFloat(0.2f, 1.2f) * 4, Firecolor, 90, Main.rand.NextFloat(TwoPi), 1f, 0.2f).SpawnToPriorityNonPreMult();
            }

            return true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int dustCount = 10;
            for (int i = 0; i < dustCount; ++i)
            {
                Vector2 dir = Projectile.SafeDirByRot();
                Vector2 pos = Projectile.Center + Main.rand.NextVector2CircularEdge(10f, 10f) + dir * Main.rand.NextFloat(10f);
                new ShinyCrossStar(pos, RandVelTwoPi(2f, 6f), RandLerpColor(Color.Orange, Color.OrangeRed), 45, RandRotTwoPi, RandZeroToOne, Projectile.scale, 0.5f).Spawn();
            }
            for (int i = 0; i < 5; i++)
            {
                Color Firecolor = RandLerpColor(Color.Black, Color.DarkOrange);
                //同样，这里需要提供速度
                new Fire(Projectile.Center + Projectile.SafeDirByRot() * Main.rand.NextFloat(10f), Vector2.UnitX.RotatedByRandom(TwoPi) * Main.rand.NextFloat(0.2f, 1.2f) * 4, Firecolor, 90, Main.rand.NextFloat(TwoPi), 1f, 0.2f).SpawnToPriorityNonPreMult();
            }

            base.OnHitNPC(target, hit, damageDone);
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
            //不要试图理解这里的码，我测，我一直热重载调试出来的，直接拿过去抄然后自己改改参数就行了
            Texture2D line = LostbeltModAssets.Specific_RocketTrail.Value;
            Vector2 offset2 = Projectile.velocity.ToSafeNormalize() * -7f;
            float OverallScale = 1 * Projectile.scale;
            int dir = (Projectile.velocity.X > 0).ToDirectionInt();
            for (int i = 0; i < Projectile.oldRot.Length; i++)
            {
                float progress = 1 - (float)i / Projectile.oldRot.Length;
                Color col = Color.Lerp(Color.LightGoldenrodYellow, Color.OrangeRed * 1f, progress) * 1f;
                Vector2 lineScale = new Vector2(0.9f * OverallScale * EaseInCubic(progress), 2f * (progress));
                float rot = Projectile.oldRot[i] + PiOver2;
                Vector2 AfterImagePos = Projectile.oldPos[i] + Projectile.Size / 2f +
                     -Main.screenPosition;
                Vector2 offset = Main.rand.NextVector2Circular(2.5f, 2f + (2f * (1f - progress))).RotatedBy(Projectile.velocity.ToRotation()) - offset2;
                Vector2 innerScale = new Vector2(0.9f * OverallScale * EaseInCubic(progress), 1.85f * (progress));
                SB.FastDraw(line, AfterImagePos + offset, col * 0.45f * progress * 1f,
                    rot, line.Size() / 2f, lineScale * 1.51f * 1.5f);
                SB.FastDraw(line, AfterImagePos + offset, Color.White * 0.35f * progress * 1f,
                   rot, line.Size() / 2f, innerScale * 1.5f);
            }

            //DrawTrail();
            //DrawTrail(.7f);
            LostbeltMethods.EndShaderAreaPixel();

        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.IsOutScreen())
                return false;
            if (!Projectile.Lostbelt().FirstFrame)
                return false;
            PixelatedRenderManager.BeginDrawProj = true;
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            int length = Projectile.oldPos.Length;
            for (int i = 0; i < length; i++)
            {
                float progress = (float)i / length;
                Vector2 oldPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                float oldRot = Projectile.oldRot[i] + PiOver2;
                Color c = Color.Lerp(Color.DarkGoldenrod, Color.Orange, progress);
                float scale = (1f - (progress * 0.8f)) * Projectile.scale;
                float opac = Lerp(1f, 0f, progress);
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(3, 3), c.ToAddColor(150) * .75f * opac * opac, oldRot, ori, scale * 1.1f, 0);
                Color c2 = Color.Lerp(Color.OrangeRed, Color.LightGoldenrodYellow, progress);
                SB.FastDraw(projTex, oldPos, c2.ToAddColor(75) * .75f * opac * opac, oldRot, ori, scale, 0);
            }
            for (int i = 0; i < 3; i++)
                SB.FastDraw(projTex, drawPos.ToRandCirclePos(3), Color.Goldenrod.ToAddColor(), Projectile.rotation + PiOver2, ori, Projectile.scale * 1.1f);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation + PiOver2, ori, Projectile.scale);
            SB.EnterShaderArea();
            SB.EndShaderArea();

            return false;
        }
        public void DrawTrail(float mult = 1)
        {
            //虽然我不想承认，但是这里Trail的绘制方法确实是用的starShape
            Texture2D starShape = LostbeltModAssets.Specific_RocketTrail.Value;
            Rectangle cutSource = starShape.Bounds;
            //切边。
            cutSource.Height /= 2;
            //重新设定原点
            Vector2 ori = new Vector2(cutSource.Width / 2, cutSource.Height);
            //设定缩放大小
            Vector2 baseScale = new Vector2(10.2f * mult, 12.2f);
            Effect shader = LostbeltShaderAssets.VolcanoEruptingShader;
            shader.Parameters["uBaseColor"].SetValue(Color.DarkOrange.ToVector4() * 0.5f);
            shader.Parameters["uTargetColor"].SetValue(Color.OrangeRed.ToVector4() * 0.9f);
            shader.Parameters["uTime"].SetValue(-Main.GlobalTimeWrappedHourly * 25);
            shader.Parameters["uIntensity"].SetValue(0.30f);
            shader.Parameters["uNoiseScale"].SetValue(new Vector2(8f, 5f));
            shader.Parameters["UseColor"].SetValue(true);
            shader.Parameters["uFadeRange"].SetValue(0.6f);
            shader.CurrentTechnique.Passes[0].Apply();
            GD.Textures[1] = LostbeltModAssets.Noise_Smoke.Value;
            GD.SamplerStates[1] = SamplerState.PointWrap;
            //绘制，然后叠图。
            //这里一共会画20次
            //是的没错，然后又有一堆火箭，所以理论来说会有总共超过500多次的绘制
            //希望电脑没事。
            for (float k = 1; k >= 0f; k -= 0.05f)
            {
                Vector2 scale3 = baseScale * new Vector2((1 - k) * .30f, .6f * k);
                float colorRatios = Clamp(k, 0.6f, 1f);
                shader.Parameters["uColorFactor"].SetValue(colorRatios);
                SB.Draw(starShape, Projectile.Center - Main.screenPosition, cutSource, Color.OrangeRed * colorRatios, Projectile.rotation - PiOver2, ori, scale3, 0, 0);
            }
        }

    }
}
