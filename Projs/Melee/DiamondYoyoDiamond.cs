using LostbeltTraveler.Buffs.GoodBuffs;
using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.Primitives.Trail;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Items.Weapons.Melee;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Projs.Melee
{
    public class DiamondYoyoDiamond : LostbeltProjs
    {
        public override string Texture => GetVanillaAssetPath(VanillaAsset.Item, ItemID.Diamond);
        public override EnumDamageClass Category => EnumDamageClass.Melee;
        public ref float Timer => ref Projectile.ai[0];
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
        public int GemTypes = 7;

        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(16);
        }
        public override void ExSD()
        {
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.MaxUpdates = 2;
            Projectile.width = Projectile.height = 16;
            Projectile.timeLeft = 600;
            Projectile.SetupImmnuity(-1);
            Projectile.penetrate = 1;
        }
        public override void OnFirstFrame()
        {
            if (Owner.IsHolding<DiamondYoyo>())
                GemTypes = 6;
            else if (GemStringDiamond)
            {
                if (Owner.IsHolding<OnyxYoyo>())
                    GemTypes = 7;
                if (Owner.IsHolding<RubyYoyo>())
                    GemTypes = 5;
                if (Owner.IsHolding<AmberYoyo>())
                    GemTypes = 4;
                if (Owner.IsHolding<EmeraldYoyo>())
                    GemTypes = 3;
                if (Owner.IsHolding<SapphireYoyo>())
                    GemTypes = 2;
                if (Owner.IsHolding<TopazYoyo>())
                    GemTypes = 1;
                if (Owner.IsHolding<AmethystYoyo>())
                    GemTypes = 0;
            }
        }
        public override void ProjAI()
        {
            if (GemStringTopaz)
                Projectile.tileCollide = false;
            DiamondGemAI();
            DiamondGemParticle();
        }
        public void DiamondGemParticle()
        {
            if (Projectile.IsOutScreen())
                return;
            switch (GemTypes)
            {
                case 0:
                    SetUpColor(RandLerpColor(Color.Violet, Color.DarkViolet), Color.Purple);
                    break;
                case 1:
                    SetUpColor(RandLerpColor(Color.Gold, Color.Goldenrod), Color.Goldenrod);
                    break;
                case 2:
                    SetUpColor(RandLerpColor(Color.RoyalBlue, Color.LightSkyBlue), Color.RoyalBlue);
                    break;
                case 3:
                    SetUpColor(RandLerpColor(Color.Green, Color.LimeGreen), Color.Green);
                    break;
                case 4:
                    SetUpColor(RandLerpColor(Color.Orange, Color.OrangeRed), Color.Goldenrod);
                    break;
                case 5:
                    SetUpColor(RandLerpColor(Color.Red, Color.Crimson), Color.Red);
                    break;
                case 7:
                    break;
                default:
                    SetUpColor(RandLerpColor(Color.White, Color.Silver), Color.White);
                    break;
            }

        }
        public void SetUpColor(Color starColor, Color squareColor)
        {
            if (Main.rand.NextBool(8))
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePosEdge(6), Projectile.velocity / 4f,
                    starColor, 45, 1, Projectile.scale * Main.rand.NextFloat(.75f, 1.15f) * .6f, .2f);
            if (Main.rand.NextBool(8))
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), Projectile.velocity / 4f,
                    squareColor, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
        }

        public void DiamondGemAI()
        {
            Projectile.rotation += .15f;
            if (Projectile.damage != 0 && Projectile.timeLeft > 50)
            {
                Projectile.scale = Lerp(Projectile.scale, 1.01f, .12f);
                if (Projectile.scale > 1 && Timer > 45)
                {
                    NPC CurTarget = Projectile.Lostbelt().CurStoredTarget;
                    if (CurTarget.IsLegal())
                    {
                        float speedValue = Projectile.velocity.Length();
                        float rotation = Projectile.velocity.ToRotation();
                        float angleTo = Projectile.AngleTo(CurTarget.Center);
                        float dist = Projectile.Distance(CurTarget.Center);
                        float r = dist * 0.30f / (float)Math.Abs(Math.Sin(rotation - angleTo));
                        if (Vector2.Dot(Projectile.velocity, Projectile.DirectionTo(CurTarget.Center)) < 0)
                        {
                            r = Clamp(r, 1, 240);
                        }
                        Projectile.velocity = Projectile.velocity.RotatedBy(-Math.Sign(WrapAngle(rotation - angleTo)) * speedValue / r);
                        if (Projectile.velocity.LengthSquared() < 11f * 11f)
                            Projectile.velocity *= 1.01f;
                        else
                            Projectile.velocity *= 0.9f;
                    }
                    else
                    {
                        if (Projectile.GetTargetSafe(out NPC tar, true, 1200f, true))
                        {
                            Projectile.Lostbelt().CurStoredTarget = tar;
                        }
                    }
                }
                else
                {
                    if (Projectile.velocity.LengthSquared() < 8f * 8f)
                        Projectile.velocity *= 1.01f;
                    else
                        Projectile.velocity *= 0.9f;

                }
            }
            else
            {
                Projectile.scale = Lerp(Projectile.scale, 0f, .12f);
                if (Projectile.scale <= 0.2f)
                    Projectile.Kill();
            }
            Timer++;

        }
        public override bool? CanDamage()
        {
            return Timer > 45;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.BounceOnTile(oldVelocity, .5f);
            Projectile.timeLeft -= 100;
            ScarletSound(SoundID.Dig, Projectile.Center);
            switch (GemTypes)
            {
                case 5:
                    TileColor(Color.Crimson, Color.DarkRed);
                    break;
                default:
                    TileColor(Color.White, Color.White);
                    break;
            }

            return false;
        }

        public void TileColor(Color killColor, Color orbColor)
        {
            for (int i = 0; i < 16; i++)
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6),
                    (-Projectile.oldVelocity).ToRandVelocity(0, 1f, 7), killColor, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
            ECSParticle.HRShinyOrb(Projectile.Center, Vector2.Zero, orbColor, 40, 1, 0.2f, 0.4f);

        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.HitDirectionOverride = Projectile.ApplyDirectionOverride(target);
        }

        public override void OnKill(int timeLeft)
        {
            switch (GemTypes)
            {
                case 5:
                    KillColor(Color.Crimson);
                    break;
                default:
                    KillColor(Color.White);
                    break;
            }
        }
        public void KillColor(Color c)
        {
            for (int i = 0; i < 16; i++)
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), (-Projectile.oldVelocity).ToRandVelocity(0, 1f, 7), c, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
        }


        public override bool PreDraw(ref Color lightColor)
        {
            switch (GemTypes)
            {
                //紫水晶
                case 0:
                    break;
                //黄玉
                case 1:
                    break;
                //蓝玉
                case 2:
                    break;
                //绿宝石
                case 3:
                    break;
                //琥珀
                case 4:
                    break;
                //红玉
                case 5:
                    DrawGem(GetVanillaAsset(VanillaAsset.Item, ItemID.Ruby),
                        Color.DarkRed, Color.Crimson, Color.White,
                        Color.Crimson,
                        Color.White, Color.Lerp(Color.DarkRed, Color.Crimson, .63f),
                        Color.DarkRed, Color.Lerp(Color.Crimson, Color.Red, .63f),
                        Color.Red.ToAddColor(), Color.White);
                    break;
                default:
                    DrawGem(GetVanillaAsset(VanillaAsset.Item, ItemID.Diamond), 
                        Color.White, Color.Silver, Color.White,
                        Color.White,
                        Color.White, Color.Lerp(Color.SkyBlue, Color.Silver, .63f),
                        Color.White, Color.Lerp(Color.SkyBlue, Color.AliceBlue, .63f),
                        Color.AliceBlue.ToAddColor(), Color.White);
                    break;
            }
            return false;
        }
        #region 复杂度奇高的绘制
        public void DrawGem(Texture2D tex, Color bloomDualLineColor3, Color bloomDualLineColor2, Color bloomDualLineColor1,
            Color crossGlowColor, Color gemOutlineTrailBeginColor, Color gemOutlineTrailEndColor, Color gemMainTrailBeginColor, Color gemMainTrailEndColor,
            Color gemSelfOutlineColor, Color gemSelfMainColor)
        {
            Texture2D projTex = tex;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            int length = Projectile.oldPos.Length;
            SB.EnterShaderArea();
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, bloomDualLineColor3, 1f, 1f, 1.1f);
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, bloomDualLineColor2, 0.9f, 1f, 1.1f);
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, bloomDualLineColor1, 1f, 1f, 1.1f);
            SB.EnterShaderArea();
            Texture2D glowTex = LostbeltModAssets.Particle_CrossGlow.Value;
            SB.FastDraw(glowTex, drawPos.ToRandCirclePos(1.5f, 1.5f), crossGlowColor, 0, glowTex.Size() / 2f, Projectile.scale * Main.rand.NextFloat(.9f, 1.01f) * .15f, 0);
            SB.EndShaderArea();
            SB.EndShaderArea();

            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], 0f);
                Vector2 oldPos = lerpPos - Main.screenPosition + Projectile.Size / 2f;
                float oldRot = Projectile.oldRot[i];
                //图竖直
                float progress = i / (float)length;
                float xMult = Lerp(1f, .55f, (progress));
                float yMult = Lerp(1f, .55f, progress);
                Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale;
                Color c = Color.Lerp(gemOutlineTrailBeginColor, gemOutlineTrailEndColor, EaseInOutQuad(progress));
                float opac = Lerp(1f, .3f, EaseInOutExpo(progress));
                Color pixelColor = Color.Lerp(gemMainTrailBeginColor, gemMainTrailEndColor, EaseInOutQuad(progress));
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(1.5f, 1.5f), pixelColor.ToAddColor(50) * opac * 0.815f, oldRot, projTex.Size() / 2f, scale * .95f, 0);
                SB.FastDraw(projTex, oldPos, c.ToAddColor(0) * opac * .315f, oldRot, projTex.Size() / 2f, scale * .98f, 0);
            }

            for (int i = 0; i < 8; i++)
                SB.FastDraw(projTex, drawPos + Main.rand.NextVector2Circular(1.5f, 1.5f) + (TwoPi / 8f * i).ToRotationVector2() * 1.5f, gemSelfOutlineColor, Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
            SB.FastDraw(projTex, drawPos, gemSelfMainColor, Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);

        }
        #endregion
        public void DrawTrails(Asset<Texture2D> useTex, Color drawColor, float multipleSize = 1f, float alphaValue = 1f, float offsetHeight = 1f)
        {
            float laserLength = 50;
            LostbeltShaderAssets.TerrarRayLaser.Parameters["LaserTextureSize"].SetValue(useTex.Size());
            LostbeltShaderAssets.TerrarRayLaser.Parameters["targetSize"].SetValue(new Vector2(laserLength, useTex.Height()));
            LostbeltShaderAssets.TerrarRayLaser.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * -0f);
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
                trailDrawDates.Add(new(validPosition[j] + Projectile.Size / 2 + posOffset, drawColor, new Vector2(0, 6 * multipleSize * Projectile.scale), rot));
            }
            TrailRender.DrawTrail([.. trailDrawDates], drawSetting);
        }
    }
}
