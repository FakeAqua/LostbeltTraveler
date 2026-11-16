using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.PixelatedRender;
using LostbeltTraveler.Core.ScreenEffect;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Graphics.Particles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace LostbeltTraveler.Projs.Executor.ColdSteel
{
    public class DragonBallChi : LostbeltProjs, IPixelatedRenderer
    {
        public override string Texture => LostbeltModAssets.Texture_Spirite.Path;
        public Vector2 BeginPos;
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(16);
        }
        public override void ExSD()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.noEnchantmentVisuals = true;
            Projectile.netImportant = true;
            Projectile.extraUpdates = 10;
            Projectile.timeLeft = 70 * Projectile.MaxUpdates;
            Projectile.localNPCHitCooldown = Projectile.MaxUpdates * 3;
            Projectile.Opacity = 1f;
            base.ExSD();
        }
        public override void OnFirstFrame()
        {
            BeginPos = Projectile.Center;
            for (int i = 0; i < 120; i++)
            {
                Vector2 BeginPos = Projectile.Center + new Vector2(1, 0).RotatedBy(Projectile.rotation) * 24 + Main.rand.NextVector2Circular(18, 18);
                Vector2 FirVel = Projectile.rotation.ToRotationVector2() * 32 * Main.rand.NextFloat(0, 1f);
                new TrailGlowBall(BeginPos, FirVel, Color.White * 0.5f, Main.rand.Next(45, 65), 0.2f, true).Spawn();
            }
            for (int i = 0; i < 60; i++)
            {
                Vector2 BeginPos = Projectile.Center + new Vector2(1, 0).RotatedBy(Projectile.rotation) * 24 + Main.rand.NextVector2Circular(18, 18);
                Vector2 FirVel = Projectile.rotation.ToRotationVector2() * -38 * Main.rand.NextFloat(0, 1f);
                new TrailGlowBall(BeginPos, FirVel, Color.White * 0.5f, Main.rand.Next(45, 65), 0.2f, true).Spawn();
            }
            for (int i = 0; i < 120; i++)
            {
                Vector2 BeginPos = Projectile.Center + new Vector2(1, 0).RotatedBy(Projectile.rotation) * 24 + Main.rand.NextVector2Circular(18, 18);
                Vector2 FirVel = Projectile.rotation.ToRotationVector2() * 68 * Main.rand.NextFloat(0, 1f) * 1.4f;
                new Fire(BeginPos, FirVel, Color.White, 90, Main.rand.NextFloat(MathHelper.TwoPi), 1f, Main.rand.NextFloat(0.1f, 0.2f)).Spawn();
            }
            for (int i = 0; i < 40; i++)
            {
                Vector2 BeginPos = Projectile.Center + new Vector2(1, 0).RotatedBy(Projectile.rotation) * 24 + Main.rand.NextVector2Circular(18, 18);
                Vector2 FirVel = Projectile.rotation.ToRotationVector2() * -48 * Main.rand.NextFloat(0, 1f) * 1.4f;
                new Fire(BeginPos, FirVel, Color.White, 90, Main.rand.NextFloat(MathHelper.TwoPi), 1f, Main.rand.NextFloat(0.2f, 0.4f)).Spawn();
            }
            ScreenShakeSystem.AddScreenShakes(Projectile.Center, 20f, 60, Projectile.rotation, 2f);
            ScarletSound(LostbeltSoundAssets.TheMars_Toss, Projectile.Center, .28f, 1, -.16f, .1f);
        }
        public override void ProjAI()
        {
            //Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.velocity *= .9f;
            int max = 50 * Projectile.MaxUpdates;
            if (Projectile.timeLeft < max)
            {
                float p = Projectile.timeLeft / (float)max;
                Projectile.Opacity = Lerp(0, 1, EaseInCubic(p));
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return base.OnTileCollide(oldVelocity);
        }
        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            ScarletSound(LostbeltSoundAssets.Dialectics_Hit, Projectile.Center, 0.25f, 1, .45f);
            for (int i = 0; i < 2; i++)
            {
                ECSParticle.Stain(target.Center.ToRandCirclePos(16), Projectile.rotation.ToRotationVector2() * Main.rand.NextFloat(-3f, 3.1f), Color.White, 40, 1, Projectile.rotation, 1, null, 1.1f, .9f);
            }
            for (int i = -1; i < 2; i++)
            {
                Vector2 BeginPos = target.Center + new Vector2(1, 0).RotatedBy(Projectile.rotation) * 24 + Main.rand.NextVector2Circular(18, 18);
                Vector2 FirVel = Projectile.rotation.ToRotationVector2() * -7 * i * Main.rand.NextFloat(0, 1f);
                new TrailGlowBall(BeginPos, FirVel, Color.White * 0.5f, Main.rand.Next(45, 65), 0.2f, true).Spawn();
                BeginPos = target.Center + new Vector2(1, 0).RotatedBy(Projectile.rotation) * 24 + Main.rand.NextVector2Circular(18, 18);
                FirVel = Projectile.rotation.ToRotationVector2() * 10 * i * Main.rand.NextFloat(0, 1f) * 1.4f;
                new Fire(BeginPos, FirVel, Color.White, 90, Main.rand.NextFloat(MathHelper.TwoPi), 1f, Main.rand.NextFloat(0.1f, 0.2f)).Spawn();

            }

            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Projectile.Opacity < .5f)
                return false;
            float _ = float.NaN;
            Vector2 dir = Projectile.rotation.ToRotationVector2();
            Vector2 beamBeginPos = Projectile.Center - dir * 800f * Projectile.scale;
            Vector2 beamEndPos = Projectile.Center + Projectile.rotation.ToRotationVector2() * Projectile.scale * 800f;
            bool c = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), beamBeginPos, beamEndPos, 30f, ref _);
            return c;
        }
        public ScarletDrawLayer LayerToRenderTo => ScarletDrawLayer.BeforeDusts;
        public BlendState BlendState => BlendState.Additive;
        public void RenderPixelated(SpriteBatch sb)
        {

            //LAPContent.ReSetToEndShader_Pixel();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (!Projectile.Lostbelt().FirstFrame)
                return false;
            SB.EnterShaderArea();
            float ProjRotOffset = 0;
            Texture2D texture = LostbeltModAssets.Texture_LiliesHoodTrail.Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition + Vector2.UnitX.RotatedBy(Projectile.rotation + ProjRotOffset) * 600;
            Vector2 orig = new Vector2(texture.Width, texture.Height / 2);
            Main.spriteBatch.Draw(texture, drawPosition, null, Color.White * 0.755f * Projectile.Opacity, Projectile.rotation + ProjRotOffset, orig, new Vector2(5f, 0.4f), SpriteEffects.FlipHorizontally, 0f);

            Main.spriteBatch.Draw(texture, drawPosition, null, Color.White * 0.4f * Projectile.Opacity, Projectile.rotation + ProjRotOffset, orig, new Vector2(4f, 0.1f), SpriteEffects.FlipHorizontally, 0f);


            Vector2 drawPosition2 = Projectile.Center - Main.screenPosition + Vector2.UnitX.RotatedBy(Projectile.rotation + ProjRotOffset) * 600 - Vector2.UnitY * 10 * Owner.direction;

            Texture2D texture2 = LostbeltModAssets.Noise_Aura.Value;
            LostbeltMethods.ApplyAlphaCut(new Vector4(.2f, .3f, .2f, .2f), new Vector2(Main.GlobalTimeWrappedHourly * .5f, 0), new Vector2(2, .2f), Color.White);
            Main.spriteBatch.Draw(texture2, drawPosition2, null, Color.White * Projectile.Opacity, Projectile.rotation + ProjRotOffset, orig, new Vector2(5f, 0.2f), SpriteEffects.FlipHorizontally, 0f);

            LostbeltMethods.ApplyAlphaCut(new Vector4(.2f, .3f, .2f, .2f), new Vector2(Main.GlobalTimeWrappedHourly * .5f, 0), new Vector2(2.5f, 1), Color.White);
            Texture2D texture3 = LostbeltModAssets.Noise_Aura.Value;

            Main.spriteBatch.Draw(texture3, drawPosition2, null, Color.White * Projectile.Opacity, Projectile.rotation + ProjRotOffset, orig, new Vector2(4f, 0.2f), SpriteEffects.FlipHorizontally, 0f);
            SB.EndShaderArea();
            return false;
        }
    }
}
