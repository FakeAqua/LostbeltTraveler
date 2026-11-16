using LostbeltTraveler.Core.PixelatedRender;
using LostbeltTraveler.Globals.Classes;
using System;
using System.Collections.Generic;
using LostbeltTraveler.Core.Primitives.Trail;
using LostbeltTraveler.Globals.Database.Enums;
using ReLogic.Content;
using Terraria;
using LostbeltTraveler.Core.ParticleECS;


namespace LostbeltTraveler.Projs.Executor.Firearm
{
    public class OutlawSniperBulletExtra : LostbeltProjs, IPixelatedRenderer
    {
        public float StickProgress => Utils.GetLerpValue(0, 80, Projectile.timeLeft, true);
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public override EnumDamageClass Category => EnumDamageClass.Executor;
        public ref float Timer => ref Projectile.ai[0];
        public NPC CurTarget = null;
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(26);
        }
        public override void ExSD()
        {

            Projectile.width = Projectile.height = 16;
            Projectile.MaxUpdates = 5;
            Projectile.penetrate = 2;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
            Projectile.timeLeft = 600;
            Projectile.SetupImmnuity(30);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
        public override void ProjAI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Lighting.AddLight(Projectile.Center, Color.RoyalBlue.ToVector3());
                        if (Projectile.GetTargetSafe(out NPC target, searchDistance: 1200f) && CurTarget is null)
            {
                CurTarget = target;
            }
            CurTarget = CurTarget.IsLegal() ? CurTarget : null;
                            Timer++;
            if (Timer > 15f&&Projectile.damage!=0)
            {
                if (CurTarget.IsLegal())
                {
                    Projectile.timeLeft = GetSeconds(5);
                    float speedValue = Projectile.velocity.Length();
                    float rotation = Projectile.velocity.ToRotation();
                    float angleTo = Projectile.AngleTo(CurTarget.Center);
                    float dist = Projectile.Distance(CurTarget.Center);
                    float r = dist * .85f / (float)Math.Abs(Math.Sin(rotation - angleTo));
                    if (Vector2.Dot(Projectile.velocity, Projectile.DirectionTo(CurTarget.Center)) < 0)
                    {
                        r = Clamp(r, 1, 180);
                    }
                    Projectile.velocity = Projectile.velocity.RotatedBy(-Math.Sign(WrapAngle(rotation - angleTo)) * speedValue / r);
                    if (Projectile.velocity.LengthSquared() < 10f * 10f)
                        Projectile.velocity *= 1.1f;
                    else
                        Projectile.velocity *= 0.9f;

                }
                else
                {
                    if (Projectile.velocity.LengthSquared() > 10f * 10f)
                        Projectile.velocity *= 0.9f;
                    else
                        Projectile.velocity *= 1.1f;
                }
            }
            if (Projectile.damage==0)
            {
                if (Projectile.timeLeft > 25)
                    Projectile.timeLeft = 25;
                Projectile.velocity *= .925f;
                if(Projectile.damage==0)
                Projectile.velocity *= .5925f;
            }
            if (Projectile.IsOutScreen())
                return;

            if (Main.rand.NextBool(9))
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePosEdge(8), Projectile.velocity,
                    RandLerpColor(Color.Goldenrod, Color.Orange), 10, 1, .56f * StickProgress, 0.2f);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = oldVelocity;
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
        }
        public BlendState BlendState => BlendState.Additive;
        public ScarletDrawLayer LayerToRenderTo => ScarletDrawLayer.BeforeDusts;
        public void RenderPixelated(SpriteBatch spriteBatch)
        {
            float Progress = StickProgress;
            float overallScale = 1.75f*Progress;
            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            ////这里是强行使用ex98拼凑出来的子弹效果
            Texture2D tex = LostbeltModAssets.Specific_RocketTrail.Value;
            Texture2D projTex = tex;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Texture2D glowTex = LostbeltModAssets.Particle_CrossGlow.Value;
            float glowScale = Projectile.scale * .2f;
            SB.FastDraw(glowTex, drawPos, Color.OrangeRed, Projectile.rotation, glowTex.Size() / 2f, glowScale, 0);
            SB.FastDraw(glowTex, drawPos, Color.LightGoldenrodYellow, Projectile.rotation + PiOver2, glowTex.Size() / 2f, glowScale, 0);

            int length = Projectile.oldPos.Length;
            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], 0f);
                Vector2 oldPos = lerpPos - Main.screenPosition + Projectile.Size / 2f;
                float oldRot = Projectile.oldRot[i] + PiOver2;
                //图竖直
                float progress = i / (float)length;
                float xMult = Lerp(0.7f, .05f, (progress));
                float yMult = Lerp(1f, .2f, progress);
                Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale * 1.2f * overallScale;
                Color c = Color.Lerp(Color.OrangeRed, Color.Lerp(Color.Goldenrod, Color.DarkOrange, .75f), EaseInOutQuad(progress));
                float opac = Lerp(1f, .79f, EaseInOutExpo(progress));
                Color pixelColor = Color.Lerp(Color.OrangeRed, Color.Lerp(Color.LightGoldenrodYellow, Color.OrangeRed, 0.85f), EaseInOutExpo(progress));
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
            DrawTrails(LostbeltModAssets.Trail_ManaStreak.Texture, Color.OrangeRed, 10 * overallScale);
            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            DrawTrails(LostbeltModAssets.Trail_ManaStreak.Texture, Color.DarkOrange, 10 * overallScale);
            DrawTrails(LostbeltModAssets.Trail_ManaStreak.Texture, Color.White * .5f, 6 * overallScale);
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
