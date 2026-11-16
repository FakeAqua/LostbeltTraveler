using LostbeltTraveler.Core.NetCode;
using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.PixelatedRender;
using LostbeltTraveler.Core.ScreenEffect;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Items.Weapons.Executor.ColdSteel;
using System.Collections.Generic;
using Terraria;

namespace LostbeltTraveler.Projs.Executor.ColdSteel
{
    public class DragonBallSwordHeldProj : ExecutorHeldProj, IPixelatedRenderer
    {
        public override string Texture => GetInstance<DragonBallSword>().Texture;
        public override int OriginalItemID => ItemType<DragonBallSword>();
        public AnimationStruct Helper = new AnimationStruct(3);
        public float SwordLength = 60;
        public float BeginTargetRotation = 0;
        public float TargetRotation = 0;
        public bool Flip = false;
        public float Height = 1f;
        public float Width = 1.2f;
        public float SwingScale = 1.21f;
        public float SwordScale = 1f;
        public List<Vector2> OldAimPos = [];
        public bool ThirdSwing = false;
        public float SwingTime = 0;
        public float SlashOpacity = 1f;
        public float SlashProgress => (Helper.GetAniProgress(2));
        public float ArmRotation = 0;
        public float StopTiming = 0;
        public override void SetStaticDefaults()
        {
            ScarletProjIDSets.IsHeldProj[Type] = true;
        }
        public override void ExSD()
        {
            Projectile.SetUpHeldProj(10);
            Projectile.penetrate = -1;
            Projectile.SetupImmnuity(-1);
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
            Projectile.noEnchantmentVisuals = true;
        }
        public override void OnFirstFrame()
        {
            if (Projectile.IsOwnerSide())
            {
                int c = Main.rand.Next(3, 5);
                //ScarletSound(LostbeltSoundAssets.Gaia_Toss, Projectile.Center, 0.25f, 1, -0.21f + 0.14f * SwingTime, 0.21f,2);
            }
            Helper.MaxProgress[0] = (int)(AttackSpeed * .35f);
            Helper.MaxProgress[1] = (int)(AttackSpeed * .5f);
            Helper.MaxProgress[2] = (int)(AttackSpeed * 1);
            BeginTargetRotation = (Owner.AimWorldOf() - Owner.Center).SafeNormalize(Vector2.UnitX).ToRotation();
            TargetRotation = BeginTargetRotation;
            ArmRotation = TargetRotation;
        }
        public override void ProjAI()
        {
            Projectile.velocity = Projectile.velocity.ToSafeNormalize();
            UpdateAnimation();
            UpdateHeldState();
            UpdatePlayerState();
            HandleExecution();

        }
        public void UpdatePlayerState()
        {
            Projectile.velocity = TargetRotation.ToRotationVector2();
            Owner.ChangeDir(Projectile.direction);
            Projectile.spriteDirection = Flip.ToDirectionInt() * Projectile.direction;
            Owner.ControlPlayerArm(ArmRotation);

        }

        public void UpdateHeldState()
        {
            Projectile.Center = Owner.MountedCenter;
            if (Helper.Progress[2] <= 0)
            {
                Owner.itemTime = 2;
                Owner.itemAnimation = 2;
            }
            Owner.heldProj = Projectile.whoAmI;
            if (Owner.dead)
                Projectile.Kill();
            else
                Projectile.timeLeft = 2;
        }

        public void UpdateAnimation()
        {
            if (StopTiming > 0)
            {
                StopTiming--;
                return;
            }
            UpdateHalfCircleSwingAnimation();
        }
        public void UpdateHalfCircleSwingAnimation()
        {
            if (!Helper.IsDone[2])
            {
                UpdateSlashAnimation();
            }
            if (!Helper.IsDone[0])
            {
                if (Helper.OnAnimationBegin(0) && Projectile.IsOwnerSide())
                {


                    Vector2 fireVel = (Main.MouseWorld - Owner.Center).ToSafeNormalize() * 40;
                    Vector2 pos = Owner.MountedCenter - fireVel.ToSafeNormalize() * 300;
                    Vector2 dir = (pos - Projectile.Center).ToSafeNormalize(Vector2.UnitX);
                    Vector2 vel = dir.RotatedBy(PiOver4 * Projectile.spriteDirection);
                    ScreenShakeSystem.AddScreenShakes(Projectile.Center, 8f, 64, vel.ToRotation(), 0, true, 1000, EaseOutBack);
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), pos, fireVel, ProjectileType<DragonBallSlash>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    ScarletSound(LostbeltSoundAssets.Misc_ManaStrike, Projectile.Center, .75f, 0, 0.10f, 0.5f);
                }
                UpdateBeginAnimation();
            }
            else if (!Helper.IsDone[1])
                UpdateEndAnimation();
            else
            {
                if (Helper.IsDone[0] && Helper.IsDone[1] && Helper.IsDone[2])
                {
                    Projectile.Kill();
                }
            }

        }
        public void UpdateSlashAnimation()
        {
            Helper.UpdateAniState(2);

        }
        private void UpdateEndAnimation()
        {
            float heldScale = Owner.HeldItem.scale;
            Helper.UpdateAniState(1);
            float easedProgress = EaseOutCubic(Helper.GetAniProgress(1));
            float beginAngle = 175f * Flip.ToDirectionInt();
            float endAngle = 200f * Flip.ToDirectionInt();
            float rot = Helper.UpdateAngle(beginAngle, endAngle, Owner.direction, easedProgress);
            Matrix tForm = Matrix.CreateRotationZ(rot) * Matrix.CreateScale(Width, Height, 1);
            Vector2 tarPos = Vector2.Transform(Vector2.UnitX, tForm) * SwingScale * heldScale;
            Projectile.scale = tarPos.Length();
            Projectile.rotation = tarPos.ToRotation() + TargetRotation;
            beginAngle = 125f * Flip.ToDirectionInt();
            endAngle = 150f * Flip.ToDirectionInt();
            rot = Helper.UpdateAngle(beginAngle, endAngle, Owner.direction, easedProgress);
            tForm = Matrix.CreateRotationZ(rot) * Matrix.CreateScale(1, 1, 1);
            tarPos = Vector2.Transform(Vector2.UnitX, tForm) * SwingScale;
            ArmRotation = tarPos.ToRotation() + TargetRotation;

            TargetRotation = TargetRotation.AngleTowards((Owner.AimWorldOf() - Projectile.Center).SafeNormalize(Vector2.UnitX).ToRotation(), .01f);

        }

        public void UpdateBeginAnimation()
        {
            float heldScale = Owner.HeldItem.scale;
            Helper.UpdateAniState(0);
            float easedProgress = EaseOutCubic(Helper.GetAniProgress(0));
            float beginAngle = -200f * Flip.ToDirectionInt();
            float endAngle = 175f * Flip.ToDirectionInt();
            float rot = Helper.UpdateAngle(beginAngle, endAngle, Owner.direction, easedProgress);
            Matrix tForm = Matrix.CreateRotationZ(rot) * Matrix.CreateScale(Width, Height, 1);
            Vector2 tarPos = Vector2.Transform(Vector2.UnitX, tForm) * SwingScale * heldScale;
            Projectile.scale = tarPos.Length();
            Projectile.rotation = tarPos.ToRotation() + TargetRotation;
            float armBeginAngle = -150f * Flip.ToDirectionInt();
            float armEndAngle = 125f * Flip.ToDirectionInt();
            float armRot = Helper.UpdateAngle(armBeginAngle, armEndAngle, Owner.direction, easedProgress);
            Matrix armTForm = Matrix.CreateRotationZ(armRot) * Matrix.CreateScale(1, 1, 1);
            Vector2 armTarPos = Vector2.Transform(Vector2.UnitX, armTForm) * SwingScale;
            ArmRotation = armTarPos.ToRotation() + TargetRotation;

            if (easedProgress < .01f)
                TargetRotation = TargetRotation.AngleTowards((Owner.AimWorldOf() - Projectile.Center).SafeNormalize(Vector2.UnitX).ToRotation(), .5f);
            else
            {
                //下面基本上是粒子生成了。
                float slashTrailRotation = Helper.UpdateAngle(beginAngle, endAngle, Owner.direction, easedProgress);
                Matrix tFormSlash = Matrix.CreateRotationZ(slashTrailRotation) * Matrix.CreateScale(Width, Height, 1f);
                Vector2 slashTargetPos = Vector2.Transform(Vector2.UnitX, tFormSlash) * 1.2f * SwingScale * heldScale;
                Vector2 slashPosFinal = slashTargetPos.RotatedBy(TargetRotation) * 110;
                OldAimPos.Add(slashPosFinal);
                if (easedProgress >= 0.95f)
                    return;
                if (Main.rand.NextBool(9) && Projectile.IsOwnerSide())
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 pos = Vector2.Lerp(Projectile.Center, Projectile.Center + tarPos.RotatedBy(TargetRotation) * 100, Main.rand.NextFloat(0.41f, 1f));
                        Vector2 dir = (pos - Projectile.Center).ToSafeNormalize(Vector2.UnitX);
                        Vector2 vel = dir.RotatedBy(PiOver2 * Projectile.spriteDirection);
                        Vector2 orbVel = vel * Main.rand.NextFloat(.5f, 1.1f) * 10f;
                        ECSParticle.ShinyCrossStarECS(pos + orbVel, vel, RandLerpColor(Color.WhiteSmoke, Color.White), 40, 1f, Main.rand.NextFloat(.95f, 1.10f) * .13f);
                    }
                }
                if (Main.rand.NextBool(4))
                {
                    if (!Projectile.IsOwnerSide())
                        return;
                    Vector2 pos = Vector2.Lerp(Projectile.Center, Projectile.Center + tarPos.RotatedBy(TargetRotation) * 70, Main.rand.NextFloat(0.45f, 1f));
                    Vector2 dir = (pos - Projectile.Center).ToSafeNormalize(Vector2.UnitX);
                    Vector2 vel = dir.RotatedBy(PiOver2 * Projectile.spriteDirection) * Main.rand.NextFloat(.1f, 1.2f) * 5f;
                    for (int i = 0; i < 3; i++)
                    {
                        ECSParticle.LiliesFire(pos + dir * 5 * i, vel, RandLerpColor(Color.White, Color.WhiteSmoke), 45,
                            RandRotTwoPi, .85f, Main.rand.NextFloat(.95f, 1.10f) * Projectile.scale * .141f, true, BlendState.Additive);
                    }
                }

            }

        }

        public override void OnKill(int timeLeft)
        {
            if (!Projectile.IsOwnerSide())
                return;
            if (LostbeltNetInput.LocalMouseLeft && !Owner.dead)
            {
                if (!Projectile.Lostbelt().ExecutionStrike)
                {
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, Type, Projectile.originalDamage, Projectile.knockBack, Projectile.owner);
                    ((DragonBallSwordHeldProj)proj.ModProjectile).Flip = !Flip;
                    proj.Lostbelt().HasExecutionMechanic = true;
                    proj.Lostbelt().ExecutionStrike = Owner.GetExecutionSrike();
                }
            }
            else
            {
                if (Projectile.Lostbelt().ExecutionStrike)
                    Owner.RemoveExecutionProgress(OriginalItemID);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.numHits < 1)
            {
                StopTiming = 10 * Projectile.MaxUpdates;
                ScarletSound(LostbeltSoundAssets.Misc_HeavySwordHit, Projectile.Center, 0.6f, 1, .3f, 0.1f);
                Vector2 dir = (target.Center - Projectile.Center).ToSafeNormalize(Vector2.UnitX);
                Vector2 vel = dir.RotatedBy(PiOver2 * Projectile.spriteDirection);
                ScreenShakeSystem.AddScreenShakes(Projectile.Center, 32f, 24, vel.ToRotation(), 0, false, 1000, EaseOutCubic);


            }
            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (!Projectile.Lostbelt().FirstFrame)
                return false;
            float easedProgress = EaseOutCubic(Helper.GetAniProgress(0));
            if (easedProgress < 0.01f)
                return false;
            float _ = float.NaN;
            Vector2 beamBeginPos = Owner.Center;
            Vector2 beamEndPos = Projectile.Center + Projectile.rotation.ToRotationVector2() * Projectile.scale * 128;
            bool c = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), beamBeginPos, beamEndPos, 14f, ref _);
            return c;
        }
        public ScarletDrawLayer LayerToRenderTo => ScarletDrawLayer.BeforeDusts;
        public BlendState BlendState => BlendState.Additive;

        public void RenderPixelated(SpriteBatch spriteBatch)
        {
            if (!Projectile.Lostbelt().FirstFrame)
                return;

            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            DrawThisSmear();
            LostbeltMethods.EndShaderAreaPixel();

        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (!Projectile.Lostbelt().FirstFrame)
                return false;

            PixelatedRenderManager.BeginDrawProj = true;
            Projectile.GetProjDrawInfo_Melee(out Texture2D tex, out Vector2 drawPosition, out float drawRotation, out Vector2 rotationPoint, out SpriteEffects flipSprite);
            float easedProgress = EaseOutCubic(Helper.GetAniProgress(0));
            float endPro = easedProgress;
            float pro = 0;
            float threshold = .68f;
            if (endPro < threshold)
            {
                pro = Utils.GetLerpValue(0, threshold, endPro, true);
            }
            else
                pro = 1 - Utils.GetLerpValue(threshold, 1f, endPro, true);
            Color c = Color.White;
            float offset = (10);
            Vector2 ori = Projectile.spriteDirection == -1 ? new Vector2(tex.Width, tex.Height) - new Vector2(offset) : new Vector2(offset, tex.Height - offset);
            drawPosition += (new Vector2(15, 10) * new Vector2(1, -Projectile.spriteDirection)).RotatedBy(Projectile.rotation) * Clamp((1 - pro), .34f, 1f);
            if (StopTiming > 0)
                drawPosition += Main.rand.NextVector2Circular(1f, 1f);
            for (int i = 0; i < 16; i++)
                SB.Draw(tex, drawPosition + (TwoPi / 16f * i).ToRotationVector2() * 1.5f * (1 - pro), null, Color.White.ToAddColor() * (1 - pro), drawRotation, ori, Projectile.scale, flipSprite, 0);
            SB.Draw(tex, drawPosition, null, c, drawRotation, ori, Projectile.scale, flipSprite, 0);
            SB.EnterShaderArea();
            SB.EndShaderArea();
            SB.EndShaderArea();
            return false;
        }

        public void DrawThisSmear()
        {
            Projectile.GetProjDrawInfo_Melee(out Texture2D tex, out Vector2 drawPosition, out float drawRotation, out Vector2 rotationPoint, out SpriteEffects flipSprite);
            float endPro = SlashProgress;
            float pro;
            float threshold = .2f;
            if (endPro < threshold)
            {
                float p = Utils.GetLerpValue(0, threshold, endPro, true);
                pro = EaseOutCubic(p);
            }
            else
            {
                float p = 1 - Utils.GetLerpValue(threshold, 1f, endPro, true);
                pro = (p);
            }
            Vector2 pos2 = drawPosition + Vector2.UnitX.RotatedBy(Projectile.rotation) * (0) * Projectile.scale;
            Texture2D smear = LostbeltModAssets.Particle_SmearEdge.Value;
            float glowScale2 = 2f;
            float opac = pro;
            float rot = drawRotation + Pi * .15f * Flip.ToDirectionInt() * Owner.direction;
            SB.Draw(smear, pos2, null, Color.White * opac, rot, smear.Size() / 2, glowScale2, flipSprite, 0);
            smear = LostbeltModAssets.Particle_Smear.Value;
            SB.Draw(smear, pos2, null, Color.White * opac, rot, smear.Size() / 2, glowScale2, flipSprite, 0);
            SB.Draw(smear, pos2, null, Color.White * .85f * opac, rot, smear.Size() / 2, glowScale2, flipSprite, 0);
            SB.Draw(smear, pos2, null, Color.Silver * .78f * opac, rot, smear.Size() / 2, glowScale2 * 90f, flipSprite, 0);
            SB.Draw(smear, pos2, null, Color.LightGray * opac * .58f, rot, smear.Size() / 2, glowScale2 * .80f, flipSprite, 0);
        }
    }
}
