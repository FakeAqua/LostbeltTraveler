using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.PixelatedRender;
using LostbeltTraveler.Core.ScreenEffect;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Graphics.Particles;
using LostbeltTraveler.Items.Weapons.Magic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Projs.Magic
{
    public class AXCannonPlayer : ModPlayer
    {
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            Player p = drawInfo.drawPlayer;
            if (p.CanUseHoldout<AXCannon>())
            {
            }
            base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
        }
    }
    public class AXCannonHeldProj : LostbeltHeldProj, IPixelatedRenderer
    {
        public override int OriginalItemID => ItemType<AXCannon>();
        public override string Texture => GetInstance<AXCannon>().Texture;
        public Vector2 HoldoutOffset => new Vector2(17, -3);
        public bool ChargingReady
        {
            get => Projectile.ai[2] == 1f;
            set => Projectile.ai[2] = value ? 1 : 0;
        }
        public int ChargingMax => AttackSpeed * 8;
        public int ChargingHalf => AttackSpeed * 3;
        public ChargingEnum ChargingLevel
        {
            get => (ChargingEnum)Projectile.ai[1];
            set => Projectile.ai[1] = (float)value;
        }
        public float RecoilTimer = 0;
        public float ChargingRecoil
        {
            get
            {
                switch (ChargingLevel)
                {
                    case ChargingEnum.Full:
                        return Projectile.MaxUpdates * 60;
                    default:
                        return 0;
                }
            }
        }
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
        public override void ExSD()
        {
            base.ExSD();
        }
        public override void OnFirstFrame()
        {
            base.OnFirstFrame();
        }
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
        public void BlastAttackFull()
        {
            float rot = Projectile.rotation + (Projectile.spriteDirection > 0 ? 0 : Pi);
            Vector2 offset = (HoldoutOffset + new Vector2(5 * 7, 0)) * new Vector2(Owner.direction, 1);
            Vector2 orbPosition = Projectile.Center + offset.RotatedBy(rot);

            if (!IsChargingAttacking)
            {
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
                float ringScale = Projectile.scale * 1.2f;
                for (float i = 0; i < 1f; i += .2f)
                {
                    new NoiseShockRing(orbPosition, Vector2.Zero, Color.Lerp(Color.DodgerBlue, Color.LightSkyBlue, i), 30, 1, .9f * Lerp(0.95f, 1f, i), -1, Vector2.Zero, false).Spawn();
                    ECSParticle.CrossGlow(orbPosition, Color.LightSkyBlue, 40, i, 0.21f);
                    ECSParticle.CrossGlow(orbPosition, Color.White, 40, i * .85f, 0.21f);
                }
                for (int i = 0; i < 15; i++)
                {
                    Vector2 args = (TwoPi / 16f * i).ToRotationVector2();
                    Vector2 particlePos = orbPosition + args * 10f;
                    ECSParticle.ShinyCrossStarECS(particlePos, RandVelTwoPi(1.5f, 9.4f) * 1.1f, Color.SkyBlue, 45, 1, .95f, .2f);

                }
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        Vector2 args2 = (TwoPi / 4f * j + 0).ToRotationVector2();
                        ECSParticle.LightntingGlow(orbPosition.ToRandCirclePos(3), args2 * 7.277f * Main.rand.NextFloat(), Color.LightSkyBlue, 40, Main.rand.NextFloat(), 0.75f * 1 * Main.rand.NextFloat(.9f, 1.1f), 4);
                    }
                }
                for (int i = 0; i < 20; i++)
                {
                    ECSParticle.TurbulenceShinyOrb(orbPosition.ToRandCirclePos(60), 1.9f, Color.SkyBlue, 60, 1, .1f);
                }
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.rotation.ToRotationVector2() * 12f, ProjectileType<AXCannonFullCharge>(), Projectile.originalDamage, Projectile.knockBack, Projectile.owner);
                IsChargingAttacking = true;
            }
            UpdateRecoil(60);
        }
        public void BlastAttackHalf()
        {
            UpdateRecoil(30);

        }
        public void BlastAttackNone()
        {
            UpdateRecoil(10);

        }
        public void UpdateRecoil(float RecoilPower)
        {
        }
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
                OrbPosOffsetX = Lerp(1, 3f, lerping);
            }
            else if (Timer < ChargingMax)
            {
                float lerping = EaseInCubic(Utils.GetLerpValue(AttackSpeed, ChargingMax, Timer, true));
                OrbScale = Lerp(noChargeScale, halfChargeScale, lerping);
                OrbColor = Color.Lerp(Color.Orange, Color.LimeGreen, lerping);
                OrbPosOffsetX = Lerp(3, 5f, lerping);
            }
            else
            {
                float lerping = EaseInCubic(Utils.GetLerpValue(ChargingMax, ChargingMax + AttackSpeed / 2, Timer, true));
                OrbScale = Lerp(halfChargeScale, maxChargeScale, lerping);
                OrbColor = Color.Lerp(Color.LimeGreen, Color.RoyalBlue, (lerping));
                OrbPosOffsetX = 7;
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
                    RecoilTimer = 60 * Projectile.MaxUpdates;
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
                    RecoilTimer = Projectile.MaxUpdates * 40;
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
            if (Timer < AttackSpeed)
                Timer = 0;

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
        public BlendState BlendState => BlendState.Additive;
        public ScarletDrawLayer LayerToRenderTo => ScarletDrawLayer.BeforeDusts;
        public Color OrbColor = Color.Orange;
        public float OrbPosOffsetX = 1;
        public float OrbScale = 1f;
        public void RenderPixelated(SpriteBatch spriteBatch)
        {
            if (IsChargingAttacking && ChargingLevel == ChargingEnum.Full)
            {
                Texture2D star = LostbeltModAssets.Particle_HRShinyOrbSmall.Value;
                Color mainColor = OrbColor;

                float rot = Projectile.rotation + (Projectile.spriteDirection > 0 ? 0 : Pi);
                Vector2 offset = (HoldoutOffset + new Vector2(5 * 7, 0)) * new Vector2(Owner.direction, 1);
                Vector2 pos2 = Projectile.Center - Main.screenPosition + offset.BetterRotatedBy(rot);
                float lerping = (Utils.GetLerpValue(60 * Projectile.MaxUpdates, 0, RecoilTimer, true));
                float glow = 0;
                float threshold = 0.05f;
                float opa = 0;
                float scale = .2f;
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

                return;
            }
            if (Owner.channel)
                DrawChargingOrb();
        }
        public void DrawChargingOrb()
        {
            Texture2D star = LostbeltModAssets.Particle_HRShinyOrbSmall.Value;
            Color mainColor = OrbColor;
            float rot = Projectile.rotation + (Projectile.spriteDirection > 0 ? 0 : Pi);
            Vector2 offset = (HoldoutOffset + new Vector2(5 * OrbPosOffsetX, 0)) * new Vector2(Owner.direction, 1);
            Vector2 pos2 = Projectile.Center - Main.screenPosition + offset.BetterRotatedBy(rot);
            float lerping = (Utils.GetLerpValue(ChargingMax, ChargingMax + 40 * Projectile.MaxUpdates, Timer, true));
            float glow = 0;
            float threshold = 0.15f;
            float opa = 0;
            float scale = .2f;
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
                    float progress = Utils.GetLerpValue(1f, .85f, i, true);
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
                float p = Utils.GetLerpValue(1, 7f, OrbPosOffsetX, true);
                drawPos += Main.rand.NextVector2Circular(1.5f, 1.5f) * p;
                for (int i = 0; i < 8; i++)
                    SB.FastDraw(tex, drawPos + (TwoPi / 8f * i).ToRotationVector2() * 1.2f, OrbColor.ToAddColor(), drawRot, rotPoint, scale * 1.1f, se);
            }
            SB.FastDraw(tex, drawPos, Color.White, drawRot, rotPoint, scale, se);
            return false;
        }
    }
}
