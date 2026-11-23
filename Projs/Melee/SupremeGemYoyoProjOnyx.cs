using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using Terraria;

namespace LostbeltTraveler.Projs.Melee
{
    public class SupremeGemYoyoProjOnyx : LostbeltProjs
    {
        public override string Texture => GetInstance<Onyx>().Texture;
        public override EnumDamageClass Category => EnumDamageClass.Melee;
        public ref float Timer => ref Projectile.ai[0];
        public float OverallProgress => EaseInOutQuad(Utils.GetLerpValue(0, Projectile.MaxUpdates * 15, Timer, true));
        public float TargerRotation = 0;
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(32);
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
            TargerRotation = RandRotTwoPi;
            base.OnFirstFrame();
        }
        public override void ProjAI()
        {
            Projectile.rotation += Lerp(.15f, .05f, OverallProgress);
            Timer++;
            Projectile farther = Main.projectile[(int)Projectile.ai[2]];
            Vector2 targetCenter = Projectile.Center;
            if (farther.active && farther.owner == Owner.whoAmI && farther.type == ProjectileType<SupremeGemYoyoProjAmethystSmear>())
            {
                targetCenter = farther.Center + TargerRotation.ToRotationVector2() * Main.rand.NextFloat(60f, 80f);
            }
            else
                Projectile.velocity *= .96f;

            float maxTime = Projectile.MaxUpdates * 60f;
            float progress = Clamp(Timer / maxTime, 0f, 1f);
            float speedMult = 20f;
            Vector2 vecToPlayer = targetCenter - Projectile.Center;
            float playerDist = vecToPlayer.Length();
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
        }

        public override bool? CanDamage()
        {
            return false;
        }
        public override void OnKill(int timeLeft)
        {

            Projectile.SpawnInvisBoom(new LostbeltMethods.InvisBoomOptions { DamageClass = Terraria.ModLoader.DamageClass.Melee, HitCooldown = 60, LifeTime = 60, Resize = 250, DamageRatio = .5f });
            ScarletSound(LostbeltSoundAssets.Dialectics_Hit, Projectile.Center, volume: .224f, pitch: .415f);

            if (Projectile.ai[1] > 0)
            {
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.ToSafeNormalize() * 14, ProjectileType<SupremeGemYoyoProjDiamond>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

                float scale = .42f;
                float opac = .85f;
                ECSParticle.CrossGlow(Projectile.Center, Color.LightSkyBlue, 45, opac, scale, 0.1f, BlendState.Additive);
                ECSParticle.CrossGlow(Projectile.Center, Color.Silver, 45, opac, scale * .95f, 0.1f, BlendState.Additive);
                ECSParticle.CrossGlow(Projectile.Center, Color.White, 45, opac, scale * .9f, 0.1f, BlendState.Additive);

                for (int i = 0; i < 16; i++)
                    ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePos(1.2f), RandVelTwoPi(0f, 9f), RandLerpColor(Color.White, Color.LightSkyBlue), 45, 1, Main.rand.NextFloat(.85f, 1.15f) * .64f, 0.2f, BlendState.Additive);
                for (int i = 0; i < 8; i++)
                    ECSParticle.SmokeParticle(Projectile.Center.ToRandCirclePos(2f), RandVelTwoPi(9f), RandLerpColor(Color.White, Color.LightSkyBlue), 45, RandRotTwoPi, .51f, .41f, Main.rand.NextBool(), BlendState.Additive);
                for (int i = 0; i < 8; i++)
                    ECSParticle.LiliesFire(Projectile.Center.ToRandCirclePos(2f), RandVelTwoPi(13f), RandLerpColor(Color.White, Color.LightSkyBlue), 45, RandRotTwoPi, .51f, .31f, true, BlendState.Additive);


            }
            else
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
        }
        public override bool PreDraw(ref Color lightColor)
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
            int length = Projectile.oldPos.Length - 5;
            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], 0f);
                Vector2 oldPos = lerpPos - Main.screenPosition + Projectile.Size / 2f;
                float oldRot = Projectile.oldRot[i];
                //图竖直
                float progress = i / (float)length;
                float xMult = Lerp(1f, .55f, (progress));
                float yMult = Lerp(1f, .55f, progress);
                Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale * lerpOverallScale * .85f;
                Color c = Color.Lerp(Color.DarkViolet, Color.Lerp(Color.Purple, Color.Black, .63f), EaseInOutQuad(progress));
                float opac = Lerp(1f, .3f, EaseInOutExpo(progress)) * .96f;
                Color pixelColor = Color.Lerp(Color.Purple, Color.Lerp(Color.DarkViolet, Color.Violet, .63f), EaseInOutQuad(progress));
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(1f, 1f),
                    pixelColor.ToAddColor(0) * opac * 1.3f, oldRot, projTex.Size() / 2f, scale * 1.51f, 0);
                SB.FastDraw(projTex, oldPos, c * opac * opac * .55f, oldRot, projTex.Size() / 2f, scale * .98f, 0);
            }
            for (int i = 0; i < 3; i++)
                SB.FastDraw(projTex, drawPos.ToRandCirclePos(1), Color.DarkViolet.ToAddColor(), Projectile.rotation, projTex.Size() / 2f, Projectile.scale * 1.31f * lerpOverallScale, 0);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, projTex.Size() / 2f, Projectile.scale * lerpOverallScale, 0);

            return false;
        }
    }
}
