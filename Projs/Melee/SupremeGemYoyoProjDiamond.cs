using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.PixelatedRender;
using LostbeltTraveler.Core.Primitives.Trail;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Projs.Melee
{
    public class SupremeGemYoyoProjDiamond : LostbeltProjs, IPixelatedRenderer
    {
        public override string Texture => GetVanillaAssetPath(VanillaAsset.Item, ItemID.Diamond);
        public override EnumDamageClass Category => EnumDamageClass.Melee;
        public ref float Timer => ref Projectile.ai[0];
        public NPC CurTarget = null;
        public List<Vector2> OldPos = new List<Vector2>();
        public List<float> OldRot = new List<float>();

        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(24);
        }
        public override void ExSD()
        {
            Projectile.ignoreWater = true;
            Projectile.MaxUpdates = 4;
            Projectile.width = Projectile.height = 16;
            Projectile.timeLeft = 600;
            Projectile.SetupImmnuity(-1);
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;

        }
        public override void OnFirstFrame()
        {
            base.OnFirstFrame();
        }
        public void DiamondAI()
        {
            Timer++;
            Projectile.rotation = Projectile.velocity.ToRotation() + PiOver2;
            if (Projectile.damage != 0 && Projectile.timeLeft > 50)
            {
                Projectile.scale = Lerp(Projectile.scale, 1.01f, .12f);
                if (Projectile.scale > 1 && Timer > 45)
                {
                    if (CurTarget.IsLegal())
                    {
                        float speedValue = Projectile.velocity.Length();
                        float rotation = Projectile.velocity.ToRotation();
                        float angleTo = Projectile.AngleTo(CurTarget.Center);
                        float dist = Projectile.Distance(CurTarget.Center);
                        float r = dist * 0.35f / (float)Math.Abs(Math.Sin(rotation - angleTo));
                        if (Vector2.Dot(Projectile.velocity, Projectile.DirectionTo(CurTarget.Center)) < 0)
                        {
                            r = Clamp(r, 1, 240);
                        }
                        Projectile.velocity = Projectile.velocity.RotatedBy(-Math.Sign(WrapAngle(rotation - angleTo)) * speedValue / r);
                        if (Projectile.velocity.LengthSquared() < 8f * 8f)
                            Projectile.velocity *= 1.01f;
                        else
                            Projectile.velocity *= 0.9f;
                    }
                    else
                    {
                        if (Projectile.GetTargetSafe(out NPC tar, true, 1200f, true))
                        {
                            CurTarget = tar;
                        }
                        Projectile.velocity *= 0.9f;
                    }
                }
                else
                {
                    Projectile.velocity *= 0.96f;
                }
            }
            else
            {
                Projectile.scale = Lerp(Projectile.scale, 0f, .12f);
                if (Projectile.scale <= 0.2f)
                    Projectile.Kill();
            }

        }
        public override void ProjAI()
        {
            OldPos.Add(Projectile.Center);
            OldRot.Add(Projectile.velocity.ToRotation());
            if (OldPos.Count > 35)
            {
                OldPos.RemoveAt(0);
                OldRot.RemoveAt(0);
            }
            DiamondAI();
            if (Projectile.IsOutScreen())
                return;
            if (Main.rand.NextBool(19))
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center.ToRandCirclePosEdge(6), DustID.GemDiamond);
                d.velocity = Projectile.velocity / 4f;
                d.noGravity = true;
                d.scale *= Main.rand.NextFloat(.8f, 1.1f);
            }
            //    ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePosEdge(6), Projectile.velocity / 4f,
            //        RandLerpColor(Color.LightSkyBlue, Color.White), 45, 1, Projectile.scale * Main.rand.NextFloat(.75f, 1.15f) * .46f, .2f);
            //if (Main.rand.NextBool(19))
            //    ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), Projectile.velocity / 4f,
            //        Color.WhiteSmoke, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);


        }
        public override bool? CanDamage()
        {
            return Timer > 45;
        }
        public override void OnKill(int timeLeft)
        {
            float scale = .42f;
            float opac = .85f;
            ECSParticle.CrossGlow(Projectile.Center, Color.Crimson, 45, opac, scale, 0.1f, BlendState.Additive);
            ECSParticle.CrossGlow(Projectile.Center, Color.Red, 45, opac, scale * .95f, 0.1f, BlendState.Additive);
            ECSParticle.CrossGlow(Projectile.Center, Color.White, 45, opac, scale * .9f, 0.1f, BlendState.Additive);
            Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.ToSafeNormalize() * 10, ProjectileType<SupremeGemYoyoProjRuby>(), 0, Projectile.knockBack, Projectile.owner);


            for (int i = 0; i < 16; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePos(1.2f), RandVelTwoPi(0f, 9f), RandLerpColor(Color.Red, Color.Crimson), 45, 1, Main.rand.NextFloat(.85f, 1.15f) * .64f, 0.2f, BlendState.Additive);
            for (int i = 0; i < 8; i++)
                ECSParticle.SmokeParticle(Projectile.Center.ToRandCirclePos(2f), RandVelTwoPi(9f), RandLerpColor(Color.Red, Color.Crimson), 45, RandRotTwoPi, .51f, .41f, Main.rand.NextBool(), BlendState.Additive);
            for (int i = 0; i < 8; i++)
                ECSParticle.LiliesFire(Projectile.Center.ToRandCirclePos(2f), RandVelTwoPi(13f), RandLerpColor(Color.Red, Color.Crimson), 45, RandRotTwoPi, .51f, .31f, true, BlendState.Additive);
        }
        public ScarletDrawLayer LayerToRenderTo => ScarletDrawLayer.BeforeDusts;
        public BlendState BlendState => BlendState.Additive;
        public void RenderPixelated(SpriteBatch sb)
        {
            if (!Projectile.Lostbelt().FirstFrame)
                return;
            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            //不要试图理解这里的码，我测，我一直热重载调试出来的，直接拿过去抄然后自己改改参数就行了

            DrawTrails(LostbeltModAssets.Trail_TerraRayFlow.Texture, Color.RoyalBlue * 1f, 1.15f);
            DrawTrails(LostbeltModAssets.Trail_TerraRayFlow.Texture, Color.LightSkyBlue * .75f, 1f);
            DrawTrails(LostbeltModAssets.Trail_TerraRayFlow.Texture, Color.White * .45f, 0.85f, offsetHeight: 0f);
            LostbeltMethods.EndShaderAreaPixel();

        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (!Projectile.Lostbelt().FirstFrame)
                return false;
            PixelatedRenderManager.BeginDrawProj = true;
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            int length = Projectile.oldPos.Length - 8;
            for (int i = 0; i < length; i++)
            {
                float progress = (float)i / length;
                Vector2 oldPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                float oldRot = Projectile.oldRot[i] + PiOver2;
                Color c = Color.Lerp(Color.White, Color.LightSkyBlue, progress);
                float scale = (1f - (progress * 0.38f)) * Projectile.scale;
                float opac = Lerp(1f, 0f, progress) * .6f;
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(3, 3), c.ToAddColor(50) * .85f * opac * opac, oldRot, ori, scale * 1.1f, 0);
                Color c2 = Color.Lerp(Color.White, Color.LightSkyBlue, progress);
                SB.FastDraw(projTex, oldPos, c2.ToAddColor(0) * .85f * opac * opac, oldRot, ori, scale, 0);
            }
            for (int i = 0; i < 8; i++)
                SB.FastDraw(projTex, drawPos + Main.rand.NextVector2Circular(1.5f, 1.5f) +
                    (TwoPi / 8f * i).ToRotationVector2() * 1.5f, Color.AliceBlue.ToAddColor(), Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);

            return false;
        }
        public void DrawTrails(Asset<Texture2D> useTex, Color drawColor, float multipleSize = 1f, float alphaValue = 1f, float offsetHeight = 1f)
        {
            float laserLength = 150;
            Effect shader = LostbeltShaderAssets.TerrarRayLaser;
            shader.Parameters["LaserTextureSize"].SetValue(useTex.Size());
            shader.Parameters["targetSize"].SetValue(new Vector2(laserLength, useTex.Height()));
            shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * -150f);
            shader.Parameters["uColor"].SetValue(drawColor.ToVector4() * .8f);
            shader.Parameters["uFadeoutLength"].SetValue(0.8f);
            shader.Parameters["uFadeinLength"].SetValue(0.3f);
            shader.CurrentTechnique.Passes[0].Apply();
            if (OldPos.Count < 3)
                return;
            //做掉可能存在的零向量
            DrawSetting drawSetting = new DrawSetting(useTex.Value, true);
            List<TrailDrawDate> trailDrawDates = [];
            int posCount = OldPos.Count;
            for (int j = posCount - 1; j >= 0; j--)
            {
                if (OldPos[j] == Vector2.Zero)
                    continue;
                float rot = OldRot[j];
                Vector2 posOffset = Main.rand.NextVector2Circular(1.5f, 1.5f);
                trailDrawDates.Add(new(OldPos[j] + posOffset, drawColor, new Vector2(0, 50 * multipleSize * Projectile.scale), rot));
            }
            TrailRender.DrawTrail([.. trailDrawDates], drawSetting);
        }
    }
}
