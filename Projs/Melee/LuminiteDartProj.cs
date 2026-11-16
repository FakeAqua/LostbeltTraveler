using LostbeltTraveler.Core.PixelatedRender;
using LostbeltTraveler.Core.Primitives.Trail;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Items.Weapons.Melee;
using System.Collections.Generic;
using Terraria;

namespace LostbeltTraveler.Projs.Melee
{
    public class LuminiteDartProj : LostbeltProjs, IPixelatedRenderer
    {
        public override EnumDamageClass Category => EnumDamageClass.Melee;
        public override string Texture => GetInstance<LuminiteDart>().Texture;
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(20);
        }
        public override void ExSD()
        {
            Projectile.MaxUpdates = 3;
            Projectile.width = Projectile.height = 16;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.SetupImmnuity(30);
            Projectile.penetrate = 3;
        }
        public override void ProjAI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return base.OnTileCollide(oldVelocity);
        }
        public BlendState BlendState => BlendState.Additive;
        public ScarletDrawLayer LayerToRenderTo => ScarletDrawLayer.BeforeDusts;
        public void RenderPixelated(SpriteBatch spriteBatch)
        {
            if (!Projectile.Lostbelt().FirstFrame)
                return;

            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            TrailFunc(LostbeltModAssets.Trail_ManaStreak.Value, Color.SkyBlue, 10);
            //SB.EnterShaderArea();
            TrailFunc(LostbeltModAssets.Trail_ManaStreak.Value, Color.DarkOliveGreen, 10);
            TrailFunc(LostbeltModAssets.Trail_ManaStreak.Value, Color.White * .65f, 6);
            LostbeltMethods.EndShaderAreaPixel();
        }
        public void TrailFunc(Texture2D tex, Color color, float primitiveHeight = 30, float heightPosOffset = 0f)
        {

            float laserLength = 100;
            Effect shader = LostbeltShaderAssets.StandardFlowShader;
            shader.Parameters["LaserTextureSize"].SetValue(tex.Size());
            shader.Parameters["targetSize"].SetValue(new Vector2(laserLength, tex.Height));
            shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * -100);
            shader.Parameters["uColor"].SetValue(color.ToVector4() * 1);
            shader.Parameters["uFadeoutLength"].SetValue(1.3f);
            shader.Parameters["uFadeinLength"].SetValue(0.1f);
            shader.CurrentTechnique.Passes[0].Apply();

            DrawSetting sets = new(tex);
            List<TrailDrawDate> date = [];
            for (int i = 0; i < (Projectile.oldPos.Length); i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    continue;
                Vector2 listPos = Projectile.oldPos[i] + Projectile.Size / 2;
                date.Add(new(listPos, Color.White, new(0, primitiveHeight * 5f), Projectile.oldRot[i]));
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
            for (int i = 0; i < 8; i++)
            {
                SB.FastDraw(tex, pos + (TwoPi / 8 * i).ToRotationVector2() * 1.5f + Main.rand.NextVector2Circular(3, 3), Color.DarkGoldenrod.ToAddColor(), rot, rotPoint, Projectile.scale, se);
            }
            SB.FastDraw(tex, pos, Color.White, rot, rotPoint, Projectile.scale, se);
            return false;
        }
    }
}
