using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.Primitives.Trail;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Projs.Melee
{
    public class OnyxYoyoProj : GemYoyoProj
    {
        public ref float BoosterTime => ref Projectile.ai[2];
        protected override float YoyoLifeTime => -1;
        protected override int YoyoMaxUpdates => 9;
        protected override float YoyoTopSpeed => 16;
        protected override float YoyoLength => LostbeltMethods.TilePixel(29.175f);
        public override bool ForceOnyx => true;
        public int OnyxTimer = 0;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Type] = YoyoLifeTime;
            ProjectileID.Sets.YoyosMaximumRange[Type] = YoyoLength;
            ProjectileID.Sets.YoyosTopSpeed[Type] = YoyoTopSpeed / 3;
            Projectile.ToTrailSetting(36);
        }
        public override void ExSD()
        {
            base.ExSD();
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Rectangle rec = Utils.CenteredRectangle(Projectile.Center, new Vector2(64));
            return rec.Intersects(targetHitbox);
        }
        public override void ProjAI()
        {
            base.ProjAI();
            Lighting.AddLight(Projectile.Center, TorchID.Corrupt);
            if (Projectile.velocity.LengthSquared() > 1f * 1f)
            {
                ECSParticle.StarShape(Projectile.Center.ToRandCirclePosEdge(0) - Projectile.velocity * 4, Projectile.velocity / 2f, Color.DarkViolet, 45, .75f, .6f, 0f, BlendState.NonPremultiplied);
            }
            else
                ECSParticle.LiliesFire(Projectile.Center.ToRandCirclePos(8), RandVelTwoPi(1, 2) * 2, RandLerpColor(Color.DarkViolet, Color.Black), 40, RandRotTwoPi, 1, 0.1f, true, BlendState.NonPremultiplied);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            ECSParticle.LightntingGlow(target.Center, RandRotTwoPi.ToRotationVector2() * .01f, Color.DarkViolet, 40, 1, 0.6f, blendstate: BlendState.NonPremultiplied);
            for (int i = 0; i < 8; i++)
                ECSParticle.ShinyCrossStarECS(target.Center, RandVelTwoPi(3), RandLerpColor(Color.Violet, Color.DarkViolet), 45, 1, Main.rand.NextFloat(.9f, 1.1f) * .4f, .2f);
            ScarletSound(LostbeltSoundAssets.TheSevenStar_Hit, target.Center);
            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            Color innerColor = Color.Lerp(Color.Purple, Color.White, .025f) * .7f;
            Color outterColor = Color.DarkViolet * .55f;
            if (GemStringAmethyst)
            {
                //DrawAmethystSmear(Projectile, innerColor, outterColor, 1);
                //DrawAmethystSmear(Projectile, innerColor, outterColor, 1.5f, Pi);
                //DrawAmethystSmear(Projectile, innerColor, outterColor, 2f, PiOver2);
                //DrawAmethystSmear(Projectile, innerColor, outterColor, 2.5f, -PiOver2);
            }
            Texture2D star = LostbeltModAssets.Particle_ShinyOrb.Value;
            Vector2 starScale = new Vector2(1.35f, 1.35f);
            float rot = 0;
            Vector2 pos = Projectile.Center - Main.screenPosition;
            float scale2 = 0.75f;
            Main.spriteBatch.Draw(star, pos, null, Color.DarkViolet.ToAddColor(50), rot, star.Size() / 2, starScale * Projectile.scale * scale2, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(star, pos, null, Color.Purple, rot, star.Size() / 2, starScale * Projectile.scale * scale2 * .75f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(star, pos, null, Color.DarkViolet.ToAddColor(50), rot + PiOver2, star.Size() / 2, starScale * Projectile.scale * scale2, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(star, pos, null, Color.Purple, rot + PiOver2, star.Size() / 2, starScale * Projectile.scale * scale2 * .75f, SpriteEffects.None, 0);
            int length = Projectile.oldPos.Length;
            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], .2f);
                Vector2 oldPos = lerpPos - Main.screenPosition + Projectile.Size / 2f;
                float oldRot = Projectile.oldRot[i];
                //图竖直
                float progress = i / (float)length;
                float xMult = Lerp(1f, .05f, (progress));
                float yMult = Lerp(1f, .05f, progress);
                Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale;
                Color c = Color.Lerp(Color.Black, Color.Lerp(Color.Purple, Color.Black, .63f), EaseInOutQuad(progress));
                float opac = Lerp(1f, .19f, EaseInOutExpo(progress));
                Color pixelColor = Color.Lerp(Color.Purple, Color.Lerp(Color.DarkViolet, Color.Violet, .63f), EaseInOutQuad(progress));
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(1, 1), pixelColor.ToAddColor(20) * opac * 0.815f, oldRot, projTex.Size() / 2f, scale * 1.51f, 0);
                SB.FastDraw(projTex, oldPos, c * opac * opac * .75f, oldRot, projTex.Size() / 2f, scale * .98f, 0);
            }

            for (int i = 0; i < 3; i++)
                SB.FastDraw(projTex, drawPos.ToRandCirclePos(1), Color.DarkViolet, Projectile.rotation, projTex.Size() / 2f, Projectile.scale * 1.1f, 0);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
            return false;
        }
        public void DrawTrails(Asset<Texture2D> useTex, Color drawColor, float multipleSize = 1f, float alphaValue = 1f, float offsetHeight = 1f)
        {
            float laserLength = 50;
            LostbeltShaderAssets.TerrarRayLaser.Parameters["LaserTextureSize"].SetValue(useTex.Size());
            LostbeltShaderAssets.TerrarRayLaser.Parameters["targetSize"].SetValue(new Vector2(laserLength, useTex.Height()));
            LostbeltShaderAssets.TerrarRayLaser.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * -0f);
            LostbeltShaderAssets.TerrarRayLaser.Parameters["uColor"].SetValue(drawColor.ToVector4() * .995f);
            LostbeltShaderAssets.TerrarRayLaser.Parameters["uFadeoutLength"].SetValue(0.8f);
            LostbeltShaderAssets.TerrarRayLaser.Parameters["uFadeinLength"].SetValue(0.05f);
            LostbeltShaderAssets.TerrarRayLaser.CurrentTechnique.Passes[0].Apply();
            if (Projectile.oldPos.Length < 3)
                return;
            //做掉可能存在的零向量
            Projectile.ClearInvaidData(out List<Vector2> validPosition, out List<float> validRot, Projectile.oldPos, Projectile.oldRot);
            DrawSetting drawSetting = new DrawSetting(useTex.Value, true);
            List<TrailDrawDate> trailDrawDates = [];
            int posCount = validPosition.Count;
            for (int j = 0; j < posCount - 1; j++)
            {
                float rot = (validPosition[j + 1] - validPosition[j]).ToRotation();
                float ratio = j / (posCount - 1);
                Vector2 posOffset = Main.rand.NextVector2Circular(1.5f, 1.5f);
                trailDrawDates.Add(new(validPosition[j] + Projectile.Size / 2 + posOffset, drawColor, new Vector2(0, 50 * multipleSize * Projectile.scale), rot));
            }
            TrailRender.DrawTrail([.. trailDrawDates], drawSetting);
        }

    }
}
