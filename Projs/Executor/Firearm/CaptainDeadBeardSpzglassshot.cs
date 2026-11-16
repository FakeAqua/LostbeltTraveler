using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.PixelatedRender;
using LostbeltTraveler.Core.Primitives.Trail;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Items.Weapons.Executor.Firearm;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Projs.Executor.Firearm
{
    public class CaptainDeadBeardSpzglassshot : LostbeltProjs, IPixelatedRenderer
    {
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(10);
        }
        public override void ExSD()
        {
            Projectile.width = Projectile.height = 32;
            Projectile.extraUpdates = 5;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.SetupImmnuity(-1);
            Projectile.penetrate = 1;
        }
        public override void ProjAI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.IsOutScreen())
                return;
            if (Projectile.extraUpdates > 6)
            {
                if (Main.rand.NextBool(3))
                    ECSParticle.SmokeParticle(Projectile.Center.ToRandCirclePos(16), Projectile.velocity / 6f,
                        RandLerpColor(Color.OrangeRed, Color.Orange), 45, RandRotTwoPi, 1, Main.rand.NextFloat(.9f, 1.1f) * .34f, false, blendstate: BlendState.Additive);

            }
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
        public override void OnKill(int timeLeft)
        {
            if (Projectile.penetrate == 0)
                return;
            for (int i = 0; i < 24; i++)
            {
                Vector2 vel = (TwoPi / 24f * i).ToRotationVector2() * 8f * Main.rand.NextFloat(0f, 1f);

                Vector2 spawnpos = Projectile.Center.ToRandCirclePos(4f) + vel.ToSafeNormalize() * Main.rand.NextFloat() * 2f;
                Color color = RandLerpColor(Color.Lerp(Color.Orange, Color.Red, 0.50f), Color.Orange);
                float scale = 0.40f * Main.rand.NextFloat(0.55f, 1.1f);
                ECSParticle.SmokeParticle(spawnpos, vel, color, Main.rand.Next(10, 41), RandRotTwoPi, Main.rand.NextFloat(.75f, 1f), scale, true, BlendState.Additive);
            }
            for (int j = 0; j < 15; j++)
            {
                Vector2 dir = RandVelTwoPi(.1f, 4.9f);
                Vector2 pos = Projectile.Center.ToRandCirclePos(3f) + dir * Main.rand.NextFloat(0f, 3f);
                ECSParticle.ShinyCrossStarECS(pos, dir, RandLerpColor(Color.Orange, Color.OrangeRed), Main.rand.Next(15, 50), 1f, 1f * Main.rand.NextFloat(.7f, .9f), .2f);
            }
            for (int i = 0; i < 10; i++)
            {
                Vector2 pos = Projectile.Center.ToRandCirclePos(2f);
                Vector2 vel = RandVelTwoPi(.1f, 4.9f);
                ECSParticle.ShinyCrossStarECS(pos, vel, RandLerpColor(Color.Lerp(Color.Red, Color.Orange, .5f), Color.OrangeRed), Main.rand.Next(15, 50), 1f, .99f * Main.rand.NextFloat(.6f, 1f), .2f);
            }
            for (int i = 0; i < 15; i++)
            {
                Vector2 pos = Projectile.Center.ToRandCirclePos(2f);
                Vector2 vel = RandVelTwoPi(.1f, 4.9f);
                ECSParticle.HRShinyOrb(pos, vel, RandLerpColor(Color.Lerp(Color.Red, Color.Orange, .5f), Color.OrangeRed), Main.rand.Next(15, 50), 1f, .15f * Main.rand.NextFloat(.6f, 1f), .5f);
            }
            base.OnKill(timeLeft);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.AddExecutionTimeImmediate<CaptainDeadBeard>();
            for (int i = 0; i < 24; i++)
            {
                Vector2 vel = (TwoPi / 24f * i).ToRotationVector2() * 8f * Main.rand.NextFloat(0f, 1f);

                Vector2 spawnpos = Projectile.Center.ToRandCirclePos(4f) + vel.ToSafeNormalize() * Main.rand.NextFloat() * 2f;
                Color color = RandLerpColor(Color.Lerp(Color.Orange, Color.Red, 0.50f), Color.Orange);
                float scale = 0.40f * Main.rand.NextFloat(0.55f, 1.1f);
                ECSParticle.SmokeParticle(spawnpos, vel, color, Main.rand.Next(10, 41), RandRotTwoPi, Main.rand.NextFloat(.75f, 1f), scale, true, BlendState.Additive);
            }
            for (int j = 0; j < 15; j++)
            {
                Vector2 dir = RandVelTwoPi(.1f, 4.9f);
                Vector2 pos = Projectile.Center.ToRandCirclePos(3f) + dir * Main.rand.NextFloat(0f, 3f);
                ECSParticle.ShinyCrossStarECS(pos, dir, RandLerpColor(Color.Orange, Color.OrangeRed), Main.rand.Next(15, 50), 1f, 1f * Main.rand.NextFloat(.7f, .9f), .2f);
            }
            for (int i = 0; i < 10; i++)
            {
                Vector2 pos = Projectile.Center.ToRandCirclePos(2f);
                Vector2 vel = RandVelTwoPi(.1f, 4.9f);
                ECSParticle.ShinyCrossStarECS(pos, vel, RandLerpColor(Color.Lerp(Color.Red, Color.Orange, .5f), Color.OrangeRed), Main.rand.Next(15, 50), 1f, .99f * Main.rand.NextFloat(.6f, 1f), .2f);
            }
            for (int i = 0; i < 15; i++)
            {
                Vector2 pos = Projectile.Center.ToRandCirclePos(2f);
                Vector2 vel = RandVelTwoPi(.1f, 4.9f);
                ECSParticle.HRShinyOrb(pos, vel, RandLerpColor(Color.Lerp(Color.Red, Color.Orange, .5f), Color.OrangeRed), Main.rand.Next(15, 50), 1f, .15f * Main.rand.NextFloat(.6f, 1f), .5f);
            }

            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (!Projectile.Lostbelt().FirstFrame)
                return false;
            if (Projectile.IsOutScreen())
                return false;
            PixelatedRenderManager.BeginDrawProj = true;

            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            int length = Projectile.oldPos.Length;
            for (int i = 0; i < length; i++)
            {
                float progress = (float)i / length;
                Vector2 oldPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                float oldRot = Projectile.oldRot[i];
                Color c = Color.Lerp(Color.LightGoldenrodYellow, Color.Orange, EaseOutCubic(progress));
                float scale = (1f - (progress * 0.05f)) * Projectile.scale * 1.31f;
                float opac = Lerp(1f, 0f, progress);
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(3, 3), c.ToAddColor(150) * .95f * opac * opac, oldRot, ori, scale * 1.1f, 0);
                Color c2 = Color.Lerp(Color.White, Color.OrangeRed, EaseOutCubic(progress));
                SB.FastDraw(projTex, oldPos, c2.ToAddColor(150) * .9f * opac * opac, oldRot, ori, scale, 0);
            }
            for (int i = 0; i < 8; i++)
                SB.FastDraw(projTex, drawPos + Main.rand.NextVector2Circular(1.5f, 1.5f) + (TwoPi / 8f * i).ToRotationVector2() * 1.5f,
                    Color.OrangeRed.ToAddColor(), Projectile.rotation, ori, Projectile.scale * 1.31f, 0);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, ori, Projectile.scale * 1.31f, 0);
            return false;
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
            Vector2 offset2 = Projectile.velocity.ToSafeNormalize() * 0f;
            float OverallScale = 1 * Projectile.scale;
            int dir = (Projectile.velocity.X > 0).ToDirectionInt();
            for (int i = 0; i < Projectile.oldRot.Length; i++)
            {
                float progress = 1 - (float)i / Projectile.oldRot.Length;
                Color col = Color.Lerp(Color.LightGoldenrodYellow, Color.OrangeRed * 1f, progress) * 1f;
                Vector2 lineScale = new Vector2(0.75f * OverallScale * progress, 2.19f * (progress));
                float rot = Projectile.oldRot[i] + PiOver2;
                Vector2 AfterImagePos = Projectile.oldPos[i] + Projectile.Size / 2f +
                    new Vector2(-10, -2f * dir).RotatedBy(Projectile.rotation) - Main.screenPosition;
                Vector2 offset = Main.rand.NextVector2Circular(2.5f, 2f + (2f * (1f - progress))).RotatedBy(Projectile.velocity.ToRotation()) - offset2;
                Vector2 innerScale = new Vector2(0.75f * OverallScale * progress, 1.99f * (progress));
                SB.FastDraw(line, AfterImagePos + offset, col * 0.45f * progress * 1f,
                    rot, line.Size() / 2f, lineScale * 1.41f * 1.5f);
                SB.FastDraw(line, AfterImagePos + offset, Color.White * 0.45f * progress * 1f,
                   rot, line.Size() / 2f, innerScale * 1.4f);
            }
            LostbeltMethods.EndShaderAreaPixel();
        }

        public void DrawTrails(Asset<Texture2D> useTex, Color drawColor, float multipleSize = 1f, float alphaValue = 1f, float offsetHeight = 1f)
        {
            float laserLength = 50;
            LostbeltShaderAssets.StandardFlowShader.Parameters["LaserTextureSize"].SetValue(useTex.Size());
            LostbeltShaderAssets.StandardFlowShader.Parameters["targetSize"].SetValue(new Vector2(laserLength, useTex.Height()));
            LostbeltShaderAssets.StandardFlowShader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * -150f);
            LostbeltShaderAssets.StandardFlowShader.Parameters["uColor"].SetValue(drawColor.ToVector4() * .85f);
            LostbeltShaderAssets.StandardFlowShader.Parameters["uFadeoutLength"].SetValue(1.4f);
            LostbeltShaderAssets.StandardFlowShader.Parameters["uFadeinLength"].SetValue(0.05f);
            LostbeltShaderAssets.StandardFlowShader.CurrentTechnique.Passes[0].Apply();
            if (Projectile.oldPos.Length < 3)
                return;
            //做掉可能存在的零向量
            DrawSetting drawSetting = new DrawSetting(useTex.Value, true);
            List<TrailDrawDate> trailDrawDates = [];
            int posCount = Projectile.oldPos.Length;
            for (int j = 0; j < posCount - 1; j++)
            {
                if (Projectile.oldPos[j] == Vector2.Zero)
                    continue;
                float rot = Projectile.oldRot[j];
                Vector2 posOffset = Main.rand.NextVector2Circular(1.5f, 1.5f);
                trailDrawDates.Add(new(Projectile.oldPos[j] + Projectile.Size / 2, drawColor, new Vector2(0, 10 * multipleSize * Projectile.scale), rot));
            }
            TrailRender.DrawTrail([.. trailDrawDates], drawSetting);
        }

    }
}
