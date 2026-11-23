using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.PixelatedRender;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Items.Weapons.Melee;
using System.Collections.Generic;
using Terraria;

namespace LostbeltTraveler.Projs.Melee
{
    public class SupremeGemYoyoProjAmethystSmear : LostbeltProjs, IPixelatedRenderer
    {
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public override EnumDamageClass Category => EnumDamageClass.Melee;
        public List<Vector2> OldPos = new List<Vector2>();
        public List<float> OldRot = new List<float>();
        public float SmearFrame = 0;
        public float HitOpac = 0;
        public Projectile MountedProj = null;
        public float GlobalLerpingColor = 0;
        public ref float TimerForOnyx => ref Projectile.ai[1];
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(20);
        }
        public override void ExSD()
        {
            Projectile.width = Projectile.height = 160;
            Projectile.MaxUpdates = 4;
            Projectile.SetupImmnuity(5 * Projectile.MaxUpdates);
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.noEnchantmentVisuals = true;
            Projectile.timeLeft = 1000;
        }
        public override void OnFirstFrame()
        {
            HitOpac = 0;
            base.OnFirstFrame();
        }
        public override void ProjAI()
        {
            SmearAI();
            TimerForOnyx++;
            if (TimerForOnyx > Projectile.MaxUpdates * 12)
            {
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, RandVelTwoPi(12f, 15f), ProjectileType<SupremeGemYoyoProjOnyx>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                TimerForOnyx = 0;
                //除非确实在攻击敌人，不然玛瑙不会去生成钻石射弹
                if (SmearFrame > 20 * Projectile.MaxUpdates)
                    proj.ai[1] = 1;
                proj.ai[2] = Projectile.whoAmI;
            }
        }
        public void SmearAI()
        {
            if (SmearFrame > 0)
                SmearFrame--;
            if (SmearFrame > 20 * Projectile.MaxUpdates)
            {
                HitOpac = Lerp(HitOpac, 1f, 0.12f);
                if (HitOpac > .98f)
                    HitOpac = 1;
            }
            else

            {
                HitOpac = Lerp(HitOpac, 0f, 0.1f);
                if (HitOpac < .02f)
                    HitOpac = 0;
            }
            Projectile.rotation += .2f * Projectile.direction;
            Projectile farther = Main.projectile[(int)Projectile.ai[0]];
            if (Collision.SolidCollision(Projectile.Center, 16, 16))
            {
                if (GlobalLerpingColor < .98f)
                    GlobalLerpingColor = Lerp(GlobalLerpingColor, 1f, 0.2f);
                else
                    GlobalLerpingColor = 1;
            }
            else
            {

                if (GlobalLerpingColor > .2f)
                    GlobalLerpingColor = Lerp(GlobalLerpingColor, 0f, 0.2f);
                else
                    GlobalLerpingColor = 0;
            }
            if (farther.active && farther.owner == Owner.whoAmI && farther.type == ProjectileType<SupremeGemYoyoProj>())
            {
                Projectile.Center = farther.Center;
                Projectile.timeLeft = 2;
            }
            else
                HitOpac = 0;

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SmearFrame = Projectile.MaxUpdates * 40;
            ScarletSound(LostbeltSoundAssets.TheSevenStar_Hit, Projectile.Center, .275f, 1, .72f, .05f);
            for (int i = 0; i < 1; i++)
            {
                Vector2 stainBeginPos = target.Center.ToRandCirclePosEdge(0);
                Vector2 stainDir = RandDirTwoPi;
                float scale = Main.rand.NextFloat(.9f, 1.1f) * .86f;
                ECSParticle.Stain(stainBeginPos, stainDir * 1, Color.LightSkyBlue, 40, 1, stainDir.ToRotation(), scale, new Vector2(0.8f, 1.4f), 1.051f, 0.985f);
                ECSParticle.Stain(stainBeginPos, stainDir * 1, Color.White, 40, 1, stainDir.ToRotation(), scale * .8f, new Vector2(0.8f, 1.4f), 1.051f, .985f);
            }
            //过一个判定才确定其是否应该需要生成绿宝石
            if (Owner.IsHolding<SupremeGem>())
            {
                bool spawn = ((SupremeGem)Owner.HeldItem.ModItem).EnableCoinMode;
                if (spawn)
                {
                    Projectile p1 = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, RandDirTwoPi.ToSafeNormalize() * 14f, ProjectileType<EmeraldYoyoEmerald>(), 0, Projectile.knockBack, Projectile.owner);
                    p1.tileCollide = false;
                }
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Rectangle rec = Utils.CenteredRectangle(Projectile.Center, new Vector2(16 * 8));
            return rec.Intersects(targetHitbox);
        }
        public ScarletDrawLayer LayerToRenderTo => ScarletDrawLayer.BeforeDusts;
        public BlendState BlendState => BlendState.Additive;
        public void RenderPixelated(SpriteBatch sb)
        {
            if (!Projectile.Lostbelt().FirstFrame)
                return;

            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            Color outterColor = Color.White * 1f;
            float smearOpac = HitOpac;
            Color innerColor = Color.Lerp(Color.LightSkyBlue, Color.White, .125f) * .85f;
            for (int i = -1; i < 2; i++)
            {
                for (float j = 1.3f; j < 2; j += .25f)
                {
                    float lerping = Utils.GetLerpValue(1.3f / 2f, 1.8f, j, true);
                    float lerping2 = Utils.GetLerpValue(-1, 1, i, true);
                    DrawSmearAlt(j * 1 * smearOpac, ToRadians(60f * j * i), innerColor, outterColor, lerping * lerping2);
                }
            }
            DrawSmearAlt(1.5f * smearOpac, ToRadians(-70f), innerColor, outterColor, .7f);
            DrawSmearAlt(2f * smearOpac, ToRadians(-120f), innerColor, outterColor, .9f);
            DrawSmearAlt(2.35f * smearOpac, ToRadians(-180f), innerColor, outterColor, 1.2f);
            LostbeltMethods.EndShaderAreaPixel();
        }
        public void DrawSmearAlt(float overallScale, float rotFix, Color innerColor, Color outterColor, float opac2 = 1)
        {
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            Vector2 pos2 = drawPosition;
            Texture2D smear = LostbeltModAssets.Particle_SmearEdge.Value;
            float glowScale2 = .33f * overallScale * Projectile.scale;
            float opac = 1.0f * opac2;
            float rot = Projectile.rotation + Pi * .2f + rotFix;
            Color violetColor = Color.Lerp(Color.Violet, Color.Goldenrod, GlobalLerpingColor);
            Color darkVioletColor = Color.Lerp(Color.DarkViolet, Color.DarkGoldenrod, GlobalLerpingColor);
            SB.Draw(smear, pos2, null, violetColor * 1.5f * opac, rot, smear.Size() / 2, glowScale2, 0, 0);
            smear = LostbeltModAssets.Particle_Smear.Value;
            opac = .67f * opac2;
            SB.Draw(smear, pos2, null, darkVioletColor * opac, rot, smear.Size() / 2, glowScale2, 0, 0);
            opac = .43f * opac2;
            SB.Draw(smear, pos2, null, Color.White * .45f * opac, rot, smear.Size() / 2, glowScale2, 0, 0);
            SB.Draw(smear, pos2, null, violetColor * .78f * opac, rot, smear.Size() / 2, glowScale2 * .90f, 0, 0);
            SB.Draw(smear, pos2, null, darkVioletColor * opac * .58f, rot, smear.Size() / 2, glowScale2 * .80f, 0, 0);
        }


        public override bool PreDraw(ref Color lightColor)
        {
            if (!Projectile.Lostbelt().FirstFrame)
                return false;
            PixelatedRenderManager.BeginDrawProj = true;
            Texture2D star = LostbeltModAssets.Particle_ShinyOrb.Value;
            Vector2 starScale = new Vector2(1.35f, 1.35f);
            Color darkVioletColor = Color.Lerp(Color.DarkViolet, Color.DarkGoldenrod, GlobalLerpingColor);
            float rot = 0;
            Vector2 pos = Projectile.Center - Main.screenPosition;
            float scale2 = 1f * Lerp(0.75f, 1.13f, HitOpac);
            Main.spriteBatch.Draw(star, pos, null, darkVioletColor.ToAddColor(50), rot, star.Size() / 2, starScale * Projectile.scale * scale2, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(star, pos, null, Color.White, rot, star.Size() / 2, starScale * Projectile.scale * scale2 * .75f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(star, pos, null, darkVioletColor.ToAddColor(50), rot + PiOver2, star.Size() / 2, starScale * Projectile.scale * scale2, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(star, pos, null, Color.White, rot + PiOver2, star.Size() / 2, starScale * Projectile.scale * scale2 * .75f, SpriteEffects.None, 0);


            return false;
        }
    }
}
