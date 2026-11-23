using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.Primitives.Trail;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;

namespace LostbeltTraveler.Projs.Melee
{
    public class DiamondYoyoProj : GemYoyoProj
    {
        protected override float YoyoLength => LostbeltMethods.TilePixel(17.5f);
        protected override float YoyoLifeTime => 8;
        public ref float FloatingTimer => ref Projectile.ai[2];
        protected override int YoyoMaxUpdates => 2;
        protected override float YoyoTopSpeed => 29;
        public override bool ForceDiamond => true;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Projectile.ToTrailSetting(8);
        }
        public override void ExSD()
        {
            base.ExSD();
        }
        public override void OnFirstFrame()
        {
            base.OnFirstFrame();
        }
        public override void ProjAI()
        {
            base.ProjAI();
            Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 1.2f);
            if (Projectile.velocity.LengthSquared() > 2.5f * 2.5f)
                ECSParticle.Stain(Projectile.Center.ToRandCirclePosEdge(4), Projectile.velocity / 8f, RandLerpColor(Color.AliceBlue, Color.White), 40, 1, Projectile.velocity.ToRotation(), 0.16f);
            if (Main.rand.NextBool(4))
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePos(4), RandVelTwoPi(1), Color.White, 40, 1, Main.rand.NextFloat(.9f, 1.1f) * .4f, .2f);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            Color innerColor = Color.Lerp(Color.Silver, Color.White, .325f) * .7f;
            Color outterColor = Color.White * .55f;
            if (GemStringAmethyst)
            {
                DrawAmethystSmear(Projectile, innerColor, outterColor, 1);
                DrawAmethystSmear(Projectile, innerColor, outterColor, 1.5f, Pi);
                DrawAmethystSmear(Projectile, innerColor, outterColor, 2f, PiOver2);
                DrawAmethystSmear(Projectile, innerColor, outterColor, 2.5f, -PiOver2);
            }
            int length = Projectile.oldPos.Length;
            SB.EnterShaderArea();

            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.White, 1f, 1f, 1.1f);
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.Silver, 0.9f, 1f, 1.1f);
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.White, 1f, 1f, 1.1f);
            SB.EnterShaderArea();
            Texture2D glowTex = LostbeltModAssets.Particle_CrossGlow.Value;
            SB.FastDraw(glowTex, drawPos.ToRandCirclePos(1.5f, 1.5f), Color.White, 0, glowTex.Size() / 2f, Projectile.scale * Main.rand.NextFloat(.9f, 1.01f) * .15f, 0);
            SB.EndShaderArea();
            SB.EndShaderArea();

            for (int i = 0; i < 8; i++)
                SB.FastDraw(projTex, drawPos + Main.rand.NextVector2Circular(1.5f, 1.5f) + (TwoPi / 8f * i).ToRotationVector2() * 1.5f, Color.AliceBlue.ToAddColor(), Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
            return false;
        }
        public void DrawTrails(Asset<Texture2D> useTex, Color drawColor, float multipleSize = 1f, float alphaValue = 1f, float offsetHeight = 1f)
        {
            float laserLength = 50;
            Effect shader = LostbeltShaderAssets.TerrarRayLaser;
            shader.Parameters["LaserTextureSize"].SetValue(useTex.Size());
            shader.Parameters["targetSize"].SetValue(new Vector2(laserLength, useTex.Height()));
            shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * -0f);
            shader.Parameters["uColor"].SetValue(drawColor.ToVector4() * .995f);
            shader.Parameters["uFadeoutLength"].SetValue(0.8f);
            shader.Parameters["uFadeinLength"].SetValue(0.05f);
            shader.CurrentTechnique.Passes[0].Apply();
            if (Projectile.oldPos.Length < 3)
                return;
            //做掉可能存在的零向量
            DrawSetting drawSetting = new DrawSetting(useTex.Value, true);
            List<TrailDrawDate> trailDrawDates = [];
            int posCount = Projectile.oldPos.Length;
            for (int j = 0; j < posCount - 1; j++)
            {
                float rot = (Projectile.oldPos[j + 1] - Projectile.oldPos[j]).ToRotation();
                float ratio = j / (posCount - 1);
                Vector2 posOffset = Main.rand.NextVector2Circular(1.5f, 1.5f);
                trailDrawDates.Add(new(Projectile.oldPos[j] + Projectile.Size / 2 + posOffset, drawColor, new Vector2(0, 10 * multipleSize * Projectile.scale), rot));
            }
            TrailRender.DrawTrail([.. trailDrawDates], drawSetting);
        }
    }
}
