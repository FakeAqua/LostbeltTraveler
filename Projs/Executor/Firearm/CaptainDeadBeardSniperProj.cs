using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.PixelatedRender;
using LostbeltTraveler.Core.ScreenEffect;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Items.Weapons.Executor.Firearm;
using System;
using Terraria;

namespace LostbeltTraveler.Projs.Executor.Firearm
{
    public class CaptainDeadBeardSniperProj : LostbeltRangedWeaponoutClass, IPixelatedRenderer
    {
        public override EnumDamageClass Category => EnumDamageClass.Executor;
        public override int OriginalItemID => ItemType<CaptainDeadBeard>();
        public override string Texture => GetInstance<CaptainDeadBeard>().Texture + "Sniper";
        public override float RecoilPower => HandleRecoilPower();
        public override int ProjExtraUpdates => 2;
        public override Vector2 HoldoutOffset => new(20, 0);
        public override Color HoldoutEdgeColor => Color.Goldenrod;
        public AnimationStruct Helper = new AnimationStruct(2);
        public bool IsTurningIntoSniperMode = false;
        public Vector2 TargetPos = Vector2.Zero;
        public float HoldoutTimer = 0;
        public float MaxHoldingTime = GetSeconds(3);
        public float HoldingProgress => Clamp(HoldoutTimer / MaxHoldingTime, 0f, 1f);
        public override bool IsUsing => base.IsUsing || Projectile.Lostbelt().ExecutionStrike;
        public override int AttackSpeed => base.AttackSpeed * 3 / 2;
        public float TargetRotation = 0;
        public float ArmRotation = 0;

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            //当然你可以做圆形的hitbox，但是既然方块已经够了为什么还要做圆形的？
            Rectangle rec = Utils.CenteredRectangle(Projectile.Center, new Vector2(300));
            if (rec.Intersects(targetHitbox))
                return true;
            return base.Colliding(projHitbox, targetHitbox);
        }

        public override bool? CanDamage()
        {
            return IsTurningIntoSniperMode;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            float rot = RandRotTwoPi;
            float scale = 1.2f;
            ScarletSound(LostbeltSoundAssets.TheSevenStar_Hit, Projectile.Center, 0.8f, 1, 0.3f, 0.1f);
            ECSParticle.Stain(target.Center, Vector2.Zero, Color.Goldenrod, 60, 1, rot, scale, new Vector2(0.35f, 1.2f), squashSpeedX: 1.15f, squashSpeedY: .90f);
            ECSParticle.Stain(target.Center, Vector2.Zero, Color.White, 60, 1, rot, scale * .75f, new Vector2(0.35f, 1.2f), squashSpeedX: 1.15f, squashSpeedY: .90f);
            for (int i = 0; i < 2; i++)
            {
                ECSParticle.ShinyCrossStarECS(target.Center + rot.ToRotationVector2() * Main.rand.NextFloat(-1f, 1.01f) * 0f, rot.ToRotationVector2().ToRandVelocity(0, -5.2f, 5.6f),
                    RandLerpColor(Color.Goldenrod, Color.LightGoldenrodYellow), 40, 1, Main.rand.NextFloat(.9f, 1.1f) * .8f, .2f);
            }

            base.OnHitNPC(target, hit, damageDone);
        }
        public override void ProjAI()
        {
            if (Owner.JustPressRightClick() && !IsTurningIntoSniperMode)
                InitSpining();

            if (!IsTurningIntoSniperMode)
            {
                base.ProjAI();
                return;
            }

            UpdatePlayerState();
            UpdateProjs();

            if (!Helper.IsDone[0])
                UpdateSpining();
            else
                DoneSpining();
        }

        private void InitSpining()
        {
            IsTurningIntoSniperMode = true;
            ScarletSound(LostbeltSoundAssets.Air_HeavyFlow, Projectile.Center, 1f, 0, .5f, .1f, 2);
            Helper.MaxProgress[0] = Projectile.MaxUpdates * 40;
            TargetRotation = Owner.ToMouseVector2().ToRotation();
            TargetPos = Projectile.Center + Owner.ToMouseVector2() * 350f;
            Projectile.penetrate = -1;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
            Projectile.SetupImmnuity(20);
        }

        private void DoneSpining()
        {
            Projectile.Kill();
            ScreenShakeSystem.AddScreenShakes(Projectile.Center, 20, 20, RandRotTwoPi);
            ScarletSound(LostbeltSoundAssets.Misc_SwordHit, Projectile.Center, pitch: .24f);
            Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity,
                ProjectileType<CaptainDeadBeardHeldProj>(), Projectile.originalDamage, Projectile.knockBack, Projectile.owner);
            ((CaptainDeadBeard)Owner.HeldItem.ModItem).SniperMode = false;
        }
        private void UpdateSpining()
        {
            float progress = Helper.GetAniProgress(0);
            const float threshold = .5f;

            if (progress > threshold)
            {
                float prevProgress = Utils.GetLerpValue(threshold, 1f, progress, true);
                Projectile.Center = Vector2.Lerp(TargetPos, Owner.MountedCenter, EaseInCubic(prevProgress));
                Projectile.rotation += Lerp(.3f, 0f, EaseInCubic(prevProgress)) * Projectile.spriteDirection;

                if (Main.rand.NextBool(3))
                {
                    ECSParticle.ShinyCrossStarSmall(Projectile.Center.ToRandCirclePos(130), -Owner.MountedCenter.GetNormalVector2(TargetPos) * 1.1f,
                        Color.Gold, 40, 1, 0.4f, Main.rand.NextFloat(-.1f, .1f));
                }

                float zoomPro = 1 - prevProgress;
                ScreenZoomSystem.MoveCameraTo(Projectile.Center, EaseOutBack(zoomPro * zoomPro) * .13f);

            }
            else
            {
                float prevProgress = Utils.GetLerpValue(0, threshold, progress, true);
                Projectile.Center = Vector2.Lerp(Projectile.Center, TargetPos, EaseOutCubic(prevProgress));
                Projectile.rotation += Lerp(.0f, .3f, EaseOutCubic(prevProgress)) * Projectile.spriteDirection;

                if (Main.rand.NextBool(3))
                {
                    ECSParticle.ShinyCrossStarSmall(Projectile.Center.ToRandCirclePos(130), Owner.MountedCenter.GetNormalVector2(TargetPos) * 1.11f,
                        Color.Gold, 40, 1, 0.4f, Main.rand.NextFloat(-.1f, .1f));
                }

                ScreenZoomSystem.MoveCameraTo(Projectile.Center, EaseOutBack(prevProgress * prevProgress) * .13f);

            }

            float easedProgress = EaseOutExpo(Helper.GetAniProgress(0));
            float rot = Helper.UpdateAngle(195f, -195f, Projectile.direction, easedProgress);
            Matrix tForm = Matrix.CreateRotationZ(rot) * Matrix.CreateScale(1, 1, 1);
            Vector2 tarPos = Vector2.Transform(Vector2.UnitX, tForm) * 1.1f;

            ArmRotation = tarPos.ToRotation() + TargetRotation;
            Owner.ControlPlayerArm(ArmRotation);
            Owner.itemAnimation = Owner.itemTime = 2;

            Helper.UpdateAniState(0);
        }
        public void UpdateProjs()
        {
            Projectile.spriteDirection = Projectile.direction = (Owner.LocalMouseWorld().X > Owner.Center.X).ToDirectionInt();
            Owner.ChangeDir(Projectile.direction);
            Owner.heldProj = Projectile.whoAmI;
            Projectile.position.Y += Owner.gfxOffY;
        }
        /// <summary>
        /// 复写这个是因为月火的特殊处决攻击，需要让月火在玩家按下处决键（互动键）时立刻发射一枚子弹
        /// <br>而这个钩子会在执行完毕后立刻执行攻击的判定，即上方的<see cref="IsUsing"/></br>
        /// </summary>
        protected override void UpdateRecoil()
        {
            //保留原本的自动管理方案
            base.UpdateRecoil();
            ExecutionCheck();
        }

        public void ExecutionCheck()
        {
            if (!Owner.IsHolding(OriginalItemID))
                return;
            if (Owner.GetExecuteProgress() == 0)
                return;
            if (Owner.Lostbelt().tacticalExecutionInputCache <= 0)
                return;

            Projectile.Lostbelt().ExecutionStrike = true;
            Owner.RemoveExecutionProgress();
            Owner.Lostbelt().tacticalExecutionInputCache = 0;
            Timer = AttackSpeed;
        }

        protected override void PreAttack()
        {
        }

        protected override void OnAttack()
        {
            if (Projectile.Lostbelt().ExecutionStrike)
                DoExecutionAttack();
            DoNormalAttack();
        }

        public void DoNormalAttack()
        {
            Vector2 offset = new Vector2(30, -5 * Projectile.direction).RotatedBy(Projectile.rotation);
            Vector2 pos = Projectile.Center + offset;
            Vector2 dir = Projectile.SafeDirByRot();
            int type = ProjectileType<CaptainDeadBeardSpzglassshot>();
            pos -= new Vector2(80, 0).RotatedBy(Projectile.rotation);

            int damage = (int)(Projectile.originalDamage * Lerp(1f, 3f, HoldingProgress));
            Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), pos, dir * 10f, type, damage, Projectile.knockBack, Projectile.owner);
            proj.Lostbelt().HasExecutionMechanic = true;
            proj.scale = Lerp(1f, 1.13f, HoldingProgress);
            pos = Projectile.Center + offset;

            if (HoldingProgress > .5f)
            {
                proj.extraUpdates += 2;
                proj.penetrate = 3;
            }

            ScarletSound(LostbeltSoundAssets.Misc_Boom, Projectile.Center, .6f, 0, .4f, .1f, 1);
            Vector2 particleOffset = new Vector2(10, 0 * Projectile.direction).RotatedBy(Projectile.rotation);

            for (int i = 0; i < 20; i++)
            {
                Vector2 pos2 = pos.ToRandCirclePos(8) - particleOffset;
                Vector2 vel = dir.ToRandVelocity(ToRadians(15), .1f, 11.6f);
                float scale = Projectile.scale * Main.rand.NextFloat(.95f, 1.15f) * 0.38f;
                int timeLeft = Main.rand.Next(30, 45);
                ECSParticle.ShinyCrossStarECS(pos2, vel, RandLerpColor(Color.OrangeRed, Color.Red), timeLeft, 1, scale);
            }

            for (int i = 0; i < 14; i++)
            {
                bool alt = Main.rand.NextBool();
                BlendState bs = alt ? BlendState.Additive : BlendState.AlphaBlend;
                ECSParticle.SmokeParticle(pos, dir.ToRandVelocity(ToRadians(15), 0.4f, 21.4f),
                    RandLerpColor(Color.OrangeRed, Color.White), Main.rand.Next(45, 65), RandRotTwoPi, 1, 0.23f * Main.rand.NextFloat(.95f, 1.25f), alt, bs);
            }

            if (HoldingProgress > .8f)
            {

                for (int i = 0; i < 10; i++)
                {
                    bool alt = Main.rand.NextBool();
                    BlendState bs = alt ? BlendState.Additive : BlendState.AlphaBlend;
                    ECSParticle.SmokeParticle(pos, dir.ToRandVelocity(ToRadians(15), 0.4f, 21.4f),
                        RandLerpColor(Color.OrangeRed, Color.White), Main.rand.Next(45, 65), RandRotTwoPi, 1, 0.23f * Main.rand.NextFloat(.95f, 1.25f), alt, bs);
                }

                for (int i = 0; i < 18; i++)
                {
                    Vector2 pos2 = pos.ToRandCirclePos(8) - particleOffset;
                    Vector2 vel = dir.ToRandVelocity(ToRadians(15), .1f, 14.6f);
                    float scale = Projectile.scale * Main.rand.NextFloat(.95f, 1.15f) * 0.48f;
                    int timeLeft = Main.rand.Next(30, 45);
                    ECSParticle.LightntingGlow(pos2, vel, RandLerpColor(Color.OrangeRed, Color.Goldenrod), 60, 1, scale);
                }

                ScarletSound(LostbeltSoundAssets.DeathsToll_Toss, Projectile.Center, .3f, 1, .1f, .1f);
                ScreenShakeSystem.AddScreenShakes(pos, 25, 50, -Projectile.SafeDirByRot().ToRotation(), RandRotTwoPi, true, easingFunc: EaseOutExpo);
            }
            else
            {
                ScreenShakeSystem.AddScreenShakes(pos, 12 + Projectile.Lostbelt().ExecutionStrike.ToInt() * 12, 60,
                -Projectile.SafeDirByRot().ToRotation(), 0, true, easingFunc: EaseOutExpo);
            }
            Projectile.Lostbelt().ExecutionStrike = false;
            HoldoutTimer = 0;
            Projectile.frameCounter = 0;

        }

        public void DoExecutionAttack()
        {
            Vector2 dir = Projectile.SafeDirByRot();
            int type = ProjectileType<CaptainDeadBeardCannonRodeo>();
            int damage = (int)(Projectile.originalDamage * Lerp(1f, 3f, HoldingProgress));
            Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, dir * 15f, type, damage, Projectile.knockBack, Projectile.owner);

            if (HoldingProgress > .5f)
            {
                proj.extraUpdates += 1;
            }

            ScarletSound(LostbeltSoundAssets.SpearofEscape_Boom, Projectile.Center, 0.350f, 0, .34f, 0.1f);
            ScarletSound(LostbeltSoundAssets.SpearofEscape_Toss, Projectile.Center, 0.350f, 0, .34f, 0.1f);
            ScreenShakeSystem.AddScreenShakes(Projectile.Center, 25, 50, RandRotTwoPi, RandRotTwoPi, true, easingFunc: EaseOutExpo);
            Vector2 offset = new Vector2(45, -5 * Projectile.direction).RotatedBy(Projectile.rotation);
            Vector2 pos = Projectile.Center + offset;
            Vector2 particleOffset = new Vector2(10, 0 * Projectile.direction).RotatedBy(Projectile.rotation);

            for (int i = 0; i < 7; i++)
            {
                Vector2 skyPos = new Vector2(Owner.LocalMouseWorld().X + Main.rand.NextFloat(-250f, 251f), Owner.Center.Y - Main.rand.NextFloat(750f, 950f));
                Vector2 skyVel = Vector2.UnitY.ToRandVelocity(ToRadians(5), 12f, 17f);
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), skyPos, skyVel, ProjectileType<CaptainDeadBeardCannonRodeoSmall>(), Projectile.originalDamage, Projectile.knockBack, Projectile.owner);
            }

            for (int i = 0; i < 16; i++)
            {
                Vector2 pos2 = pos.ToRandCirclePos(8) - particleOffset;
                Vector2 vel = dir.ToRandVelocity(ToRadians(25), .1f, 11.6f);
                float scale = Projectile.scale * Main.rand.NextFloat(.95f, 1.15f) * 0.48f;
                ECSParticle.ShinyCrossStarECS(pos2, vel, RandLerpColor(Color.OrangeRed, Color.Orange), Main.rand.Next(30, 45), 1, scale);
            }

            for (int i = 0; i < 16; i++)
            {
                bool alt = Main.rand.NextBool();
                BlendState bs = alt ? BlendState.Additive : BlendState.AlphaBlend;
                ECSParticle.SmokeParticle(pos, dir.ToRandVelocity(ToRadians(10), 0.4f, 21.4f), RandLerpColor(Color.Orange, Color.Orange), Main.rand.Next(45, 65), RandRotTwoPi, 1, 0.33f * Main.rand.NextFloat(.95f, 1.25f), alt, bs);
            }

            for (int i = 0; i < 10; i++)
            {
                bool alt = Main.rand.NextBool();
                BlendState bs = alt ? BlendState.Additive : BlendState.AlphaBlend;
                ECSParticle.SmokeParticle(pos, dir.ToRandVelocity(ToRadians(15), 0.4f, 21.4f),
                    RandLerpColor(Color.OrangeRed, Color.White), Main.rand.Next(45, 65), RandRotTwoPi, 1, 0.23f * Main.rand.NextFloat(.95f, 1.25f), alt, bs);
            }

            for (int i = 0; i < 18; i++)
            {
                Vector2 pos2 = pos.ToRandCirclePos(8) - particleOffset;
                Vector2 vel = dir.ToRandVelocity(ToRadians(15), .1f, 14.6f);
                float scale = Projectile.scale * Main.rand.NextFloat(.95f, 1.15f) * 0.48f;
                ECSParticle.LightntingGlow(pos2, vel, RandLerpColor(Color.OrangeRed, Color.Goldenrod), 60, 1, scale);
            }

        }

        protected override void PostAttack()
        {
            RecoilTimer = AttackSpeed;
            Timer = 0;
        }
        public float HandleRecoilPower()
        {
            if (Projectile.Lostbelt().ExecutionStrike)
                return 21f;
            else
                return 10f;

        }
        protected override void UpdateGlobalReset()
        {
            //计时器的重置
            if (RecoilTimer > 0)
            {
                RecoilTimer--;
                if (RecoilTimer == 0)
                    Projectile.Lostbelt().ExecutionStrike = false;
            }
        }
        protected override void UpdateWeaponIdle()
        {
            HoldoutTimer += 1f;

            if (Projectile.frameCounter < 2 && HoldingProgress > .5f)
                HandleChargeThresholdReached();

            if (Projectile.frameCounter < 5 * Projectile.MaxUpdates && Projectile.frameCounter >= 2 && HoldingProgress == 1)
                HandleChargeFull();

            base.UpdateWeaponIdle();
        }

        private void HandleChargeThresholdReached()
        {
            Projectile.frameCounter = 2;
            Vector2 flipVec = new Vector2(Owner.direction, 1);
            float drawRot = Projectile.rotation + (Projectile.spriteDirection == -1 ? Pi : 0);
            Vector2 drawPos = Projectile.Center + (HoldoutOffset * flipVec).RotatedBy(drawRot);
            Vector2 pos2 = drawPos + ((HoldoutOffset + new Vector2(25, -3)) * flipVec).RotatedBy(drawRot);

            ScarletSound(LostbeltSoundAssets.Misc_ManaTransform, Projectile.Center, .3f, pitch: .1f);
            for (int i = 0; i < 16; i++)
            {
                float vec = TwoPi / 16f * i;
                ECSParticle.ShinyCrossStarSmall(pos2, vec.ToRotationVector2() * 1f, Color.Goldenrod, 40, 1, 0.4f, 0);
            }
        }

        private void HandleChargeFull()
        {
            if (Projectile.frameCounter == 2)
                SpawnFullChargeBurst();

            Projectile.frameCounter += 1;

            float progress = Utils.GetLerpValue(2, Projectile.MaxUpdates * 5, Projectile.frameCounter, true);
            ScreenShakeSystem.AddScreenShakes(Projectile.Center, 8, 6, RandRotTwoPi);
            ScreenZoomSystem.ZoomIn(SmoothStep(0, .011f, EaseOutCubic(progress)));
        }

        private void SpawnFullChargeBurst()
        {
            Vector2 flipVec = new Vector2(Owner.direction, 1);
            float drawRot = Projectile.rotation + (Projectile.spriteDirection == -1 ? Pi : 0);
            Vector2 drawPos = Projectile.Center + (HoldoutOffset * flipVec).RotatedBy(drawRot);
            Vector2 pos2 = drawPos + ((HoldoutOffset + new Vector2(25, -3)) * flipVec).RotatedBy(drawRot);

            //ScreenShakeSystem.AddScreenShakes(Projectile.Center, 12, 8, RandRotTwoPi);
            ScarletSound(LostbeltSoundAssets.Misc_ManaStrike, Projectile.Center, .83f, pitch: .2f);
            for (int i = 0; i < 16; i++)
                ECSParticle.TurbulenceShinyOrb(pos2, 2.4f, Color.Goldenrod, 50, 1, .14f, glowMult: .4f);
        }
        public ScarletDrawLayer LayerToRenderTo => ScarletDrawLayer.BeforePlayer;
        public BlendState BlendState => BlendState.Additive;

        public void RenderPixelated(SpriteBatch sb)
        {
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 offset = HoldoutOffset * new Vector2(Owner.direction, 1);
            float drawRot = Projectile.rotation + (Projectile.spriteDirection == -1 ? Pi : 0);
            drawPos += offset.BetterRotatedBy(drawRot);
            Texture2D star = LostbeltModAssets.Particle_CrossGlow.Value;

            offset = (HoldoutOffset + new Vector2(25, -3)) * new Vector2(Owner.direction, 1);
            Vector2 starScale = new Vector2(.6f, 1.2f).SwapVector2Data();
            Color mainColor = Color.OrangeRed;
            float rot = Projectile.rotation + (Projectile.spriteDirection > 0 ? 0 : Pi);
            Vector2 pos2 = drawPos + offset.BetterRotatedBy(rot);

            for (int i = 0; i < 8f; i++)
            {
                float rotAdd = (TwoPi / 8f * i);
                SB.FastDraw(star, pos2.ToRandCirclePos(3), mainColor * .74f, rot + rotAdd, star.Size() / 2f, .18f * Projectile.scale * HoldingProgress);
                SB.FastDraw(star, pos2.ToRandCirclePos(3), Color.LightGoldenrodYellow * .34f, rot + rotAdd, star.Size() / 2f, .15f * Projectile.scale * HoldingProgress);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (!IsTurningIntoSniperMode)
                DrawNormalMode();
            else
                DrawSniperMode();

            return false;
        }

        private void DrawNormalMode()
        {
            Projectile.GetRangedWeaponHeldProjData(out Texture2D tex, out Vector2 drawPos, out Vector2 rotPoint, out float _, out SpriteEffects se);
            Vector2 offset = HoldoutOffset * new Vector2(Owner.direction, 1);
            float drawRot = Projectile.rotation + (Projectile.spriteDirection == -1 ? Pi : 0);
            drawPos += offset.BetterRotatedBy(drawRot);

            float scale = Projectile.scale * HoldoutDrawScale;
            if (HoldoutEdgeEnable)
                DrawHeldEdge(tex, drawPos, drawRot, rotPoint, scale, se);

            SB.FastDraw(tex, drawPos, Color.White, drawRot, rotPoint, scale, se);
            PixelatedRenderManager.BeginDrawProj = true;
            DrawCharging();
        }

        private void DrawHeldEdge(Texture2D tex, Vector2 drawPos, float drawRot, Vector2 rotPoint, float scale, SpriteEffects se)
        {
            float progress = Utils.GetLerpValue(0, AttackSpeed, RecoilTimer, true);
            float edgeProgress = Math.Max(EaseInCubic(progress), HoldingProgress);

            for (int i = 0; i < 8; i++)
            {
                if (edgeProgress <= .02f)
                    break;

                Vector2 shaking = Main.rand.NextVector2Circular(3f, 3f) * HoldingProgress;
                DrawWeaponEdge(tex, drawPos + (TwoPi / 8f * i).ToRotationVector2() * 2.5f * edgeProgress + shaking, drawRot, rotPoint, scale, se);
            }
        }

        private void DrawSniperMode()
        {
            Projectile.GetRangedWeaponHeldProjData(out Texture2D tex, out Vector2 drawPos, out Vector2 rotPoint, out float _, out SpriteEffects se);

            SB.EnterShaderArea();
            Texture2D shiny = LostbeltModAssets.Particle_HRShinyOrb.Value;
            SB.FastDraw(shiny, drawPos, Color.DarkGoldenrod, 0, shiny.Size() / 2f, Projectile.scale * 1.15f * .40f, se);
            SB.FastDraw(shiny, drawPos, Color.LightGoldenrodYellow, 0, shiny.Size() / 2f, Projectile.scale * 1.15f * .85f * .4f, se);
            SB.EndShaderArea();
            SB.EndShaderArea();

            drawSlash();

            Vector2 offset = HoldoutOffset * new Vector2(Owner.direction, 1);
            float drawRot = Projectile.rotation + (Projectile.spriteDirection == -1 ? Pi : 0);
            drawPos += offset.BetterRotatedBy(drawRot);
            float scale = Projectile.scale * HoldoutDrawScale;

            for (int i = 0; i < 8; i++)
            {
                SB.FastDraw(tex, drawPos.ToRandCirclePos(3) + (TwoPi / 8f * i).ToRotationVector2() * 1.5f, Color.White.ToAddColor(), drawRot, rotPoint, scale * 1.051f, se);
            }
            SB.FastDraw(tex, drawPos.ToRandCirclePos(3), Color.White, drawRot, rotPoint, scale, se);
        }

        public void DrawCharging()
        {
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 offset = HoldoutOffset * new Vector2(Owner.direction, 1);
            float drawRot = Projectile.rotation + (Projectile.spriteDirection == -1 ? Pi : 0);
            drawPos += offset.BetterRotatedBy(drawRot);
            float scale = Projectile.scale * HoldoutDrawScale;

            Texture2D tex2 = LostbeltModAssets.Particle_SharpTear;
            Vector2 rotPoint2 = new Vector2(tex2.Width / 2f, tex2.Height);
            float overallAlpha = HoldingProgress * .9f;

            for (int i = 0; i < 4; i++)
                DrawChargingSharp(drawPos, drawRot, scale, tex2, rotPoint2, overallAlpha);
        }

        private void DrawChargingSharp(Vector2 drawPos, float drawRot, float scale, Texture2D tex2, Vector2 rotPoint2, float overallAlpha)
        {
            Vector2 sharpPosOffset = new Vector2(65, -3) * new Vector2(Owner.direction, 1) + Main.rand.NextVector2Circular(3.5f, 1.5f);
            float rotFixer = -PiOver2 - Main.rand.NextFloat(-0.3f, 0.3f) - (Projectile.spriteDirection > 0 ? 0 : Pi);
            Vector2 sharpScale = new Vector2(0.17f * HoldingProgress, 2.19f);
            Color c = RandLerpColor(Color.DarkGoldenrod, Color.OrangeRed).ToAddColor();

            Vector2 pos = drawPos + sharpPosOffset.RotatedBy(drawRot);
            SB.FastDraw(tex2, pos, c * .75f * overallAlpha, drawRot + rotFixer, rotPoint2, scale * 1.10f * sharpScale, 0);

            sharpPosOffset = new Vector2(70, -3) * new Vector2(Owner.direction, 1) + Main.rand.NextVector2Circular(3.5f, 1.5f);
            pos = drawPos + sharpPosOffset.RotatedBy(drawRot);
            SB.FastDraw(tex2, pos, Color.LightGoldenrodYellow.ToAddColor() * .75f * overallAlpha, drawRot + rotFixer, rotPoint2, scale * sharpScale, 0);
        }

        public void drawSlash()
        {
            Vector2 position = Projectile.Center - Main.screenPosition + Main.rand.NextVector2Circular(3, 3);
            Texture2D texture = LostbeltModAssets.Texture_EnergySword.Value;
            Rectangle sourceRectangle = texture.Frame(1, 4); // The sourceRectangle says which frame to use.
            Vector2 origin = sourceRectangle.Size() / 2f;
            float origScale = 1.4f;
            SpriteEffects spriteEffects = (Projectile.spriteDirection < 0) ? SpriteEffects.FlipVertically : SpriteEffects.None;
            float offDir = (Projectile.spriteDirection < 0) ? 1 : -1;
            float offRot = Pi * 0.8f / 4 * offDir;
            float lightingColor = 1;
            float lifeSpan = lightingColor * 0.5f;
            float trailL = 1;
            lightingColor *= 0.8f;

            Color backDarkColor = Color.LightGoldenrodYellow;
            Color middleMediumColor = Color.Orange;
            Color frontLightColor = Color.Goldenrod;
            Color whiteTimesLerpTime = Color.White * 0.5f;
            whiteTimesLerpTime.A = (byte)(whiteTimesLerpTime.A * (1f - lightingColor));
            Color faintLightingColor = whiteTimesLerpTime * lightingColor * 0.5f;
            faintLightingColor.G = (byte)(faintLightingColor.G * lightingColor);
            faintLightingColor.B = (byte)(faintLightingColor.R * (0.25f + lightingColor * 0.75f));
            float scale = 1f;
            for (int i = 0; i < 2; i++)
            {
                scale = origScale * (i == 0 ? 0.4f : 1f);
                Main.EntitySpriteDraw(texture, position, sourceRectangle, backDarkColor * lightingColor,
                    Projectile.rotation + offRot - offDir * 0.2f, origin, scale, spriteEffects, 0f);
                Main.EntitySpriteDraw(texture, position, sourceRectangle, faintLightingColor * 0.15f,
                    Projectile.rotation + offRot + offDir * 0.1f, origin, scale, spriteEffects, 0f);
                Main.EntitySpriteDraw(texture, position, sourceRectangle, middleMediumColor * 0.3f,
                    Projectile.rotation + offRot - offDir * 0.1f, origin, scale, spriteEffects, 0f);
                Main.EntitySpriteDraw(texture, position, sourceRectangle, frontLightColor * lightingColor * 0.5f,
                    Projectile.rotation + offRot + offDir * 0.2f, origin, scale * 0.975f, spriteEffects, 0f);
                Main.EntitySpriteDraw(texture, position, sourceRectangle, frontLightColor * lightingColor * 0.35f,
                    Projectile.rotation + offRot + offDir * trailL * 0.9f, origin, scale * 0.975f, spriteEffects, 0f);
                Main.EntitySpriteDraw(texture, position, sourceRectangle, frontLightColor * lightingColor * 0.1f,
                    Projectile.rotation + offRot + offDir * trailL * 1.8f, origin, scale * 0.975f, spriteEffects, 0f);
            }
            scale = origScale;
            Main.EntitySpriteDraw(texture, position, texture.Frame(1, 4, 0, 3), Color.White * 0.6f * lightingColor, Projectile.rotation + offRot + offDir * 0.01f, origin, scale, spriteEffects, 0f);
            Main.EntitySpriteDraw(texture, position, texture.Frame(1, 4, 0, 3), Color.White * 0.5f * lightingColor, Projectile.rotation + offRot - offDir * 0.05f, origin, scale * 0.8f, spriteEffects, 0f);
            Main.EntitySpriteDraw(texture, position, texture.Frame(1, 4, 0, 3), Color.White * 0.4f * lightingColor, Projectile.rotation + offRot - offDir * 0.1f, origin, scale * 0.6f, spriteEffects, 0f);
            for (float i = 0f; i < 10f; i += 1f)
            {
                float edgeRotation = Projectile.rotation + i * (Pi * 2f) * 0.035f * offDir;
                Vector2 drawPos = position + edgeRotation.ToRotationVector2() * ((float)texture.Width * 0.5f - 6f) * scale;
                DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, drawPos,
                    new Color(255, 255, 255, 0) * (i / 9f) * lightingColor * 0.5f, middleMediumColor, lifeSpan, 0f, 0.5f, 0.5f, 1f, edgeRotation, new Vector2(.41f, 1.5f) * origScale, Vector2.One * scale);
            }
        }
        private void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawPos, Color drawColor,
            Color shineColor, float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, Vector2 scale, Vector2 fatness)
        {
            Texture2D sparkleTexture = LostbeltModAssets.Particle_SharpTear;
            Color bigColor = shineColor * opacity * 0.5f;
            bigColor.A = 0;
            Vector2 origin = sparkleTexture.Size() / 2f;
            Color smallColor = drawColor * 0.5f;
            float lerpValue = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, clamped: true) *
                Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, clamped: true);
            Vector2 scaleLeftRight = new Vector2(fatness.X * 0.25f, scale.X) * lerpValue;
            Vector2 scaleUpDown = new Vector2(fatness.Y * 0.25f, scale.Y) * lerpValue;
            bigColor *= lerpValue;
            smallColor *= lerpValue;
            // Bright, large part
            Main.EntitySpriteDraw(sparkleTexture, drawPos, null, bigColor, ToRadians(10) + rotation, origin, scaleLeftRight, dir);
            Main.EntitySpriteDraw(sparkleTexture, drawPos, null, bigColor, 0f + rotation, origin, scaleUpDown, dir);
            // Dim, small part
            Main.EntitySpriteDraw(sparkleTexture, drawPos, null, smallColor, ToRadians(10) + rotation, origin, scaleLeftRight * 0.86f, dir);
            Main.EntitySpriteDraw(sparkleTexture, drawPos, null, smallColor, 0f + rotation, origin, scaleUpDown * 0.56f, dir);
        }
    }
}
