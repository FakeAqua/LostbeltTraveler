using ContinentOfJourney.Items.ThrowerWeapons;
using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.ScreenEffect;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Items.Weapons.Executor.Firearm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace LostbeltTraveler.Projs.Executor.Firearm
{
    public class PrestigiousEnergyRifleBurstHeldProj : ExecutorHeldProj
    {
        public override EnumDamageClass Category => EnumDamageClass.Executor;
        public override string Texture => GetInstance<PrestigiousEnergyRifleHeldProj>().Texture;
        public AnimationStruct Helper = new AnimationStruct(2);
        public override int OriginalItemID => ItemType<PrestigiousEnergyRifle>();
        public float RecoilTimer = 0;
        public float Timer = 0;
        public float TargetRotation = 0;
        public Vector2 RotationRange = new Vector2(0, 360);
        public float RecoilThreshold = .3f;
        public float RecoilPower = 18;
        public float RecoilPowerLerp()
        {
            return 32f * Lerp(.5f, 1f, BurstLerping);
        }
        public int BurstTime
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        public int BurstAttackSpeedBase => Owner.ApplyWeaponAttackSpeed(Owner.HeldItem, Owner.HeldItem.useTime * Projectile.MaxUpdates, 10 * Projectile.MaxUpdates);
        public float BurstLerping => Utils.GetLerpValue(0, 7, BurstTime, true);
        public int BurstAttackSpeed()
        {
            int curAttackSpeed = (int)(BurstAttackSpeedBase * Lerp(2f, .82f, BurstLerping));
            return curAttackSpeed;
        }

        public override void SetStaticDefaults()
        {
            ScarletProjIDSets.IsHeldProj[Type] = true;
        }
        public override void ExSD()
        {
            Projectile.SetUpHeldProj(10);
        }
        public override void OnFirstFrame()
        {

            Helper.MaxProgress[0] = AttackSpeed;
            Helper.MaxProgress[1] = AttackSpeed / 2;
            TargetRotation = Owner.ToMouseVector2().ToRotation();
            ScarletSound(SoundID.Item149, Projectile.Center);
            base.OnFirstFrame();
        }
        public override void ProjAI()
        {
            UpdateHoldout();
            UpdateAnimation();
        }
        public void UpdateAnimation()
        {
            if (!Helper.IsDone[0])
                UpdateBeginAnimation();
            else if (!Helper.IsDone[1])
            {
                Timer = BurstAttackSpeed() / 9;
                UpdateEndAnimation();
            }
            else
            {
                if (BurstTime >= 8 && RecoilTimer == 0)
                {
                    Timer++;
                Projectile.rotation = Owner.ToMouseVector2().ToRotation();
                    if (Timer > AttackSpeed * 2)
                        Projectile.Kill();
                    return;
                }
                Projectile.rotation = Owner.ToMouseVector2().ToRotation();
                UpdateRecoil();
                UpdateAttack();
                if (RecoilTimer > 0)
                    RecoilTimer--;
            }
        }
        public void UpdateBeginAnimation()
        {
            float heldscale = Owner.HeldItem.scale;
            //这里挥砍动画一定程度上使用了矩阵变化。
            Helper.UpdateAniState(0);
            float easedProgress = EaseOutBack(Helper.GetAniProgress(0));
            //末尾角度，也是下一个动画进程的起始角度
            float beginAngle = 0;
            float endAngle = -20;
            //更新当前的转角
            float rot = Helper.UpdateAngle(beginAngle, endAngle, Owner.direction, easedProgress);
            //将其投影到矩阵上，并进行形变
            Matrix tForm = Matrix.CreateRotationZ(rot) * Matrix.CreateScale(1, 1, 1f);
            //而后再转化为射弹的目标指向，这个tarPos同时拥有指向和武器模长的信息。而不是一个单位向量
            Vector2 tarPos = Vector2.Transform(Vector2.UnitX, tForm) * 1f * heldscale;
            //这样，Scale就是tarPos的向量模长
            Projectile.scale = tarPos.Length();
            //武器的角度为（起始角度 + 目标角度）的值
            Projectile.rotation = tarPos.ToRotation() + TargetRotation;
                TargetRotation = TargetRotation.AngleTowards(Owner.GetToMouseVector2(Projectile.Center).ToRotation(), .15f);
        }
        public void UpdateEndAnimation()
        {
            float heldscale = Owner.HeldItem.scale;
            //这里挥砍动画一定程度上使用了矩阵变化。
            Helper.UpdateAniState(1);
            float easedProgress = EaseInCubic(Helper.GetAniProgress(1));
            //末尾角度，也是下一个动画进程的起始角度
            float beginAngle = -20;
            float endAngle = 0;
            //更新当前的转角
            float rot = Helper.UpdateAngle(beginAngle, endAngle, Owner.direction, easedProgress);
            //将其投影到矩阵上，并进行形变
            Matrix tForm = Matrix.CreateRotationZ(rot) * Matrix.CreateScale(1, 1, 1f);
            //而后再转化为射弹的目标指向，这个tarPos同时拥有指向和武器模长的信息。而不是一个单位向量
            Vector2 tarPos = Vector2.Transform(Vector2.UnitX, tForm) * 1f * heldscale;
            //这样，Scale就是tarPos的向量模长
            Projectile.scale = tarPos.Length();
            //武器的角度为（起始角度 + 目标角度）的值
            Projectile.rotation = tarPos.ToRotation() + TargetRotation;
                TargetRotation = TargetRotation.AngleTowards(Owner.GetToMouseVector2(Projectile.Center).ToRotation(), .15f);

        }
        public void UpdateAttack()
        {
            Timer++;
            int attackSpeed = BurstAttackSpeed();
            if (Timer < attackSpeed)
                return;
            //ScarletSound(LostbeltSoundAssets.ASMD_ExecutionFire, Projectile.Center, .3f);
            Vector2 dir = Projectile.SafeDirByRot();
            Vector2 offset = new Vector2(45, -5 * Projectile.direction).RotatedBy(Projectile.rotation);
            Vector2 pos = Projectile.Center + offset;
            Vector2 particleOffset = new Vector2(10, 0 * Projectile.direction).RotatedBy(Projectile.rotation);
            ScreenShakeSystem.AddScreenShakes(Projectile.Center, 20f, 30, RandRotTwoPi);
            for (int i = -2; i <= 2; i++)
            {
                Vector2 shotgunDirection = Projectile.SafeDirByRot().RotatedBy(ToRadians(10 * i));
                float shotgunSpeed = 8;
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, shotgunDirection * shotgunSpeed,
                    ProjectileType<PrestigiousEnergyRifleBurst>(), Projectile.originalDamage, Projectile.knockBack, Projectile.owner, shotgunSpeed, i);
                proj.Lostbelt().ExecutionStrike = true;
                int type = ProjectileType<PrestigiousEnergyRifleBullet>();
                Vector2 shotgunDirection2 = Projectile.SafeDirByRot().RotatedBy(ToRadians(10 * Lerp(1f, 0.25f, BurstLerping) * i));
                proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(),
                   Projectile.Center, shotgunDirection2 * 17f, type, Projectile.originalDamage, Projectile.knockBack, Projectile.owner);
                proj.Lostbelt().ExecutionStrike = true;
            }
            float globalLerping = Lerp(1f, .25f, BurstLerping);
            for (int i = 0; i < 24; i++)
            {
                Vector2 vel = dir.ToRandVelocity(ToRadians(45 * globalLerping), 5f, 20.6f);
                Vector2 pos2 = pos.ToRandCirclePos(8) - particleOffset + vel * 5;
                float scale = Projectile.scale * Main.rand.NextFloat(.95f, 1.15f) * 0.921f;
                ECSParticle.HighResolutionThunder(pos2, vel, RandLerpColor(Color.RoyalBlue, Color.DodgerBlue), 20, 1, vel.ToRotation() + PiOver2, scale, 3);

            }
            for (int i = 0; i < 24; i++)
            {
                Vector2 pos2 = pos.ToRandCirclePos(8) - particleOffset;
                Vector2 vel = dir.ToRandVelocity(ToRadians(45 * globalLerping), .1f, 21.6f);
                float scale = Projectile.scale * Main.rand.NextFloat(.95f, 1.15f) * 0.78f;
                ECSParticle.ShinyCrossStarECS(pos2, vel, RandLerpColor(Color.RoyalBlue, Color.LightSkyBlue), Main.rand.Next(30, 45), 1, scale);
            }

            for (int i = 0; i < 24; i++)
            {
                bool alt = Main.rand.NextBool();
                BlendState bs = alt ? BlendState.Additive : BlendState.AlphaBlend;
                ECSParticle.SmokeParticle(pos, dir.ToRandVelocity(ToRadians(30 * globalLerping), 0.4f, 30.4f),
                    RandLerpColor(Color.RoyalBlue, Color.LightSkyBlue), Main.rand.Next(45, 65), RandRotTwoPi, 1, 0.63f * Main.rand.NextFloat(.95f, 1.25f), alt, bs);
            }

            ScarletSound(LostbeltSoundAssets.ASMD_ExecutionFire, Projectile.Center, .25f, 1, Lerp(0f, 0.35f, BurstLerping));
            BurstTime++;
            Timer = 0;
            RecoilTimer = attackSpeed;
        }
        public void UpdateHoldout()
        {
            Projectile.spriteDirection = Projectile.direction = (Owner.LocalMouseWorld().X > Owner.Center.X).ToDirectionInt();
            Owner.ChangeDir(Projectile.direction);
            Owner.heldProj = Projectile.whoAmI;
            Owner.ControlPlayerArm(Projectile.rotation);
            Projectile.Center = Owner.MountedCenter;
            Projectile.position.Y += Owner.gfxOffY;
        }
        public void UpdateRecoil()
        {
            float progress = Utils.GetLerpValue(BurstAttackSpeed(), 0, RecoilTimer, true);
            float pullback;
            float rot = (Projectile.Center - Owner.LocalMouseWorld()).ToRotation() * Owner.gravDir;
            if (progress > RecoilThreshold)
            {
                float pro = (1 - progress) / (1 - RecoilThreshold);
                pullback = Lerp(0, RecoilPowerLerp(), EaseOutBack(pro));
            }
            else
            {
                float pro = progress / RecoilThreshold;
                pullback = Lerp(0, RecoilPowerLerp(), EaseOutCubic(pro));
            }
            Projectile.Center += rot.ToRotationVector2() * pullback;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public Vector2 HoldoutOffset = new Vector2(20, 0);
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.GetRangedWeaponHeldProjData(out Texture2D tex, out Vector2 drawPos, out Vector2 rotPoint, out float _, out SpriteEffects se);
            Vector2 offset = HoldoutOffset * new Vector2(Owner.direction, 1);
            float drawRot = Projectile.rotation + (Projectile.spriteDirection == -1 ? Pi : 0);
            drawPos += offset.BetterRotatedBy(drawRot);
            float scale = Projectile.scale * 1;
            float progress = Utils.GetLerpValue(0, AttackSpeed, RecoilTimer, true);
            if (!Helper.IsDone[0])
            {
                progress = EaseOutExpo( Helper.GetAniProgress(0));
            }
            else if (!Helper.IsDone[1])
            {
                progress =EaseOutCubic( 1- Helper.GetAniProgress(1));

            }
            else
            {
            progress = Utils.GetLerpValue(0, BurstAttackSpeed(), RecoilTimer, true);

            }
            float edgeProgress = EaseInCubic(progress);
            for (int i = 0; i < 8; i++)
            {
                if (edgeProgress <= .02f)
                    break;
                SB.FastDraw(tex, drawPos + (TwoPi / 8f * i).ToRotationVector2() * 2.5f * edgeProgress, Color.SkyBlue.ToAddColor(), drawRot, rotPoint, scale, se);
            }
            SB.FastDraw(tex, drawPos, Color.White, drawRot, rotPoint, scale, se);

            return false;
        }
    }
}
