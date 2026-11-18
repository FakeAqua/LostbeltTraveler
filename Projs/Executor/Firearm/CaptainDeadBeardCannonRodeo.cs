using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.PixelatedRender;
using LostbeltTraveler.Core.Primitives.Trail;
using LostbeltTraveler.Core.ScreenEffect;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Projs.Melee;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Projs.Executor.Firearm
{
    public class CaptainDeadBeardCannonRodeo : LostbeltProjs, IPixelatedRenderer
    {
        public ref float Timer => ref Projectile.ai[0];
        public bool HitTile = false;
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(32);
        }
        public override void ExSD()
        {
            Projectile.MaxUpdates = 4;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600 * Projectile.MaxUpdates;
            Projectile.penetrate = -1;
            Projectile.SetupImmnuity(30);
        }
        public override void OnFirstFrame()
        {
            base.OnFirstFrame();
        }
        public override void ProjAI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            //复制原版双足翼龙怒焰的AI
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 10f)
            {
                Projectile.velocity.Y += 0.1f;
            }
            if (Projectile.ai[0] >= 20f)
            {
                Projectile.velocity.Y += 0.15f;
            }
            if (Projectile.ai[0] > 20f)
            {
                Projectile.ai[0] = 20f;
            }
            Projectile.velocity.X *= 0.99f;
            if (Projectile.velocity.Y > 32f)
            {
                Projectile.velocity.Y = 32f;
            }
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
            if (!HitTile)
            {
                HitTile = true;
                Projectile.velocity *= 0f;
                //嵌入进去
                Projectile.position += oldVelocity.ToSafeNormalize() * 10f;
                ScreenShakeSystem.AddScreenShakes(Projectile.Center, 30, 30, RandRotTwoPi);
                ScarletSound(LostbeltSoundAssets.Smash_GroundHeavy, Projectile.Center);
                Projectile pro = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<SpearofEscapeBoom>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                pro.DamageType = GetInstance<ExecutorDamageClass>();
            }
            else
                Projectile.velocity *= .9f;
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if (!HitTile)
                overWiresUI.Add(index);
        }
        public float OverallOpa = 1;
        public ScarletDrawLayer LayerToRenderTo => ScarletDrawLayer.BeforePlayer;
        public BlendState BlendState => BlendState.Additive;
        public void RenderPixelated(SpriteBatch sb)
        {
            if (Projectile.IsOutScreen())
                return;
            if (!Projectile.Lostbelt().FirstFrame)
                return;
            if (HitTile)
                OverallOpa = Lerp(OverallOpa, 0f, .12f);
            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            DrawTrails(LostbeltModAssets.Trail_MagmaStreak.Texture, Color.Orange * .6f, 4.05f, 1f);
            DrawTrails(LostbeltModAssets.Trail_FadedStreak.Texture, Color.OrangeRed * .95f, 3.95f, 1f, 0.8f);
            DrawTrails(LostbeltModAssets.Trail_Lightning0.Texture, Color.OrangeRed * .82f, 3.75f, 1.2f);
            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            Texture2D line = LostbeltModAssets.Specific_RocketTrail.Value;
            Vector2 offset2 = Projectile.velocity.ToSafeNormalize() * 0f;
            int length = (int)(Projectile.oldRot.Length * OverallOpa);
            for (int i = 0; i < length; i++)
            {
                float progress = 1 - (float)i / length;
                Color col = Color.Lerp(Color.OrangeRed, Color.Orange * 1f, progress) * 1f;
                Vector2 lineScale = new Vector2(0.405f * 1, 1.1f * progress);
                float rot = Projectile.oldRot[i] + PiOver2;
                Vector2 AfterImagePos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                Vector2 offset = Main.rand.NextVector2Circular(2.5f, 2f + (2f * (1f - progress))).RotatedBy(Projectile.velocity.ToRotation()) - offset2;
                Vector2 innerScale = new Vector2(0.405f * 1, 1.1f * progress);
                SB.FastDraw(line, AfterImagePos + offset, col * 0.45f * progress * 1f * OverallOpa,
                    rot, line.Size() / 2f, lineScale * 1.25f * 1.5f);
                SB.FastDraw(line, AfterImagePos + offset, Color.White * 0.405f * progress * 1f * OverallOpa,
                   rot, line.Size() / 2f, innerScale * 1.5f);
            }

            LostbeltMethods.EndShaderAreaPixel();
        }
        public void DrawTrails(Asset<Texture2D> useTex, Color drawColor, float multipleSize = 1f, float alphaValue = 1f, float laserLengthMult = 1f)
        {
            float laserLength = 400 * laserLengthMult;
            LostbeltShaderAssets.StandardFlowShader.Parameters["LaserTextureSize"].SetValue(useTex.Size());
            LostbeltShaderAssets.StandardFlowShader.Parameters["targetSize"].SetValue(new Vector2(laserLength, useTex.Height()));
            LostbeltShaderAssets.StandardFlowShader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * -130f * alphaValue);
            LostbeltShaderAssets.StandardFlowShader.Parameters["uColor"].SetValue(drawColor.ToVector4() * OverallOpa);
            LostbeltShaderAssets.StandardFlowShader.Parameters["uFadeoutLength"].SetValue(0.9f);
            LostbeltShaderAssets.StandardFlowShader.Parameters["uFadeinLength"].SetValue(0.1f);
            LostbeltShaderAssets.StandardFlowShader.CurrentTechnique.Passes[0].Apply();
            if (Projectile.oldPos.Length < 3)
                return;
            //做掉可能存在的零向量
            DrawSetting drawSetting = new DrawSetting(useTex.Value, true);
            List<TrailDrawDate> trailDrawDates = [];
            int posCount = (int)(Projectile.oldPos.Length * OverallOpa);
            for (int j = 0; j < posCount - 1; j++)
            {
                if (Projectile.oldPos[j] == Vector2.Zero)
                    continue;
                float rot = Projectile.oldRot[j];
                Vector2 posOffset = Main.rand.NextVector2Circular(1.5f, 1.5f);
                trailDrawDates.Add(new(Projectile.oldPos[j] + Projectile.SafeDir() * 15f + Projectile.Size / 2, drawColor, new Vector2(0, 7 * multipleSize * Projectile.scale), rot));
            }
            TrailRender.DrawTrail([.. trailDrawDates], drawSetting);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.IsOutScreen())
                return false;
            if (!Projectile.Lostbelt().FirstFrame)
                return false;
            PixelatedRenderManager.BeginDrawProj = true;
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            SB.EnterShaderArea();
            Texture2D tex2 = LostbeltModAssets.Particle_ShinyOrb.Value;
            SB.FastDraw(tex2, drawPos, Color.OrangeRed, 0, tex2.Size() / 2f, Projectile.scale * 1.9f);
            SB.EndShaderArea();
            for (int i = 0; i < 3; i++)
                SB.FastDraw(projTex, drawPos.ToRandCirclePos(3), Color.Orange.ToAddColor(), Projectile.rotation, ori, Projectile.scale * 1.1f);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, ori, Projectile.scale);
            return false;
        }


    }
}
