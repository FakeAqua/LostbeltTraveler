using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.Primitives.Trail;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;


namespace LostbeltTraveler.Projs.Melee
{
    public class OnyxYoyoOnyx : LostbeltProjs
    {
        public override string Texture => GetInstance<Onyx>().Texture;
        public override EnumDamageClass Category => EnumDamageClass.Melee;
        public ref float Timer => ref Projectile.ai[0];
        protected string CurrentyGemString => Owner.Lostbelt().gemYoyoStrings;
        public GemYoyoProj.GemYoyoTypes GemTypes = GemYoyoProj.GemYoyoTypes.Onyx;

        public float OverallProgress => EaseInOutQuad(Utils.GetLerpValue(0, Projectile.MaxUpdates * 15, Timer, true));

        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(16);
        }
        public override void ExSD()
        {
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.MaxUpdates = 2;
            Projectile.timeLeft = Projectile.MaxUpdates * 40;
            Projectile.width = Projectile.height = 16;
            Projectile.SetupImmnuity(-1);
            Projectile.penetrate = 1;
        }
        public override void OnFirstFrame()
        {
            base.OnFirstFrame();
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void ProjAI()
        {
            Projectile.rotation += Lerp(.15f, .05f, OverallProgress);
            Timer++;
            Projectile.velocity *= .92f;

            if (Projectile.IsOutScreen())
                return;
        }
        public override void OnKill(int timeLeft)
        {
            Projectile.SpawnInvisBoom(new LostbeltMethods.InvisBoomOptions { DamageClass = Terraria.ModLoader.DamageClass.Melee, HitCooldown = 60, LifeTime = 60, Resize = 150, DamageRatio = .5f });
            ScarletSound(LostbeltSoundAssets.Dialectics_Hit, Projectile.Center, pitch: .15f);
            switch (GemTypes)
            {
                case GemYoyoProj.GemYoyoTypes.Amethyst:
                    DoBehaviourOnKill_BoomAmethyst();
                    break;
                case GemYoyoProj.GemYoyoTypes.Topaz:
                    DoBehaviourOnKill_BoomTopaz();
                    break;
                case GemYoyoProj.GemYoyoTypes.Sapphire:
                    DoBehaviourOnKill_BoomSapphire();
                    break;
                case GemYoyoProj.GemYoyoTypes.Emerald:
                    DoBehaviourOnKill_BoomEmerald();
                    break;
                case GemYoyoProj.GemYoyoTypes.Amber:
                    DoBehaviourOnKill_BoomAmber();
                    break;
                case GemYoyoProj.GemYoyoTypes.Ruby:
                    DoBehaviourOnKill_BoomRuby();
                    break;
                case GemYoyoProj.GemYoyoTypes.Diamond:
                    DoBehaviourOnKill_BoomDiamond();
                    break;
                case GemYoyoProj.GemYoyoTypes.Onyx:
                    DoBehaviourOnKill_BoomOnyx();
                    break;
                default:
                    DoBehaviourOnKill_BoomOnyx();
                    break;
            }
        }
        public void DoBehaviourOnKill_BoomAmethyst()
        {
            float scale = .42f;
            ECSParticle.CrossGlow(Projectile.Center, Color.DarkViolet, 45, 1, scale, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(Projectile.Center, Color.Black, 45, 1, scale * .95f, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(Projectile.Center, Color.Violet, 45, 1, scale * .9f, 0.1f, BlendState.NonPremultiplied);

            for (int i = 0; i < 16; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePos(1.2f), RandVelTwoPi(0f, 9f), RandLerpColor(Color.Violet, Color.DarkViolet), 45, 1, Main.rand.NextFloat(.85f, 1.15f) * .64f, 0.2f, BlendState.AlphaBlend);
            for (int i = 0; i < 16; i++)
                ECSParticle.SmokeParticle(Projectile.Center.ToRandCirclePos(2f), RandVelTwoPi(9f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .41f, Main.rand.NextBool(), BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.LiliesFire(Projectile.Center.ToRandCirclePos(2f), RandVelTwoPi(13f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .31f, true, BlendState.Additive);
        }

        public void DoBehaviourOnKill_BoomTopaz()
        {
            float scale = .42f;
            ECSParticle.CrossGlow(Projectile.Center, Color.DarkViolet, 45, 1, scale, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(Projectile.Center, Color.Black, 45, 1, scale * .95f, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(Projectile.Center, Color.Violet, 45, 1, scale * .9f, 0.1f, BlendState.NonPremultiplied);

            for (int i = 0; i < 16; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePos(1.2f), RandVelTwoPi(0f, 9f), RandLerpColor(Color.Violet, Color.DarkViolet), 45, 1, Main.rand.NextFloat(.85f, 1.15f) * .64f, 0.2f, BlendState.AlphaBlend);
            for (int i = 0; i < 16; i++)
                ECSParticle.SmokeParticle(Projectile.Center.ToRandCirclePos(2f), RandVelTwoPi(9f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .41f, Main.rand.NextBool(), BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.LiliesFire(Projectile.Center.ToRandCirclePos(2f), RandVelTwoPi(13f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .31f, true, BlendState.Additive);
        }

        public void DoBehaviourOnKill_BoomSapphire()
        {
            float scale = .42f;
            ECSParticle.CrossGlow(Projectile.Center, Color.DarkViolet, 45, 1, scale, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(Projectile.Center, Color.Black, 45, 1, scale * .95f, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(Projectile.Center, Color.Violet, 45, 1, scale * .9f, 0.1f, BlendState.NonPremultiplied);

            for (int i = 0; i < 16; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePos(1.2f), RandVelTwoPi(0f, 9f), RandLerpColor(Color.Violet, Color.DarkViolet), 45, 1, Main.rand.NextFloat(.85f, 1.15f) * .64f, 0.2f, BlendState.AlphaBlend);
            for (int i = 0; i < 16; i++)
                ECSParticle.SmokeParticle(Projectile.Center.ToRandCirclePos(2f), RandVelTwoPi(9f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .41f, Main.rand.NextBool(), BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.LiliesFire(Projectile.Center.ToRandCirclePos(2f), RandVelTwoPi(13f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .31f, true, BlendState.Additive);
        }

        public void DoBehaviourOnKill_BoomEmerald()
        {
            float scale = .42f;
            ECSParticle.CrossGlow(Projectile.Center, Color.DarkViolet, 45, 1, scale, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(Projectile.Center, Color.Black, 45, 1, scale * .95f, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(Projectile.Center, Color.Violet, 45, 1, scale * .9f, 0.1f, BlendState.NonPremultiplied);

            for (int i = 0; i < 16; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePos(1.2f), RandVelTwoPi(0f, 9f), RandLerpColor(Color.Violet, Color.DarkViolet), 45, 1, Main.rand.NextFloat(.85f, 1.15f) * .64f, 0.2f, BlendState.AlphaBlend);
            for (int i = 0; i < 16; i++)
                ECSParticle.SmokeParticle(Projectile.Center.ToRandCirclePos(2f), RandVelTwoPi(9f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .41f, Main.rand.NextBool(), BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.LiliesFire(Projectile.Center.ToRandCirclePos(2f), RandVelTwoPi(13f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .31f, true, BlendState.Additive);
        }

        public void DoBehaviourOnKill_BoomAmber()
        {
            float scale = .42f;
            ECSParticle.CrossGlow(Projectile.Center, Color.DarkViolet, 45, 1, scale, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(Projectile.Center, Color.Black, 45, 1, scale * .95f, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(Projectile.Center, Color.Violet, 45, 1, scale * .9f, 0.1f, BlendState.NonPremultiplied);

            for (int i = 0; i < 16; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePos(1.2f), RandVelTwoPi(0f, 9f), RandLerpColor(Color.Violet, Color.DarkViolet), 45, 1, Main.rand.NextFloat(.85f, 1.15f) * .64f, 0.2f, BlendState.AlphaBlend);
            for (int i = 0; i < 16; i++)
                ECSParticle.SmokeParticle(Projectile.Center.ToRandCirclePos(2f), RandVelTwoPi(9f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .41f, Main.rand.NextBool(), BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.LiliesFire(Projectile.Center.ToRandCirclePos(2f), RandVelTwoPi(13f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .31f, true, BlendState.Additive);
        }

        public void DoBehaviourOnKill_BoomRuby()
        {
            float scale = .42f;
            ECSParticle.CrossGlow(Projectile.Center, Color.DarkViolet, 45, 1, scale, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(Projectile.Center, Color.Black, 45, 1, scale * .95f, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(Projectile.Center, Color.Violet, 45, 1, scale * .9f, 0.1f, BlendState.NonPremultiplied);

            for (int i = 0; i < 16; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePos(1.2f), RandVelTwoPi(0f, 9f), RandLerpColor(Color.Violet, Color.DarkViolet), 45, 1, Main.rand.NextFloat(.85f, 1.15f) * .64f, 0.2f, BlendState.AlphaBlend);
            for (int i = 0; i < 16; i++)
                ECSParticle.SmokeParticle(Projectile.Center.ToRandCirclePos(2f), RandVelTwoPi(9f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .41f, Main.rand.NextBool(), BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.LiliesFire(Projectile.Center.ToRandCirclePos(2f), RandVelTwoPi(13f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .31f, true, BlendState.Additive);
        }

        public void DoBehaviourOnKill_BoomDiamond()
        {
            float scale = .42f;
            ECSParticle.CrossGlow(Projectile.Center, Color.DarkViolet, 45, 1, scale, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(Projectile.Center, Color.Black, 45, 1, scale * .95f, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(Projectile.Center, Color.Violet, 45, 1, scale * .9f, 0.1f, BlendState.NonPremultiplied);

            for (int i = 0; i < 16; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePos(1.2f), RandVelTwoPi(0f, 9f), RandLerpColor(Color.Violet, Color.DarkViolet), 45, 1, Main.rand.NextFloat(.85f, 1.15f) * .64f, 0.2f, BlendState.AlphaBlend);
            for (int i = 0; i < 16; i++)
                ECSParticle.SmokeParticle(Projectile.Center.ToRandCirclePos(2f), RandVelTwoPi(9f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .41f, Main.rand.NextBool(), BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.LiliesFire(Projectile.Center.ToRandCirclePos(2f), RandVelTwoPi(13f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .31f, true, BlendState.Additive);
        }

        public void DoBehaviourOnKill_BoomOnyx()
        {
            float scale = .42f;
            ECSParticle.CrossGlow(Projectile.Center, Color.DarkViolet, 45, 1, scale, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(Projectile.Center, Color.Black, 45, 1, scale * .95f, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(Projectile.Center, Color.Violet, 45, 1, scale * .9f, 0.1f, BlendState.NonPremultiplied);

            for (int i = 0; i < 16; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePos(1.2f), RandVelTwoPi(0f, 9f), RandLerpColor(Color.Violet, Color.DarkViolet), 45, 1, Main.rand.NextFloat(.85f, 1.15f) * .64f, 0.2f, BlendState.AlphaBlend);
            for (int i = 0; i < 16; i++)
                ECSParticle.SmokeParticle(Projectile.Center.ToRandCirclePos(2f), RandVelTwoPi(9f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .41f, Main.rand.NextBool(), BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.LiliesFire(Projectile.Center.ToRandCirclePos(2f), RandVelTwoPi(13f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .31f, true, BlendState.Additive);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            switch (GemTypes)
            {
                case GemYoyoProj.GemYoyoTypes.Amethyst:
                    DoPreDraw_Amethyst();
                    break;
                case GemYoyoProj.GemYoyoTypes.Topaz:
                    DoPreDraw_Topaz();
                    break;
                case GemYoyoProj.GemYoyoTypes.Sapphire:
                    DoPreDraw_Sapphire();
                    break;
                case GemYoyoProj.GemYoyoTypes.Emerald:
                    DoPreDraw_Emerald();
                    break;
                case GemYoyoProj.GemYoyoTypes.Amber:
                    DoPreDraw_Amber();
                    break;
                case GemYoyoProj.GemYoyoTypes.Ruby:
                    DoPreDraw_Ruby();
                    break;
                case GemYoyoProj.GemYoyoTypes.Diamond:
                    DoPreDraw_Diamond();
                    break;
                case GemYoyoProj.GemYoyoTypes.Onyx:
                    DoPreDraw_Onyx();
                    break;
                default:
                    DoPreDraw_Onyx();
                    break;
            }
            return false;
        }
        public void DoPreDraw_Amethyst()
        {
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            float threshold = .35f;
            float lerpOverallScale;
            float overallProgress1;
            float scaleThreshold = 1.12f;
            if (OverallProgress > threshold)
            {
                float p = Utils.GetLerpValue(threshold, 1, OverallProgress, true);
                overallProgress1 = EaseOutCubic(1 - p);
                lerpOverallScale = Lerp(1f, scaleThreshold, overallProgress1);
            }
            else
            {
                float p = Utils.GetLerpValue(0, threshold, OverallProgress, true);
                overallProgress1 = EaseOutCubic(p);
                lerpOverallScale = Lerp(0, scaleThreshold, overallProgress1);
            }
            int length = Projectile.oldPos.Length;
            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], 0f);
                Vector2 oldPos = lerpPos - Main.screenPosition + Projectile.Size / 2f;
                float oldRot = Projectile.oldRot[i];
                //图竖直
                float progress = i / (float)length;
                float xMult = Lerp(1f, .55f, (progress));
                float yMult = Lerp(1f, .55f, progress);
                Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale * lerpOverallScale;
                Color c = Color.Lerp(Color.Black, Color.Lerp(Color.Purple, Color.Black, .63f), EaseInOutQuad(progress));
                float opac = Lerp(1f, .3f, EaseInOutExpo(progress));
                Color pixelColor = Color.Lerp(Color.Purple, Color.Lerp(Color.DarkViolet, Color.Violet, .63f), EaseInOutQuad(progress));
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(1f, 1f), pixelColor.ToAddColor(0) * opac * 0.995f, oldRot, projTex.Size() / 2f, scale * 1.51f, 0);
                SB.FastDraw(projTex, oldPos, c * opac * opac * .55f, oldRot, projTex.Size() / 2f, scale * .98f, 0);
            }
            for (int i = 0; i < 3; i++)
                SB.FastDraw(projTex, drawPos.ToRandCirclePos(1), Color.DarkViolet.ToAddColor(), Projectile.rotation, projTex.Size() / 2f, Projectile.scale * 1.31f * lerpOverallScale, 0);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, projTex.Size() / 2f, Projectile.scale * lerpOverallScale, 0);
        }

        public void DoPreDraw_Topaz()
        {
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            float threshold = .35f;
            float lerpOverallScale;
            float overallProgress1;
            float scaleThreshold = 1.12f;
            if (OverallProgress > threshold)
            {
                float p = Utils.GetLerpValue(threshold, 1, OverallProgress, true);
                overallProgress1 = EaseOutCubic(1 - p);
                lerpOverallScale = Lerp(1f, scaleThreshold, overallProgress1);
            }
            else
            {
                float p = Utils.GetLerpValue(0, threshold, OverallProgress, true);
                overallProgress1 = EaseOutCubic(p);
                lerpOverallScale = Lerp(0, scaleThreshold, overallProgress1);
            }
            int length = Projectile.oldPos.Length;
            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], 0f);
                Vector2 oldPos = lerpPos - Main.screenPosition + Projectile.Size / 2f;
                float oldRot = Projectile.oldRot[i];
                //图竖直
                float progress = i / (float)length;
                float xMult = Lerp(1f, .55f, (progress));
                float yMult = Lerp(1f, .55f, progress);
                Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale * lerpOverallScale;
                Color c = Color.Lerp(Color.Black, Color.Lerp(Color.Purple, Color.Black, .63f), EaseInOutQuad(progress));
                float opac = Lerp(1f, .3f, EaseInOutExpo(progress));
                Color pixelColor = Color.Lerp(Color.Purple, Color.Lerp(Color.DarkViolet, Color.Violet, .63f), EaseInOutQuad(progress));
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(1f, 1f), pixelColor.ToAddColor(0) * opac * 0.995f, oldRot, projTex.Size() / 2f, scale * 1.51f, 0);
                SB.FastDraw(projTex, oldPos, c * opac * opac * .55f, oldRot, projTex.Size() / 2f, scale * .98f, 0);
            }
            for (int i = 0; i < 3; i++)
                SB.FastDraw(projTex, drawPos.ToRandCirclePos(1), Color.DarkViolet.ToAddColor(), Projectile.rotation, projTex.Size() / 2f, Projectile.scale * 1.31f * lerpOverallScale, 0);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, projTex.Size() / 2f, Projectile.scale * lerpOverallScale, 0);
        }

        public void DoPreDraw_Sapphire()
        {
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            float threshold = .35f;
            float lerpOverallScale;
            float overallProgress1;
            float scaleThreshold = 1.12f;
            if (OverallProgress > threshold)
            {
                float p = Utils.GetLerpValue(threshold, 1, OverallProgress, true);
                overallProgress1 = EaseOutCubic(1 - p);
                lerpOverallScale = Lerp(1f, scaleThreshold, overallProgress1);
            }
            else
            {
                float p = Utils.GetLerpValue(0, threshold, OverallProgress, true);
                overallProgress1 = EaseOutCubic(p);
                lerpOverallScale = Lerp(0, scaleThreshold, overallProgress1);
            }
            int length = Projectile.oldPos.Length;
            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], 0f);
                Vector2 oldPos = lerpPos - Main.screenPosition + Projectile.Size / 2f;
                float oldRot = Projectile.oldRot[i];
                //图竖直
                float progress = i / (float)length;
                float xMult = Lerp(1f, .55f, (progress));
                float yMult = Lerp(1f, .55f, progress);
                Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale * lerpOverallScale;
                Color c = Color.Lerp(Color.Black, Color.Lerp(Color.Purple, Color.Black, .63f), EaseInOutQuad(progress));
                float opac = Lerp(1f, .3f, EaseInOutExpo(progress));
                Color pixelColor = Color.Lerp(Color.Purple, Color.Lerp(Color.DarkViolet, Color.Violet, .63f), EaseInOutQuad(progress));
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(1f, 1f), pixelColor.ToAddColor(0) * opac * 0.995f, oldRot, projTex.Size() / 2f, scale * 1.51f, 0);
                SB.FastDraw(projTex, oldPos, c * opac * opac * .55f, oldRot, projTex.Size() / 2f, scale * .98f, 0);
            }
            for (int i = 0; i < 3; i++)
                SB.FastDraw(projTex, drawPos.ToRandCirclePos(1), Color.DarkViolet.ToAddColor(), Projectile.rotation, projTex.Size() / 2f, Projectile.scale * 1.31f * lerpOverallScale, 0);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, projTex.Size() / 2f, Projectile.scale * lerpOverallScale, 0);
        }

        public void DoPreDraw_Emerald()
        {
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            float threshold = .35f;
            float lerpOverallScale;
            float overallProgress1;
            float scaleThreshold = 1.12f;
            if (OverallProgress > threshold)
            {
                float p = Utils.GetLerpValue(threshold, 1, OverallProgress, true);
                overallProgress1 = EaseOutCubic(1 - p);
                lerpOverallScale = Lerp(1f, scaleThreshold, overallProgress1);
            }
            else
            {
                float p = Utils.GetLerpValue(0, threshold, OverallProgress, true);
                overallProgress1 = EaseOutCubic(p);
                lerpOverallScale = Lerp(0, scaleThreshold, overallProgress1);
            }
            int length = Projectile.oldPos.Length;
            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], 0f);
                Vector2 oldPos = lerpPos - Main.screenPosition + Projectile.Size / 2f;
                float oldRot = Projectile.oldRot[i];
                //图竖直
                float progress = i / (float)length;
                float xMult = Lerp(1f, .55f, (progress));
                float yMult = Lerp(1f, .55f, progress);
                Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale * lerpOverallScale;
                Color c = Color.Lerp(Color.Black, Color.Lerp(Color.Purple, Color.Black, .63f), EaseInOutQuad(progress));
                float opac = Lerp(1f, .3f, EaseInOutExpo(progress));
                Color pixelColor = Color.Lerp(Color.Purple, Color.Lerp(Color.DarkViolet, Color.Violet, .63f), EaseInOutQuad(progress));
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(1f, 1f), pixelColor.ToAddColor(0) * opac * 0.995f, oldRot, projTex.Size() / 2f, scale * 1.51f, 0);
                SB.FastDraw(projTex, oldPos, c * opac * opac * .55f, oldRot, projTex.Size() / 2f, scale * .98f, 0);
            }
            for (int i = 0; i < 3; i++)
                SB.FastDraw(projTex, drawPos.ToRandCirclePos(1), Color.DarkViolet.ToAddColor(), Projectile.rotation, projTex.Size() / 2f, Projectile.scale * 1.31f * lerpOverallScale, 0);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, projTex.Size() / 2f, Projectile.scale * lerpOverallScale, 0);
        }

        public void DoPreDraw_Amber()
        {
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            float threshold = .35f;
            float lerpOverallScale;
            float overallProgress1;
            float scaleThreshold = 1.12f;
            if (OverallProgress > threshold)
            {
                float p = Utils.GetLerpValue(threshold, 1, OverallProgress, true);
                overallProgress1 = EaseOutCubic(1 - p);
                lerpOverallScale = Lerp(1f, scaleThreshold, overallProgress1);
            }
            else
            {
                float p = Utils.GetLerpValue(0, threshold, OverallProgress, true);
                overallProgress1 = EaseOutCubic(p);
                lerpOverallScale = Lerp(0, scaleThreshold, overallProgress1);
            }
            int length = Projectile.oldPos.Length;
            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], 0f);
                Vector2 oldPos = lerpPos - Main.screenPosition + Projectile.Size / 2f;
                float oldRot = Projectile.oldRot[i];
                //图竖直
                float progress = i / (float)length;
                float xMult = Lerp(1f, .55f, (progress));
                float yMult = Lerp(1f, .55f, progress);
                Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale * lerpOverallScale;
                Color c = Color.Lerp(Color.Black, Color.Lerp(Color.Purple, Color.Black, .63f), EaseInOutQuad(progress));
                float opac = Lerp(1f, .3f, EaseInOutExpo(progress));
                Color pixelColor = Color.Lerp(Color.Purple, Color.Lerp(Color.DarkViolet, Color.Violet, .63f), EaseInOutQuad(progress));
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(1f, 1f), pixelColor.ToAddColor(0) * opac * 0.995f, oldRot, projTex.Size() / 2f, scale * 1.51f, 0);
                SB.FastDraw(projTex, oldPos, c * opac * opac * .55f, oldRot, projTex.Size() / 2f, scale * .98f, 0);
            }
            for (int i = 0; i < 3; i++)
                SB.FastDraw(projTex, drawPos.ToRandCirclePos(1), Color.DarkViolet.ToAddColor(), Projectile.rotation, projTex.Size() / 2f, Projectile.scale * 1.31f * lerpOverallScale, 0);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, projTex.Size() / 2f, Projectile.scale * lerpOverallScale, 0);
        }

        public void DoPreDraw_Ruby()
        {
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            float threshold = .35f;
            float lerpOverallScale;
            float overallProgress1;
            float scaleThreshold = 1.12f;
            if (OverallProgress > threshold)
            {
                float p = Utils.GetLerpValue(threshold, 1, OverallProgress, true);
                overallProgress1 = EaseOutCubic(1 - p);
                lerpOverallScale = Lerp(1f, scaleThreshold, overallProgress1);
            }
            else
            {
                float p = Utils.GetLerpValue(0, threshold, OverallProgress, true);
                overallProgress1 = EaseOutCubic(p);
                lerpOverallScale = Lerp(0, scaleThreshold, overallProgress1);
            }
            int length = Projectile.oldPos.Length;
            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], 0f);
                Vector2 oldPos = lerpPos - Main.screenPosition + Projectile.Size / 2f;
                float oldRot = Projectile.oldRot[i];
                //图竖直
                float progress = i / (float)length;
                float xMult = Lerp(1f, .55f, (progress));
                float yMult = Lerp(1f, .55f, progress);
                Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale * lerpOverallScale;
                Color c = Color.Lerp(Color.Black, Color.Lerp(Color.Purple, Color.Black, .63f), EaseInOutQuad(progress));
                float opac = Lerp(1f, .3f, EaseInOutExpo(progress));
                Color pixelColor = Color.Lerp(Color.Purple, Color.Lerp(Color.DarkViolet, Color.Violet, .63f), EaseInOutQuad(progress));
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(1f, 1f), pixelColor.ToAddColor(0) * opac * 0.995f, oldRot, projTex.Size() / 2f, scale * 1.51f, 0);
                SB.FastDraw(projTex, oldPos, c * opac * opac * .55f, oldRot, projTex.Size() / 2f, scale * .98f, 0);
            }
            for (int i = 0; i < 3; i++)
                SB.FastDraw(projTex, drawPos.ToRandCirclePos(1), Color.DarkViolet.ToAddColor(), Projectile.rotation, projTex.Size() / 2f, Projectile.scale * 1.31f * lerpOverallScale, 0);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, projTex.Size() / 2f, Projectile.scale * lerpOverallScale, 0);
        }

        public void DoPreDraw_Diamond()
        {
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            float threshold = .35f;
            float lerpOverallScale;
            float overallProgress1;
            float scaleThreshold = 1.12f;
            if (OverallProgress > threshold)
            {
                float p = Utils.GetLerpValue(threshold, 1, OverallProgress, true);
                overallProgress1 = EaseOutCubic(1 - p);
                lerpOverallScale = Lerp(1f, scaleThreshold, overallProgress1);
            }
            else
            {
                float p = Utils.GetLerpValue(0, threshold, OverallProgress, true);
                overallProgress1 = EaseOutCubic(p);
                lerpOverallScale = Lerp(0, scaleThreshold, overallProgress1);
            }
            int length = Projectile.oldPos.Length;
            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], 0f);
                Vector2 oldPos = lerpPos - Main.screenPosition + Projectile.Size / 2f;
                float oldRot = Projectile.oldRot[i];
                //图竖直
                float progress = i / (float)length;
                float xMult = Lerp(1f, .55f, (progress));
                float yMult = Lerp(1f, .55f, progress);
                Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale * lerpOverallScale;
                Color c = Color.Lerp(Color.Black, Color.Lerp(Color.Purple, Color.Black, .63f), EaseInOutQuad(progress));
                float opac = Lerp(1f, .3f, EaseInOutExpo(progress));
                Color pixelColor = Color.Lerp(Color.Purple, Color.Lerp(Color.DarkViolet, Color.Violet, .63f), EaseInOutQuad(progress));
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(1f, 1f), pixelColor.ToAddColor(0) * opac * 0.995f, oldRot, projTex.Size() / 2f, scale * 1.51f, 0);
                SB.FastDraw(projTex, oldPos, c * opac * opac * .55f, oldRot, projTex.Size() / 2f, scale * .98f, 0);
            }
            for (int i = 0; i < 3; i++)
                SB.FastDraw(projTex, drawPos.ToRandCirclePos(1), Color.DarkViolet.ToAddColor(), Projectile.rotation, projTex.Size() / 2f, Projectile.scale * 1.31f * lerpOverallScale, 0);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, projTex.Size() / 2f, Projectile.scale * lerpOverallScale, 0);
        }

        public void DoPreDraw_Onyx()
        {
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            float threshold = .35f;
            float lerpOverallScale;
            float overallProgress1;
            float scaleThreshold = 1.12f;
            if (OverallProgress > threshold)
            {
                float p = Utils.GetLerpValue(threshold, 1, OverallProgress, true);
                overallProgress1 = EaseOutCubic(1 - p);
                lerpOverallScale = Lerp(1f, scaleThreshold, overallProgress1);
            }
            else
            {
                float p = Utils.GetLerpValue(0, threshold, OverallProgress, true);
                overallProgress1 = EaseOutCubic(p);
                lerpOverallScale = Lerp(0, scaleThreshold, overallProgress1);
            }
            int length = Projectile.oldPos.Length;
            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], 0f);
                Vector2 oldPos = lerpPos - Main.screenPosition + Projectile.Size / 2f;
                float oldRot = Projectile.oldRot[i];
                //图竖直
                float progress = i / (float)length;
                float xMult = Lerp(1f, .55f, (progress));
                float yMult = Lerp(1f, .55f, progress);
                Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale * lerpOverallScale;
                Color c = Color.Lerp(Color.Black, Color.Lerp(Color.Purple, Color.Black, .63f), EaseInOutQuad(progress));
                float opac = Lerp(1f, .3f, EaseInOutExpo(progress));
                Color pixelColor = Color.Lerp(Color.Purple, Color.Lerp(Color.DarkViolet, Color.Violet, .63f), EaseInOutQuad(progress));
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(1f, 1f), pixelColor.ToAddColor(0) * opac * 0.995f, oldRot, projTex.Size() / 2f, scale * 1.51f, 0);
                SB.FastDraw(projTex, oldPos, c * opac * opac * .55f, oldRot, projTex.Size() / 2f, scale * .98f, 0);
            }
            for (int i = 0; i < 3; i++)
                SB.FastDraw(projTex, drawPos.ToRandCirclePos(1), Color.DarkViolet.ToAddColor(), Projectile.rotation, projTex.Size() / 2f, Projectile.scale * 1.31f * lerpOverallScale, 0);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, projTex.Size() / 2f, Projectile.scale * lerpOverallScale, 0);
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
                trailDrawDates.Add(new(validPosition[j] + Projectile.Size / 2 + posOffset, drawColor, new Vector2(0, 6 * multipleSize * Projectile.scale), rot));
            }
            TrailRender.DrawTrail([.. trailDrawDates], drawSetting);
        }
    }
}
