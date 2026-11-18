using LostbeltTraveler.Core.Primitives.Trail;
using LostbeltTraveler.Globals.Graphics.Particles;
using LostbeltTraveler.Globals.Handlers;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Requirement
{
    public class DragonStaff : ModItem, ILocalizedModType
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public new string LocalizationCategory => "Weapons.Magic";
        public override void SetDefaults()
        {
            Item.width = Item.height = 16;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 66;
            Item.rare = ItemRarityID.Red;
            Item.useTime = Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = null;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
            Item.knockBack = 4f;
            Item.shoot = ProjectileType<DragonStaffHeldProj>();
            Item.shootSpeed = 16;
        }
        public override bool CanUseItem(Player player)
        {
            return (player.ownedProjectileCounts[Item.shoot] < 1 && player.ownedProjectileCounts[ProjectileType<DragonStaffHeldProjAlt>()] < 1);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.direction = (Main.MouseWorld.X - player.Center.X > 0).ToDirectionInt();
            Vector2 ownerToSky = new Vector2(player.Center.X + 250 * player.direction, player.Center.Y) + new Vector2(0, -500) - player.Center;
            Vector2 skyDir = -(ownerToSky).ToSafeNormalize();
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            proj.originalDamage = damage;
            ((DragonStaffHeldProj)proj.ModProjectile).BeginTargetRotation = skyDir.ToRotation();
            ((DragonStaffHeldProj)proj.ModProjectile).Flip = 1;
            return false;
        }
    }
    public class DragonStaffFrieball : ModProjectile, ILocalizedModType
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public Player Owner => Main.player[Projectile.owner];
        public SpriteBatch SB
        {
            get => Main.spriteBatch;
        }
        public GraphicsDevice GD { get => Main.graphics.GraphicsDevice; }
        public ref float Timer => ref Projectile.ai[0];
        public int RandTimeLeft
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        public ref float RandMaxAngle => ref Projectile.ai[2];
        public bool EverytimeItBurst = false;
        public NPC CurTarget = null;
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(12);
        }
        public override void SetDefaults()
        {
            Projectile.extraUpdates = 1;
            Projectile.DamageType = DamageClass.Magic;
            //这里的体积故意这么大的
            Projectile.height = Projectile.width = 30;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 360;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }
        public override void OnKill(int timeLeft)
        {
            Vector2 dir = Projectile.SafeDirByRot();
            for (int i = 0; i < 9; i++)
            {
                Vector2 randDir = RandVelTwoPi(0.1f, 4.2f);
                Vector2 spawnpos = Projectile.Center.ToRandCirclePos(4f);
                Color color = Color.Lerp(Color.Lerp(Color.Orange, Color.Red, .5f), Color.Orange, Main.rand.NextFloat());
                Vector2 vel = randDir + dir * Main.rand.NextFloat(1f, 8f);
                new SmokeParticle(spawnpos, vel, color, 40, RandRotTwoPi, .35f, 0.30f * Main.rand.NextFloat(0.75f, 1.1f), true).SpawnToPriority();
                if (Main.rand.NextBool())
                {
                    vel = Projectile.oldVelocity.ToSafeNormalize() * Main.rand.NextFloat(-3f, 12f);
                    new SmokeParticle(spawnpos, vel, RandLerpColor(Color.Lerp(Color.Orange, Color.Red, 0.75f), Color.OrangeRed), 40, RandRotTwoPi, .35f, 0.30f * Main.rand.NextFloat(0.75f, 1.1f), true).SpawnToPriority();
                }
            }
            for (int j = 0; j < 10; j++)
            {
                Vector2 spawnPos = Projectile.Center.ToRandCirclePos(20f) + dir * Main.rand.NextFloat(0f, 6f);
                new ShinyCrossStar(spawnPos, dir * 12f * Main.rand.NextFloat(), RandLerpColor(Color.Orange, Color.OrangeRed), 50, RandRotTwoPi, 1, 0.7f, false).Spawn();
            }
        }
        public override void AI()
        {
            if (!Owner.HasProj<DragonStaffHeldProjAlt>())
            {
                Projectile.Kill();
                return;
            }
            Timer++;
            Projectile.rotation = Projectile.velocity.ToRotation();
            SetUpDustType();
            if (Timer < 40 * Projectile.MaxUpdates)
            {
                Projectile.AffactedByGrav(yMult: 1f, yAdd: .08f, maxGravSpeed: 18f);
                Projectile.timeLeft = RandTimeLeft;
                if (Math.Abs(Projectile.velocity.Y) < 5f)
                    Timer = 40 * Projectile.MaxUpdates;
                //提前返回，最主要的原因是为了尽量减少大缩进
                return;
            }

            if (CurTarget is null)
            {
                NPC target = Main.MouseWorld.FindClosestTarget(600);
                if (target.IsLegal())
                    CurTarget = target;
            }

            float speedValue = Clamp((12f + (Timer - 30f) / 5f) / 2f, 0f, 20f);
            float speed = Projectile.velocity.Length();
            Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitX).RotatedBy(ToRadians(5) * Main.rand.NextBool().ToDirectionInt()) * speed;
            if (CurTarget.IsLegal())
            {
                Projectile.HomingTarget(CurTarget.Center, -1f, speedValue, 15f, RandMaxAngle);
                Projectile.timeLeft = 150;
            }
            else
            {
                Projectile.HomingTarget(Main.MouseWorld, -1f, speedValue, 15f, RandMaxAngle);
                CurTarget = null;
            }
        }

        private void SetUpDustType()
        {
            if (Projectile.IsOutScreen())
                return;
            Color baseColor = Color.DarkOrange;
            Color targetColor = Color.OrangeRed;
            for (int i = 0; i < 2; i++)
            {
                if (Main.rand.NextBool(6))
                    new TurbulenceGlowOrb(Projectile.Center.ToRandCirclePosEdge(4f), 0.5f, Color.Lerp(baseColor, targetColor, Main.rand.NextFloat()), 20, Main.rand.NextFloat(0.1f, 0.12f), RandRotTwoPi).Spawn();
                if (Main.rand.NextBool(4))
                    new SmokeParticle(Projectile.Center.ToRandCirclePos(8f), -Projectile.velocity / 8f, Color.Lerp(baseColor, Color.DarkGray, Main.rand.NextFloat()), 20, RandRotTwoPi, .41f, Main.rand.NextFloat(0.12f, 0.16f) * 1.1f, true).SpawnToPriority();
            }
            Vector2 vel = Projectile.velocity.ToRandVelocity(ToRadians(10f), 0.8f, 1.4f);
            if (Main.rand.NextBool(3))
                new ShinyCrossStar(Projectile.Center.ToRandCirclePosEdge(4f), vel, Color.Lerp(baseColor, targetColor, Main.rand.NextFloat()), 40, RandRotTwoPi, 1f, 0.3f, ToRadians(10f)).Spawn();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.DD2_BetsyFlameBreath with { MaxInstances = 1, Pitch = -.5f, PitchVariance = .1f }, Projectile.Center);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (!Projectile.Lostbelt().FirstFrame)
                return false;
            DrawTrailFireballs();
            DrawShaderTrail();
            DrawMainFireballs();
            return false;
        }

        public void DrawTrailFireballs()
        {
            Texture2D projTex = LostbeltModAssets.Particle_ShinyOrbHard.Value;
            Vector2 ori = projTex.Size() / 2f;
            //绘制残影，这里的残影更多是为了一个“打底图”的作用
            float oriScale = 0.64f * Projectile.scale;
            float scale = 1f;
            int length = (int)(8 * Projectile.Opacity);
            for (int i = 0; i < length; i++)
            {
                scale *= 0.975f;
                float rads = (float)i / length;
                float miscRtaios = Clamp(Projectile.velocity.Length(), 0f, 1f) * (1 - rads) * Projectile.Opacity;
                Color edgeColor = Color.Lerp(Color.DarkOrange, Color.OrangeRed, 1 - rads).ToAddColor(10) * miscRtaios;

                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], 0.4f) - Main.screenPosition + Projectile.Size / 2;
                float rot = Lerp(Projectile.oldRot[i], Projectile.oldRot[0], 1f);

                SB.Draw(projTex, lerpPos, null, edgeColor, rot, ori, oriScale * scale * Projectile.Opacity, 0, 0);

                edgeColor = Color.Lerp(Color.Orange, Color.White, 1 - rads).ToAddColor(10) * miscRtaios;
                SB.Draw(projTex, lerpPos, null, edgeColor * 0.25f, rot, ori, oriScale * scale * Projectile.Opacity * 0.75f, 0, 0);
            }
        }
        public void DrawShaderTrail()
        {
            SB.EnterShaderArea();
            DrawTrails(LostbeltModAssets.Trail_ManaStreakTiny.Texture, Color.DarkOrange);
            DrawTrails(LostbeltModAssets.Trail_ManaStreakTiny.Texture, Color.Red, 0.75f);
            DrawTrails(LostbeltModAssets.Trail_ManaStreak.Texture, Color.OrangeRed, 0.25f);
            SB.EndShaderArea();
        }
        public void DrawTrails(Asset<Texture2D> useTex, Color drawColor, float multipleSize = 1f, float alphaValue = 1f, float offsetHeight = 1f)
        {
            if (Projectile.oldPos.Length < 3)
                return;
            Effect shader = LostbeltShaderAssets.StandardFlowShader;
            float laserLength = 50;
            shader.Parameters["LaserTextureSize"].SetValue(useTex.Size());
            shader.Parameters["targetSize"].SetValue(new Vector2(laserLength, useTex.Height()));
            shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * -1.2f);
            shader.Parameters["uColor"].SetValue(drawColor.ToVector4() * alphaValue);
            shader.Parameters["uFadeoutLength"].SetValue(0.8f);
            shader.Parameters["uFadeinLength"].SetValue(0.1f);
            shader.CurrentTechnique.Passes[0].Apply();

            DrawSetting drawSetting = new(useTex.Value);
            List<TrailDrawDate> trailDrawDates = [];
            int posCount = (int)(Projectile.oldPos.Length * Projectile.Opacity);
            for (int j = 0; j < posCount - 1; j++)
            {
                if (Projectile.oldPos[j] != Vector2.Zero)
                {
                    Vector2 drawPos = Projectile.oldPos[j] + new Vector2(Projectile.width / 2, Projectile.height / 2);
                    trailDrawDates.Add(new(drawPos, drawColor, new Vector2(0, 30 * multipleSize * Projectile.scale), Projectile.oldRot[j]));
                }
            }
            TrailRender.RenderTrail([.. trailDrawDates], drawSetting);
        }
        public void DrawMainFireballs()
        {
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 projPos, out Vector2 ori);
            float ballScale = 0.64f * Projectile.Opacity * Projectile.scale;
            SB.Draw(projTex, projPos, null, Color.DarkOrange.ToAddColor(105) * Projectile.Opacity, Projectile.rotation, ori, ballScale, 0, 0);
            SB.Draw(projTex, projPos, null, Color.White.ToAddColor(0) * Projectile.Opacity, Projectile.rotation, ori, ballScale * 0.75f, 0, 0);
        }


    }
    public class DragonStaffHeldProjAlt : ModProjectile, ILocalizedModType
    {
        public override bool IsLoadingEnabled(Mod mod) => false;

        public override string Texture => GetInstance<DragonStaffHeldProj>().Texture;
        public Player Owner => Main.player[Projectile.owner];
        public SpriteBatch SB { get => Main.spriteBatch; }
        public GraphicsDevice GD { get => Main.graphics.GraphicsDevice; }
        //这个攻速不要轻易低于20帧，不然……
        public int AttackSpeed => Owner.ApplyWeaponAttackSpeed(Owner.HeldItem, Owner.HeldItem.useTime * Projectile.MaxUpdates, 20 * Projectile.MaxUpdates);
        public ref float Timer => ref Projectile.ai[0];
        public ref float ShootTimer => ref Projectile.ai[1];
        public int CurShootTime
        {
            get => (int)Projectile.ai[2];
            set => Projectile.ai[2] = (float)value;
        }
        public ref float AnimationHandler => ref Projectile.localAI[0];
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = Projectile.height = 50;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 5;
            Projectile.noEnchantmentVisuals = true;
            Projectile.timeLeft = 10000;
            Projectile.netImportant = true;
            Projectile.friendly = true;
        }
        public float EdgeValue = 0;
        public override void AI()
        {
            Projectile.velocity = Projectile.rotation.ToRotationVector2();
            EdgeValue = Lerp(EdgeValue, 1f, .1f / Projectile.MaxUpdates);
            HandleProjAttack();
            HandleParticle();
            HandlePlayerState();
            HandlePlayerHealing();
            HandleProjStatement();
        }
        public void HandleProjAttack()
        {

            Timer++;
            if (Timer > (AttackSpeed / 10))
            {
                Vector2 dir = Projectile.rotation.ToRotationVector2();
                Vector2 posBase = Projectile.Center + dir * 95f * Projectile.scale - dir.RotatedBy(PiOver2) * 25f * Projectile.scale + dir.RotatedByRandom(TwoPi) * Main.rand.NextFloat(0f, 2.4f);
                SoundEngine.PlaySound(LostbeltSoundAssets.Misc_AirFlowAlt with { MaxInstances = 1, Pitch = -0.7f, PitchVariance = .12f, Volume = 0.3f });
                NPC target = LostbeltMethods.FindClosestTarget(Main.MouseWorld, 600);
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), posBase, dir.ToRandVelocity(ToRadians(45f), 15f), ProjectileType<DragonStaffFrieball>(), Projectile.originalDamage, Projectile.knockBack, Owner.whoAmI);
                proj.ai[1] = Main.rand.Next(50, 300);
                proj.ai[2] = Main.rand.NextFloat(4.5f, 7.5f);
                if (target.IsLegal())
                    ((DragonStaffFrieball)proj.ModProjectile).CurTarget = target;
                Timer = 0;
            }
        }
        public void HandleParticle()
        {
            Vector2 dir = Projectile.rotation.ToRotationVector2();
            if (Main.rand.NextBool(6))
            {
                Vector2 posBase = Projectile.Center + dir * 95f * Projectile.scale - dir.RotatedBy(PiOver2) * 25f * Projectile.scale + dir.RotatedByRandom(TwoPi) * Main.rand.NextFloat(0f, 2.4f);
                posBase += Projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.PiOver2) * Main.rand.NextFloat(-35f, 37f) + dir * Main.rand.NextFloat(-60f, 60f);
                if (Main.rand.NextBool(3))
                {
                    Color fireColor = Color.Lerp(Color.Lerp(Color.DarkOrange, Color.OrangeRed, Main.rand.NextFloat()), Color.WhiteSmoke, Main.rand.NextFloat());
                    new Fire(posBase, -Vector2.UnitY * Main.rand.NextFloat(0.4f, 24f), fireColor, Main.rand.Next(35, 50), RandRotTwoPi, 0.57f, Main.rand.NextFloat(.9f, 1.1f) * .55f).SpawnToPriority();
                }
                else
                {
                    Color fireColor = Color.Lerp(Color.Lerp(Color.DarkOrange, Color.OrangeRed, Main.rand.NextFloat()), Color.WhiteSmoke, Main.rand.NextFloat());
                    new SmokeParticle(posBase, -Vector2.UnitY * Main.rand.NextFloat(0.4f, 24f), fireColor, Main.rand.Next(35, 50), RandRotTwoPi, 0.57f, Main.rand.NextFloat(.9f, 1.1f) * .75f, Main.rand.NextBool()).SpawnToPriority();
                }
            }
            if (Main.rand.NextBool(6))
            {
                Vector2 posBase = Projectile.Center + dir * 95f * Projectile.scale - dir.RotatedBy(PiOver2) * 25f * Projectile.scale + dir.RotatedByRandom(TwoPi) * Main.rand.NextFloat(0f, 2.4f);
                posBase += Projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.PiOver2) * Main.rand.NextFloat(-35f, 37f) + dir * Main.rand.NextFloat(-60f, 60f);
                Dust d = Dust.NewDustPerfect(posBase, DustID.Torch);
                d.scale *= Main.rand.NextFloat(1.1f, 1.4f);
                d.velocity = -Vector2.UnitY * Main.rand.NextFloat(0.4f, 24f);
                d.noGravity = true;
            }
            if (Main.rand.NextBool(4))
            {
                Vector2 posBase = Projectile.Center + dir * 95f * Projectile.scale - dir.RotatedBy(PiOver2) * 25f * Projectile.scale + dir.RotatedByRandom(TwoPi) * Main.rand.NextFloat(0f, 2.4f);
                Vector2 setPos = posBase + Projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.PiOver2) * Main.rand.NextFloat(-35f, 37f) + dir * Main.rand.NextFloat(-60f, 60f);
                Vector2 vel = -Vector2.UnitY.RotatedByRandom(ToRadians(15)) * Main.rand.NextFloat(4f, 12f);
                float offsetX = setPos.X - posBase.X;

                bool needMirror = (offsetX > 0 && vel.X < 0) || (offsetX < 0 && vel.X > 0);
                if (needMirror)
                    setPos.X = posBase.X - offsetX;
                new ShinyCrossStar(setPos, vel, Color.Lerp(Color.DarkOrange, Color.OrangeRed, Main.rand.NextFloat()), 40, vel.ToRotation(), 1f, Main.rand.NextFloat(0.8f, 1.2f) * 0.8f, false).SpawnToPriority();
            }
        }
        public void HandlePlayerState()
        {
            Owner.ControlPlayerArm(Projectile.rotation);
            int dir = (Main.MouseWorld.X > Owner.Center.X).ToDirectionInt();
            Owner.ChangeDir(dir);
            Owner.heldProj = Projectile.whoAmI;
            Owner.itemAnimation = Owner.itemTime = 2;
            Projectile.Center = Owner.MountedCenter;
            Projectile.position.Y += Owner.gfxOffY;
            bool ifStillUse = (Main.mouseLeft || Owner.controlUseTile) && !Owner.noItems && !Owner.CCed;
            if (ifStillUse)
            {
                Projectile.timeLeft = 2;
            }
            else
                Projectile.Kill();
        }
        public void HandlePlayerHealing()
        {
            //仅仅用于计时器，不用于别的
            Projectile.frameCounter++;
            bool hasLife = Owner.statLife < Owner.statLifeMax2;
            bool hasMana = Owner.statMana < Owner.statManaMax2;
            bool drawSmoke = Main.rand.NextBool(13) && (hasLife || hasMana);
            if (drawSmoke)
                new SmokeParticle(Owner.ToRandRec(), -Vector2.UnitY, RandLerpColor(Color.DarkOrange, Color.WhiteSmoke), 40, RandRotTwoPi, 0.4f, Main.rand.NextFloat(.5f, .7f), true).Spawn();
            if (Projectile.frameCounter < Projectile.MaxUpdates * 20)
                return;
            Projectile.frameCounter = 0;
            if (hasLife)
            {
                Owner.Heal(1);
            }
            if (hasMana)
            {
                Owner.statMana += 1;
                Owner.ManaEffect(1);
            }
        }
        public void HandleProjStatement()
        {
            float mirroredX = Owner.Center.X * 2 - Owner.ToClampMouseVector2().X;
            float finalX = MathHelper.Lerp(mirroredX, Owner.Center.X, 0.70f);
            Vector2 ownerToSky = new Vector2(finalX, Owner.Center.Y - 500) - Owner.Center;
            Vector2 skyDir = (ownerToSky).ToSafeNormalize();
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, skyDir, AnimationHandler);
            // 修正旋转跳变
            float targetRot = Projectile.velocity.ToRotation();
            float currentRot = Projectile.rotation;
            float delta = WrapAngle(targetRot - currentRot);
            Projectile.rotation = currentRot + delta;
            if (Projectile.numUpdates == 0)
            {
                AnimationHandler += 0.01f;
                if (AnimationHandler >= .05f)
                    AnimationHandler = .05f;
            }
        }

        public override bool ShouldUpdatePosition() => false;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            //基础的一些数据
            Texture2D tex = Projectile.GetTexture();
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            float drawRot = Projectile.rotation + PiOver4 + (Projectile.spriteDirection == -1).ToInt() * PiOver2;
            Vector2 drawPoint = new Vector2(tex.Width * (Projectile.spriteDirection == -1).ToInt(), tex.Height);
            Vector2 realDrawPos = drawPos + Vector2.UnitX.RotatedBy(Projectile.rotation) * -40 + Vector2.UnitX.RotatedBy(Projectile.rotation).RotatedBy(PiOver2) * 0;
            SpriteEffects se = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            for (int i = 0; i < 16; i++)
                SB.Draw(tex, realDrawPos + ToRadians(60f * i).ToRotationVector2() * 1.5f * EdgeValue, null, Color.White.ToAddColor(), drawRot, drawPoint, Projectile.scale, se, 0);
            SB.Draw(tex, realDrawPos, null, Color.White, drawRot, drawPoint, Projectile.scale, se, 0);
            DrawGlowCenter(realDrawPos);
            return false;
        }
        public void DrawGlowCenter(Vector2 realDrawPos)
        {
            Texture2D star = LostbeltModAssets.Particle_CrossGlow.Value;
            SB.EnterShaderArea();
            Vector2 dir = Projectile.rotation.ToRotationVector2();
            Vector2 pos = realDrawPos + dir * 125f * Projectile.scale - dir.RotatedBy(PiOver2) * 25f * Projectile.scale;
            float scale = Projectile.scale * .255f;
            SB.Draw(star, pos, null, Color.OrangeRed * .9f, 0, star.ToOrigin(), scale * 0.95f, 0, 0);
            SB.Draw(star, pos, null, Color.Orange * .9f, 0, star.ToOrigin(), scale * .90f, 0, 0);
            SB.Draw(star, pos, null, Color.LightCoral * .85f, 0, star.ToOrigin(), scale * 0.85f, 0, 0);
            SB.EndShaderArea();
            SB.EndShaderArea();

        }
    }
    public class DragonStaffHeldProj : ModProjectile, ILocalizedModType
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public Player Owner => Main.player[Projectile.owner];
        public SpriteBatch SB { get => Main.spriteBatch; }
        public GraphicsDevice GD { get => Main.graphics.GraphicsDevice; }
        public bool PostFirstFrame = false;
        public AnimationStruct Helper = new(3);
        public float BeginTargetRotation = 0;
        public float TargetRotation = 0;
        public int Flip = 1;
        public float CurRotation = 0;

        public int AttackSpeed => Owner.ApplyWeaponAttackSpeed(Owner.HeldItem, Owner.HeldItem.useTime * Projectile.extraUpdates, 5 * Projectile.extraUpdates);
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = Projectile.height = 50;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 5;
            Projectile.scale = 1;
            Projectile.noEnchantmentVisuals = true;
            Projectile.timeLeft = 10000;
            Projectile.netImportant = true;
            Projectile.friendly = true;
        }
        public void InitFirstFrame()
        {
            if (PostFirstFrame)
                return;
            PostFirstFrame = true;
            Helper.MaxProgress[0] = (int)(AttackSpeed * 1f);
            Helper.MaxProgress[1] = (int)(AttackSpeed * 1.2f);
            Helper.MaxProgress[2] = (int)(AttackSpeed * 0.3f);
            TargetRotation = BeginTargetRotation;
        }

        public override void AI()
        {
            InitFirstFrame();
            Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitX);
            HandleHeldProjState();
            HandleAttackAnimation();
            HandlePlayerState();

        }
        public void HandleAttackAnimation()
        {
            if (!Helper.IsDone[0])
                UpdateBeginAnimation();
            else if (!Helper.IsDone[1])
                UpdateMidAnimation();
            else
            {
                if (!Owner.HasProj<DragonStaffHeldProjAlt>() && Main.mouseLeft)
                {
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ProjectileType<DragonStaffHeldProjAlt>(), Projectile.originalDamage, Projectile.knockBack, Projectile.owner);
                    proj.rotation = Projectile.rotation;
                    proj.ai[0] = 9999;
                }
                Projectile.Kill();
            }
        }

        public void UpdateBeginAnimation()
        {
            if (Helper.GetAniProgress(0) == 0)
            {
                SoundEngine.PlaySound(SoundID.DD2_BetsyFlameBreath with { MaxInstances = 1, Pitch = .82f });
            }
            //这里挥砍动画一定程度上使用了矩阵变化。
            float easedProgress = EasingHelper.EaseOutBack(Helper.GetAniProgress(0));
            //末尾角度，也是下一个动画进程的起始角度
            float endAngle = -215f * Flip;
            float beginAngle = -105f * Flip;
            //更新当前的转角
            float rot = Helper.UpdateAngle(beginAngle, endAngle, Owner.direction, easedProgress);
            //将其投影到矩阵上，并进行形变
            Matrix tForm = Matrix.CreateRotationZ(rot) * Matrix.CreateScale(1f, 1, 1f);
            //而后再转化为射弹的目标指向，这个tarPos同时拥有指向和武器模长的信息。而不是一个单位向量
            Vector2 tarPos = Vector2.Transform(Vector2.UnitX, tForm) * 1f;
            //这样，Scale就是tarPos的向量模长
            Projectile.scale = tarPos.Length();
            //武器的角度为（起始角度 + 目标角度）的值
            Projectile.rotation = tarPos.ToRotation() + TargetRotation;
            HandleParticleBegin();
            //更新动画进程，封装的方法
            Helper.UpdateAniState(0);

        }
        private void HandleParticleBegin()
        {
            Vector2 dir = Projectile.rotation.ToRotationVector2();
            Vector2 posBase = Projectile.Center + dir * 95f * Projectile.scale - dir.RotatedBy(PiOver2) * 25f * Projectile.scale;
            Vector2 starShapeSpawnPos = posBase + Main.rand.NextFloat(TwoPi).ToRotationVector2() * Main.rand.NextFloat(120f, 190f);
            Vector2 starShapeDir = (posBase - starShapeSpawnPos).SafeNormalize(Vector2.UnitX);
            if (Main.rand.NextBool(7))
            {
                Vector2 vel = starShapeDir * Main.rand.NextFloat(4f, 9);
                float scale = Main.rand.NextFloat(0.7f, 0.91f) * Projectile.scale;
                new StarShape(starShapeSpawnPos, vel, RandLerpColor(Color.DarkOrange, Color.OrangeRed), scale, 40, true).Spawn();
            }
            if (Main.rand.NextBool(5))
            {
                starShapeSpawnPos = posBase + Main.rand.NextFloat(TwoPi).ToRotationVector2() * Main.rand.NextFloat(120f, 190f);
                starShapeDir = (posBase - starShapeSpawnPos).SafeNormalize(Vector2.UnitX);
                new ShinyCrossStar(starShapeSpawnPos, starShapeDir * Main.rand.NextFloat(0.8f, 8f), Color.Lerp(Color.DarkOrange, Color.OrangeRed, Main.rand.NextFloat()), 40, 0, 1, 0.8f * Main.rand.NextFloat(0.8f, 1.1f), false).Spawn();
            }
            if (Main.rand.NextBool(3))
            {
                starShapeSpawnPos = posBase + Main.rand.NextFloat(TwoPi).ToRotationVector2() * Main.rand.NextFloat(100f, 160f);
                starShapeDir = (posBase - starShapeSpawnPos).SafeNormalize(Vector2.UnitX);
                new SmokeParticle(starShapeSpawnPos, starShapeDir * Main.rand.NextFloat(2f, 12f), RandLerpColor(RandLerpColor(Color.DarkOrange, Color.OrangeRed), Color.White), 45, RandRotTwoPi, 0.35f, Main.rand.NextFloat(0.4f, 0.7f), true).SpawnToPriority();
            }
        }

        public void UpdateMidAnimation()
        {
            //这里挥砍动画一定程度上使用了矩阵变化。
            if (Helper.GetAniProgress(1) == 0)
            {
                SoundEngine.PlaySound(LostbeltSoundAssets.Misc_AirCharge with { MaxInstances = 1, Pitch = -.2f });
                HandleMidParticleInit();
            }
            float easedProgress = EasingHelper.EaseOutBack(Helper.GetAniProgress(1));
            //末尾角度，也是下一个动画进程的起始角度
            float endAngle = 160f * Flip;
            float beginAngle = -215f * Flip;
            //更新当前的转角
            float rot = Helper.UpdateAngle(beginAngle, endAngle, Owner.direction, easedProgress);
            //将其投影到矩阵上，并进行形变
            Matrix tForm = Matrix.CreateRotationZ(rot) * Matrix.CreateScale(1f, 1, 1f);
            //而后再转化为射弹的目标指向，这个tarPos同时拥有指向和武器模长的信息。而不是一个单位向量
            Vector2 tarPos = Vector2.Transform(Vector2.UnitX, tForm) * 1f;
            //这样，Scale就是tarPos的向量模长
            Projectile.scale = tarPos.Length();
            //武器的角度为（起始角度 + 目标角度）的值
            Projectile.rotation = tarPos.ToRotation() + TargetRotation;
            //更新动画进程，封装的方法
            HandleMidParticle();
            HandlePlayerHealing();
            Helper.UpdateAniState(1);

        }

        private void HandleMidParticle()
        {
            Vector2 dir = Projectile.rotation.ToRotationVector2();
            if (Main.rand.NextBool(5))
            {
                //这里代码基本上是没法省的，不用看了
                //这里基本上是为了对准法杖中心的位置，直接的硬编码
                Vector2 posBase = Projectile.Center + dir * 95f * Projectile.scale - dir.RotatedBy(PiOver2) * 25f * Projectile.scale + dir.RotatedByRandom(TwoPi) * Main.rand.NextFloat(0f, 2.4f);
                posBase += dir.RotatedBy(MathHelper.PiOver2) * Main.rand.NextFloat(-15f, 16f);
                new SmokeParticle(posBase, -Vector2.UnitY * Main.rand.NextFloat(0.4f, 24f), RandLerpColor(RandLerpColor(Color.DarkOrange, Color.OrangeRed), Color.WhiteSmoke), Main.rand.Next(35, 50), RandRotTwoPi, 0.5f, Main.rand.NextFloat(.9f, 1.1f) * .75f, true).SpawnToPriority();
            }
            if (Main.rand.NextBool(4))
            {
                Vector2 posBase = Projectile.Center + dir * 95f * Projectile.scale - dir.RotatedBy(PiOver2) * 25f * Projectile.scale + dir.RotatedByRandom(TwoPi) * Main.rand.NextFloat(0f, 2.4f);
                posBase += dir.RotatedBy(MathHelper.PiOver2) * Main.rand.NextFloat(-35f, 37f) + dir * Main.rand.NextFloat(-60f, 60f);
                Dust d = Dust.NewDustPerfect(posBase, DustID.Torch);
                d.scale *= Main.rand.NextFloat(1.1f, 1.4f);
                d.velocity = -Vector2.UnitY * Main.rand.NextFloat(0.4f, 24f);
                d.noGravity = true;
            }
            if (Main.rand.NextBool(4))
            {
                Vector2 posBase = Projectile.Center + dir * 95f * Projectile.scale - dir.RotatedBy(PiOver2) * 25f * Projectile.scale + dir.RotatedByRandom(TwoPi) * Main.rand.NextFloat(0f, 2.4f);
                Vector2 setPos = posBase + Projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.PiOver2) * Main.rand.NextFloat(-35f, 37f) + dir * Main.rand.NextFloat(-60f, 60f);
                Vector2 vel = -Vector2.UnitY.RotatedByRandom(ToRadians(15)) * Main.rand.NextFloat(4f, 12f);
                //计算水平偏移量
                float offsetX = setPos.X - posBase.X;
                //判断是否需要镜像
                bool needMirror = (offsetX > 0 && vel.X < 0) || (offsetX < 0 && vel.X > 0);
                if (needMirror)
                {
                    //镜像：偏移量取反，保持绝对距离一致
                    setPos.X = posBase.X - offsetX;
                }
                //这样，我们就能确保火星永远向外展开
                new ShinyCrossStar(setPos, vel, RandLerpColor(Color.DarkOrange, Color.OrangeRed), 40, vel.ToRotation(), 1f, Main.rand.NextFloat(0.8f, 1.2f) * 0.8f, Main.rand.NextBool()).Spawn();
            }
        }

        public void HandleMidParticleInit()
        {
            Vector2 dir = Projectile.rotation.ToRotationVector2();
            Vector2 posBase = Projectile.Center + dir * 95f * Projectile.scale - dir.RotatedBy(PiOver2) * 25f * Projectile.scale + dir.RotatedByRandom(TwoPi) * Main.rand.NextFloat(0f, 2.4f);
            for (int i = 0; i < 40; i++)
            {
                Color color = Color.Lerp(Color.Lerp(Color.DarkOrange, Color.OrangeRed, Main.rand.NextFloat()), Color.WhiteSmoke, Main.rand.NextFloat());
                Vector2 smokeVel = dir.RotatedByRandom(TwoPi) * Main.rand.NextFloat(0.4f, 24f);
                new SmokeParticle(posBase, smokeVel, color, Main.rand.Next(35, 50), RandRotTwoPi, 0.5f, Main.rand.NextFloat(.9f, 1.1f) * .75f, true).SpawnToPriority();
            }
            for (int i = 0; i < 30; i++)
            {
                Vector2 pos = posBase + RandVelTwoPi(20f, 80f);
                Vector2 vel = Main.rand.NextFloat(TwoPi).ToRotationVector2() * Main.rand.NextFloat(1.2f, 26.4f);
                float scale = Main.rand.NextFloat(0.4f, 0.9f) * .20f;
                new HRShinyOrb(pos, vel, Color.Lerp(Color.DarkOrange, Color.OrangeRed, Main.rand.NextFloat()), 45, scale).Spawn();
                new HRShinyOrb(pos, vel, Color.White, 45, scale * .75f).Spawn();
                Dust d = Dust.NewDustPerfect(pos, DustID.Torch, RandVelTwoPi(0.2f, 3.1f));
                d.scale *= 1.3f;
            }
            for (int i = 0; i < 30; i++)
            {
                Vector2 nextPos = posBase + RandVelTwoPi(10f, 30f);
                new StarShape(nextPos, (nextPos - posBase).ToRandVelocity(ToRadians(10f), 8f, 18f), RandLerpColor(Color.OrangeRed, Color.DarkOrange), Main.rand.NextFloat(0.95f, 1.27f) * 1.1f, 45, true).Spawn();
            }
        }
        public void HandlePlayerHealing()
        {
            //仅仅用于计时器，不用于别的
            Projectile.frameCounter++;
            if (Main.rand.NextBool(18))
                new SmokeParticle(Owner.ToRandRec(), -Vector2.UnitY, RandLerpColor(Color.DarkOrange, Color.OrangeRed), 40, RandRotTwoPi, 0.4f, Main.rand.NextFloat(.5f, .7f), true).Spawn();
            if (Projectile.frameCounter < Projectile.MaxUpdates * 5)
                return;
            Projectile.frameCounter = 0;
            Owner.Heal(1);
            if (Owner.statMana < Owner.statManaMax2)
                Owner.statMana += 1;
            Owner.ManaEffect(1);
        }

        public void UpdateEndAnimation()
        {
            //这里挥砍动画一定程度上使用了矩阵变化。
            float easedProgress = EasingHelper.EaseInOutExpo(Helper.GetAniProgress(2));
            //末尾角度，也是下一个动画进程的起始角度
            float endAngle = 160f * Flip;
            float beginAngle = 150f * Flip;
            //更新当前的转角
            float rot = Helper.UpdateAngle(beginAngle, endAngle, Owner.direction, easedProgress);
            //将其投影到矩阵上，并进行形变
            Matrix tForm = Matrix.CreateRotationZ(rot) * Matrix.CreateScale(1f, 1, 1f);
            //而后再转化为射弹的目标指向，这个tarPos同时拥有指向和武器模长的信息。而不是一个单位向量
            Vector2 tarPos = Vector2.Transform(Vector2.UnitX, tForm) * 1f;
            //这样，Scale就是tarPos的向量模长
            Projectile.scale = tarPos.Length();
            //武器的角度为（起始角度 + 目标角度）的值
            Projectile.rotation = tarPos.ToRotation() + TargetRotation;
            //更新动画进程，封装的方法
            CurRotation = Projectile.rotation;
            Helper.UpdateAniState(2);

        }
        public void HandleHeldProjState()
        {
            Projectile.Center = Owner.MountedCenter;
            Owner.itemTime = 2;
            Owner.itemAnimation = 2;
            Owner.heldProj = Projectile.whoAmI;
            if (Owner.dead)
                Projectile.Kill();
            else
                Projectile.timeLeft = 2;
        }
        public void HandlePlayerState()
        {
            Projectile.velocity = TargetRotation.ToRotationVector2();
            Owner.SetPlayerArm(Projectile.rotation);
        }
        public override bool ShouldUpdatePosition() => false;
        public override bool? CanDamage() => false;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            //基础的一些数据
            Texture2D tex = Projectile.GetTexture();
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            float drawRot = Projectile.rotation + PiOver4 + (Projectile.spriteDirection == -1).ToInt() * PiOver2;
            Vector2 drawPoint = new Vector2(tex.Width * (Projectile.spriteDirection == -1).ToInt(), tex.Height);
            Vector2 realDrawPos = drawPos + Vector2.UnitX.RotatedBy(Projectile.rotation) * -40 + Vector2.UnitX.RotatedBy(Projectile.rotation).RotatedBy(PiOver2) * 0;
            SpriteEffects se = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            //绘制法杖本身
            SB.Draw(tex, realDrawPos, null, Color.White, drawRot, drawPoint, Projectile.scale, se, 0);
            DrawGlowCenter(realDrawPos);
            return false;
        }
        public void DrawGlowCenter(Vector2 drawPos)
        {
            Texture2D star = LostbeltModAssets.Particle_CrossGlow.Value;
            SB.End();
            SB.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Vector2 dir = Projectile.rotation.ToRotationVector2();
            float easedProgress = EasingHelper.EaseOutCubic(Helper.GetAniProgress(0));
            Vector2 pos = drawPos + dir * 125f * Projectile.scale - dir.RotatedBy(PiOver2) * 25f * Projectile.scale;
            float scale = Projectile.scale * .255f * easedProgress;
            SB.Draw(star, pos, null, Color.OrangeRed * .9f, 0, star.ToOrigin(), scale * 0.95f, 0, 0);
            SB.Draw(star, pos, null, Color.Orange * .9f, 0, star.ToOrigin(), scale * .90f, 0, 0);
            SB.Draw(star, pos, null, Color.LightCoral * .85f, 0, star.ToOrigin(), scale * 0.85f, 0, 0);
            //出于我不清楚的原因，这里得重置两次批次，才能确保正常
            SB.End();
            SB.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            SB.End();
            SB.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}
