using LostbeltTraveler.Core.NetCode;
using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.ScreenEffect;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Items.Weapons.Executor.Firearm;
using System;
using Terraria;

namespace LostbeltTraveler.Projs.Executor.Firearm
{
    public class PrestigiousEnergyRifleHeldProj : LostbeltRangedWeaponoutClass
    {
        public override EnumDamageClass Category => EnumDamageClass.Executor;
        public override int OriginalItemID => ItemType<PrestigiousEnergyRifle>();
        public override string Texture => GetInstance<PrestigiousEnergyRifle>().Texture;
        public override float RecoilPower => 10;
        public override int ProjExtraUpdates => 2;
        public override float HoldoutDrawScale => base.HoldoutDrawScale;
        public override Vector2 HoldoutOffset => new Vector2(20, 0);
        public override Color HoldoutEdgeColor => Color.LightSkyBlue;
        public bool CloseEnoughToShotgun = false;
        public float ShotgunTimer = 0;
        public float ShotgunAttackSpeed => (int)(Owner.ApplyWeaponAttackSpeed(Owner.HeldItem,
            Owner.HeldItem.useTime * Projectile.MaxUpdates, 5 * Projectile.MaxUpdates) * 5.5f);
        public NPC TryGetClosetTarget()
        {
            float searchDistance = 320;
            NPC n = null;
            foreach (NPC npc in Main.ActiveNPCs)
            {
                float exDist = npc.width + npc.height;
                if ((Projectile.Center - npc.Center).LengthSquared() > (searchDistance + exDist) * (searchDistance + exDist))
                    continue;
                if (!npc.IsLegal())
                    continue;
                if (Math.Sign(npc.Center.X - Projectile.Center.X) != Owner.direction)
                    continue;
                float curDistance = Vector2.Distance(npc.Center, Projectile.Center);
                if (curDistance < searchDistance)
                {
                    searchDistance = curDistance;
                    n = npc;
                }

            }
            return n;
        }
        protected override void UpdateGlobalReset()
        {
            base.UpdateGlobalReset();
            CloseEnoughToShotgun = false;
        }
        protected override void PreAttack()
        {

            if (!Owner.GetExecutionSrike())
                Projectile.Lostbelt().ExecutionStrike = false;
            if (Owner.GetExecutionSrike() && !Projectile.Lostbelt().ExecutionStrike)
            {
                Projectile.Lostbelt().ExecutionStrike = true;
                Owner.RemoveExecutionProgress(OriginalItemID);
                return;
            }
        }
        protected override void UpdateRecoil()
        {
            NPC safeTarget = TryGetClosetTarget();
            if (safeTarget.IsLegal())
                CloseEnoughToShotgun = true;
            base.UpdateRecoil();
        }
        protected override void UpdateWeaponUsing()
        {
            if (!CloseEnoughToShotgun)
                return;
            ShotgunTimer++;
            if (ShotgunTimer >= ShotgunAttackSpeed && Projectile.CanSpawnChild())
            {
                ScarletSound(LostbeltSoundAssets.ASMD_ExecutionFire, Projectile.Center, .3f);
                Vector2 dir = Projectile.SafeDirByRot();
                Vector2 offset = new Vector2(45, -5 * Projectile.direction).RotatedBy(Projectile.rotation);
                Vector2 pos = Projectile.Center + offset;
                Vector2 particleOffset = new Vector2(10, 0 * Projectile.direction).RotatedBy(Projectile.rotation);
                ScreenShakeSystem.AddScreenShakes(Projectile.Center, 20f, 30, RandRotTwoPi);
                int projDamage = (int)Owner.GetTotalDamage<ExecutorDamageClass>().ApplyTo(Owner.HeldItem.damage);
                for (int i = -2; i <= 2; i++)
                {
                    Vector2 shotgunDirection = Projectile.SafeDirByRot().RotatedBy(ToRadians(10 * i));
                    float shotgunSpeed = 8;
                    Projectile proj2 = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, shotgunDirection * shotgunSpeed,
                        ProjectileType<PrestigiousEnergyRifleBurst>(), projDamage, Projectile.knockBack, Projectile.owner, shotgunSpeed, i);
                }
                for (int i = 0; i < 36; i++)
                {
                    Vector2 vel = dir.ToRandVelocity(ToRadians(45), 5f, 20.6f);
                    Vector2 pos2 = pos.ToRandCirclePos(8) - particleOffset + vel;
                    float scale = Projectile.scale * Main.rand.NextFloat(.95f, 1.15f) * 0.921f;
                    ECSParticle.HighResolutionThunder(pos2, vel, RandLerpColor(Color.RoyalBlue, Color.DodgerBlue), 20, 1, vel.ToRotation() + PiOver2, scale, 3);

                }
                for (int i = 0; i < 36; i++)
                {
                    Vector2 pos2 = pos.ToRandCirclePos(8) - particleOffset;
                    Vector2 vel = dir.ToRandVelocity(ToRadians(45), .1f, 21.6f);
                    float scale = Projectile.scale * Main.rand.NextFloat(.95f, 1.15f) * 0.78f;
                    ECSParticle.ShinyCrossStarECS(pos2, vel, RandLerpColor(Color.RoyalBlue, Color.LightSkyBlue), Main.rand.Next(30, 45), 1, scale);
                }

                for (int i = 0; i < 36; i++)
                {
                    bool alt = Main.rand.NextBool();
                    BlendState bs = alt ? BlendState.Additive : BlendState.AlphaBlend;
                    ECSParticle.SmokeParticle(pos, dir.ToRandVelocity(ToRadians(30), 0.4f, 30.4f),
                        RandLerpColor(Color.RoyalBlue, Color.LightSkyBlue), Main.rand.Next(45, 65), RandRotTwoPi, 1, 0.63f * Main.rand.NextFloat(.95f, 1.25f), alt, bs);
                }
                ShotgunTimer = 0;
            }
        }
        protected override void UpdateWeaponIdle()
        {
            if (ShotgunTimer < ShotgunAttackSpeed && CloseEnoughToShotgun)
                ShotgunTimer++;
            base.UpdateWeaponIdle();
        }
        protected override void PostAttack()
        {
            Timer = 0;
            RecoilTimer = AttackSpeed;
            Projectile.ContinuouslyUpdateDamageStats = true;

            //base.PostAttack();
        }
        protected override void OnAttack()
        {
            if(Projectile.Lostbelt().ExecutionStrike)
            {
                int projDamage2 = (int)Owner.GetTotalDamage<ExecutorDamageClass>().ApplyTo(Owner.HeldItem.damage);
                Projectile proj3 = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ProjectileType<PrestigiousEnergyRifleBurstHeldProj>(), projDamage2, Projectile.knockBack, Projectile.owner);
                proj3.Lostbelt().ExecutionStrike = true;
                Projectile.Kill();
                return;

            }
            Vector2 dir = Projectile.SafeDirByRot();
            int type = ProjectileType<PrestigiousEnergyRifleBullet>();
            int projDamage = (int)Owner.GetTotalDamage<ExecutorDamageClass>().ApplyTo(Owner.HeldItem.damage);
            Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(),
                Projectile.Center, dir * 17f, type, projDamage, Projectile.knockBack, Projectile.owner);
            proj.Lostbelt().HasExecutionMechanic = true;
            ScarletSound(LostbeltSoundAssets.ASMD_IceBlockSplit, Projectile.Center, 0.20f, 0, .34f, 0.1f);
            Vector2 offset = new Vector2(45, -5 * Projectile.direction).RotatedBy(Projectile.rotation);
            Vector2 pos = Projectile.Center + offset;
            Vector2 particleOffset = new Vector2(10, 0 * Projectile.direction).RotatedBy(Projectile.rotation);
            for (int i = 0; i < 4; i++)
            {
                Vector2 vel = dir.ToRandVelocity(ToRadians(10f), 1).ToSafeNormalize();
                ECSParticle.HighResolutionThunder(pos, vel * .1f, RandLerpColor(Color.DodgerBlue, Color.RoyalBlue),
                    40, 1, vel.ToRotation() + PiOver2, Projectile.scale * .43f, 0);
            }
            for (int i = 0; i < 12; i++)
            {
                Vector2 pos2 = pos.ToRandCirclePos(8) - particleOffset;
                Vector2 vel = Projectile.SafeDirByRot().ToRandVelocity(ToRadians(15), .1f, 11.6f);
                float scale = Projectile.scale * Main.rand.NextFloat(.95f, 1.15f) * 0.38f;
                int timeLeft = Main.rand.Next(30, 45);
                ECSParticle.ShinyCrossStarECS(pos2, vel, RandLerpColor(Color.CornflowerBlue, Color.White), timeLeft, 1, scale);
            }
            for (int i = 0; i < 12; i++)
            {
                Vector2 pos2 = pos.ToRandCirclePos(8) - particleOffset;
                Vector2 vel = Projectile.SafeDirByRot().ToRandVelocity(ToRadians(0), .1f, 12.6f);
                float scale = Projectile.scale * Main.rand.NextFloat(.95f, 1.15f) * 0.38f;
                int timeLeft = Main.rand.Next(30, 45);
                ECSParticle.LightntingGlow(pos2, vel, RandLerpColor(Color.CornflowerBlue, Color.White), timeLeft, 1, scale);
            }
        }
    }
}
