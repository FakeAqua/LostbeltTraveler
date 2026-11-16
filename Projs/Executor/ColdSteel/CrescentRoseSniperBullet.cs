using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.PixelatedRender;
using LostbeltTraveler.Core.Primitives.Trail;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Projs.Executor.ColdSteel
{
    public class CrescentRoseSniperBullet : LostbeltProjs, IPixelatedRenderer
    {
        public override EnumDamageClass Category => EnumDamageClass.Executor;
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(32);
        }
        public override void ExSD()
        {
                        Projectile.width = Projectile.height = 16;
            Projectile.MaxUpdates = 5;
            Projectile.penetrate = 3;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
            Projectile.timeLeft = 600;
            Projectile.SetupImmnuity(30);
        }
        public override void OnFirstFrame()
        {
            base.OnFirstFrame();
        }
        public override void ProjAI()
        {
                        Projectile.rotation = Projectile.velocity.ToRotation();
            Lighting.AddLight(Projectile.Center, Color.OrangeRed.ToVector3());
            if (!Projectile.tileCollide)
            {
                Projectile.velocity *= .95f;
            }
            if (Projectile.IsOutScreen())
                return;

            if (Main.rand.NextBool(9))
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePosEdge(8), Projectile.velocity / 6f,
                    RandLerpColor(Color.Red, Color.Crimson), 50, 1, .56f, 0.2f);
                            if (Main.rand.NextBool(3))
                    ECSParticle.SmokeParticle(Projectile.Center.ToRandCirclePos(16), Projectile.velocity / 6f,
                        RandLerpColor(Color.OrangeRed, Color.Crimson), 45, RandRotTwoPi, 1, Main.rand.NextFloat(.9f, 1.1f) * .34f, false, blendstate: BlendState.Additive);
            {
                if (Main.rand.NextBool(3))
                {
                    Vector2 pos = Projectile.Center.ToRandCirclePosEdge(24);
                    ECSParticle.TurbulenceShinyOrb(pos, 1.5f, RandLerpColor(Color.Red, Color.Crimson), 45, 1, .1f, glowMult: .4f);
                    ECSParticle.ShrinkParticle(pos, Projectile.velocity / 4f, RandLerpColor(Color.Red, Color.Crimson), 45, .51f, Projectile.velocity.ToRotation(), .6f, 1);
                }
                if (Main.rand.NextBool(2))
                    ECSParticle.LightntingGlow(Projectile.Center.ToRandCirclePosEdge(10), Projectile.velocity, RandLerpColor(Color.Red, Color.MediumVioletRed), Main.rand.Next(20, 30), 1f, Main.rand.NextFloat(.75f, 1.1f) * .55f, 2);
            }
            base.ProjAI();
        }
        public BlendState BlendState => BlendState.Additive;
        public ScarletDrawLayer LayerToRenderTo => ScarletDrawLayer.BeforeDusts;
        public void RenderPixelated(SpriteBatch spriteBatch)
        {
            float Progress = 1;
            float overallScale = 1.75f*Progress;
            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            ////这里是强行使用ex98拼凑出来的子弹效果
            Texture2D tex = LostbeltModAssets.Specific_RocketTrail.Value;
            Texture2D projTex = tex;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Texture2D glowTex = LostbeltModAssets.Particle_CrossGlow.Value;
            float glowScale = Projectile.scale * .2f;
            SB.FastDraw(glowTex, drawPos, Color.Crimson, Projectile.rotation, glowTex.Size() / 2f, glowScale, 0);
            SB.FastDraw(glowTex, drawPos, Color.White, Projectile.rotation + PiOver2, glowTex.Size() / 2f, glowScale, 0);

            int length = Projectile.oldPos.Length;
            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], .2f);
                Vector2 oldPos = lerpPos - Main.screenPosition + Projectile.Size / 2f;
                float oldRot = Projectile.oldRot[i] + PiOver2;
                //图竖直
                float progress = i / (float)length;
                float xMult = Lerp(0.7f, .05f, (progress));
                float yMult = Lerp(1f, .2f, progress);
                Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale * 1.2f * overallScale;
                Color c = Color.Lerp(Color.DarkRed, Color.Lerp(Color.Crimson, Color.Violet, .75f), EaseInOutQuad(progress));
                float opac = Lerp(1f, .79f, EaseInOutExpo(progress));
                Color pixelColor = Color.Lerp(Color.Crimson, Color.Lerp(Color.Red, Color.MediumVioletRed, 0.85f), EaseInOutExpo(progress));
                int by = (int)Lerp(50, 0, progress);
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(1.5f, 1.5f), 
                    c.ToAddColor(0) * opac * 1.2f, oldRot, projTex.Size() / 2f, scale * 1.1f, 0);
                //这里重复多画一次。
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(1.5f, 1.5f), 
                    c.ToAddColor((byte)by) * opac * .6f, oldRot, projTex.Size() / 2f, scale, 0);
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(0.5f, 0.5f),
                    pixelColor.ToAddColor(100) * opac, oldRot, projTex.Size() / 2f, scale, 0);
            }
            LostbeltMethods.EnterShaderAreaPixel(BlendState.NonPremultiplied);
            DrawTrails(LostbeltModAssets.Trail_ManaStreak.Texture, Color.DarkRed, 10 * overallScale);
            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            DrawTrails(LostbeltModAssets.Trail_ManaStreak.Texture, Color.PaleVioletRed*1f, 10 * overallScale);
            DrawTrails(LostbeltModAssets.Trail_ManaStreak.Texture, Color.White * .75f, 6 * overallScale);
            LostbeltMethods.EndShaderAreaPixel();
        }
                public void DrawTrails(Asset<Texture2D> useTex, Color drawColor, float multipleSize = 1f, float alphaValue = 1f, float offsetHeight = 1f)
        {
            float laserLength = (int)Projectile.localAI[1];
            Effect shader = LostbeltShaderAssets.StandardFlowShader;
            shader.Parameters["LaserTextureSize"].SetValue(useTex.Value.Size());
            shader.Parameters["targetSize"].SetValue(new Vector2(laserLength, useTex.Value.Height));
            shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * -100);
            shader.Parameters["uColor"].SetValue(drawColor.ToVector4() * 1);
            shader.Parameters["uFadeoutLength"].SetValue(1.13f);
            shader.Parameters["uFadeinLength"].SetValue(0.01f);
            shader.CurrentTechnique.Passes[0].Apply();

            DrawSetting sets = new(useTex.Value);
            List<TrailDrawDate> date = [];
            for (int i = 0; i < (Projectile.oldPos.Length); i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    continue;
                Vector2 listPos = Projectile.oldPos[i] + Projectile.Size / 2;
                date.Add(new(listPos, Color.White, new(0, multipleSize), Projectile.oldRot[i]));
            }
            TrailRender.DrawTrail(date.ToArray(), sets);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            PixelatedRenderManager.BeginDrawProj = true;
            return false;
        }
    }
}
