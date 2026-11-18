using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.PixelatedRender;
using LostbeltTraveler.Core.Primitives.Trail;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Items.Weapons.Melee;
using System.Collections.Generic;
using Terraria;

namespace LostbeltTraveler.Projs.Melee
{
    public class RaceAgainstTimeProj : LostbeltProjs, IPixelatedRenderer
    {
        public override EnumDamageClass Category => EnumDamageClass.Melee;
        public override string Texture => GetInstance<RaceAgainstTime>().Texture;
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(20);
        }
        public override void ExSD()
        {
            Projectile.MaxUpdates = 4;
            Projectile.width = Projectile.height = 30;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 2;
            Projectile.SetupImmnuity(1);
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
            Projectile.timeLeft = 600 * Projectile.MaxUpdates;
        }
        public float LerpProgress => Clamp((float)Projectile.timeLeft / (30 * Projectile.MaxUpdates), 0, 1);
        public override void ProjAI()
        {
            int lifeTime = Projectile.MaxUpdates * 30;
            if (Projectile.timeLeft > lifeTime && Projectile.damage == 0)
            {
                Projectile.timeLeft = lifeTime;
            }
            if (Projectile.timeLeft <= lifeTime)
            {
                Projectile.velocity *= .95f;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.IsOutScreen() && Main.rand.NextFloat() > LerpProgress)
                return;
            if (Main.rand.NextBool(8))
                ECSParticle.ShinyCrossStarSmall(Projectile.Center.ToRandCirclePosEdge(6), Projectile.velocity / 6f,
                    RandLerpColor(Color.Goldenrod, Color.LightGoldenrodYellow), 40, 1, Main.rand.NextFloat(.9f, 1.1f) * .3f * LerpProgress * LerpProgress, Main.rand.NextFloat(-.1f, .1f));

        }
        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.penetrate == 1)
            {
                ScarletSound(LostbeltSoundAssets.TheMars_Hit, target.Center, pitch: -.5f);
                for (int i = 0; i < 3; i++)
                {
                    ECSParticle.ShinyCrossStarECS(target.Center.ToRandCirclePosEdge(8), RandVelTwoPi(1.2f, 7.6f),
                        RandLerpColor(Color.DarkGoldenrod, Color.LightGoldenrodYellow), 40, 1, Main.rand.NextFloat(.9f, 1.1f) * .8f, .2f);
                }
                for (int i = 0; i < 10; i++)
                {
                    ECSParticle.TurbulenceShinyOrb(target.Center.ToRandCirclePos(50), 2.2f, RandLerpColor(Color.DarkGoldenrod, Color.LightGoldenrodYellow), 45, 1, 0.13f, glowMult: .3f);
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return base.OnTileCollide(oldVelocity);
        }
        public void RenderPixelated(SpriteBatch spriteBatch)
        {
            if (!Projectile.Lostbelt().FirstFrame)
                return;

            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            ////这里是强行使用ex98拼凑出来的子弹效果
            Texture2D tex = LostbeltModAssets.Particle_SharpTear;
            Texture2D projTex = tex;
            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);

            int length = (int)((Projectile.oldPos.Length - 3) * LerpProgress);
            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], .1f);
                Vector2 oldPos = lerpPos - Main.screenPosition + Projectile.Size / 2f;
                float oldRot = Projectile.oldRot[i] + PiOver2;
                //图竖直
                float progress = i / (float)length;
                float xMult = Lerp(.9f, .5f, (progress));
                float yMult = Lerp(1.2f, .2f, progress);
                Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale * 1f;
                Color c = Color.Lerp(Color.DarkGoldenrod, Color.Lerp(Color.Gold, Color.White, .75f), EaseInOutQuad(progress));
                float opac = Lerp(1f, .79f, EaseInOutExpo(progress)) * LerpProgress * LerpProgress;
                Color pixelColor = Color.Lerp(Color.White, Color.Lerp(Color.Gold, Color.Goldenrod, 0.85f), EaseInOutExpo(progress));
                int by = (int)Lerp(50, 0, progress);
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(1.5f, 1.5f), c.ToAddColor(0) * opac * 1.2f, oldRot, projTex.Size() / 2f, scale * 1.1f, 0);
                //这里重复多画一次。
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(1.5f, 1.5f), c.ToAddColor((byte)by) * opac * .6f, oldRot, projTex.Size() / 2f, scale, 0);
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(0.5f, 0.5f), pixelColor.ToAddColor(100) * opac, oldRot, projTex.Size() / 2f, scale, 0);
            }
            LostbeltMethods.EndShaderAreaPixel();
        }
        public void TrailFunc(Texture2D tex, Color color, float primitiveHeight = 30, float heightPosOffset = 0f)
        {
            float laserLength = (int)Projectile.localAI[1];
            Effect shader = LostbeltShaderAssets.StandardFlowShader;
            shader.Parameters["LaserTextureSize"].SetValue(tex.Size());
            shader.Parameters["targetSize"].SetValue(new Vector2(laserLength, tex.Height));
            shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * -100);
            shader.Parameters["uColor"].SetValue(color.ToVector4() * 1 * LerpProgress);
            shader.Parameters["uFadeoutLength"].SetValue(1.13f);
            shader.Parameters["uFadeinLength"].SetValue(0.2f);
            shader.CurrentTechnique.Passes[0].Apply();

            DrawSetting sets = new(tex);
            List<TrailDrawDate> date = [];
            for (int i = 0; i < (Projectile.oldPos.Length); i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    continue;
                Vector2 listPos = Projectile.oldPos[i] + Projectile.Size / 2;
                date.Add(new(listPos, Color.White, new(0, primitiveHeight * 1.2f), Projectile.oldRot[i]));
            }
            TrailRender.DrawTrail(date.ToArray(), sets);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.IsOutScreen())
                return false;
            if (!Projectile.Lostbelt().FirstFrame)
                return false;
            PixelatedRenderManager.BeginDrawProj = true;
            Texture2D tex = Projectile.GetTexture();
            Texture2D sharp = LostbeltModAssets.Particle_SharpTear;
            Vector2 pos = Projectile.Center - Main.screenPosition;
            float rotFix = (Projectile.spriteDirection == -1 ? PiOver2 + PiOver4 : PiOver4);
            float rot = Projectile.rotation + rotFix;
            Vector2 rotPoint = tex.Size() / 2f;
            SpriteEffects se = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            int length = (int)((Projectile.oldPos.Length - 5) * LerpProgress);
            for (int i = 0; i < length; i++)
            {
                float progress = (float)i / length;
                Vector2 oldPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                float oldRot = Projectile.oldRot[i] + rotFix;
                Color sharpColor = Color.Lerp(Color.RoyalBlue, Color.CornflowerBlue, progress);
                Color c = Color.Lerp(Color.DarkGoldenrod, Color.Goldenrod, progress);
                float scale = Lerp(1f, .45f, progress) * .60f;
                float opac = Lerp(1f, 0f, progress);
                Vector2 sharpScale = new Vector2(Lerp(.75f, .25f, progress), Lerp(1.35f, 0.35f, progress));
                SB.FastDraw(tex, oldPos + Main.rand.NextVector2Circular(3, 3), c.ToAddColor(200) * .7f * opac, oldRot, rotPoint, scale * 1.1f, se);
                Color c2 = Color.Lerp(Color.DarkGoldenrod, Color.White, progress);
                SB.FastDraw(tex, oldPos, c2.ToAddColor(200) * 1f * opac * opac, oldRot, rotPoint, scale, se);
            }
            SB.EnterShaderArea();
            Texture2D orb = LostbeltModAssets.Particle_KiraStar.Value;
            float scale2 = Projectile.scale * .5f * LerpProgress;
            SB.FastDraw(orb, pos, Color.DarkGoldenrod, 0, orb.Size() / 2f, scale2, 0, 0);
            SB.FastDraw(orb, pos, Color.White, 0, orb.Size() / 2f, scale2 * .9f, 0, 0);
            SB.EndShaderArea();
            for (int i = 0; i < 8; i++)
            {
                SB.FastDraw(tex, pos + (TwoPi / 8 * i).ToRotationVector2() * 1.5f + Main.rand.NextVector2Circular(3, 3), Color.DarkGoldenrod.ToAddColor() * LerpProgress, rot, rotPoint, Projectile.scale * LerpProgress, se);
            }
            SB.FastDraw(tex, pos, Color.White * LerpProgress * LerpProgress, rot, rotPoint, Projectile.scale, se);
            return false;
        }
    }
}
