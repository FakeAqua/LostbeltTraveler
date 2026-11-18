using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.ScreenEffect;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Items.Weapons.Executor.Firearm;
using Terraria;

namespace LostbeltTraveler.Projs.Executor.Firearm
{
    public class CaptainDeadBeardHeldProj : LostbeltRangedWeaponoutClass
    {
        public override EnumDamageClass Category => EnumDamageClass.Executor;
        public override int OriginalItemID => ItemType<CaptainDeadBeard>();
        public override string Texture => GetInstance<CaptainDeadBeard>().Texture;
        public override float RecoilPower => HandleRecoilPower();
        public override int ProjExtraUpdates => 2;

        public float LerpToSniper = 0;
        public bool IsTurningIntoSniperMode = false;
        public Vector2 TargetPos = Vector2.Zero;
        public float TargetRotation = 0;
        public float ArmRotation = 0;
        public AnimationStruct Helper = new AnimationStruct(2);

        public override Vector2 HoldoutOffset => IsTurningIntoSniperMode ? Vector2.Lerp(new Vector2(15, -2.5f), Vector2.Zero, LerpToSniper) : new Vector2(15, -2.5f);
        public override Color HoldoutEdgeColor => Color.Goldenrod;
        public override Vector2 TileHitbox => base.TileHitbox;
        public override bool IsUsing => base.IsUsing || Projectile.Lostbelt().ExecutionStrike;
        public bool DoExecute => Projectile.Lostbelt().ExecutionStrike;

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Rectangle rec = Utils.CenteredRectangle(Projectile.Center, new Vector2(160));
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

            if (Helper.IsDone[0])
            {
                DoneSpining();
                return;
            }

            UpdateSpining();
        }

        private void InitSpining()
        {
            IsTurningIntoSniperMode = true;
            ScarletSound(LostbeltSoundAssets.Air_HeavyFlow, Projectile.Center, 1f, 0, .7f, .1f, 2);
            Helper.MaxProgress[0] = Projectile.MaxUpdates * 40;
            TargetPos = Projectile.Center + Owner.ToMouseVector2() * 350f;
            Projectile.penetrate = -1;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
            Projectile.SetupImmnuity(20);
        }

        private void DoneSpining()
        {
            Projectile.Kill();
            ScreenShakeSystem.AddScreenShakes(Projectile.Center, 20, 20, RandRotTwoPi);
            ScarletSound(LostbeltSoundAssets.Misc_SwordHit, Projectile.Center, pitch: .4f);
            Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity,
                ProjectileType<CaptainDeadBeardSniperProj>(), Projectile.originalDamage, Projectile.knockBack, Projectile.owner);
            ((CaptainDeadBeard)Owner.HeldItem.ModItem).SniperMode = true;
            ((CaptainDeadBeardSniperProj)proj.ModProjectile).Timer = AttackSpeed;
        }


        private void UpdateSpining()
        {
            LerpToSniper = Lerp(LerpToSniper, 1f, .051f);

            float progress = Helper.GetAniProgress(0);
            float threshold = .5f;

            if (progress > threshold)
            {
                float prevProgress = Utils.GetLerpValue(threshold, 1f, progress, true);
                Projectile.Center = Vector2.Lerp(TargetPos, Owner.MountedCenter, EaseInCubic(prevProgress));
                Projectile.rotation += Lerp(.3f, 0f, EaseInCubic(prevProgress)) * Projectile.spriteDirection;

                if (Main.rand.NextBool(3))
                {
                    ECSParticle.ShinyCrossStarSmall(Projectile.Center.ToRandCirclePos(100), -Owner.MountedCenter.GetNormalVector2(TargetPos) * 1.1f,
                        Color.Gold, 40, 1, 0.4f, Main.rand.NextFloat(-.1f, .1f));
                }

                float zoomPro = 1 - prevProgress;
                ScreenZoomSystem.MoveCameraTo(Projectile.Center, EaseOutBack(zoomPro * zoomPro) * .13f);

            }
            else
            {
                float prevProgress = Utils.GetLerpValue(0, threshold, progress, true);
                Projectile.Center = Vector2.Lerp(Projectile.Center, TargetPos, EaseInOutExpo(prevProgress));
                Projectile.rotation += Lerp(.0f, .3f, EaseOutCubic(prevProgress)) * Projectile.spriteDirection;

                if (Main.rand.NextBool(3))
                {
                    ECSParticle.ShinyCrossStarSmall(Projectile.Center.ToRandCirclePos(100), Owner.MountedCenter.GetNormalVector2(TargetPos) * 1.11f,
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
            if (Owner.GetExecutionSrike())
                return;
            if (Owner.Lostbelt().tacticalExecutionInputCache <= 0)
                return;

            Projectile.Lostbelt().ExecutionStrike = true;
            Owner.RemoveExecutionProgress();
            Owner.Lostbelt().tacticalExecutionInputCache = 0;
            Timer = AttackSpeed;
        }

        protected override void OnAttack()
        {
            if (Projectile.Lostbelt().ExecutionStrike)
                DoExecutionAttack();
            else
                DoNormalAttack();
        }

        public void DoNormalAttack()
        {
            Vector2 dir = Projectile.SafeDirByRot();
            int type = ProjectileType<CaptainDeadBeardBullet>();
            for (int i = 0; i < ConferenceCall.BulletsPerShot; i++)
            {
                Vector2 randomVelocity = dir.RotatedByRandom(ToRadians(12.5f)) * Main.rand.NextFloat(0.88f, 1.12f);
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, randomVelocity * 17f, type, Projectile.originalDamage, Projectile.knockBack, Projectile.owner);
                if (i == 0)
                    proj.Lostbelt().HasExecutionMechanic = true;
            }
            ScarletSound(LostbeltSoundAssets.ASMD_IceBlockSplit, Projectile.Center, 0.20f, 0, .34f, 0.1f);
            Vector2 offset = new Vector2(45, -5 * Projectile.direction).RotatedBy(Projectile.rotation);
            Vector2 pos = Projectile.Center + offset;
            Vector2 particleOffset = new Vector2(10, 0 * Projectile.direction).RotatedBy(Projectile.rotation);

            for (int i = 0; i < 16; i++)
            {
                Vector2 pos2 = pos.ToRandCirclePos(8) - particleOffset;
                Vector2 vel = dir.ToRandVelocity(ToRadians(25), .1f, 11.6f);
                float scale = Projectile.scale * Main.rand.NextFloat(.95f, 1.15f) * 0.48f;
                ECSParticle.ShinyCrossStarECS(pos2, vel, RandLerpColor(Color.Gold, Color.LightGoldenrodYellow), Main.rand.Next(30, 45), 1, scale);
            }

            for (int i = 0; i < 16; i++)
            {
                bool alt = Main.rand.NextBool();
                BlendState bs = alt ? BlendState.Additive : BlendState.AlphaBlend;
                ECSParticle.SmokeParticle(pos, dir.ToRandVelocity(ToRadians(10), 0.4f, 21.4f), RandLerpColor(Color.Gold, Color.LightGoldenrodYellow), Main.rand.Next(45, 65), RandRotTwoPi, 1, 0.33f * Main.rand.NextFloat(.95f, 1.25f), alt, bs);
            }

        }

        public void DoExecutionAttack()
        {
            Vector2 dir = Projectile.SafeDirByRot();
            int type = ProjectileType<CaptainDeadBeardCannonRodeo>();
            Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, dir * 15f, type, Projectile.originalDamage, Projectile.knockBack, Projectile.owner);
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
                return 15f;
            else
                return 4.1f;

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


        public ScarletDrawLayer LayerToRenderTo => ScarletDrawLayer.BeforePlayer;
        public BlendState BlendState => BlendState.Additive;

        public void RenderPixelated(SpriteBatch sb)
        {
            if (!IsTurningIntoSniperMode)
                return;
            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            LostbeltMethods.EndShaderAreaPixel();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (!IsTurningIntoSniperMode)
            {
                base.PreDraw(ref lightColor);
                return false;
            }

            DrawSniperMode();
            return false;
        }

        private void DrawSniperMode()
        {
            Projectile.GetRangedWeaponHeldProjData(out Texture2D tex, out Vector2 drawPos, out Vector2 rotPoint, out float _, out SpriteEffects se);
            drawSlash();

            SB.EnterShaderArea();
            Texture2D shiny = LostbeltModAssets.Particle_HRShinyOrb.Value;
            SB.FastDraw(shiny, drawPos, Color.DarkGoldenrod, 0, shiny.Size() / 2f, Projectile.scale * .85f * .40f, se);
            SB.FastDraw(shiny, drawPos, Color.LightGoldenrodYellow, 0, shiny.Size() / 2f, Projectile.scale * .85f * .85f * .4f, se);
            SB.EndShaderArea();
            SB.EndShaderArea();

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
        public void drawSlash()
        {
            //下面这段复制的你与我的歌会的
            //我自己也特么看不懂这些到底是啥都啥，但是我知道改一下参数就会很好看，这就够了
            Vector2 position = Projectile.Center + Main.rand.NextVector2Circular(3, 3) - Main.screenPosition;
            Texture2D texture = LostbeltModAssets.Texture_EnergySword.Value;
            Rectangle sourceRectangle = texture.Frame(1, 4); // The sourceRectangle says which frame to use.
            Vector2 origin = sourceRectangle.Size() / 2f;
            float origScale = 1;
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
                    new Color(255, 255, 255, 0) * (i / 9f) * lightingColor * 0.5f, middleMediumColor, lifeSpan, 0f, 0.5f, 0.5f, 1f, edgeRotation, new Vector2(.41f, 1.5f), Vector2.One * scale);
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
        public void DrawSmear(Color outerColor, Color innerColor, float overallScale = 1, float rotFix = 0)
        {
            Texture2D glowTex = LostbeltModAssets.Particle_Smear.Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            SB.FastDraw(glowTex, drawPos.ToRandCirclePos(1.3f), outerColor * .75f, Projectile.rotation + rotFix, glowTex.Size() / 2f, .23f * overallScale, 0);
            SB.FastDraw(glowTex, drawPos.ToRandCirclePos(1.1f), innerColor * .75f, Projectile.rotation + rotFix, glowTex.Size() / 2f, .19f * overallScale, 0);
        }

    }
}
