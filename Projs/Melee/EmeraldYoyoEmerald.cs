using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.Primitives.Trail;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using static LostbeltTraveler.Projs.Melee.GemYoyoProj;

namespace LostbeltTraveler.Projs.Melee
{
    public class EmeraldYoyoEmerald : LostbeltProjs
    {
        public override string Texture => GetVanillaAssetPath(VanillaAsset.Item, ItemID.Emerald);
        public override EnumDamageClass Category => EnumDamageClass.Melee;
        public ref float Timer => ref Projectile.ai[0];
        protected string CurrentyGemString => Owner.Lostbelt().gemYoyoStrings;
        public GemYoyoProj.GemYoyoTypes GemTypes = GemYoyoProj.GemYoyoTypes.Emerald;

        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(16);

        }
        public override void ExSD()
        {
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.MaxUpdates = 4;
            Projectile.width = Projectile.height = 16;
            Projectile.SetupImmnuity(60);
            Projectile.timeLeft = 300 * Projectile.MaxUpdates;
            Projectile.penetrate = 3;
        }
        public override void OnFirstFrame()
        {

            base.OnFirstFrame();
        }
        public override bool? CanDamage()
        {
            return false;
        }
        #region 墙体反弹
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.timeLeft -= 50 * Projectile.MaxUpdates;
            ScarletSound(SoundID.Dig, Projectile.Center);
            switch (GemTypes)
            {
                case GemYoyoTypes.Amethyst:
                    OnTileCollide_Amethyst();
                    break;
                case GemYoyoTypes.Topaz:
                    OnTileCollide_Topaz();
                    break;
                case GemYoyoTypes.Sapphire:
                    OnTileCollide_Sapphire();
                    break;
                case GemYoyoTypes.Emerald:
                    OnTileCollide_Emerald();
                    break;
                case GemYoyoTypes.Amber:
                    OnTileCollide_Amber();
                    break;
                case GemYoyoTypes.Ruby:
                    OnTileCollide_Ruby();
                    break;
                case GemYoyoTypes.Diamond:
                    OnTileCollide_Diamond();
                    break;
                case GemYoyoTypes.Onyx:
                    OnTileCollide_Onyx();
                    break;
                default:
                    OnTileCollide_Emerald();
                    break;
            }
            return true;
        }

        public void OnTileCollide_Amethyst()
        {
            for (int i = 0; i < 8; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(1f, 4f), RandLerpColor(Color.DarkViolet, Color.Violet), 45, 1, Projectile.scale * Main.rand.NextFloat(.75f, 1.15f) * .6f, .2f);
            ECSParticle.HRShinyOrb(Projectile.Center, Vector2.Zero, Color.Violet, 40, 1, 0.2f, 0.4f);
        }

        public void OnTileCollide_Topaz()
        {
            for (int i = 0; i < 8; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(1f, 4f), RandLerpColor(Color.Goldenrod, Color.DarkGoldenrod), 45, 1, Projectile.scale * Main.rand.NextFloat(.75f, 1.15f) * .6f, .2f);
            ECSParticle.HRShinyOrb(Projectile.Center, Vector2.Zero, Color.Gold, 40, 1, 0.2f, 0.4f);
        }

        public void OnTileCollide_Sapphire()
        {
            for (int i = 0; i < 8; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(1f, 4f), RandLerpColor(Color.DodgerBlue, Color.RoyalBlue), 45, 1, Projectile.scale * Main.rand.NextFloat(.75f, 1.15f) * .6f, .2f);
            ECSParticle.HRShinyOrb(Projectile.Center, Vector2.Zero, Color.LightSkyBlue, 40, 1, 0.2f, 0.4f);
        }

        public void OnTileCollide_Emerald()
        {
            for (int i = 0; i < 8; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(1f, 4f), RandLerpColor(Color.DarkGreen, Color.Green), 45, 1, Projectile.scale * Main.rand.NextFloat(.75f, 1.15f) * .6f, .2f);
            ECSParticle.HRShinyOrb(Projectile.Center, Vector2.Zero, Color.Lime, 40, 1, 0.2f, 0.4f);
        }

        public void OnTileCollide_Amber()
        {
            for (int i = 0; i < 8; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(1f, 4f), RandLerpColor(Color.White, Color.OrangeRed), 45, 1, Projectile.scale * Main.rand.NextFloat(.75f, 1.15f) * .6f, .2f);
            ECSParticle.HRShinyOrb(Projectile.Center, Vector2.Zero, Color.DarkOrange, 40, 1, 0.2f, 0.4f);
        }

        public void OnTileCollide_Ruby()
        {
            for (int i = 0; i < 8; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(1f, 4f), RandLerpColor(Color.Red, Color.Crimson), 45, 1, Projectile.scale * Main.rand.NextFloat(.75f, 1.15f) * .6f, .2f);
            ECSParticle.HRShinyOrb(Projectile.Center, Vector2.Zero, Color.Red, 40, 1, 0.2f, 0.4f);
        }

        public void OnTileCollide_Diamond()
        {
            for (int i = 0; i < 8; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(1f, 4f), RandLerpColor(Color.White, Color.WhiteSmoke), 45, 1, Projectile.scale * Main.rand.NextFloat(.75f, 1.15f) * .6f, .2f);
            ECSParticle.HRShinyOrb(Projectile.Center, Vector2.Zero, RandLerpColor(Color.Silver, Color.White), 40, 1, 0.2f, 0.4f);
        }

        public void OnTileCollide_Onyx()
        {
            for (int i = 0; i < 8; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(1f, 4f), RandLerpColor(Color.White, Color.WhiteSmoke), 45, 1, Projectile.scale * Main.rand.NextFloat(.75f, 1.15f) * .6f, .2f);
            ECSParticle.HRShinyOrb(Projectile.Center, Vector2.Zero, RandLerpColor(Color.Silver, Color.White), 40, 1, 0.2f, 0.4f);
        }
        #endregion
        #region AI
        public override void ProjAI()
        {
            float maxTime = Projectile.MaxUpdates * 60f;
            float progress = Clamp(Timer / maxTime, 0f, 1f);
            float lerpRotSpeed = Lerp(.15f, .01f, progress);
            Projectile.rotation += lerpRotSpeed;
            float speedMult = 16f;
            Vector2 vecToPlayer = Owner.Center - Projectile.Center + new Vector2(0f, -180f);
            float playerDist = vecToPlayer.Length();
            if (playerDist < 200f && speedMult > 8f)
            {
                speedMult = 2f;
            }
            if (playerDist > 70f)
            {
                vecToPlayer.Normalize();
                vecToPlayer *= speedMult;
                Projectile.velocity = (Projectile.velocity * 30f + vecToPlayer) / 31f;
            }
            else if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f)
            {
                Projectile.velocity.X = -0.15f;
                Projectile.velocity.Y = -0.05f;
            }
            Timer++;
            if (Timer > maxTime)
            {
                Projectile.Kill();
            }
            if (Projectile.IsOutScreen())
                return;
            switch (GemTypes)
            {
                case GemYoyoTypes.Amethyst:
                    ProjAIParticle_Amethyst();
                    break;
                case GemYoyoTypes.Topaz:
                    ProjAIParticle_Topaz();
                    break;
                case GemYoyoTypes.Sapphire:
                    ProjAIParticle_Sapphire();
                    break;
                case GemYoyoTypes.Emerald:
                    ProjAIParticle_Emerald();
                    break;
                case GemYoyoTypes.Amber:
                    ProjAIParticle_Amber();
                    break;
                case GemYoyoTypes.Ruby:
                    ProjAIParticle_Ruby();
                    break;
                case GemYoyoTypes.Diamond:
                    ProjAIParticle_Diamond();
                    break;
                case GemYoyoTypes.Onyx:
                    ProjAIParticle_Onyx();
                    break;
                default:
                    ProjAIParticle_Emerald();
                    break;
            }
        }

        public void ProjAIParticle_Amethyst()
        {
            if (Main.rand.NextBool(28))
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(4), RandLerpColor(Color.Violet, Color.DarkViolet), 45, 1, Projectile.scale * Main.rand.NextFloat(.75f, 1.15f) * .6f, .2f);
            if (Main.rand.NextBool(28))
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(4), Color.Violet, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
        }

        public void ProjAIParticle_Topaz()
        {
            if (Main.rand.NextBool(28))
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(4), RandLerpColor(Color.Goldenrod, Color.DarkGoldenrod), 45, 1, Projectile.scale * Main.rand.NextFloat(.75f, 1.15f) * .6f, .2f);
            if (Main.rand.NextBool(28))
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(4), Color.Gold, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
        }

        public void ProjAIParticle_Sapphire()
        {
            if (Main.rand.NextBool(28))
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(4), RandLerpColor(Color.DodgerBlue, Color.RoyalBlue), 45, 1, Projectile.scale * Main.rand.NextFloat(.75f, 1.15f) * .6f, .2f);
            if (Main.rand.NextBool(28))
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(4), Color.LightSkyBlue, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
        }

        public void ProjAIParticle_Emerald()
        {
            if (Main.rand.NextBool(28))
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(4), RandLerpColor(Color.LimeGreen, Color.DarkGreen), 45, 1, Projectile.scale * Main.rand.NextFloat(.75f, 1.15f) * .6f, .2f);
            if (Main.rand.NextBool(28))
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(4), Color.LimeGreen, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
        }

        public void ProjAIParticle_Amber()
        {
            if (Main.rand.NextBool(28))
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(4), RandLerpColor(Color.White, Color.OrangeRed), 45, 1, Projectile.scale * Main.rand.NextFloat(.75f, 1.15f) * .6f, .2f);
            if (Main.rand.NextBool(28))
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(4), Color.DarkOrange, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
        }

        public void ProjAIParticle_Ruby()
        {
            if (Main.rand.NextBool(28))
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(4), RandLerpColor(Color.Red, Color.Crimson), 45, 1, Projectile.scale * Main.rand.NextFloat(.75f, 1.15f) * .6f, .2f);
            if (Main.rand.NextBool(28))
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(4), Color.Red, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
        }

        public void ProjAIParticle_Diamond()
        {
            if (Main.rand.NextBool(28))
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(4), RandLerpColor(Color.White, Color.WhiteSmoke), 45, 1, Projectile.scale * Main.rand.NextFloat(.75f, 1.15f) * .6f, .2f);
            if (Main.rand.NextBool(28))
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(4), RandLerpColor(Color.Silver, Color.White), 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
        }

        public void ProjAIParticle_Onyx()
        {
            if (Main.rand.NextBool(28))
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(4), RandLerpColor(Color.White, Color.WhiteSmoke), 45, 1, Projectile.scale * Main.rand.NextFloat(.75f, 1.15f) * .6f, .2f);
            if (Main.rand.NextBool(28))
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(4), RandLerpColor(Color.Silver, Color.White), 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
        }
        #endregion
        #region 处死

        public void OnKillAmethyst()
        {
            for (int i = 0; i < 16; i++)
            {
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(0, 2f), Color.MediumPurple, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
            }
            ECSParticle.HRShinyOrb(Projectile.Center, Vector2.Zero, Color.Violet, 40, 1, 0.2f, 0.4f);
        }

        public void OnKillTopaz()
        {
            for (int i = 0; i < 16; i++)
            {
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(0, 2f), Color.Gold, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
            }
            ECSParticle.HRShinyOrb(Projectile.Center, Vector2.Zero, Color.Goldenrod, 40, 1, 0.2f, 0.4f);
        }

        public void OnKillSapphire()
        {
            for (int i = 0; i < 16; i++)
            {
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(0, 2f), Color.LightSkyBlue, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
            }
            ECSParticle.HRShinyOrb(Projectile.Center, Vector2.Zero, Color.DodgerBlue, 40, 1, 0.2f, 0.4f);
        }

        public void OnKillEmerald()
        {
            for (int i = 0; i < 16; i++)
            {
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(0, 2f), Color.LightGreen, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
            }
            ECSParticle.HRShinyOrb(Projectile.Center, Vector2.Zero, Color.LimeGreen, 40, 1, 0.2f, 0.4f);
        }

        public void OnKillAmber()
        {
            for (int i = 0; i < 16; i++)
            {
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(0, 2f), Color.Orange, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
            }
            ECSParticle.HRShinyOrb(Projectile.Center, Vector2.Zero, Color.DarkOrange, 40, 1, 0.2f, 0.4f);
        }

        public void OnKillRuby()
        {
            for (int i = 0; i < 16; i++)
            {
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(0, 2f), Color.OrangeRed, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
            }
            ECSParticle.HRShinyOrb(Projectile.Center, Vector2.Zero, Color.Crimson, 40, 1, 0.2f, 0.4f);
        }

        public void OnKillDiamond()
        {
            for (int i = 0; i < 16; i++)
            {
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(0, 2f), Color.WhiteSmoke, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
            }
            ECSParticle.HRShinyOrb(Projectile.Center, Vector2.Zero, Color.White, 40, 1, 0.2f, 0.4f);
        }

        public void OnKillOnyx()
        {
            for (int i = 0; i < 16; i++)
            {
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(0, 2f), Color.Silver, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
            }
            ECSParticle.HRShinyOrb(Projectile.Center, Vector2.Zero, Color.WhiteSmoke, 40, 1, 0.2f, 0.4f);
        }
        public override void OnKill(int timeLeft)
        {
            float chance = Main.rand.NextFloat();
            int coinSpawn;
            switch (GemTypes)
            {
                case GemYoyoTypes.Amethyst:
                    OnKillAmethyst();
                    break;
                case GemYoyoTypes.Topaz:
                    OnKillTopaz();
                    break;
                case GemYoyoTypes.Sapphire:
                    OnKillSapphire();
                    break;
                case GemYoyoTypes.Emerald:
                    OnKillEmerald();
                    break;
                case GemYoyoTypes.Amber:
                    OnKillAmber();
                    break;
                case GemYoyoTypes.Ruby:
                    OnKillRuby();
                    break;
                case GemYoyoTypes.Diamond:
                    OnKillDiamond();
                    break;
                case GemYoyoTypes.Onyx:
                    OnKillOnyx();
                    break;
                default:
                    OnKillEmerald();
                    break;
            }
            if (chance < .68f)
            {
                coinSpawn = ItemID.SilverCoin;
            }
            else if (chance >= .68f && chance < .98f)
            {
                coinSpawn = ItemID.GoldCoin;
            }
            else
                coinSpawn = ItemID.PlatinumCoin;
            LostbeltMethods.ScarletSpawnItem(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.Hitbox, coinSpawn);
        }
        #endregion
        #region 绘制
        public override bool PreDraw(ref Color lightColor)
        {
            switch (GemTypes)
            {
                case GemYoyoTypes.Amethyst:
                    PreDraw_Amethyst();
                    break;
                case GemYoyoTypes.Topaz:
                    PreDraw_Topaz();
                    break;
                case GemYoyoTypes.Sapphire:
                    PreDraw_Sapphire();
                    break;
                case GemYoyoTypes.Emerald:
                    PreDraw_Emerald();
                    break;
                case GemYoyoTypes.Amber:
                    PreDraw_Amber();
                    break;
                case GemYoyoTypes.Ruby:
                    PreDraw_Ruby();
                    break;
                case GemYoyoTypes.Diamond:
                    PreDraw_Diamond();
                    break;
                case GemYoyoTypes.Onyx:
                    PreDraw_Onyx();
                    break;
                default:
                    PreDraw_Emerald();
                    break;
            }

            return false;
        }
        public void PreDraw_Amethyst()
        {
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            int drawLength = Projectile.oldPos.Length;
            int length = Projectile.oldPos.Length;
            SB.EnterShaderArea();
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.DarkViolet, 1f, 1f, 1.1f);
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.Violet, 0.9f, 1f, 1.1f);
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.White, 1f, 1f, 1.1f);
            SB.EndShaderArea();
            SB.EndShaderArea();

            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], 0f);
                Vector2 oldPos = lerpPos - Main.screenPosition + Projectile.Size / 2f;
                float oldRot = Projectile.oldRot[i];
                float progress = i / (float)length;
                float xMult = Lerp(1f, .55f, progress);
                float yMult = Lerp(1f, .55f, progress);
                Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale;
                Color c = Color.Lerp(Color.White, Color.Lerp(Color.DarkViolet, Color.Violet, .63f), EaseInOutQuad(progress));
                float opac = Lerp(1f, .3f, EaseInOutExpo(progress));
                Color pixelColor = Color.Lerp(Color.White, Color.Lerp(Color.DarkViolet, Color.Violet, .63f), EaseInOutQuad(progress));
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(1.5f, 1.5f), pixelColor.ToAddColor(50) * opac * 0.815f, oldRot, projTex.Size() / 2f, scale * .95f, 0);
                SB.FastDraw(projTex, oldPos, c.ToAddColor(0) * opac * .315f, oldRot, projTex.Size() / 2f, scale * .98f, 0);
            }

            for (int i = 0; i < 8; i++)
                SB.FastDraw(projTex, drawPos + Main.rand.NextVector2Circular(1.5f, 1.5f) + (TwoPi / 8f * i).ToRotationVector2() * 1.5f, Color.White.ToAddColor(), Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
        }

        public void PreDraw_Topaz()
        {
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            int drawLength = Projectile.oldPos.Length;
            int length = Projectile.oldPos.Length;
            SB.EnterShaderArea();
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.DarkGoldenrod, 1f, 1f, 1.1f);
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.Goldenrod, 0.9f, 1f, 1.1f);
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.White, 1f, 1f, 1.1f);
            SB.EndShaderArea();
            SB.EndShaderArea();

            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], 0f);
                Vector2 oldPos = lerpPos - Main.screenPosition + Projectile.Size / 2f;
                float oldRot = Projectile.oldRot[i];
                float progress = i / (float)length;
                float xMult = Lerp(1f, .55f, progress);
                float yMult = Lerp(1f, .55f, progress);
                Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale;
                Color c = Color.Lerp(Color.White, Color.Lerp(Color.DarkGoldenrod, Color.Goldenrod, .63f), EaseInOutQuad(progress));
                float opac = Lerp(1f, .3f, EaseInOutExpo(progress));
                Color pixelColor = Color.Lerp(Color.White, Color.Lerp(Color.DarkGoldenrod, Color.Goldenrod, .63f), EaseInOutQuad(progress));
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(1.5f, 1.5f), pixelColor.ToAddColor(50) * opac * 0.815f, oldRot, projTex.Size() / 2f, scale * .95f, 0);
                SB.FastDraw(projTex, oldPos, c.ToAddColor(0) * opac * .315f, oldRot, projTex.Size() / 2f, scale * .98f, 0);
            }

            for (int i = 0; i < 8; i++)
                SB.FastDraw(projTex, drawPos + Main.rand.NextVector2Circular(1.5f, 1.5f) + (TwoPi / 8f * i).ToRotationVector2() * 1.5f, Color.White.ToAddColor(), Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
        }

        public void PreDraw_Sapphire()
        {
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            int drawLength = Projectile.oldPos.Length;
            int length = Projectile.oldPos.Length;
            SB.EnterShaderArea();
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.RoyalBlue, 1f, 1f, 1.1f);
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.DodgerBlue, 0.9f, 1f, 1.1f);
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.White, 1f, 1f, 1.1f);
            SB.EndShaderArea();
            SB.EndShaderArea();

            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], 0f);
                Vector2 oldPos = lerpPos - Main.screenPosition + Projectile.Size / 2f;
                float oldRot = Projectile.oldRot[i];
                float progress = i / (float)length;
                float xMult = Lerp(1f, .55f, progress);
                float yMult = Lerp(1f, .55f, progress);
                Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale;
                Color c = Color.Lerp(Color.White, Color.Lerp(Color.RoyalBlue, Color.DodgerBlue, .63f), EaseInOutQuad(progress));
                float opac = Lerp(1f, .3f, EaseInOutExpo(progress));
                Color pixelColor = Color.Lerp(Color.White, Color.Lerp(Color.RoyalBlue, Color.DodgerBlue, .63f), EaseInOutQuad(progress));
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(1.5f, 1.5f), pixelColor.ToAddColor(50) * opac * 0.815f, oldRot, projTex.Size() / 2f, scale * .95f, 0);
                SB.FastDraw(projTex, oldPos, c.ToAddColor(0) * opac * .315f, oldRot, projTex.Size() / 2f, scale * .98f, 0);
            }

            for (int i = 0; i < 8; i++)
                SB.FastDraw(projTex, drawPos + Main.rand.NextVector2Circular(1.5f, 1.5f) + (TwoPi / 8f * i).ToRotationVector2() * 1.5f, Color.White.ToAddColor(), Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
        }

        public void PreDraw_Emerald()
        {
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            int drawLength = Projectile.oldPos.Length;
            int length = Projectile.oldPos.Length;
            SB.EnterShaderArea();
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.DarkGreen, 1f, 1f, 1.1f);
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.LimeGreen, 0.9f, 1f, 1.1f);
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.White, 1f, 1f, 1.1f);
            SB.EndShaderArea();
            SB.EndShaderArea();

            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], 0f);
                Vector2 oldPos = lerpPos - Main.screenPosition + Projectile.Size / 2f;
                float oldRot = Projectile.oldRot[i];
                float progress = i / (float)length;
                float xMult = Lerp(1f, .55f, progress);
                float yMult = Lerp(1f, .55f, progress);
                Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale;
                Color c = Color.Lerp(Color.White, Color.Lerp(Color.DarkGreen, Color.LimeGreen, .63f), EaseInOutQuad(progress));
                float opac = Lerp(1f, .3f, EaseInOutExpo(progress));
                Color pixelColor = Color.Lerp(Color.White, Color.Lerp(Color.DarkGreen, Color.LimeGreen, .63f), EaseInOutQuad(progress));
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(1.5f, 1.5f), pixelColor.ToAddColor(50) * opac * 0.815f, oldRot, projTex.Size() / 2f, scale * .95f, 0);
                SB.FastDraw(projTex, oldPos, c.ToAddColor(0) * opac * .315f, oldRot, projTex.Size() / 2f, scale * .98f, 0);
            }

            for (int i = 0; i < 8; i++)
                SB.FastDraw(projTex, drawPos + Main.rand.NextVector2Circular(1.5f, 1.5f) + (TwoPi / 8f * i).ToRotationVector2() * 1.5f, Color.White.ToAddColor(), Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
        }

        public void PreDraw_Amber()
        {
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            int drawLength = Projectile.oldPos.Length;
            int length = Projectile.oldPos.Length;
            SB.EnterShaderArea();
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.OrangeRed, 1f, 1f, 1.1f);
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.DarkOrange, 0.9f, 1f, 1.1f);
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.White, 1f, 1f, 1.1f);
            SB.EndShaderArea();
            SB.EndShaderArea();

            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], 0f);
                Vector2 oldPos = lerpPos - Main.screenPosition + Projectile.Size / 2f;
                float oldRot = Projectile.oldRot[i];
                float progress = i / (float)length;
                float xMult = Lerp(1f, .55f, progress);
                float yMult = Lerp(1f, .55f, progress);
                Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale;
                Color c = Color.Lerp(Color.White, Color.Lerp(Color.OrangeRed, Color.DarkOrange, .63f), EaseInOutQuad(progress));
                float opac = Lerp(1f, .3f, EaseInOutExpo(progress));
                Color pixelColor = Color.Lerp(Color.White, Color.Lerp(Color.OrangeRed, Color.DarkOrange, .63f), EaseInOutQuad(progress));
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(1.5f, 1.5f), pixelColor.ToAddColor(50) * opac * 0.815f, oldRot, projTex.Size() / 2f, scale * .95f, 0);
                SB.FastDraw(projTex, oldPos, c.ToAddColor(0) * opac * .315f, oldRot, projTex.Size() / 2f, scale * .98f, 0);
            }

            for (int i = 0; i < 8; i++)
                SB.FastDraw(projTex, drawPos + Main.rand.NextVector2Circular(1.5f, 1.5f) + (TwoPi / 8f * i).ToRotationVector2() * 1.5f, Color.White.ToAddColor(), Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
        }

        public void PreDraw_Ruby()
        {
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            int drawLength = Projectile.oldPos.Length;
            int length = Projectile.oldPos.Length;
            SB.EnterShaderArea();
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.DarkRed, 1f, 1f, 1.1f);
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.Crimson, 0.9f, 1f, 1.1f);
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.White, 1f, 1f, 1.1f);
            SB.EndShaderArea();
            SB.EndShaderArea();

            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], 0f);
                Vector2 oldPos = lerpPos - Main.screenPosition + Projectile.Size / 2f;
                float oldRot = Projectile.oldRot[i];
                float progress = i / (float)length;
                float xMult = Lerp(1f, .55f, progress);
                float yMult = Lerp(1f, .55f, progress);
                Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale;
                Color c = Color.Lerp(Color.White, Color.Lerp(Color.DarkRed, Color.Crimson, .63f), EaseInOutQuad(progress));
                float opac = Lerp(1f, .3f, EaseInOutExpo(progress));
                Color pixelColor = Color.Lerp(Color.White, Color.Lerp(Color.DarkRed, Color.Crimson, .63f), EaseInOutQuad(progress));
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(1.5f, 1.5f), pixelColor.ToAddColor(50) * opac * 0.815f, oldRot, projTex.Size() / 2f, scale * .95f, 0);
                SB.FastDraw(projTex, oldPos, c.ToAddColor(0) * opac * .315f, oldRot, projTex.Size() / 2f, scale * .98f, 0);
            }

            for (int i = 0; i < 8; i++)
                SB.FastDraw(projTex, drawPos + Main.rand.NextVector2Circular(1.5f, 1.5f) + (TwoPi / 8f * i).ToRotationVector2() * 1.5f, Color.White.ToAddColor(), Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
        }

        public void PreDraw_Diamond()
        {
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            int drawLength = Projectile.oldPos.Length;
            int length = Projectile.oldPos.Length;
            SB.EnterShaderArea();
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.Gray, 1f, 1f, 1.1f);
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.WhiteSmoke, 0.9f, 1f, 1.1f);
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.White, 1f, 1f, 1.1f);
            SB.EndShaderArea();
            SB.EndShaderArea();

            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], 0f);
                Vector2 oldPos = lerpPos - Main.screenPosition + Projectile.Size / 2f;
                float oldRot = Projectile.oldRot[i];
                float progress = i / (float)length;
                float xMult = Lerp(1f, .55f, progress);
                float yMult = Lerp(1f, .55f, progress);
                Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale;
                Color c = Color.Lerp(Color.White, Color.Lerp(Color.Gray, Color.WhiteSmoke, .63f), EaseInOutQuad(progress));
                float opac = Lerp(1f, .3f, EaseInOutExpo(progress));
                Color pixelColor = Color.Lerp(Color.White, Color.Lerp(Color.Gray, Color.WhiteSmoke, .63f), EaseInOutQuad(progress));
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(1.5f, 1.5f), pixelColor.ToAddColor(50) * opac * 0.815f, oldRot, projTex.Size() / 2f, scale * .95f, 0);
                SB.FastDraw(projTex, oldPos, c.ToAddColor(0) * opac * .315f, oldRot, projTex.Size() / 2f, scale * .98f, 0);
            }

            for (int i = 0; i < 8; i++)
                SB.FastDraw(projTex, drawPos + Main.rand.NextVector2Circular(1.5f, 1.5f) + (TwoPi / 8f * i).ToRotationVector2() * 1.5f, Color.White.ToAddColor(), Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
        }

        public void PreDraw_Onyx()
        {
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            int drawLength = Projectile.oldPos.Length;
            int length = Projectile.oldPos.Length;
            SB.EnterShaderArea();
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.Gray, 1f, 1f, 1.1f);
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.WhiteSmoke, 0.9f, 1f, 1.1f);
            DrawTrails(LostbeltModAssets.Trail_BloomDualLine.Texture, Color.White, 1f, 1f, 1.1f);
            SB.EndShaderArea();
            SB.EndShaderArea();

            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], 0f);
                Vector2 oldPos = lerpPos - Main.screenPosition + Projectile.Size / 2f;
                float oldRot = Projectile.oldRot[i];
                float progress = i / (float)length;
                float xMult = Lerp(1f, .55f, progress);
                float yMult = Lerp(1f, .55f, progress);
                Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale;
                Color c = Color.Lerp(Color.White, Color.Lerp(Color.Gray, Color.WhiteSmoke, .63f), EaseInOutQuad(progress));
                float opac = Lerp(1f, .3f, EaseInOutExpo(progress));
                Color pixelColor = Color.Lerp(Color.White, Color.Lerp(Color.Gray, Color.WhiteSmoke, .63f), EaseInOutQuad(progress));
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(1.5f, 1.5f), pixelColor.ToAddColor(50) * opac * 0.815f, oldRot, projTex.Size() / 2f, scale * .95f, 0);
                SB.FastDraw(projTex, oldPos, c.ToAddColor(0) * opac * .315f, oldRot, projTex.Size() / 2f, scale * .98f, 0);
            }

            for (int i = 0; i < 8; i++)
                SB.FastDraw(projTex, drawPos + Main.rand.NextVector2Circular(1.5f, 1.5f) + (TwoPi / 8f * i).ToRotationVector2() * 1.5f, Color.White.ToAddColor(), Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
        }
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
                trailDrawDates.Add(new(validPosition[j] + Projectile.Size / 2 + posOffset, drawColor, new Vector2(0, 9 * multipleSize * Projectile.scale), rot));
            }
            TrailRender.DrawTrail([.. trailDrawDates], drawSetting);
        }
        #endregion
    }
}
