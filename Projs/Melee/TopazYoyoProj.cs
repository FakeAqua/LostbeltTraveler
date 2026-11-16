using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Methods;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Projs.Melee
{
    public abstract class GemYoyoProj : LostbeltProjs
    {
        public override EnumDamageClass Category => EnumDamageClass.Melee;
        protected virtual float YoyoTopSpeed => 10f;
        protected virtual float YoyoLength => LostbeltMethods.TilePixel(13.4375f);
        protected virtual float YoyoLifeTime => 300;
        protected virtual int YoyoMaxUpdates => 1;
        protected string CurrentyGemString => Owner.Lostbelt().gemYoyoStrings;

        #region 快捷判定
        public bool GemStringTopaz => CurrentyGemString.Equals("Topaz");
        public bool GemStringAmethyst => CurrentyGemString.Equals("Amethyst");
        public bool GemStringSapphire => CurrentyGemString.Equals("Sapphire");
        public bool GemStringEmerald => CurrentyGemString.Equals("Emerald");
        public bool GemStringRuby => CurrentyGemString.Equals("Ruby");
        public bool GemStringAmber => CurrentyGemString.Equals("Amber");
        public bool GemStringOnyx => CurrentyGemString.Equals("Onyx");
        public bool GemStringDiamond => CurrentyGemString.Equals("Diamond");
        #endregion
        public float TimerEmerald = 0;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Type] = YoyoLifeTime;
            ProjectileID.Sets.YoyosMaximumRange[Type] = YoyoLength;
            ProjectileID.Sets.YoyosTopSpeed[Type] = YoyoTopSpeed / YoyoMaxUpdates;
        }
        public override void ExSD()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = ProjAIStyleID.Yoyo;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = YoyoMaxUpdates;
        }
        public override void ProjAI()
        {
            DoBehaviourEmerald();
            DoBehaviourTopaz();
            base.ProjAI();
        }
        #region 宝石悠悠球的行为逻辑
        public void DoBehaviourRuby()
        {

        }
        public void DoBehaviourSapphire()
        {

        }
        public void DoBehaviourTopaz()
        {
            Projectile.tileCollide = !GemStringTopaz;
        }
        public void DoBehaviourEmerald()
        {
            if (!GemStringEmerald)
                return;
        }
        #endregion
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (!GemStringAmethyst)
                return null;
            Rectangle rec = Utils.CenteredRectangle(Projectile.Center, new Vector2(16 * 8));
            return rec.Intersects(targetHitbox);
        }
        public static void DrawAmethystSmear(Projectile proj, Color outerColor, Color innerColor, float overallScale = 1, float rotFix = 0)
        {
            Main.spriteBatch.EnterShaderArea();
            Texture2D glowTex = LostbeltModAssets.Particle_Smear.Value;
            Vector2 drawPos = proj.Center - Main.screenPosition;
            Main.spriteBatch.FastDraw(glowTex, drawPos.ToRandCirclePos(1.3f), outerColor, proj.rotation + rotFix, glowTex.Size() / 2f, .23f * overallScale, 0);
            Main.spriteBatch.FastDraw(glowTex, drawPos, innerColor, proj.rotation + rotFix, glowTex.Size() / 2f, .21f * overallScale, 0);
            Main.spriteBatch.EndShaderArea();
            Main.spriteBatch.EndShaderArea();
        }
    }
    public class TopazYoyoProj : GemYoyoProj
    {
        public ref float BoosterTime => ref Projectile.ai[2];
        protected override float YoyoLifeTime => 5;
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
            Projectile.tileCollide = false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Rectangle rec = Utils.CenteredRectangle(Projectile.Center, new Vector2(64));
            return rec.Intersects(targetHitbox);
        }
        public override void ProjAI()
        {
            base.ProjAI();
            Lighting.AddLight(Projectile.Center, TorchID.Yellow);
            ECSParticle.ShrinkParticle(Projectile.Center.ToRandCirclePos(8), RandVelTwoPi(1), RandLerpColor(Color.Yellow, Color.Gold), 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .13f, 1);
            if (Projectile.velocity.LengthSquared() > 2.5f * 2.5f)
            {
                ECSParticle.Stain(Projectile.Center.ToRandCirclePosEdge(8), Projectile.velocity / 8f, RandLerpColor(Color.Yellow, Color.Gold), 40, 1, Projectile.velocity.ToRotation(), 0.16f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            ECSParticle.LightntingGlow(target.Center, Projectile.rotation.ToRotationVector2() * .01f, Color.Gold, 40, 1, 0.6f);
            for (int i = 0; i < 8; i++)
            {
                ECSParticle.ShinyCrossStarECS(target.Center, RandVelTwoPi(3), RandLerpColor(Color.Yellow, Color.Gold), 45, 1, Main.rand.NextFloat(.9f, 1.1f) * .4f, .2f);

            }
            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            if (GemStringAmethyst)
            {
                DrawAmethystSmear(Projectile, Color.Goldenrod, Color.LightGoldenrodYellow * .5f, 1);
                DrawAmethystSmear(Projectile, Color.Goldenrod, Color.LightGoldenrodYellow * .5f, 1.5f, Pi);
                DrawAmethystSmear(Projectile, Color.Goldenrod, Color.LightGoldenrodYellow * .5f, 2f, PiOver2);
                DrawAmethystSmear(Projectile, Color.Goldenrod, Color.LightGoldenrodYellow * .5f, 2.5f, -PiOver2);
            }
            int length = Projectile.oldPos.Length;
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
                Color c = Color.Lerp(Color.White, Color.Lerp(Color.DarkGoldenrod, Color.Gold, .63f), EaseInOutQuad(progress));
                float opac = Lerp(1f, .59f, EaseInOutExpo(progress));
                Color pixelColor = Color.Lerp(Color.White, Color.Lerp(Color.DarkGoldenrod, Color.Gold, .63f), EaseInOutQuad(progress));
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(5, 5), pixelColor.ToAddColor(50) * opac * 0.815f, oldRot, projTex.Size() / 2f, scale * .95f, 0);
                SB.FastDraw(projTex, oldPos, c.ToAddColor(0) * opac * .315f, oldRot, projTex.Size() / 2f, scale * .98f, 0);
            }

            for (int i = 0; i < 8; i++)
                SB.FastDraw(projTex, drawPos + Main.rand.NextVector2Circular(1.5f, 1.5f) + (TwoPi / 8f * i).ToRotationVector2() * 1.5f, Color.Gold.ToAddColor(), Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
            return false;
        }
    }
}
