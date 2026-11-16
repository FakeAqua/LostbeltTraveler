using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.ScreenEffect;
using LostbeltTraveler.Globals.Classes;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Requirement
{
    public class TechSniperHeldProj : LostbeltRangedWeaponoutClass
    {
        public override int OriginalItemID => ItemType<TechSniper>();
        public override string Texture => GetInstance<TechSniper>().Texture;
        public override float RecoilPower => 40;
        public override float RecoilWeaponPullbackRatios => base.RecoilWeaponPullbackRatios;
        public override float HoldoutDrawScale => base.HoldoutDrawScale;
        public override Color HoldoutEdgeColor => base.HoldoutEdgeColor;
        public override bool HoldoutEdgeEnable => base.HoldoutEdgeEnable;
        public override Vector2 HoldoutOffset => new Vector2(35, 0);
        public override void ExSD()
        {
            base.ExSD();
            Projectile.DamageType = DamageClass.Magic;
        }
        protected override void OnAttack()
        {
            Vector2 offset = new Vector2(90, -10 * Projectile.direction).RotatedBy(Projectile.rotation);
            Vector2 pos = Projectile.Center + offset;
            Vector2 dir = Projectile.SafeDirByRot();
            int type = ProjectileType<TechSniperLaser>();
            Projectile proj = Projectile.NewProjectileDirect(Owner.GetSource_ItemUse(Owner.HeldItem), pos, dir * 18f, type, Projectile.originalDamage, Projectile.knockBack, Projectile.owner);
            ScreenShakeSystem.AddScreenShakes(pos, 32 + Projectile.Lostbelt().ExecutionStrike.ToInt() * 12, 60, -Projectile.rotation, 0, true, easingFunc: EaseOutExpo);
            for (int i = 0; i < 8; i++)
            {
                Vector2 vel = dir.ToRandVelocity(ToRadians(15f), 1).ToSafeNormalize();
                ECSParticle.HighResolutionThunder(pos, vel * .1f, RandLerpColor(Color.DodgerBlue, Color.White), 40, 1, vel.ToRotation() + PiOver2, Projectile.scale * .63f, 0);
            }
            Vector2 particleOffset = new Vector2(10, 0 * Projectile.direction).RotatedBy(Projectile.rotation);
            for (int i = 0; i < 36; i++)
            {
                Vector2 pos2 = pos.ToRandCirclePos(8) - particleOffset;
                Vector2 vel = Projectile.SafeDirByRot().ToRandVelocity(ToRadians(15), .1f, 11.6f);
                float scale = Projectile.scale * Main.rand.NextFloat(.95f, 1.15f) * 0.38f;
                int timeLeft = Main.rand.Next(30, 45);
                ECSParticle.ShinyCrossStarECS(pos2, vel, RandLerpColor(Color.CornflowerBlue, Color.White), timeLeft, 1, scale);
            }
            for (int i = 0; i < 36; i++)
            {
                Vector2 pos2 = pos.ToRandCirclePos(8) - particleOffset;
                Vector2 vel = Projectile.SafeDirByRot().ToRandVelocity(ToRadians(0), .1f, 19.6f);
                float scale = Projectile.scale * Main.rand.NextFloat(.95f, 1.15f) * 0.38f;
                int timeLeft = Main.rand.Next(30, 45);
                ECSParticle.LightntingGlow(pos2, vel, RandLerpColor(Color.CornflowerBlue, Color.White), timeLeft, 1, scale);
            }

            for (int i = 0; i < 8; i++)
            {
                Vector2 vel = dir.ToRandVelocity(ToRadians(15f), 1).ToSafeNormalize();
                ECSParticle.HighResolutionThunder(pos + vel.ToSafeNormalize() * 35, vel * .1f, RandLerpColor(Color.DodgerBlue, Color.White), 40, 1, vel.ToRotation(), Projectile.scale * .263f, 1);
            }

            base.OnAttack();
        }
        protected override void PreAttack()
        {
            ScarletSound(LostbeltSoundAssets.ASMD_Fire, Projectile.Center, 0.20f, 0, -.4f, 0.1f);

            base.PreAttack();
        }
    }
    public class TechSniperLaser : LostbeltProjs
    {
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(50);
        }
        public override void ExSD()
        {
            Projectile.MaxUpdates = 5;
            Projectile.width = Projectile.height = 16;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.SetupImmnuity(10 * Projectile.MaxUpdates);
            Projectile.penetrate = -1;
        }
        public override void ProjAI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.IsOutScreen())
                return;
            if (Main.rand.NextBool())
                ECSParticle.HighResolutionThunder(Projectile.Center.ToRandCirclePos(4), Projectile.velocity / 8f, Color.LightSkyBlue, 40, 1,
                    Projectile.velocity.ToRotation() + PiOver2, Main.rand.NextFloat(.9f, 1.1f) * .41f, 3);
            ECSParticle.TurbulenceShinyOrb(Projectile.Center.ToRandCirclePos(8), 1f, RandLerpColor(Color.RoyalBlue, Color.DodgerBlue), 40, 1, 0.1f, RandRotTwoPi, .4f);
            //ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePos(8), Projectile.velocity / 4f, RandLerpColor(Color.DeepSkyBlue, Color.RoyalBlue),
            //    40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .8f,rotSpeed:Main.rand.NextFloat(-.05f,.05f));
        }
        public override void OnFirstFrame()
        {
            base.OnFirstFrame();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            float glowScale = Projectile.scale * .3f;
            ECSParticle.CrossGlow(Projectile.Center, Color.DeepSkyBlue, 120, 0.85f, glowScale, 0.1f);
            ECSParticle.CrossGlow(Projectile.Center, Color.White, 120, .85f, glowScale * .96f, 0.1f);
            for (int i = 0; i < 8; i++)
            {
                float args = TwoPi / 8f * i;
                ECSParticle.HighResolutionThunder(Projectile.Center.ToRandCirclePos(16), Vector2.Zero, Color.SkyBlue, 45, 1, args, Projectile.scale * .65f, 3);
            }
            float ringScale = Projectile.scale * 1.2f;
            ECSParticle.Ring(Projectile.Center, Vector2.Zero, Color.SkyBlue, 70, 1, 0, ringScale, .2f, 1);
            ECSParticle.Ring(Projectile.Center, Vector2.Zero, Color.LightSkyBlue, 70, 1, 0, ringScale * .75f, .2f, 1);
            ECSParticle.Ring(Projectile.Center, Vector2.Zero, Color.LightSkyBlue, 70, 1, 0, ringScale * .45f, .2f, 1);
            for (int i = 0; i < 32; i++)
            {
                ECSParticle.TurbulenceShinyOrb(Projectile.Center.ToRandCirclePos(60), 1.2f, RandLerpColor(Color.SkyBlue, Color.LightSkyBlue), 45, 1, Projectile.scale * Main.rand.NextFloat(.9f, 1.1f) * .1f, RandRotTwoPi, .25f);
            }
            SoundEngine.PlaySound(LostbeltSoundAssets.Lightning_Strike with { MaxInstances = 1, Pitch = .63f, PitchVariance = .2f }, Projectile.Center);

            for (int i = 0; i < 16; i++)
            {
                float args = TwoPi / 4f * i + PiOver4;
                ECSParticle.LightntingGlow(Projectile.Center.ToRandCirclePosEdge(6), args.ToRotationVector2() * Main.rand.NextFloat(0.1f, 1f) * 4.8f, Color.LightSkyBlue, 45, 1, Projectile.scale * .65f);
            }
            for (int i = 0; i < 4; i++)
            {
                Vector2 spawnPos = target.Center - Vector2.UnitY * Main.rand.NextFloat(900f, 1100f) + Vector2.UnitX * Main.rand.NextFloat(100f, 400f) * Main.rand.NextBool().ToDirectionInt();
                Vector2 vel = spawnPos.GetNormalVector2(target.Center) * Main.rand.NextFloat(.9f, 1.1f) * 16f;
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), spawnPos, vel, ProjectileType<TechThunder>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                if (target.IsLegal())
                    proj.Lostbelt().CurStoredTarget = target;
            }

            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D line = LostbeltModAssets.Particle_SharpTear;
            int count = Projectile.oldPos.Length;
            float overallScale = 1f;
            float overallAlpha = 1f;

            //Copy-right:VFX
            for (int i = 0; i < count; i++)
            {
                float progress = 1 - (float)i / count;
                float sineScale = MathF.Sin((float)Main.timeForVisualEffects * 0.45f) * 0.1f;
                Vector2 AfterImagePos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition + Main.rand.NextVector2Circular(4.5f, 4.5f); //6f
                float startScale = 1.1f + sineScale;
                float rot = Projectile.oldRot[i] + PiOver2;
                Color between = Color.Lerp(Color.RoyalBlue, Color.DeepSkyBlue, 0.15f);
                Color col = Color.Lerp(between, Color.LightSkyBlue, 1f - progress);
                float easedFadeValue = progress * progress * overallAlpha;
                Vector2 lineScale = new Vector2(0.3f + 0.4f * progress, 1.25f);
                lineScale.Y *= overallScale;
                Vector2 lineScale2 = new Vector2(0.05f + 0.1f * progress, 1.25f);
                lineScale2.Y *= overallScale;
                SB.FastDraw(line, AfterImagePos, Color.Black * .4f * easedFadeValue, rot, line.Size() / 2f, lineScale * startScale, SpriteEffects.None);
                SB.FastDraw(line, AfterImagePos, col.ToAddColor() * .985f * easedFadeValue, rot, line.Size() / 2f, lineScale * startScale, SpriteEffects.None);
                SB.FastDraw(line, AfterImagePos, Color.White.ToAddColor() * .95f * easedFadeValue, rot, line.Size() / 2f, lineScale2 * startScale, SpriteEffects.None);
            }
            return false;
        }
    }
}
