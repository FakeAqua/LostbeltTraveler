using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.PixelatedRender;
using LostbeltTraveler.Core.Primitives.Trail;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;


namespace LostbeltTraveler.Projs.Magic
{
    public class AXCannonNoCharge : LostbeltProjs, IPixelatedRenderer
    {
         public float OverallProgress = 0;
        public override EnumDamageClass Category => EnumDamageClass.Magic;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
            Projectile.ToTrailSetting(30);
        }
        public override void ExSD()
        {
            Projectile.MaxUpdates = 4;
            Projectile.penetrate = -1;
            Projectile.SetupImmnuity(15 * Projectile.MaxUpdates);
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override void OnFirstFrame()
        {
            base.OnFirstFrame();
        }
        public override void ProjAI()
        {
             OverallProgress = Lerp(OverallProgress, 1f, 0.2f / Projectile.MaxUpdates);
            if (OverallProgress > .98f)
                OverallProgress = 1;
            Projectile.AddFrames(7);
            Projectile.rotation = Projectile.velocity.ToRotation();
            Vector2 startedPos = Projectile.Center.ToRandCirclePos(10);
            if (Main.rand.NextBool(3))
                ECSParticle.HighResolutionThunder(startedPos, Vector2.Zero, Color.Orange, 40, 1, Projectile.rotation+Main.rand.NextFloat(-.4f,.4f)+PiOver2, .3f, 3);
            if (Main.rand.NextBool(9))
            {
                ECSParticle.ShinyCrossStarECS(startedPos, Projectile.velocity, Color.OrangeRed, 20, 1, 0.23f);

            }

            base.ProjAI();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
        public BlendState BlendState => BlendState.AlphaBlend;
        public ScarletDrawLayer LayerToRenderTo => ScarletDrawLayer.BeforeDusts;
        public void RenderPixelated(SpriteBatch spriteBatch) 
        {
            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 _);
            Texture2D orb = LostbeltModAssets.Particle_HRShinyOrbSmall.Value;
            Vector2 overallOrb = new Vector2(.9f, .61f)*.5f;
            SB.FastDraw(orb, drawPos + Projectile.SafeDir() * 5, Color.OrangeRed, Projectile.rotation, orb.Size() / 2f, 0.9f * OverallProgress * overallOrb);
            SB.FastDraw(orb, drawPos + Projectile.SafeDir() * 5, Color.Orange* .95f, Projectile.rotation, orb.Size() / 2f, 0.7f * OverallProgress * overallOrb);
            SB.FastDraw(orb, drawPos + Projectile.SafeDir() * 5, Color.White * .85f, Projectile.rotation, orb.Size() / 2f, .5f * OverallProgress * overallOrb);
            DrawTrails(LostbeltModAssets.Trail_TerraRayFlow.Texture, Color.Goldenrod, 1.5f, 0.95f);
            DrawTrails(LostbeltModAssets.Trail_TerraRayFlow.Texture, Color.OrangeRed, 1.25f, .95f, 1.5f);
            DrawTrails(LostbeltModAssets.Trail_ManaStreak.Texture, Color.White, 0.315f, .895f, 1.25f);
            LostbeltMethods.EndShaderAreaPixel();
}
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overWiresUI.Add(index);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            PixelatedRenderManager.BeginDrawProj = true;
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 _);
            Rectangle frame = projTex.Frame(1, Main.projFrames[Type], 0, Projectile.frame);
            Vector2 ori = frame.Size() / 2f;
            float rot = Projectile.rotation;
            int length = Projectile.oldPos.Length - 25;
            Vector2 shrinkScale = new Vector2(OverallProgress, EaseOutBack(OverallProgress));
            for (int i = length - 1; i >= 0; i--)
            {
                float progress = 1 - (float)i / length;
                Vector2 oldpos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                float oldRot = Projectile.oldRot[i];
                Color c = Color.Lerp(Color.White, Color.Orange, progress) * progress * OverallProgress;
                SB.Draw(projTex, oldpos, frame, c, oldRot, ori, Projectile.scale * shrinkScale * Lerp(.6f, 1f, progress), 0, 0);

            }
            for (int i = 0; i < 8; i++)
                SB.Draw(projTex, drawPos + (TwoPi / 8f * i).ToRotationVector2(), frame, Color.Orange.ToAddColor() * OverallProgress, rot, ori, Projectile.scale * shrinkScale, 0, 0);
            SB.Draw(projTex, drawPos, frame, Color.White * OverallProgress, rot, ori, Projectile.scale * shrinkScale, 0, 0);
            return false;
        }
        public void DrawTrails(Asset<Texture2D> useTex, Color drawColor, float multipleSize = 1f, float alphaValue = 1f, float offsetHeight = 1f)
        {
            if (!Projectile.Lostbelt().FirstFrame)
                return;

            if (Projectile.oldPos.Length < 3)
                return;
            Effect shader = LostbeltShaderAssets.StandardFlowShader;
            shader.Parameters["LaserTextureSize"].SetValue(useTex.Size());
            shader.Parameters["targetSize"].SetValue(new Vector2(useTex.Width(), useTex.Height()));
            shader.Parameters["uTime"].SetValue(-Main.GlobalTimeWrappedHourly * 210f * offsetHeight);
            shader.Parameters["uColor"].SetValue(drawColor.ToVector4() * Projectile.Opacity * alphaValue * Clamp(Projectile.velocity.Length(), 0f, 1f) * OverallProgress);
            shader.Parameters["uFadeoutLength"].SetValue(0.6f);
            shader.Parameters["uFadeinLength"].SetValue(0.1f);
            shader.CurrentTechnique.Passes[0].Apply();

            DrawSetting drawSetting = new(useTex.Value);
            List<TrailDrawDate> trailDrawDates = [];
            float rad = 1;
            if (Projectile.timeLeft < 50)
                rad = Projectile.timeLeft / 50f * Projectile.Opacity;

            int posCount = (int)((Projectile.oldPos.Length-10) * rad * OverallProgress);
            for (int j = 0; j < posCount; j++)
            {
                if (Projectile.oldPos[j] != Vector2.Zero)
                {
                    Vector2 drawPos = Projectile.oldPos[j] + Projectile.Size / 2 + Projectile.SafeDir() * 5.2f * OverallProgress;
                    trailDrawDates.Add(new(drawPos, drawColor, new Vector2(0, 30 * multipleSize * Projectile.scale), Projectile.oldRot[j]));
                }
            }
            TrailRender.RenderTrail([.. trailDrawDates], drawSetting);
        }

           }
}
