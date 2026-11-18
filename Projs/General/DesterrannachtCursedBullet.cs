using LostbeltTraveler.Core.PixelatedRender;
using LostbeltTraveler.Core.Primitives.Trail;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;

namespace LostbeltTraveler.Projs.General
{
    public class DesterrannachtCursedBullet : LostbeltProjs, IPixelatedRenderer
    {
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public override EnumDamageClass Category => EnumDamageClass.Typeless;
        public float StickProgress => Utils.GetLerpValue(0, 30, Projectile.timeLeft, true);
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(16);
        }
        public override void ExSD()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 3;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
        }
        public override void ProjAI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.Hitbox.Intersects(Owner.Hitbox))
            {
                if (Projectile.timeLeft > 30)
                    Projectile.timeLeft = 30;
            }
            else if (Projectile.timeLeft > 30)
            {
                Projectile.HomingTarget(Owner.Center, -1, 22, 5);
            }
            if (Projectile.timeLeft < 30)
                Projectile.velocity *= .19f;
        }
        public BlendState BlendState => BlendState.Additive;
        public ScarletDrawLayer LayerToRenderTo => ScarletDrawLayer.BeforeDusts;
        public void RenderPixelated(SpriteBatch spriteBatch)
        {
            float Progress = StickProgress;
            float overallScale = .3f;
            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            Texture2D glow = LostbeltModAssets.Particle_HRShinyOrb.Value;
            Vector2 pos = Projectile.Center - Main.screenPosition;
            float lerp = (float)Math.Sin(Main.timeForVisualEffects * .09f) * 1f;
            float Scale1 = .55f * Progress + 0.02f * lerp;
            Scale1 *= overallScale;
            SB.Draw(glow, pos, null, Color.DarkViolet, 0, glow.Size() / 2f, Scale1, SpriteEffects.None, 0);
            SB.Draw(glow, pos, null, Color.White, PiOver2, glow.Size() / 2f, Scale1 * .85f, SpriteEffects.None, 0);

            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);

            DrawTrails(LostbeltModAssets.Trail_TerraRayFlow.Texture, Color.DarkViolet, 1f, 1f, 1f);
            DrawTrails(LostbeltModAssets.Trail_Lightning4.Texture, Color.Violet * 1.5f, 0.7f);

            LostbeltMethods.EndShaderAreaPixel();
        }
        public void DrawTrails(Asset<Texture2D> useTex, Color drawColor, float multipleSize = 1f, float alphaValue = 1f, float offsetHeight = 1f)
        {

            if (Projectile.oldPos.Length < 3)
                return;
            Effect shader = LostbeltShaderAssets.StandardFlowShader;
            shader.Parameters["LaserTextureSize"].SetValue(useTex.Size());
            shader.Parameters["targetSize"].SetValue(new Vector2(useTex.Width(), useTex.Height()));
            shader.Parameters["uTime"].SetValue(-Main.GlobalTimeWrappedHourly * 170f * offsetHeight);
            shader.Parameters["uColor"].SetValue(drawColor.ToVector4() * Projectile.Opacity * alphaValue * StickProgress);
            shader.Parameters["uFadeoutLength"].SetValue(0.7f);
            shader.Parameters["uFadeinLength"].SetValue(0f);
            shader.CurrentTechnique.Passes[0].Apply();

            DrawSetting drawSetting = new(useTex.Value);
            List<TrailDrawDate> trailDrawDates = [];

            int posCount = (int)((Projectile.oldPos.Length) * StickProgress);
            for (int j = 0; j < posCount; j++)
            {
                if (Projectile.oldPos[j] != Vector2.Zero)
                {
                    Vector2 vec = Projectile.oldRot[j].ToRotationVector2().RotatedBy(PiOver2);
                    Vector2 drawPos = Projectile.oldPos[j] + new Vector2(Projectile.width / 2, Projectile.height / 2);
                    trailDrawDates.Add(new(drawPos, drawColor, new Vector2(0, 40 * multipleSize * Projectile.scale), Projectile.oldRot[j]));
                }
            }
            TrailRender.RenderTrail([.. trailDrawDates], drawSetting);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            PixelatedRenderManager.BeginDrawProj = true;
            return false;
        }
    }
}
