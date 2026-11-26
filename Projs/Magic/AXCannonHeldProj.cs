using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.PixelatedRender;
using LostbeltTraveler.Core.ScreenEffect;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Items.Weapons.Magic;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Projs.Magic
{
    public class AXCannonHeldProj : LostbeltHeldProj, IPixelatedRenderer
    {
        public int ShootCounter = 0;
        public override int OriginalItemID => ItemType<AXCannon>();
        public override string Texture => GetInstance<AXCannon>().Texture;
        public Vector2 HoldoutOffset => new Vector2(17, -3);
        public bool ChargingReady
        {
            get => Projectile.ai[2] == 1f;
            set => Projectile.ai[2] = value ? 1 : 0;
        }
        public int ChargingMax => AttackSpeed * 9;
        public int ChargingHalf => AttackSpeed * 4;
        public ChargingEnum ChargingLevel
        {
            get => (ChargingEnum)Projectile.ai[1];
            set => Projectile.ai[1] = (float)value;
        }
        #region 大量的传参
        private float FullChargeOrbPosOffset = 35;
        private float HalfChargeOrbPosOffset = 25;
        private float NoneChargeOrbPosOffset = 15;
        private int FullChargeAttackTime => Projectile.MaxUpdates * 60;
        private int HalfChargeAttackTime => Projectile.MaxUpdates * 30;
        private int NoneChargeAttackTime => Projectile.MaxUpdates * 10;
        private int HalfChargeShootCounts = 3;
        private int NoneChargeShootCounts = 8;
        #endregion
        public float RecoilTimer = 0;
        public bool IsChargingAttacking = false;
        public enum ChargingEnum
        {
            Regular,
            None,
            Half,
            Full,
            Break
        }

        public ref float Timer => ref Projectile.ai[0];
        public override void ProjAI()
        {
            UpdatePlayerState();
            if (Owner.controlUseTile)
            {
                RightAttack();
                return;
            }
            if (Owner.channel && !IsChargingAttacking)
                LeftAttack();
            else
                NoAttack();
        }
        public void NoAttack()
        {
            if (ChargingReady)
            {

                switch (ChargingLevel)
                {
                    case ChargingEnum.None:
                        BlastAttackNone();
                        break;
                    case ChargingEnum.Half:
                        BlastAttackHalf();
                        break;
                    case ChargingEnum.Full:
                        BlastAttackFull();
                        break;
                    default:
                        if (!IsChargingAttacking)
                            IsChargingAttacking = true;
                        break;
                }
                //Timer = 0;
                if (RecoilTimer > 0)
                    RecoilTimer--;
                if (RecoilTimer == 0)
                {
                    IsChargingAttacking = false;
                    ChargingReady = false;
                    ChargingLevel = ChargingEnum.Regular;
                    OrbColor = Color.Orange;
                    OrbPosOffsetX = 1;
                    OrbScale = 1f;
                    ShootCounter = 0;
                    Timer = 0;
                }
            }
            else
            {
                if (Timer > 0)
                    Timer--;
            }
            OrbColor = Color.Transparent;
            OrbPosOffsetX = 0;
        }
        #region 发射左键的弹幕
        public void BlastAttackFull()
        {
            float rot = Projectile.rotation + (Projectile.spriteDirection > 0 ? 0 : Pi);
            Vector2 offset = (HoldoutOffset + new Vector2(FullChargeOrbPosOffset, 0)) * new Vector2(Owner.direction, 1);
            Vector2 orbPosition = Projectile.Center + offset.RotatedBy(rot);

            if (!IsChargingAttacking)
            {
                IsChargingAttacking = true;
                ScarletSound(LostbeltSoundAssets.AX_FullChargeAttack, Projectile.Center, .5f);
                ScreenShakeSystem.AddScreenShakes(Projectile.Center, 18, 18, RandRotTwoPi);
                for (int i = 0; i < 16; i++)
                {
                    ECSParticle.ShinyCrossStarSmall(orbPosition, (TwoPi / 16f * i).ToRotationVector2() * 3.6f, Color.DodgerBlue, 30, 1, 0.63f, 0);
                }
                for (int i = 0; i < 32; i++)
                {
                    ECSParticle.TurbulenceShinyOrb(orbPosition.ToRandCirclePosEdge(50), 4.6f, Color.RoyalBlue, 50, 1, 0.12f, glowMult: .5f);
                }
                for (int i = 0; i < 15; i++)
                {
                    Vector2 args = (TwoPi / 16f * i).ToRotationVector2();
                    Vector2 particlePos = orbPosition + args * 10f;
                    ECSParticle.ShinyCrossStarECS(particlePos, RandVelTwoPi(1.5f, 9.4f) * 1.1f, Color.SkyBlue, 45, 1, .95f, .2f);

                }
                for (int i = 0; i < 20; i++)
                {
                    ECSParticle.TurbulenceShinyOrb(orbPosition.ToRandCirclePos(60), 1.9f, Color.SkyBlue, 60, 1, .1f);
                }
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.rotation.ToRotationVector2() * 12f, ProjectileType<AXCannonFullCharge>(), Projectile.originalDamage, Projectile.knockBack, Projectile.owner);
                ScreenDarknessSystem.AddScreenDarkness(0.6f, 5, 10, 5, player: Owner);
            }
            UpdateRecoil(60);
        }
        public void BlastAttackHalf()
        {
            float rot = Projectile.rotation + (Projectile.spriteDirection > 0 ? 0 : Pi);
            Vector2 offset = (HoldoutOffset + new Vector2(HalfChargeOrbPosOffset, 0)) * new Vector2(Owner.direction, 1);
            Vector2 orbPosition = Projectile.Center + offset.RotatedBy(rot);

            if (!IsChargingAttacking)
            {
                IsChargingAttacking = true;
                shoootThis();
                ShootCounter++;
            }
            else if (ShootCounter <= HalfChargeShootCounts && IsChargingAttacking)
            {
                if (RecoilTimer > (HalfChargeAttackTime - NoneChargeAttackTime) && ShootCounter > 0)
                    return;
                RecoilTimer = HalfChargeAttackTime;
                ShootCounter++;
                shoootThis();

            }
            void shoootThis()
            {
                ScarletSound(LostbeltSoundAssets.AX_HalfChargeAttack, Projectile.Center, .5f);
                ScreenShakeSystem.AddScreenShakes(Projectile.Center, 6, 6, RandRotTwoPi);
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.rotation.ToRotationVector2() * 12f, ProjectileType<AXCannonHalfCharge>(), Projectile.originalDamage, Projectile.knockBack, Projectile.owner);
                for (int i = 0; i < 6; i++)
                {
                    ECSParticle.TurbulenceShinyOrb(orbPosition.ToRandCirclePosEdge(20), 4.6f, Color.LightGreen, 50, 1, 0.1f, glowMult: .5f);
                }
                for (int i = 0; i < 6; i++)
                {
                    Vector2 args = (TwoPi / 6f * i).ToRotationVector2();
                    Vector2 particlePos = orbPosition + args * 10f;
                    ECSParticle.ShinyCrossStarECS(particlePos, RandVelTwoPi(1.5f, 9.4f) * 1.1f, Color.LightGreen, 45, 1, .95f, .2f);
                }
                for (int i = 0; i < 6; i++)
                {
                    ECSParticle.TurbulenceShinyOrb(orbPosition.ToRandCirclePos(60), 1.9f, Color.LimeGreen, 60, 1, .1f);
                }
            }
        }
        public void BlastAttackNone()
        {
            float rot = Projectile.rotation + (Projectile.spriteDirection > 0 ? 0 : Pi);
            Vector2 offset = (HoldoutOffset + new Vector2(5 * 5, 0)) * new Vector2(Owner.direction, 1);
            Vector2 orbPosition = Projectile.Center + offset.RotatedBy(rot);

            if (!IsChargingAttacking)
            {
                IsChargingAttacking = true;
                shoootThis();
                ShootCounter++;
            }
            else if (ShootCounter <= NoneChargeShootCounts && IsChargingAttacking)
            {
                if (RecoilTimer > (NoneChargeAttackTime / 2) && ShootCounter > 0)
                    return;
                RecoilTimer = NoneChargeAttackTime;
                ShootCounter++;
                shoootThis();
            }
            void shoootThis()
            {
                ScarletSound(LostbeltSoundAssets.AX_Attack, Projectile.Center, .5f);
                ScreenShakeSystem.AddScreenShakes(Projectile.Center, 6, 6, RandRotTwoPi);
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.rotation.ToRotationVector2() * 12f, ProjectileType<AXCannonNoCharge>(), Projectile.originalDamage, Projectile.knockBack, Projectile.owner);
                for (int i = 0; i < 3; i++)
                {
                    ECSParticle.TurbulenceShinyOrb(orbPosition.ToRandCirclePosEdge(20), 4.6f, Color.OrangeRed, 50, 1, 0.1f, glowMult: .5f);
                }
                for (int i = 0; i < 2; i++)
                {
                    ECSParticle.TurbulenceShinyOrb(orbPosition.ToRandCirclePos(60), 1.9f, Color.Orange, 60, 1, .1f);
                }
            }
        }
        #endregion
        public void UpdateRecoil(float RecoilPower)
        {
        }
        /// <summary>
        /// 左键的蓄力攻击，这个是艾克斯炮的核心攻击方法
        /// <br>你可以看到一对特判啥的然后传值，这些传值主要是给绘制用的</br>
        /// </summary>
        public void LeftAttack()
        {
            Owner.itemTime = Owner.itemAnimation = 2;
            float noChargeScale = 1f;
            float halfChargeScale = 1.25f;
            float maxChargeScale = 1.75f;
            if (Timer < ChargingHalf)
            {
                float lerping = EaseInOutQuad(Utils.GetLerpValue(0, ChargingHalf, Timer, true));
                OrbScale = Lerp(0, noChargeScale, lerping);
                OrbColor = Color.Lerp(Color.Transparent, Color.OrangeRed, lerping);
                OrbPosOffsetX = Lerp(1, NoneChargeOrbPosOffset, lerping);
            }
            else if (Timer < ChargingMax)
            {
                float lerping = EaseInCubic(Utils.GetLerpValue(AttackSpeed, ChargingMax, Timer, true));
                OrbScale = Lerp(noChargeScale, halfChargeScale, lerping);
                OrbColor = Color.Lerp(Color.Orange, Color.LimeGreen, lerping);
                OrbPosOffsetX = Lerp(NoneChargeOrbPosOffset, HalfChargeOrbPosOffset, lerping);
            }
            else
            {
                float lerping = EaseInCubic(Utils.GetLerpValue(ChargingMax, ChargingMax + AttackSpeed / 2, Timer, true));
                OrbScale = Lerp(halfChargeScale, maxChargeScale, lerping);
                OrbColor = Color.Lerp(Color.LimeGreen, Color.RoyalBlue, (lerping));
                OrbPosOffsetX = maxChargeScale;
            }
            float rot = Projectile.rotation + (Projectile.spriteDirection > 0 ? 0 : Pi);
            Vector2 offset = (HoldoutOffset + new Vector2(5 * OrbPosOffsetX, 0)) * new Vector2(Owner.direction, 1);
            Vector2 orbPosition = Projectile.Center + offset.RotatedBy(rot);

            if (Timer > AttackSpeed)
                ChargingReady = true;
            if (Timer >= ChargingMax)
            {
                if (ChargingLevel == ChargingEnum.Half)
                {
                    RecoilTimer = FullChargeAttackTime;
                    ScarletSound(LostbeltSoundAssets.AX_FullCharge, Projectile.Center, .5f);
                    ScreenShakeSystem.AddScreenShakes(Projectile.Center, 18, 18, RandRotTwoPi);
                    for (int i = 0; i < 16; i++)
                    {
                        ECSParticle.ShinyCrossStarSmall(orbPosition, (TwoPi / 16f * i).ToRotationVector2() * 3.6f, Color.RoyalBlue, 30, 1, 0.63f, 0);
                    }
                    for (int i = 0; i < 32; i++)
                    {
                        ECSParticle.TurbulenceShinyOrb(orbPosition.ToRandCirclePosEdge(50), 4.6f, Color.RoyalBlue, 50, 1, 0.12f, glowMult: .5f);
                    }

                }
                float progress = Utils.GetLerpValue(ChargingMax, ChargingMax + AttackSpeed / 2, Timer, false);
                if (progress <= 1)
                    ScreenZoomSystem.ZoomIn(SmoothStep(0, .05f, EaseOutCubic(progress)));
                ChargingLevel = ChargingEnum.Full;
                Vector2 startedPos = orbPosition.ToRandCirclePos(30);
                if (Main.rand.NextBool(9))
                    ECSParticle.HighResolutionThunder(startedPos, Vector2.Zero, Color.LightSkyBlue, 40, 1, RandRotTwoPi, .3f, 2);
                if (Main.rand.NextBool(9))
                    ECSParticle.TurbulenceShinyOrb(startedPos, 4.6f, Color.RoyalBlue, 50, 1, 0.12f, glowMult: .5f);
            }
            else if (Timer > ChargingHalf)
            {
                if (ChargingLevel == ChargingEnum.None)
                {
                    RecoilTimer = HalfChargeAttackTime;
                    ScarletSound(LostbeltSoundAssets.AX_HalfCharge, Projectile.Center, .5f);
                    ScreenShakeSystem.AddScreenShakes(Projectile.Center, 12, 12, RandRotTwoPi);
                    for (int i = 0; i < 16; i++)
                    {
                        ECSParticle.ShinyCrossStarSmall(orbPosition, (TwoPi / 16f * i).ToRotationVector2() * 3.6f, Color.LimeGreen, 30, 1, 0.63f, 0);
                    }
                }
                ChargingLevel = ChargingEnum.Half;
                Vector2 startedPos = orbPosition.ToRandCirclePosEdge(70);
                Vector2 targetVel = startedPos.GetNormalVector2(orbPosition) * 4f;
                ECSParticle.StarShape(startedPos, targetVel, Color.LimeGreen, 30, 1, 0.4f, 0.7f, BlendState.Additive);

            }
            else
            {
                if (ChargingLevel == ChargingEnum.Regular)
                {
                    Timer = 0;
                    ChargingLevel = ChargingEnum.None;
                    RecoilTimer = NoneChargeAttackTime;
                    ScarletSound(SoundID.DD2_SkyDragonsFuryShot, Projectile.Center);
                }
                float progress = Utils.GetLerpValue(0, AttackSpeed, Timer, true);
                if (Main.rand.NextFloat() < progress)
                {
                    Vector2 startedPos = orbPosition.ToRandCirclePosEdge(70);
                    Vector2 targetVel = startedPos.GetNormalVector2(orbPosition) * 3f;
                    ECSParticle.StarShape(startedPos, targetVel, Color.Orange, 30, 1, 0.4f, 0.7f, BlendState.Additive);
                }
            }
            Timer++;
        }
        public void RightAttack()
        {
            Owner.itemTime = Owner.itemAnimation = 2;
            ChargingLevel = ChargingEnum.Regular;
            if (Timer < AttackSpeed / 2)
            {
                Timer++;
                return;
            }
            Timer = 0;
            float rot = Projectile.rotation + (Projectile.spriteDirection > 0 ? 0 : Pi);
            Vector2 offset = (HoldoutOffset + new Vector2(5 * 5, 0)) * new Vector2(Owner.direction, 1);
            Vector2 orbPosition = Projectile.Center + offset.RotatedBy(rot);
            shoootThis();
            void shoootThis()
            {
                ScarletSound(LostbeltSoundAssets.AX_Attack, Projectile.Center, .5f);
                ScreenShakeSystem.AddScreenShakes(Projectile.Center, 2, 2, RandRotTwoPi);
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.rotation.ToRotationVector2() * 12f, ProjectileType<AXCannonNoCharge>(), Projectile.originalDamage, Projectile.knockBack, Projectile.owner);
                for (int i = 0; i < 3; i++)
                {
                    ECSParticle.TurbulenceShinyOrb(orbPosition.ToRandCirclePosEdge(20), 4.6f, Color.OrangeRed, 50, 1, 0.1f, glowMult: .5f);
                }
                for (int i = 0; i < 2; i++)
                {
                    ECSParticle.TurbulenceShinyOrb(orbPosition.ToRandCirclePos(60), 1.9f, Color.Orange, 60, 1, .1f);
                }
            }
        }
        public void UpdatePlayerState()
        {
            if (Owner.CanUseHoldout(OriginalItemID))
                Projectile.timeLeft = 2;
            else
                Projectile.Kill();
            Projectile.Center = Owner.MountedCenter;
            Projectile.rotation = Owner.ToMouseVector2().ToRotation();
            Projectile.position.Y += Owner.gfxOffY;
            Projectile.spriteDirection = Projectile.direction = Projectile.XSignValue(Owner.LocalMouseWorld());
            Owner.heldProj = Projectile.whoAmI;
            Owner.ChangeDir(Projectile.direction);
            Owner.ControlPlayerArm(Projectile.rotation);
        }
        #region 绘制
        public BlendState BlendState => BlendState.Additive;
        public ScarletDrawLayer LayerToRenderTo => ScarletDrawLayer.BeforeDusts;
        public Color OrbColor = Color.Orange;
        public float OrbPosOffsetX = 1;
        public float OrbScale = 1f;
        public void RenderPixelated(SpriteBatch spriteBatch)
        {
            Texture2D star = LostbeltModAssets.Particle_HRShinyOrbSmall.Value;
            Color mainColor = OrbColor;
            float rot = Projectile.rotation + (Projectile.spriteDirection > 0 ? 0 : Pi);
            Vector2 offset = (HoldoutOffset + new Vector2(5 * OrbPosOffsetX, 0)) * new Vector2(Owner.direction, 1);
            Vector2 pos2 = Projectile.Center - Main.screenPosition + offset.BetterRotatedBy(rot);
            //是的，每个阶段的绘制都是不一样的
            //这里的爆发是走射弹，然后每个爆发都会有不一样的特效
            //这也是写了整整700行的码去实现这个艾克斯炮的原因
            if (IsChargingAttacking)
            {
                switch (ChargingLevel)
                {
                    case ChargingEnum.Full:
                        offset = (HoldoutOffset + new Vector2(FullChargeOrbPosOffset, 0)) * new Vector2(Owner.direction, 1);
                        pos2 = Projectile.Center - Main.screenPosition + offset.BetterRotatedBy(rot);
                        DrawReleaseingOrb(pos2, star, mainColor, rot);

                        break;
                    case ChargingEnum.Half:
                        offset = (HoldoutOffset + new Vector2(HalfChargeOrbPosOffset, 0)) * new Vector2(Owner.direction, 1);
                        pos2 = Projectile.Center - Main.screenPosition + offset.BetterRotatedBy(rot);
                        DrawReleaseingOrbGreen(pos2, star, mainColor, rot);
                        break;
                    case ChargingEnum.None:
                        offset = (HoldoutOffset + new Vector2(NoneChargeOrbPosOffset, 0)) * new Vector2(Owner.direction, 1);
                        pos2 = Projectile.Center - Main.screenPosition + offset.BetterRotatedBy(rot);
                        DrawReleaseingOrbYellow(pos2, star, mainColor, rot);
                        break;
                }
                return;
            }
            if (Owner.controlUseTile && ChargingLevel == ChargingEnum.Regular)
            {
                offset = (HoldoutOffset + new Vector2(NoneChargeOrbPosOffset, 0)) * new Vector2(Owner.direction, 1);
                pos2 = Projectile.Center - Main.screenPosition + offset.BetterRotatedBy(rot);
                DrawReleaseingOrbYellowRegular(pos2, star, mainColor, rot);
            }
            if (Owner.channel && !IsChargingAttacking)
                DrawChargingOrb(pos2, star, mainColor, rot);
        }
        public void DrawReleaseingOrbYellowRegular(Vector2 pos2, Texture2D star, Color mainColor, float rot)
        {
            float lerping = Utils.GetLerpValue(0, AttackSpeed / 2, Timer, true);
            float glow;
            float threshold = 0.5f;
            float opa;
            float scale;
            if (lerping < threshold)
            {
                glow = Utils.GetLerpValue(0, threshold, lerping, true);
                opa = Lerp(1f, 1f, glow);
                scale = Lerp(1f, 1f, glow);
            }
            else
            {
                glow = Utils.GetLerpValue(threshold, 1f, lerping, true);
                opa = Lerp(1, .17f, glow);
                scale = Lerp(1f, 1.6f, glow);
            }
            scale *= .75f;
            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            if (opa != 0)
            {
                Texture2D cGlow = LostbeltModAssets.Particle_CrossGlow.Value;
                SB.FastDraw(cGlow, pos2, Color.OrangeRed * opa, 0, cGlow.Size() / 2f, .6f * scale);
                SB.FastDraw(cGlow, pos2, Color.White * opa, PiOver2, cGlow.Size() / 2f, .6f * scale);

                Effect shader = LostbeltShaderAssets.UCAPolarDistortShaderColor;
                shader.Parameters["uWidthMult"].SetValue(1);
                shader.Parameters["uRingMult"].SetValue(4f);
                shader.Parameters["uYTime"].SetValue(-1.17f * opa);
                shader.CurrentTechnique.Passes[0].Apply();
                Main.instance.GraphicsDevice.Textures[1] = LostbeltModAssets.Particle_HRShinyOrb.Value;

                Texture2D noise1 = LostbeltModAssets.Noise_WaterFlow.Value;
                float noise1Scale = 1f * scale * .36f;
                for (float i = 1f; i > .9f; i -= .05f)
                {
                    SB.FastDraw(noise1, pos2, Color.Orange * opa, TwoPi / 1f, noise1.Size() / 2f, noise1Scale * i);
                    SB.FastDraw(noise1, pos2, Color.DarkGoldenrod * opa, TwoPi / 2f, noise1.Size() / 2f, noise1Scale * i);
                    SB.FastDraw(noise1, pos2, Color.LightGoldenrodYellow * opa, TwoPi / 3f, noise1.Size() / 2f, noise1Scale * i);
                }
            }
            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            float orbScale = OrbScale * .85f;
            SB.FastDraw(star, pos2.ToRandCirclePos(3), mainColor * opa, rot, star.Size() / 2f, .98f * Projectile.scale * orbScale);
            SB.FastDraw(star, pos2.ToRandCirclePos(3), Color.White * opa, rot, star.Size() / 2f, .485f * Projectile.scale * orbScale);
            LostbeltMethods.EndShaderAreaPixel();
        }

        public void DrawReleaseingOrbYellow(Vector2 pos2, Texture2D star, Color mainColor, float rot)
        {
            float lerping = Utils.GetLerpValue(10 * Projectile.MaxUpdates, 0, RecoilTimer, true);
            float glow;
            float threshold = 0.05f;
            float opa;
            float scale;
            if (lerping < threshold)
            {
                glow = Utils.GetLerpValue(0, threshold, lerping, true);
                opa = Lerp(1f, 1f, glow);
                scale = Lerp(1f, 1f, glow);
            }
            else
            {
                glow = Utils.GetLerpValue(threshold, 1f, lerping, true);
                opa = Lerp(1, 0, glow);
                scale = Lerp(1f, 1.6f, glow);
            }
            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            if (opa != 0)
            {
                Texture2D cGlow = LostbeltModAssets.Particle_CrossGlow.Value;
                SB.FastDraw(cGlow, pos2, Color.OrangeRed * opa, 0, cGlow.Size() / 2f, .6f * scale);
                SB.FastDraw(cGlow, pos2, Color.White * opa, PiOver2, cGlow.Size() / 2f, .6f * scale);

                Effect shader = LostbeltShaderAssets.UCAPolarDistortShaderColor;
                shader.Parameters["uWidthMult"].SetValue(1);
                shader.Parameters["uRingMult"].SetValue(4f);
                shader.Parameters["uYTime"].SetValue(-1.17f * opa);
                shader.CurrentTechnique.Passes[0].Apply();
                Main.instance.GraphicsDevice.Textures[1] = LostbeltModAssets.Particle_HRShinyOrb.Value;

                Texture2D noise1 = LostbeltModAssets.Noise_WaterFlow.Value;
                float noise1Scale = 1f * scale * .36f;
                for (float i = 1f; i > .9f; i -= .05f)
                {
                    SB.FastDraw(noise1, pos2, Color.Orange * opa, TwoPi / 1f, noise1.Size() / 2f, noise1Scale * i);
                    SB.FastDraw(noise1, pos2, Color.DarkGoldenrod * opa, TwoPi / 2f, noise1.Size() / 2f, noise1Scale * i);
                    SB.FastDraw(noise1, pos2, Color.LightGoldenrodYellow * opa, TwoPi / 3f, noise1.Size() / 2f, noise1Scale * i);
                }
            }
            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            float orbScale = OrbScale;
            SB.FastDraw(star, pos2.ToRandCirclePos(3), mainColor * opa, rot, star.Size() / 2f, .98f * Projectile.scale * orbScale);
            SB.FastDraw(star, pos2.ToRandCirclePos(3), Color.White * opa, rot, star.Size() / 2f, .485f * Projectile.scale * orbScale);
            LostbeltMethods.EndShaderAreaPixel();
        }

        public void DrawReleaseingOrbGreen(Vector2 pos2, Texture2D star, Color mainColor, float rot)
        {
            float lerping = Utils.GetLerpValue(30 * Projectile.MaxUpdates, 0, RecoilTimer, true);
            float glow;
            float threshold = 0.05f;
            float opa;
            float scale;
            if (lerping < threshold)
            {
                glow = Utils.GetLerpValue(0, threshold, lerping, true);
                opa = Lerp(1f, 1f, glow);
                scale = Lerp(1f, 1f, glow);
            }
            else
            {
                glow = Utils.GetLerpValue(threshold, 1f, lerping, true);
                opa = Lerp(1, 0, glow);
                scale = Lerp(1f, 1.6f, glow);
            }
            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            if (opa != 0)
            {
                Texture2D cGlow = LostbeltModAssets.Particle_CrossGlow.Value;
                SB.FastDraw(cGlow, pos2, Color.LimeGreen * opa, 0, cGlow.Size() / 2f, .6f * scale);
                SB.FastDraw(cGlow, pos2, Color.White * opa, PiOver2, cGlow.Size() / 2f, .6f * scale);

                Effect shader = LostbeltShaderAssets.UCAPolarDistortShaderColor;
                shader.Parameters["uWidthMult"].SetValue(1);
                shader.Parameters["uRingMult"].SetValue(4f);
                shader.Parameters["uYTime"].SetValue(-1.17f * opa);
                shader.CurrentTechnique.Passes[0].Apply();
                Main.instance.GraphicsDevice.Textures[1] = LostbeltModAssets.Particle_HRShinyOrb.Value;

                Texture2D noise1 = LostbeltModAssets.Noise_WaterFlow.Value;
                float noise1Scale = 1f * scale * .36f;
                for (float i = 1f; i > .9f; i -= .05f)
                {
                    SB.FastDraw(noise1, pos2, Color.LimeGreen * opa, TwoPi / 1f, noise1.Size() / 2f, noise1Scale * i);
                    SB.FastDraw(noise1, pos2, Color.LightGreen * opa, TwoPi / 2f, noise1.Size() / 2f, noise1Scale * i);
                    SB.FastDraw(noise1, pos2, Color.White * opa, TwoPi / 3f, noise1.Size() / 2f, noise1Scale * i);
                }
            }
            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            float orbScale = OrbScale;
            SB.FastDraw(star, pos2.ToRandCirclePos(3), mainColor * opa, rot, star.Size() / 2f, .98f * Projectile.scale * orbScale);
            SB.FastDraw(star, pos2.ToRandCirclePos(3), Color.White * opa, rot, star.Size() / 2f, .485f * Projectile.scale * orbScale);
            LostbeltMethods.EndShaderAreaPixel();

        }
        public void DrawReleaseingOrb(Vector2 pos2, Texture2D star, Color mainColor, float rot)
        {
            float lerping = Utils.GetLerpValue(60 * Projectile.MaxUpdates, 0, RecoilTimer, true);
            float glow;
            float threshold = 0.05f;
            float opa;
            float scale;
            if (lerping < threshold)
            {
                glow = Utils.GetLerpValue(0, threshold, lerping, true);
                opa = Lerp(0, 1f, glow);
                scale = Lerp(.2f, 1f, glow);
            }
            else
            {
                glow = Utils.GetLerpValue(threshold, 1f, lerping, true);
                opa = Lerp(1, 0, glow);
                scale = Lerp(1f, 1.6f, glow);
            }
            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            if (opa != 0)
            {
                Texture2D cGlow = LostbeltModAssets.Particle_OpticalLineGlow.Value;
                SB.FastDraw(cGlow, pos2, Color.LightSkyBlue * opa, 0, cGlow.Size() / 2f, .6f * scale);
                SB.FastDraw(cGlow, pos2, Color.LightSkyBlue * opa, PiOver2, cGlow.Size() / 2f, .6f * scale);

                Effect shader = LostbeltShaderAssets.UCAPolarDistortShaderColor;
                shader.Parameters["uWidthMult"].SetValue(1);
                shader.Parameters["uRingMult"].SetValue(4f);
                shader.Parameters["uYTime"].SetValue(-1.17f * opa);
                shader.CurrentTechnique.Passes[0].Apply();
                Main.instance.GraphicsDevice.Textures[1] = LostbeltModAssets.Particle_HRShinyOrbSmall.Value;

                Texture2D noise1 = LostbeltModAssets.Noise_Aura.Value;
                float noise1Scale = 1f * scale * .86f;
                for (float i = 1f; i > .85f; i -= .1f)
                {
                    float progress = Utils.GetLerpValue(1f, .85f, i, true);
                    SB.FastDraw(noise1, pos2, Color.LightSkyBlue * opa, TwoPi / 1f, noise1.Size() / 2f, noise1Scale * i);
                    SB.FastDraw(noise1, pos2, Color.LightSkyBlue * opa, TwoPi / 2f, noise1.Size() / 2f, noise1Scale * i);
                    SB.FastDraw(noise1, pos2, Color.LightSkyBlue * opa, TwoPi / 3f, noise1.Size() / 2f, noise1Scale * i);
                }
            }
            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            float orbScale = OrbScale;
            SB.FastDraw(star, pos2.ToRandCirclePos(3), mainColor * opa, rot, star.Size() / 2f, .98f * Projectile.scale * orbScale);
            SB.FastDraw(star, pos2.ToRandCirclePos(3), Color.White * opa, rot, star.Size() / 2f, .485f * Projectile.scale * orbScale);
            LostbeltMethods.EndShaderAreaPixel();

        }
        public void DrawChargingOrb(Vector2 pos2, Texture2D star, Color mainColor, float rot)
        {
            float lerping = (Utils.GetLerpValue(ChargingMax, ChargingMax + 40 * Projectile.MaxUpdates, Timer, true));
            float glow;
            float threshold = 0.15f;
            float opa;
            float scale;
            if (lerping < threshold)
            {
                glow = Utils.GetLerpValue(0, threshold, lerping, true);
                opa = Lerp(0, 1f, glow);
                scale = Lerp(.2f, 1f, glow);

            }
            else
            {
                glow = Utils.GetLerpValue(threshold, 1f, lerping, true);
                opa = Lerp(1, 0, glow);
                scale = Lerp(1f, 1.6f, glow);
            }

            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            if (Timer > ChargingMax && opa != 0)
            {
                Texture2D cGlow = LostbeltModAssets.Particle_OpticalLineGlow.Value;
                SB.FastDraw(cGlow, pos2, Color.LightSkyBlue * opa, 0, cGlow.Size() / 2f, .6f * scale);
                SB.FastDraw(cGlow, pos2, Color.LightSkyBlue * opa, PiOver2, cGlow.Size() / 2f, .6f * scale);

                Effect shader = LostbeltShaderAssets.UCAPolarDistortShaderColor;
                shader.Parameters["uWidthMult"].SetValue(1);
                shader.Parameters["uRingMult"].SetValue(4f);
                shader.Parameters["uYTime"].SetValue(-1.17f * opa);
                shader.CurrentTechnique.Passes[0].Apply();
                Main.instance.GraphicsDevice.Textures[1] = LostbeltModAssets.Particle_HRShinyOrbSmall.Value;

                Texture2D noise1 = LostbeltModAssets.Noise_Aura.Value;
                float noise1Scale = 1f * scale * .86f;
                for (float i = 1f; i > .85f; i -= .1f)
                {
                    SB.FastDraw(noise1, pos2, Color.LightSkyBlue * opa, TwoPi / 1f, noise1.Size() / 2f, noise1Scale * i);
                    SB.FastDraw(noise1, pos2, Color.LightSkyBlue * opa, TwoPi / 2f, noise1.Size() / 2f, noise1Scale * i);
                    SB.FastDraw(noise1, pos2, Color.LightSkyBlue * opa, TwoPi / 3f, noise1.Size() / 2f, noise1Scale * i);
                }
                shader = LostbeltShaderAssets.UCAPolarDistortShaderColor;
                shader.Parameters["uWidthMult"].SetValue(1);
                shader.Parameters["uRingMult"].SetValue(4f);
                shader.Parameters["uYTime"].SetValue(Main.GlobalTimeWrappedHourly * .47f);
                shader.CurrentTechnique.Passes[0].Apply();
                Main.instance.GraphicsDevice.Textures[1] = LostbeltModAssets.Particle_HRShinyOrbSmall.Value;

                noise1 = LostbeltModAssets.Noise_WaterFlow.Value;
                noise1Scale = .26f;
                for (int i = 0; i < 3; i++)
                {
                    SB.FastDraw(noise1, pos2 + (TwoPi / 3f * i).ToRotationVector2() * 1.1f, Color.Aquamarine, Main.GlobalTimeWrappedHourly, noise1.Size() / 2f, noise1Scale);
                }
                SB.FastDraw(noise1, pos2, Color.RoyalBlue, (float)Main.GlobalTimeWrappedHourly, noise1.Size() / 2f, noise1Scale);
                SB.FastDraw(noise1, pos2, Color.White, (float)Main.GlobalTimeWrappedHourly, noise1.Size() / 2f, noise1Scale);

            }
            else if (Timer > ChargingMax && opa == 0 && ChargingLevel == ChargingEnum.Full)
            {
                Effect shader = LostbeltShaderAssets.UCAPolarDistortShaderColor;
                shader.Parameters["uWidthMult"].SetValue(1);
                shader.Parameters["uRingMult"].SetValue(4f);
                shader.Parameters["uYTime"].SetValue(Main.GlobalTimeWrappedHourly * .47f);
                shader.CurrentTechnique.Passes[0].Apply();
                Main.instance.GraphicsDevice.Textures[1] = LostbeltModAssets.Particle_HRShinyOrbSmall.Value;

                Texture2D noise1 = LostbeltModAssets.Noise_WaterFlow.Value;
                float noise1Scale = .26f;
                for (int i = 0; i < 3; i++)
                {
                    SB.FastDraw(noise1, pos2 + (TwoPi / 3f * i).ToRotationVector2() * 1.1f, Color.Aquamarine, Main.GlobalTimeWrappedHourly, noise1.Size() / 2f, noise1Scale);
                }
                SB.FastDraw(noise1, pos2, Color.RoyalBlue, (float)Main.GlobalTimeWrappedHourly, noise1.Size() / 2f, noise1Scale);
                SB.FastDraw(noise1, pos2, Color.White, (float)Main.GlobalTimeWrappedHourly, noise1.Size() / 2f, noise1Scale);
            }
            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            float orbScale = OrbScale;
            SB.FastDraw(star, pos2.ToRandCirclePos(3), mainColor, rot, star.Size() / 2f, .98f * Projectile.scale * orbScale);
            SB.FastDraw(star, pos2.ToRandCirclePos(3), Color.White, rot, star.Size() / 2f, .485f * Projectile.scale * orbScale);
            LostbeltMethods.EndShaderAreaPixel();
        }
        #endregion
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.GetRangedWeaponHeldProjData(out Texture2D tex, out Vector2 drawPos, out Vector2 rotPoint, out float _, out SpriteEffects se);
            PixelatedRenderManager.BeginDrawProj = true;
            Vector2 offset = HoldoutOffset * new Vector2(Owner.direction, 1);
            float drawRot = Projectile.rotation + (Projectile.spriteDirection == -1 ? Pi : 0);
            drawPos += offset.RotatedBy(drawRot);
            float scale = Projectile.scale;
            if (ChargingLevel != ChargingEnum.Regular)
            {
                float p = Utils.GetLerpValue(1, 7, OrbPosOffsetX, true);
                drawPos += Main.rand.NextVector2Circular(1.5f, 1.5f) * p;
                for (int i = 0; i < 8; i++)
                    SB.FastDraw(tex, drawPos + (TwoPi / 8f * i).ToRotationVector2() * 1.2f, OrbColor.ToAddColor(), drawRot, rotPoint, scale * 1.1f, se);
            }
            SB.FastDraw(tex, drawPos, Color.White, drawRot, rotPoint, scale, se);
            return false;
        }
    }
}
