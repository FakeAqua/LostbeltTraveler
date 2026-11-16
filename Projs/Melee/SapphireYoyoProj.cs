using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.Primitives.Trail;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Methods;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Projs.Melee
{
    public class SapphireYoyoProj : GemYoyoProj
    {
        public ref float BoosterTime => ref Projectile.ai[2];
        protected override float YoyoLifeTime => 6;
        protected override int YoyoMaxUpdates => 1;
        protected override float YoyoTopSpeed => 16;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Projectile.ToTrailSetting(6);
        }
        public override void ExSD()
        {
            base.ExSD();
        }
        public override void ProjAI()
        {
            Lighting.AddLight(Projectile.Center, TorchID.Blue);
            if (BoosterTime > 0)
            {
                Projectile.MaxUpdates = YoyoMaxUpdates + 1;
                Projectile.SetupImmnuity(8, ImmnuityType.Static);
                if (Main.rand.NextBool(2))
                {
                    ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePos(4), RandVelTwoPi(1), Color.RoyalBlue, 40, 1, Main.rand.NextFloat(.9f, 1.1f) * .4f, .2f);
                }
                if (Projectile.velocity.LengthSquared() > 2.5f * 2.5f)
                {
                    ECSParticle.Stain(Projectile.Center.ToRandCirclePosEdge(4), Projectile.velocity / 8f, RandLerpColor(Color.AliceBlue, Color.LightSkyBlue), 40, 1, Projectile.velocity.ToRotation(), 0.16f);
                }


            }
            else
            {
                Projectile.MaxUpdates = YoyoMaxUpdates;
                Projectile.SetupImmnuity(10, ImmnuityType.Static);
                if (Main.rand.NextBool(2))
                {
                    ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePos(4), RandVelTwoPi(1), Color.RoyalBlue, 40, 1, Main.rand.NextFloat(.9f, 1.1f) * .4f, .2f);
                }
            }
            base.ProjAI();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (BoosterTime == 0)
            {
                ScarletSound(LostbeltSoundAssets.GrabCharge, Projectile.Center);
                for (int i = 0; i < 16; i++)
                {
                    ECSParticle.TurbulenceShinyOrb(Owner.Center, 1.2f, RandLerpColor(Color.AliceBlue, Color.LightSkyBlue), 45, 1, 0.1f, glowMult: .4f);
                }
            }
            for (int i = 0; i < 8; i++)
            {
                ECSParticle.ShinyCrossStarECS(Projectile.Center, RandVelTwoPi(3), RandLerpColor(Color.AliceBlue, Color.LightSkyBlue), 45, 1, Main.rand.NextFloat(.9f, 1.1f) * .4f, .2f);

            }
            BoosterTime = GetSeconds(1);
            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            int length = Projectile.oldPos.Length;
            Color innerColor = Color.Lerp(Color.MidnightBlue,Color.White,.75f) * .85f;
            Color outterColor = Color.RoyalBlue* .95f;

            if (GemStringAmethyst)
            {
                DrawAmethystSmear(Projectile, innerColor, outterColor, 1);
                DrawAmethystSmear(Projectile, innerColor, outterColor, 1.5f, Pi);
                DrawAmethystSmear(Projectile, innerColor, outterColor, 2f, PiOver2);
                DrawAmethystSmear(Projectile, innerColor, outterColor, 2.5f, -PiOver2);
            }

            if (BoosterTime > 0)
            {
                SB.EnterShaderArea();
                DrawTrails(LostbeltModAssets.Trail_Lightning2.Texture, Color.RoyalBlue, 1f, 1f, -105f);
                DrawTrails(LostbeltModAssets.Trail_Lightning3.Texture, Color.LightSkyBlue, 0.9f, 1f, -111f);
                DrawTrails(LostbeltModAssets.Trail_Lightning4.Texture, Color.White, 1f, 1f, -100f);
                SB.EndShaderArea();
                SB.EndShaderArea();

            }
            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], .2f);
                Vector2 oldPos = lerpPos - Main.screenPosition + Projectile.Size / 2f;
                float oldRot = Projectile.oldRot[i];
                //图竖直
                float progress = i / (float)length;
                float xMult = Lerp(1f, .55f, (progress));
                float yMult = Lerp(1f, .55f, progress);
                Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale;
                Color c = Color.Lerp(Color.White, Color.Lerp(Color.SkyBlue, Color.Blue, .63f), EaseInOutQuad(progress));
                float opac = Lerp(1f, .59f, EaseInOutExpo(progress));
                Color pixelColor = Color.Lerp(Color.White, Color.Lerp(Color.SkyBlue, Color.AliceBlue, .63f), EaseInOutQuad(progress));
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(5, 5), pixelColor.ToAddColor(50) * opac * 0.815f, oldRot, projTex.Size() / 2f, scale * .95f, 0);
                SB.FastDraw(projTex, oldPos, c.ToAddColor(0) * opac * .315f, oldRot, projTex.Size() / 2f, scale * .98f, 0);
            }

            for (int i = 0; i < 8; i++)
                SB.FastDraw(projTex, drawPos + Main.rand.NextVector2Circular(1.5f, 1.5f) + (TwoPi / 8f * i).ToRotationVector2() * 1.5f, Color.AliceBlue.ToAddColor(), Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
            return false;
        }
        public void DrawTrails(Asset<Texture2D> useTex, Color drawColor, float multipleSize = 1f, float alphaValue = 1f, float offsetHeight = 1f)
        {
            float laserLength = 50;
            LostbeltShaderAssets.TerrarRayLaser.Parameters["LaserTextureSize"].SetValue(useTex.Size());
            LostbeltShaderAssets.TerrarRayLaser.Parameters["targetSize"].SetValue(new Vector2(laserLength, useTex.Height()));
            LostbeltShaderAssets.TerrarRayLaser.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * offsetHeight);
            LostbeltShaderAssets.TerrarRayLaser.Parameters["uColor"].SetValue(drawColor.ToVector4() * .85f);
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
                trailDrawDates.Add(new(validPosition[j] + Projectile.Size / 2 + posOffset, drawColor, new Vector2(0, 10 * multipleSize * Projectile.scale), rot));
            }
            TrailRender.DrawTrail([.. trailDrawDates], drawSetting);
        }

    }
}
