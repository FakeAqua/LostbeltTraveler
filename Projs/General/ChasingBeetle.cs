using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using Terraria;
using Terraria.GameContent;

namespace LostbeltTraveler.Projs.General
{
    public class ChasingBeetle : LostbeltProjs
    {
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public override EnumDamageClass Category => EnumDamageClass.Typeless;
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(10);
        }
        public override void ExSD()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.MaxUpdates = 2;
            Projectile.noEnchantmentVisuals = true;
        }
        public override void ProjAI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.AddFrames(totalFrames: 3);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Beetle.Value;
            Rectangle frame = tex.Frame(1, 3, 0, Projectile.frame);
            Vector2 ori = frame.Size() / 2f;
            Vector2 pos = Projectile.Center - Main.screenPosition;
            SpriteEffects se = Projectile.spriteDirection > 0 ? 0 : SpriteEffects.FlipHorizontally;
            int count = Projectile.oldPos.Length;
            Color midColor = Color.Lerp(Color.DarkViolet, Color.White, .2f);
            for (int i = 0; i < count; i++)
            {
                float progress = i / (float)count;
                Vector2 oldPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                float scale = Lerp(1f, 0.45f, progress) * Projectile.scale;
                Color lowerColor = Color.Lerp(Color.DarkViolet, midColor, progress);
                float opac = Lerp(1f, 0.2f, progress);
                SB.Draw(tex, oldPos, frame, lowerColor.ToAddColor() * opac, 0, ori, scale * 1.05f, se, 0);
                SB.Draw(tex, oldPos, frame, lightColor * opac, 0, ori, scale * 1.05f, se, 0);
            }
            for (int i = 0; i < 6; i++)
            {
                Vector2 posSet = (TwoPi / 6f * i).ToRotationVector2() * 1.2f;
                SB.Draw(tex, pos + posSet, frame, Color.Violet.ToAddColor(), 0, ori, Projectile.scale, se, 0);
            }
            SB.Draw(tex, pos, frame, lightColor, 0, ori, Projectile.scale, se, 0);
            return false;
        }
    }
}
