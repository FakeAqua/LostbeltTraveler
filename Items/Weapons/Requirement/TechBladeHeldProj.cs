using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.PixelatedRender;
using LostbeltTraveler.Core.Primitives.Trail;
using LostbeltTraveler.Core.ScreenEffect;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Handlers;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Requirement
{
    public class TechBladeHeldProj : ModProjectile, IPixelatedRenderer
    {

        public Player Owner => Main.player[Projectile.owner];
        public AnimationStruct Helper = new AnimationStruct(2);
        public float TargetRotation = 0;
        public bool Flip = false;
        public float Height = 1f;
        public float Width = 1f;
        public float StopTiming = 0;
        public List<Vector2> OldAimPos = [];
        public float SlashOpa = 1;
        public int AttackSpeed => Owner.ApplyWeaponAttackSpeed(Owner.HeldItem, Owner.HeldItem.useTime * Projectile.MaxUpdates, 5 * Projectile.MaxUpdates);
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 6;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.width = Projectile.height = 320;
            Projectile.timeLeft = 10000;
            Projectile.extraUpdates = 10;
            Projectile.friendly = true;
            Projectile.noEnchantmentVisuals = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public bool FirstFrame = false;
        public void OnFirstFrame()
        {
            ScarletSound(LostbeltSoundAssets.Misc_AirFlowAlt, Projectile.Center, 0.5f, 1, -0.24f, 0.1f);
            Projectile.originalDamage = Projectile.damage;
            Helper.MaxProgress[0] = (int)(AttackSpeed * .60f);
            Helper.MaxProgress[1] = (int)(AttackSpeed * .40f);
            TargetRotation = Owner.Center.ToMouseVector2().ToRotation();

        }
        public override void AI()
        {
            if (!FirstFrame)
            {
                OnFirstFrame();
                FirstFrame = true;
            }
            UpdateHeldState();
            UpdatePlayerState();
            UpdateAnimationState();


        }

        public void UpdateAnimationState()
        {
            if (StopTiming > 0)
            {
                StopTiming--;
                return;
            }
            if (!Helper.IsDone[0])
            {
                UpdateBeginAnimation();
                if (OldAimPos.Count > 15 * Projectile.MaxUpdates)
                    OldAimPos.RemoveAt(0);


            }
            else if (!Helper.IsDone[1])
            {
                UpdateEndAnimation();
                SlashOpa = Lerp(SlashOpa, 0, 0.02f);
                if (SlashOpa < 0.2f)
                    SlashOpa = 0;

            }
            else
                Projectile.Kill();
        }
        public bool JustSpawned = false;
        public float TrailSlashLength = 160;
        public void UpdateBeginAnimation()
        {
            Helper.UpdateAniState(0);
            float heldScale = Owner.HeldItem.scale;
            float easedProgress = EaseOutCubic(Helper.GetAniProgress(0));
            float beginAngle = 280f * Flip.ToDirectionInt();
            float endAngle = -80f * Flip.ToDirectionInt();
            float rot = Helper.UpdateAngle(beginAngle, endAngle, Owner.direction, easedProgress);
            Matrix tForm = Matrix.CreateRotationZ(rot) * Matrix.CreateScale(Width, Height, 1);
            Vector2 tarPos = Vector2.Transform(Vector2.UnitX, tForm) * heldScale;
            Projectile.scale = tarPos.Length();
            Projectile.rotation = tarPos.ToRotation() + TargetRotation;
            if (easedProgress < .01f)
                TargetRotation = TargetRotation.AngleTowards(Owner.GetToMouseVector2(Projectile.Center).ToRotation(), .5f);
            else
            {
                if (!JustSpawned)
                {

                    for (int i = -2; i < 3; i++)
                    {
                        Vector2 tarCenter = Projectile.Center.GetNormalVector2(Main.MouseWorld);
                        Vector2 args = tarCenter.RotatedBy(ToRadians(15f * i));
                        tarCenter = Projectile.Center;
                        Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), tarCenter, args * 35f, ProjectileType<TechBladeLaser>(), Projectile.originalDamage, Projectile.knockBack, Owner.whoAmI);
                    }
                    JustSpawned = true;
                }
                //下面基本上是粒子生成了。
                float slashbeginAngle = 295f * Flip.ToDirectionInt();
                float slashendAngle = -85f * Flip.ToDirectionInt();
                easedProgress = EaseOutCubic(Helper.GetAniProgress(0));
                float slashTrailRotation = Helper.UpdateAngle(slashbeginAngle, slashendAngle + (0 * (Flip).ToDirectionInt()), Owner.direction, easedProgress);
                Matrix tFormSlash = Matrix.CreateRotationZ(slashTrailRotation) * Matrix.CreateScale(Width, Height, 1f);
                Vector2 slashTargetPos = Vector2.Transform(Vector2.UnitX, tFormSlash) * heldScale;
                Vector2 slashPosFinal = slashTargetPos.RotatedBy(TargetRotation) * TrailSlashLength;
                OldAimPos.Add(slashPosFinal);
                if (easedProgress >= 0.95f)
                    return;
                ParticlePlayer(tarPos);
            }
        }
        public void UpdateEndAnimation()
        {
            Helper.UpdateAniState(1);
            float heldScale = Owner.HeldItem.scale;
            float easedProgress = EaseInCubic(Helper.GetAniProgress(1));
            float beginAngle = -80f * Flip.ToDirectionInt();
            float endAngle = (-105) * Flip.ToDirectionInt();
            float rot = Helper.UpdateAngle(beginAngle, endAngle, Owner.direction, easedProgress);
            Matrix tForm = Matrix.CreateRotationZ(rot) * Matrix.CreateScale(1f, Height, 1);
            Vector2 tarPos = Vector2.Transform(Vector2.UnitX, tForm) * heldScale;
            Projectile.scale = tarPos.Length();
            Projectile.rotation = tarPos.ToRotation() + TargetRotation;
            TargetRotation = TargetRotation.AngleTowards(Owner.GetToMouseVector2(Projectile.Center).ToRotation(), .05f);
        }
        public void ParticlePlayer(Vector2 tarPos)
        {
            if (Main.rand.NextBool(4))
            {
                Vector2 pos = Vector2.Lerp(Projectile.Center, Projectile.Center + tarPos.RotatedBy(TargetRotation) * TrailSlashLength, Main.rand.NextFloat(0.41f, .81f));
                Vector2 dir = (pos - Projectile.Center).ToSafeNormalize(Vector2.UnitX);
                Vector2 vel = dir.RotatedBy(PiOver2 * Projectile.spriteDirection) * Main.rand.NextFloat(10f, 12f);
                ECSParticle.ShinyCrossStarECS(pos, vel, RandLerpColor(Color.SkyBlue, Color.RoyalBlue), 10, 1, 0.63f);
            }
            if (Main.rand.NextBool(4))
            {
                Vector2 pos = Vector2.Lerp(Projectile.Center, Projectile.Center + tarPos.RotatedBy(TargetRotation) * TrailSlashLength, Main.rand.NextFloat(0.81f, 1f));
                Vector2 dir = (pos - Projectile.Center).ToSafeNormalize(Vector2.UnitX);
                Vector2 vel = dir.RotatedBy(PiOver2 * Projectile.spriteDirection) * Main.rand.NextFloat(10f, 13f);
                ECSParticle.HighResolutionThunder(pos, Vector2.Zero, RandLerpColor(Color.White, Color.LightSkyBlue), 20, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .16f, 3);
            }
        }
        public void UpdateHeldState()
        {
            Projectile.Center = Owner.MountedCenter;
            Projectile.position.Y += Owner.gfxOffY;
            Owner.itemTime = Owner.itemAnimation = 2;
            Owner.heldProj = Projectile.whoAmI;
            if (Owner.dead)
                Projectile.Kill();
            else
                Projectile.timeLeft = 2;
        }
        public void UpdatePlayerState()
        {
            Projectile.velocity = TargetRotation.ToRotationVector2();
            Projectile.spriteDirection = Owner.direction;
            Owner.ChangeDir(Projectile.direction);
            Owner.ControlPlayerArm(Projectile.rotation);
        }
        public override void OnKill(int timeLeft)
        {
            if (Main.mouseLeft && !Owner.dead)
            {
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, Type, Projectile.originalDamage, Projectile.knockBack, Projectile.owner);
                ((TechBladeHeldProj)proj.ModProjectile).Flip = Flip;
                ((TechBladeHeldProj)proj.ModProjectile).TargetRotation = TargetRotation;
            }
        }

        public override bool ShouldUpdatePosition() => false;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (!FirstFrame)
                return false;
            float easedProgress = EaseOutCubic(Helper.GetAniProgress(0));
            if (easedProgress < 0.01f)
                return false;
            float _ = float.NaN;
            Vector2 beamBeginPos = Owner.Center;
            Vector2 beamEndPos = Projectile.Center + (Projectile.rotation).ToRotationVector2() * Projectile.scale * TrailSlashLength;
            bool c = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), beamBeginPos, beamEndPos, 64f, ref _);
            return c;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.numHits < 1)
            {
                ScreenShakeSystem.AddScreenShakes(target.Center, 20, 20, Projectile.Center.GetNormalVector2(target.Center).ToRotation(), ToRadians(2f));
                StopTiming = 15;
                SpawnLaser(target.Center);
            }
            Vector2 dir = Projectile.Center.GetNormalVector2(target.Center);
            for (int i = 0; i < 26; i++)
            {
                ECSParticle.ShinyCrossStarSmall(target.Center.ToRandCirclePos(4), dir.ToRandVelocity(ToRadians(35), 0.1f, 22f), RandLerpColor(Color.SkyBlue, Color.White), 25, 1, 0.5f, 0);
            }
            for (int i = 0; i < 26; i++)
            {
                ECSParticle.SmokeParticle(target.Center.ToRandCirclePos(4), RandVelTwoPi(.2f, 16f), RandLerpColor(Color.SkyBlue, Color.LightSkyBlue), 40, RandRotTwoPi, 1, Main.rand.NextFloat(.9f, 1.1f) * .8f, true, blendstate: BlendState.Additive);
            }
            for (int i = 0; i < 20; i++)
            {
                ECSParticle.ShinyCrossStarECS(target.Center.ToRandCirclePos(4), dir.ToRandVelocity(ToRadians(35), 0.1f, 22f), RandLerpColor(Color.SkyBlue, Color.White), 25, 1, 0.85f, 0.2f);
            }
            for (int i = 0; i < 8; i++)
            {
                ECSParticle.HighResolutionThunder(target.Center.ToRandCirclePos(16), Vector2.Zero, RandLerpColor(Color.White, Color.LightSkyBlue), 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .86f, 3);
            }
            SoundEngine.PlaySound(LostbeltSoundAssets.Tlipoca_StoneBonk with { MaxInstances = 1, Pitch = -.3f, PitchVariance = .12f, Volume = .54f, Variants = [2] }, target.Center);
        }

        public void SpawnLaser(Vector2 tarCenter)
        {
            for (int i = -1; i < 2; i++)
            {
                if (i == 0)
                    continue;
                float radians = ToRadians(30f * i);
                Vector2 args = Projectile.Center.GetNormalVector2(Main.MouseWorld).RotatedBy(radians);
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), tarCenter, args * 20f, ProjectileType<TechBladeLaser>(), Projectile.originalDamage, Projectile.knockBack, Owner.whoAmI);
            }
        }

        public SpriteBatch SB { get => Main.spriteBatch; }
        public GraphicsDevice GD { get => Main.graphics.GraphicsDevice; }
        public BlendState BlendState => BlendState.Additive;
        public ScarletDrawLayer LayerToRenderTo => ScarletDrawLayer.BeforeDusts;
        public void RenderPixelated(SpriteBatch spriteBatch)
        {
            if (!FirstFrame)
                return;
            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            Vector2 topPos = drawPosition + (new Vector2(0.85f, -.18f * Projectile.spriteDirection)).RotatedBy(Projectile.rotation) * 70f * Projectile.scale;
            Texture2D glow = LostbeltModAssets.Particle_CrossGlow.Value;
            float scale = Projectile.scale * .52f;
            SB.Draw(glow, topPos, null, Color.DeepSkyBlue, 0, glow.Size() / 2, scale, 0, 0);
            SB.Draw(glow, topPos, null, Color.SkyBlue, 0, glow.Size() / 2, scale * .95f, 0, 0);
            SB.Draw(glow, topPos, null, Color.White * .75f, 0, glow.Size() / 2, scale * .66f, 0, 0);
            Texture2D texture = LostbeltModAssets.Texture_StandardGradient.Value;
            LostbeltMethods.ApplyAlphaCut(new Vector4(0.31f, 0.31f, 0, 0.3f), Vector2.Zero, Vector2.One);
            DrawSlash(texture, Color.White * 0.95f, 0.95f);
            DrawSlash(texture, Color.White * 0.60f, 0.55f);

            LostbeltMethods.ApplyAlphaCut(new Vector4(0.42f, 0.32f, 0, 0), new Vector2(-Main.GlobalTimeWrappedHourly * .935f, 0), new Vector2(2f), Color.SkyBlue);
            Texture2D texture2 = LostbeltModAssets.Noise_Misc.Value;
            DrawSlash(texture2, Color.SkyBlue * .95f, 0.90f);
            texture2 = LostbeltModAssets.Noise_Aura.Value;
            DrawSlash(texture2, Color.White * .80f, 0.55f);
            texture2 = LostbeltModAssets.Noise_WaterFlow.Value;
            DrawSlash(texture2, Color.SkyBlue * .65f, 0.70f);


            texture = LostbeltModAssets.Texture_SwordSlash.Value;
            LostbeltMethods.ApplyAlphaCut(new Vector4(0.41f, 0.31f, 0, .1f), Vector2.Zero, Vector2.One);
            DrawSlash(texture, Color.DeepSkyBlue * 0.95f, 0.95f);
            DrawSlash(texture, Color.LightSkyBlue * 0.70f, 0.50f);

            LostbeltMethods.EndShaderAreaPixel();
        }

        private List<ScarletVertex> _vertexCache = new List<ScarletVertex>(); // 类级别缓存
        public void DrawSlash(Texture2D texture, Color drawcolor, float mult = 0.8f)
        {
            if (OldAimPos.Count < 3)
                return;
            _vertexCache.Clear();
            List<ScarletVertex> Vertexlist = new List<ScarletVertex>();
            for (int i = 0; i < OldAimPos.Count; i++)
            {
                float progress = (float)i / OldAimPos.Count;
                Vector2 DrawPos_Head = OldAimPos[i] + Projectile.Center - Main.screenPosition;
                Vector2 DrawPos_Source = OldAimPos[i] * mult + Projectile.Center - Main.screenPosition;
                _vertexCache.Add(new ScarletVertex(DrawPos_Head, drawcolor * SlashOpa, new Vector3(progress, 0, 0)));
                _vertexCache.Add(new ScarletVertex(DrawPos_Source, drawcolor * SlashOpa, new Vector3(progress, 1, 0)));
            }
            GD.Textures[0] = texture;
            GD.SamplerStates[0] = SamplerState.PointWrap;
            GD.DrawUserPrimitives(PrimitiveType.TriangleStrip, _vertexCache.ToArray(), 0, _vertexCache.Count - 2);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (!FirstFrame)
                return false;
            Projectile.GetProjDrawInfo_Melee(out Texture2D tex, out Vector2 drawPosition, out float drawRotation, out Vector2 rotationPoint, out SpriteEffects flipSprite);
            PixelatedRenderManager.BeginDrawProj = true;
            for (int i = 0; i < 16; i++)
                SB.Draw(tex, drawPosition + (TwoPi / 16f * i).ToRotationVector2() * 1.5f, null, Color.LightSkyBlue.ToAddColor(), drawRotation, rotationPoint, Projectile.scale, flipSprite, 0);
            SB.Draw(tex, drawPosition, null, Color.White, drawRotation, rotationPoint, Projectile.scale, flipSprite, 0);

            return false;

        }
    }
}
