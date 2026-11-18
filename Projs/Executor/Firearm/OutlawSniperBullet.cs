using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.PixelatedRender;
using LostbeltTraveler.Core.Primitives.Trail;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Players.InnerCooldown.Methods;
using LostbeltTraveler.Items.Weapons.Executor.Firearm;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace LostbeltTraveler.Projs.Executor.Firearm
{
    public class OutlawSniperBullet : LostbeltProjs, IPixelatedRenderer
    {
        public float StickProgress => Utils.GetLerpValue(0, 80, Projectile.timeLeft, true);
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public override EnumDamageClass Category => EnumDamageClass.Executor;
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
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.numHits < 1)
            {
                Owner.AddInnerCooldown(InnerCooldownType.OutlawSniperMulter, 10);
                if (Owner.Lostbelt().outlawSniperStack <= OutlawSniper.MaxBuffTime)
                    Owner.Lostbelt().outlawSniperStack += 1;
            }
            for (int i = 0; i < 16; i++)
            {
                Vector2 pos = Projectile.Center.ToRandCirclePosEdge(8);
                Vector2 vel = Projectile.SafeDir().ToRandVelocity(ToRadians(0), 1.2f, 18f);
                ECSParticle.LightntingGlow(pos, vel, RandLerpColor(Color.OrangeRed, Color.Goldenrod), Main.rand.Next(20, 30), 1f, Main.rand.NextFloat(.75f, 1.1f) * .55f, 2);
            }

            for (int i = 0; i < 16; i++)
            {
                Vector2 pos = Projectile.Center.ToRandCirclePos(2);
                Vector2 vel = Projectile.SafeDir().ToRandVelocity(ToRadians(15), 1.2f, 11.8f);
                ECSParticle.SmokeParticle(pos, vel, RandLerpColor(Color.OrangeRed, Color.Goldenrod), Main.rand.Next(35, 55), RandRotTwoPi, .45f, 0.30f * Main.rand.NextFloat(.7f, 1.1f), true, BlendState.Additive);
            }
            for (int i = 0; i < 16; i++)
            {
                Vector2 pos = Projectile.Center.ToRandCirclePos(2);
                Vector2 vel = Projectile.SafeDir().ToRandVelocity(ToRadians(15), 1.2f, 11.8f);
                ECSParticle.ShinyCrossStarECS(pos, vel, RandLerpColor(Color.OrangeRed, Color.Goldenrod), 35, .585f, 0.45f * Main.rand.NextFloat(.75f, 1f), 0.2f);
            }

            if (!Projectile.Lostbelt().ExecutionStrike)
                Projectile.AddExecutionTimeImmediate<OutlawSniper>();
            else
            {
                ScarletSound(LostbeltSoundAssets.Misc_ManaTransform, target.Center);
                for (int i = 0; i < 5; i++)
                {
                    float args = (TwoPi / 5f * i);
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), target.Center, args.ToRotationVector2() * 10f,
                        ProjectileType<OutlawSniperBulletExtra>(), Projectile.originalDamage / 2, Projectile.knockBack, Projectile.owner);
                }
                for (int i = 0; i < 16; i++)
                {
                    ECSParticle.SmokeParticle(target.Center, RandVelTwoPi(.2f, 12f), RandLerpColor(Color.Orange, Color.DarkGoldenrod),
                        Main.rand.Next(35, 45), RandRotTwoPi, 1, Main.rand.NextFloat(.9f, 1.1f) * .435f, Main.rand.NextBool(), BlendState.AlphaBlend);
                }
                for (int i = 0; i < 16; i++)
                {
                    ECSParticle.ShinyCrossStarECS(target.Center.ToRandCirclePos(5), RandVelTwoPi(.2f, 8f), RandLerpColor(Color.Orange, Color.Goldenrod),
                        Main.rand.Next(30, 50), 1, Main.rand.NextFloat(.9f, 1.1f) * .88f, .2f);
                }
            }

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
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePos(8), Projectile.velocity / 8f,
                    RandLerpColor(Color.LightGoldenrodYellow, Color.Orange), 50, 1, RandRotTwoPi, 0.64f * StickProgress, 0, Main.rand.NextFloat(-.1f, .1f), 0.4f);

            if (Main.rand.NextBool(9))
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePosEdge(8), Projectile.velocity / 6f,
                    RandLerpColor(Color.Goldenrod, Color.Orange), 50, 1, .56f * StickProgress, 0.2f);
            if (Projectile.Lostbelt().ExecutionStrike)
            {
                if (Main.rand.NextBool(6))
                {
                    Dust d = Dust.NewDustPerfect(Projectile.Center.ToRandCirclePos(24), DustID.OrangeTorch);
                    d.velocity = Projectile.velocity / 4f;
                    d.scale = 1f;
                    d.noGravity = true;
                }
                if (Main.rand.NextBool(5))
                {
                    Vector2 pos = Projectile.Center.ToRandCirclePosEdge(24);
                    ECSParticle.TurbulenceShinyOrb(pos, 1.5f, RandLerpColor(Color.Orange, Color.OrangeRed), 45, 1, .1f, glowMult: .4f);
                    ECSParticle.ShrinkParticle(pos, Projectile.velocity / 4f, RandLerpColor(Color.Orange, Color.OrangeRed), 45, .51f, Projectile.velocity.ToRotation(), .6f, 1);
                }
                if (Main.rand.NextBool(2))
                    ECSParticle.LightntingGlow(Projectile.Center.ToRandCirclePosEdge(10), Projectile.velocity, RandLerpColor(Color.OrangeRed, Color.Goldenrod), Main.rand.Next(20, 30), 1f, Main.rand.NextFloat(.75f, 1.1f) * .55f, 2);
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.tileCollide = false;
            Projectile.velocity = oldVelocity;
            if (Projectile.timeLeft > 50)
                Projectile.timeLeft = 50;
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
            float overallScale = 1.75f * Progress;
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
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], .2f);
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
