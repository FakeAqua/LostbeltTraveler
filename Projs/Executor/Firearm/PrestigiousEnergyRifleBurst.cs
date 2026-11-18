using LostbeltTraveler.Core.PixelatedRender;
using LostbeltTraveler.Core.Primitives.Trail;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;


namespace LostbeltTraveler.Projs.Executor.Firearm
{
    public class PrestigiousEnergyRifleBurst : LostbeltProjs, IPixelatedRenderer
    {
        public float StickProgress => Utils.GetLerpValue(0, 80, Projectile.timeLeft, true);
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public override EnumDamageClass Category => EnumDamageClass.Executor;
        public int Index
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(21);
        }
        public override void ExSD()
        {
            Projectile.width = Projectile.height = 160;
            Projectile.MaxUpdates = 4;
            Projectile.penetrate = 2;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
            Projectile.timeLeft = Projectile.MaxUpdates * 8;
            Projectile.SetupImmnuity(5, ImmnuityType.Static);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.Lostbelt().ExecutionStrike)
            {
                ScarletSound(LostbeltSoundAssets.Lightning_Strike, target.Center, .6f, pitch: .6f);
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), target.Center, Vector2.Zero, ProjectileType<PrestigiousEnergyRifleThunder>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            target.PunchTarget(Owner.Center.GetNormalVector2(target.Center), 10);

            base.OnHitNPC(target, hit, damageDone);
        }
        public ref float Isollation => ref Projectile.ai[2];
        public override void ProjAI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
        }
        public BlendState BlendState => BlendState.AlphaBlend;
        public ScarletDrawLayer LayerToRenderTo => ScarletDrawLayer.BeforeDusts;
        public void RenderPixelated(SpriteBatch spriteBatch)
        {
            //float Progress = StickProgress;
            //float overallScale = 1.35f;
            //LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            //////这里是强行使用ex98拼凑出来的子弹效果
            //Texture2D tex = LostbeltModAssets.Specific_RocketTrail.Value;
            //Texture2D projTex = tex;
            //Vector2 drawPos = Projectile.Center - Main.screenPosition;
            //Texture2D glowTex = LostbeltModAssets.Particle_CrossGlow.Value;
            //LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            //int length = Projectile.oldPos.Length;
            //for (int i = length - 1; i >= 0; i--)
            //{
            //    Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], .2f);
            //    Vector2 oldPos = lerpPos - Main.screenPosition + Projectile.Size / 2f;
            //    float oldRot = Projectile.oldRot[i] + PiOver2;
            //    //图竖直
            //    float progress = i / (float)length;
            //    float xMult = Lerp(0.7f, .05f, (progress));
            //    float yMult = Lerp(1f, .2f, progress);
            //    Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale * 1.2f * overallScale;
            //    Color c = Color.Lerp(Color.RoyalBlue, Color.Lerp(Color.SkyBlue, Color.White, .75f), EaseInOutQuad(progress));
            //    float opac = Lerp(1f, .79f, EaseInOutExpo(progress));
            //    Color pixelColor = Color.Lerp(Color.White, Color.Lerp(Color.SkyBlue, Color.DeepSkyBlue, 0.85f), EaseInOutExpo(progress));
            //    int by = (int)Lerp(50, 0, progress);
            //    SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(1.5f, 1.5f), c.ToAddColor(0) * opac * 1.2f, oldRot, projTex.Size() / 2f, scale * 1.1f, 0);
            //    //这里重复多画一次。
            //    SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(1.5f, 1.5f), c.ToAddColor((byte)by) * opac * .6f, oldRot, projTex.Size() / 2f, scale, 0);
            //    SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(0.5f, 0.5f), pixelColor.ToAddColor(100) * opac, oldRot, projTex.Size() / 2f, scale, 0);
            //}
            //LostbeltMethods.EnterShaderAreaPixel(BlendState.NonPremultiplied);
            //DrawTrails(LostbeltModAssets.Trail_ManaStreak.Texture, Color.RoyalBlue, 10 * overallScale);
            //LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            //DrawTrails(LostbeltModAssets.Trail_ManaStreak.Texture, Color.SkyBlue, 10 * overallScale);
            //DrawTrails(LostbeltModAssets.Trail_ManaStreak.Texture, Color.White * .5f, 6 * overallScale);
            //LostbeltMethods.EndShaderAreaPixel();
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
            shader.Parameters["uFadeinLength"].SetValue(0.2f);
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
