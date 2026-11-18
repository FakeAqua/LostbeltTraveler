using LostbeltTraveler.Core.PixelatedRender;
using LostbeltTraveler.Core.Primitives.Trail;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Graphics.Particles;
using LostbeltTraveler.Globals.Handlers;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Requirement
{
    public class DragonSword : ModItem, ILocalizedModType
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public new string LocalizationCategory => "Weapons.Magic";
        public override void SetDefaults()
        {
            Item.width = Item.height = 16;
            Item.damage = 66;
            Item.DamageType = DamageClass.Melee;
            Item.rare = ItemRarityID.Red;
            Item.useTime = Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = null;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
            Item.knockBack = 4f;
            Item.shoot = ProjectileType<DragonSwordHeldProj>();
            Item.shootSpeed = 16;
        }
        public override bool MeleePrefix() => true;
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 dir = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            ((DragonSwordHeldProj)proj.ModProjectile).BeginTargetRotation = dir.ToRotation();
            ((DragonSwordHeldProj)proj.ModProjectile).Flip = true;
            return false;
        }
    }
    /// <summary>
    /// 感谢Demon Lilies的技术支持
    /// </summary>
    public class DragonSwordHeldProj : LostbeltProjs
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public override string Texture => GetInstance<DragonSword>().Texture;
        public bool PostFirstFrame = false;
        public AnimationStruct Helper = new(3);
        public float BeginTargetRotation = 0;
        public float TargetRotation = 0;
        public int HeavySwordLength = 140;
        public bool Flip = false;
        public float Height = .81f;
        public bool KeepSpawning = false;
        public int AttackSpeed => Owner.ApplyWeaponAttackSpeed(Owner.HeldItem, Owner.HeldItem.useTime * Projectile.extraUpdates, 5 * Projectile.extraUpdates);
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(16);
        }
        public override void ExSD()
        {
            //这里的数值无所谓，后面会手动接管这个东西的hitbox的
            Projectile.width = Projectile.height = 50;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 5;
            Projectile.noEnchantmentVisuals = true;
            Projectile.timeLeft = 10000;
            Projectile.netImportant = true;

        }
        public override bool ShouldUpdatePosition() => false;
        public void SetFirstFrame()
        {
            if (PostFirstFrame)
                return;
            PostFirstFrame = true;
            if (KeepSpawning)
            {
                Helper.MaxProgress[0] = (int)(AttackSpeed * .40f);
            }
            else
            {
                Helper.MaxProgress[0] = AttackSpeed;
            }
            Helper.MaxProgress[1] = (int)(AttackSpeed * .30f);
            Helper.MaxProgress[2] = (int)(AttackSpeed * .10f);
            TargetRotation = BeginTargetRotation;
            HeavySwordLength = 140;
        }
        public override void AI()
        {
            SetFirstFrame();
            Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitX);
            HandleHeldProjState();
            HandleAttackAnimation();
            HandlePlayerState();

        }
        public override void OnKill(int timeLeft)
        {
            if (Main.mouseLeft)
            {
                //挥舞结束的时候处死并立刻生成新的射弹。这样我们不用重置大部分的动画进程，实现起来稍微方便点
                if (!Flip)
                {
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner);
                    //改向
                    ((DragonSwordHeldProj)proj.ModProjectile).Flip = true;
                    //存储当前挥舞角度
                    ((DragonSwordHeldProj)proj.ModProjectile).BeginTargetRotation = TargetRotation;
                    //如果玩家试图继续攻击，标记这个
                    //这样我们会跳过武器挥砍动画的最后阶段，并减少下一次挥舞的前摇，以提升手感和攻速
                    //如果需要做平衡性调整，我不建议直接改对这个下手，因为其直接影响了这把武器的一些特效方面的东西，除非你知道你自己在做什么
                    //如果真的需要做平衡，直接改物品的攻速就行了。
                    ((DragonSwordHeldProj)proj.ModProjectile).KeepSpawning = true;
                }
                else
                {
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner);
                    ((DragonSwordHeldProj)proj.ModProjectile).Flip = false;
                    ((DragonSwordHeldProj)proj.ModProjectile).BeginTargetRotation = TargetRotation;
                    ((DragonSwordHeldProj)proj.ModProjectile).KeepSpawning = true;
                }
            }
        }

        public void HandlePlayerState()
        {
            Projectile.velocity = TargetRotation.ToRotationVector2();
            Owner.ChangeDir(Projectile.direction);
            Owner.SetPlayerArm(Projectile.rotation);
        }

        public void HandleAttackAnimation()
        {
            //跳过末尾动画，如果玩家试图一直攻击的话
            if (!Helper.IsDone[0])
                UpdateBeginAnimation();
            else if (!Helper.IsDone[1])
                UpdateMidAnimation();
            else if (!Helper.IsDone[2] && KeepSpawning)
                UpdateEndAnimation();
            else
                Projectile.Kill();
        }
        public void UpdateBeginAnimation()
        {
            TargetRotation = TargetRotation.AngleTowards(Owner.GetToMouseVector2(Projectile.Center).ToRotation(), .1f);
            //这里挥砍动画一定程度上使用了矩阵变化。
            float easedProgress = EasingHelper.EaseInOutQuad(Helper.GetAniProgress(0));
            //末尾角度，也是下一个动画进程的起始角度
            float endAngle = KeepSpawning ? -190f * Flip.ToDirectionInt() : -165f * Flip.ToDirectionInt();
            float beginAngle = KeepSpawning ? -125f * Flip.ToDirectionInt() : -115f * Flip.ToDirectionInt();
            //更新当前的转角
            float rot = Helper.UpdateAngle(beginAngle, endAngle, Owner.direction, easedProgress);
            //将其投影到矩阵上，并进行形变
            Matrix tForm = Matrix.CreateRotationZ(rot) * Matrix.CreateScale(1.3f, Height, 1f);
            //而后再转化为射弹的目标指向，这个tarPos同时拥有指向和武器模长的信息。而不是一个单位向量
            Vector2 tarPos = Vector2.Transform(Vector2.UnitX, tForm) * 1.3f;
            //这样，Scale就是tarPos的向量模长
            Projectile.scale = tarPos.Length();
            //武器的角度为（起始角度 + 目标角度）的值
            Projectile.rotation = tarPos.ToRotation() + TargetRotation;
            //更新动画进程，封装的方法
            Helper.UpdateAniState(0);
            //下面基本上是粒子生成了。
            if (Main.rand.NextFloat() < easedProgress * 1.1f || KeepSpawning)
            {
                if (Main.rand.NextBool(4))
                {
                    Vector2 pos = Vector2.Lerp(Projectile.Center, Projectile.Center + tarPos.RotatedBy(TargetRotation) * 160, Main.rand.NextFloat(.2f, .8f));
                    Vector2 dir = (pos - Projectile.Center).SafeNormalize(Vector2.UnitX);
                    Vector2 vel = Owner.velocity * Main.rand.NextFloat(0.1f, 1.5f) + dir * Main.rand.NextFloat(0.1f, 44f);
                    float scale = .64f * Main.rand.NextFloat(0.8f, 1.3f);
                    new SmokeParticle(pos + dir.RotatedBy(PiOver2) * Main.rand.NextFloat(-45f, 45f), vel, RandLerpColor(Color.OrangeRed, Color.DarkGray), 40, RandRotTwoPi, .4f, scale, Main.rand.NextBool()).SpawnToPriority();
                }
                if (Main.rand.NextBool(3))
                {
                    Vector2 pos = Vector2.Lerp(Projectile.Center, Projectile.Center + tarPos.RotatedBy(TargetRotation) * 160, Main.rand.NextFloat(.2f, .8f));
                    Vector2 vel = (pos - Projectile.Center).SafeNormalize(Vector2.UnitX);
                    pos = Vector2.Lerp(Projectile.Center, Projectile.Center + tarPos.RotatedBy(TargetRotation) * 160, Main.rand.NextFloat(.20f, 1.25f)) + vel.RotatedBy(PiOver2) * Main.rand.NextFloat(-30f, 30f);
                    new TurbulenceGlowOrb(pos, 3.8f, RandLerpColor(Color.OrangeRed, Color.Orange), 80, .4f * Main.rand.NextFloat(0.4f, 0.6f), vel.ToRotation(), true).Spawn();
                }
            }
            int setRate;
            //仅用于起手时的效果，相当于是给武器提供了一个“蓄力”
            if (!KeepSpawning)
            {
                setRate = (int)(AttackSpeed * .40f);
                int rate = (int)(easedProgress * setRate);
                //排除掉rate=0的情况
                if (rate % 50 == 0 && rate != 0)
                {
                    SoundEngine.PlaySound(SoundID.DD2_BetsyFlameBreath with { MaxInstances = 0, Pitch = -.325f, PitchVariance = 0.1f, Volume = .75f });
                    for (int i = 0; i < 36; i++)
                    {
                        Vector2 pos = Vector2.Lerp(Projectile.Center, Projectile.Center + tarPos.RotatedBy(TargetRotation) * 160, Main.rand.NextFloat(.12f, .8f));
                        Vector2 vel = (pos - Projectile.Center).ToSafeNormalize() * Main.rand.NextFloat(7.8f, 62.4f);
                        pos = vel.SafeNormalize(Vector2.UnitX).RotatedBy(PiOver2) * Main.rand.NextFloat(-45f, 45f) + pos;
                        new SmokeParticle(pos + vel.SafeNormalize(Vector2.UnitX).RotatedBy(PiOver2) * Main.rand.NextFloat(-45f, 45f), Owner.velocity * Main.rand.NextFloat(0.1f, 1.5f) + vel.SafeNormalize(Vector2.UnitX) * Main.rand.NextFloat(0.1f, 44f), RandLerpColor(Color.OrangeRed, Color.DarkGray), 40, RandRotTwoPi, .7f, 0.64f * Main.rand.NextFloat(0.8f, 1.3f), Main.rand.NextBool()).SpawnToPriority();
                    }
                }
            }
        }
        public void UpdateMidAnimation()
        {
            if (Helper.GetAniProgress(1) == 0f)
            {
                SoundEngine.PlaySound(LostbeltSoundAssets.Blunt_Swing with { Variants = [3], Pitch = -.25f });
                SoundEngine.PlaySound(SoundID.DD2_BetsyFlameBreath with { MaxInstances = 0, Pitch = -.325f, PitchVariance = 0.1f, Volume = .75f });
                SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot with { MaxInstances = 0, Pitch = -.25f, PitchVariance = .2f });
                Vector2 fireVel = (Main.MouseWorld - Owner.Center).ToSafeNormalize() * 60;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - fireVel * 3.5f, fireVel, ProjectileType<DragonSwordSlash>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

            }
            float easedProgress = EasingHelper.EaseOutCubic(Helper.GetAniProgress(1));
            float beginAngle = KeepSpawning ? -195f * Flip.ToDirectionInt() : -165 * Flip.ToDirectionInt();
            float endAngle = KeepSpawning ? 125f * Flip.ToDirectionInt() : 115 * Flip.ToDirectionInt();
            float rot = Helper.UpdateAngle(beginAngle, endAngle, Owner.direction, easedProgress);
            Matrix tForm = Matrix.CreateRotationZ(rot) * Matrix.CreateScale(1.3f, Height, 1f);
            Vector2 tarPos = Vector2.Transform(Vector2.UnitX, tForm) * 1.3f;
            Projectile.scale = tarPos.Length();
            Projectile.rotation = tarPos.ToRotation() + TargetRotation;
            if (easedProgress < .01f)
                TargetRotation = TargetRotation.AngleTowards(Owner.GetToMouseVector2(Projectile.Center).ToRotation(), .5f);
            else
            {
                if (Main.rand.NextFloat() > easedProgress - .12f)
                {
                    Vector2 pos = Vector2.Lerp(Projectile.Center, Projectile.Center + tarPos.RotatedBy(TargetRotation) * 160, Main.rand.NextFloat(.10f, 1f));
                    Vector2 vel = (pos - Projectile.Center).ToSafeNormalize();
                    new SmokeParticle(pos + vel.RotatedBy(PiOver2) * Main.rand.NextFloat(-15f * Projectile.scale, 15f * Projectile.scale), Owner.velocity * Main.rand.NextFloat(0.1f, 1.5f) + vel * Main.rand.NextFloat(0.1f, 19f), RandLerpColor(Color.OrangeRed, Color.DarkGray), 40, RandRotTwoPi, .6f, 0.94f, Main.rand.NextBool()).SpawnToPriority();
                    pos = Vector2.Lerp(Projectile.Center, Projectile.Center + tarPos.RotatedBy(TargetRotation) * 160, Main.rand.NextFloat(.4f, 1f));
                    vel = (pos - Projectile.Center).ToSafeNormalize().RotatedBy(PiOver2);
                    new ShinyCrossStar(pos + vel * Main.rand.NextFloat(-15f, 15f), Owner.velocity * Main.rand.NextFloat(.1f, 1.5f) + vel * 5f, RandLerpColor(Color.OrangeRed, Color.Orange), 45, 0, 1, 0.5f, false).Spawn();
                    new TurbulenceGlowOrb(pos, 3.8f, RandLerpColor(Color.OrangeRed, Color.Orange), 80, .4f * Main.rand.NextFloat(0.4f, 0.6f), RandRotTwoPi, true).Spawn();
                }
            }
            Helper.UpdateAniState(1);
        }


        public void UpdateEndAnimation()
        {
            float easedProgress = EasingHelper.EaseOutCubic(Helper.GetAniProgress(2));
            float rot = Helper.UpdateAngle(115 * Flip.ToDirectionInt(), 125 * Flip.ToDirectionInt(), Owner.direction, easedProgress);
            Matrix tForm = Matrix.CreateRotationZ(rot) * Matrix.CreateScale(1.3f, Height, 1);
            Vector2 tarPos = Vector2.Transform(Vector2.UnitX, tForm) * 1.3f;
            Projectile.scale = tarPos.Length();
            Projectile.rotation = tarPos.ToRotation() + TargetRotation;
            Helper.UpdateAniState(2);
            if (Main.rand.NextFloat() < easedProgress * 1.1f)
            {
                Vector2 pos = Vector2.Lerp(Projectile.Center, Projectile.Center + tarPos.RotatedBy(TargetRotation) * 160, Main.rand.NextFloat(.2f, .8f));
                Vector2 vel = (pos - Projectile.Center).ToSafeNormalize();
                new SmokeParticle(pos + vel.RotatedBy(PiOver2) * Main.rand.NextFloat(-45f, 45f), Owner.velocity * Main.rand.NextFloat(0.1f, 1.5f) + vel * Main.rand.NextFloat(0.1f, 44f), RandLerpColor(Color.OrangeRed, Color.DarkGray), 40, RandRotTwoPi, .4f, 0.64f * Main.rand.NextFloat(0.8f, 1.3f), Main.rand.NextBool()).SpawnToPriority();
                vel = (pos - Projectile.Center).ToSafeNormalize();
                pos = Vector2.Lerp(Projectile.Center, Projectile.Center + tarPos.RotatedBy(TargetRotation) * 160, Main.rand.NextFloat(.20f, 1.25f)) + vel.RotatedBy(PiOver2) * Main.rand.NextFloat(-30f, 30f);
                new TurbulenceGlowOrb(pos, 3.8f, RandLerpColor(Color.OrangeRed, Color.Orange), 80, .4f * Main.rand.NextFloat(0.4f, 0.6f), vel.ToRotation(), true).Spawn();
            }
        }

        public void HandleHeldProjState()
        {
            Projectile.Center = Owner.MountedCenter;
            Owner.itemTime = 2;
            Owner.itemAnimation = 2;
            Owner.ChangeDir(Owner.LocalMouseWorld().X > Owner.Center.X ? 1 : -1);
            Owner.heldProj = Projectile.whoAmI;
            if (Owner.dead)
                Projectile.Kill();
            else
                Projectile.timeLeft = 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (!PostFirstFrame)
                return false;
            DrawSword();
            return false;
        }

        public void DrawSword()
        {
            //基础的一些数据
            Texture2D tex = TextureAssets.Item[GetInstance<DragonSword>().Type].Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            float drawRot = Projectile.rotation + PiOver4 + (Projectile.spriteDirection == -1).ToInt() * PiOver2;
            Vector2 drawPoint = new Vector2(tex.Width * (Projectile.spriteDirection == -1).ToInt(), tex.Height);
            SpriteEffects se = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Vector2 realDrawPos = drawPos + Vector2.UnitX.RotatedBy(Projectile.rotation) * -9;
            Color edgeColor;
            if (!Helper.IsDone[0])
            {
                edgeColor = Color.Lerp(Color.Transparent, Color.White, Helper.GetAniProgress(0));
            }
            else
                edgeColor = Color.Lerp(Color.White, Color.Transparent, Helper.GetAniProgress(2));
            if (KeepSpawning)
                edgeColor = Color.White;
            //绘制残影
            for (int i = 0; i < 8; i++)
                Main.spriteBatch.Draw(tex, realDrawPos + ToRadians(360f / 8 * i).ToRotationVector2() * 2f, null, edgeColor.ToAddColor(), drawRot, drawPoint, Projectile.scale, se, 0);

            Main.spriteBatch.Draw(tex, realDrawPos, null, Color.White, drawRot, drawPoint, Projectile.scale, se, 0);
        }
    }
    public class DragonSwordSlash : LostbeltProjs
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public List<Vector2> CenterPosList = [];
        private Vector2 TopLeftPoint = new Vector2(-300, -20);
        private Vector2 TopRightPoint = new Vector2(0, -100);
        private Vector2 BottomLeftPoint = new Vector2(0, 100);
        private Vector2 BottomRightPoint = new Vector2(-300, 20);
        public float RandOffset1;
        public float RandOffset2;
        public float RandOffset3;
        public float RandOffset4;
        public bool PostFirstFrame = false;
        public override void ExSD()
        {
            Projectile.width = 600;
            Projectile.height = 600;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.timeLeft = 120;
            Projectile.noEnchantmentVisuals = true;
            Projectile.extraUpdates = 1;
            Projectile.Opacity = 0f;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            SetFirstFrame();
            Projectile.Opacity = Lerp(Projectile.Opacity, 1f, 0.12f);
            if (Projectile.IsOutScreen())
                return;
            for (int i = 0; i < 4; i++)
            {
                int index = Main.rand.Next(10, CenterPosList.Count - 10);
                Vector2 pos = CenterPosList[index] + Projectile.Center;
                new SmokeParticle(pos, Projectile.velocity * Main.rand.NextFloat(.4f), RandLerpColor(Color.OrangeRed, Color.Gray), 45, Main.rand.NextFloat(TwoPi), 0.64f, Main.rand.NextFloat(.9f, 1.3f), Main.rand.NextBool()).Spawn();
            }
            for (int i = 0; i < 2; i++)
            {
                int index = Main.rand.Next(3, CenterPosList.Count - 3);
                Vector2 pos = CenterPosList[index] + Projectile.Center;
                float scale = Main.rand.NextFloat(.52f, .82f) * .2f;
                int lifeTime = Main.rand.Next(30, 60);
                new HRShinyOrb(pos, Projectile.velocity * .5f, RandLerpColor(Color.OrangeRed, Color.Orange), lifeTime, scale).Spawn();
                new HRShinyOrb(pos, Projectile.velocity * .5f, Color.White, lifeTime, scale * .65f).Spawn();
            }
            for (int i = 0; i < 3; i++)
            {
                int index = Main.rand.Next(3, CenterPosList.Count - 3);
                Vector2 pos = CenterPosList[index] + Projectile.Center;
                float scale = Main.rand.NextFloat(.4f, .8f) * .51f;
                new TurbulenceGlowOrb(pos, Main.rand.NextFloat(4.8f, 6.1f), RandLerpColor(Color.OrangeRed, Color.Orange), Main.rand.Next(60, 100), scale, Main.rand.NextFloat(TwoPi), true).Spawn();
            }

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int dustCount = 16;

            SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact with { MaxInstances = 1, Pitch = -0.5f });
            if (Projectile.numHits < 4)
            {
                for (int i = -3; i < 0; i++)
                {
                    Vector2 vel = Projectile.velocity.SafeNormalize(Vector2.UnitX).RotatedByRandom(TwoPi) * 26f;
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), target.Center, vel, ProjectileType<DragonSwordFireball>(), Projectile.damage, Projectile.knockBack, Owner.whoAmI);
                }
            }
            for (int i = 0; i < dustCount; ++i)
            {
                Vector2 dir = Projectile.SafeDirByRot();
                Vector2 pos = target.Center + Main.rand.NextVector2CircularEdge(10f, 10f) + dir * Main.rand.NextFloat(10f);
                new ShinyCrossStar(pos, RandVelTwoPi(2f, 6f), RandLerpColor(Color.Red, Color.OrangeRed), 45, RandRotTwoPi, RandZeroToOne, Projectile.scale, false, 0.5f).Spawn();
            }
            for (int i = 0; i < 16; i++)
            {
                Vector2 pos = target.Center + Main.rand.NextVector2CircularEdge(10f, 10f);
                Vector2 vel = Main.rand.NextFloat(TwoPi).ToRotationVector2() * Main.rand.NextFloat(0.2f, 7.4f);
                float scale = Main.rand.NextFloat(0.4f, 0.9f) * .2f;
                new HRShinyOrb(pos, vel, RandLerpColor(Color.Orange, Color.OrangeRed), 45, scale).Spawn();
                new HRShinyOrb(pos, vel, Color.White, 45, scale * .75f).Spawn();
                Dust d = Dust.NewDustPerfect(pos, DustID.Torch, RandVelTwoPi(0.2f, 3.1f));
                d.scale *= 1.3f;
            }

            for (int i = 0; i < 16; i++)
            {
                Color Firecolor = RandLerpColor(Color.White, Color.DarkOrange);
                Vector2 spawnPos = target.Center + Projectile.SafeDirByRot() * Main.rand.NextFloat(10f);
                Vector2 vel = Vector2.UnitX.RotatedByRandom(TwoPi) * Main.rand.NextFloat(0.2f, 1.2f) * 4;
                new SmokeParticle(spawnPos, vel, Firecolor, 90, Main.rand.NextFloat(TwoPi), .5f, 0.8f, Main.rand.NextBool()).Spawn();
            }
        }
        public void SetFirstFrame()
        {
            if (PostFirstFrame)
                return;
            PostFirstFrame = true;
            RandOffset1 = Main.rand.NextFloat(0, 10);
            RandOffset2 = Main.rand.NextFloat(0, 10);
            RandOffset3 = Main.rand.NextFloat(0, 10);
            RandOffset4 = Main.rand.NextFloat(0, 10);
            float maxPoints = 50;
            float xMult = 3f;
            float yMult = 2f;
            for (int i = 0; i < maxPoints; i++)
            {
                float progress = i / maxPoints;
                Vector2 finalPos = Vector2.CatmullRom(TopLeftPoint, TopRightPoint, BottomLeftPoint, BottomRightPoint, progress);
                finalPos.X *= xMult;
                finalPos.Y *= yMult;
                CenterPosList.Add(finalPos.RotatedBy(Projectile.rotation));
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (!PostFirstFrame)
                return false;
            PixelatedRenderManager.BeginDrawProj = true;
            Texture2D tex = LostbeltModAssets.Texture_StandardGradient.Value;
            SB.EnterShaderArea();
            Effect e = LostbeltShaderAssets.AlphaFade;
            e.Parameters["uFadeoutLeftLength"].SetValue(0.1f);
            e.Parameters["uFadeinRigtLength"].SetValue(0.1f);
            e.Parameters["uFadeinTopLength"].SetValue(0);
            e.Parameters["uFadeinBottomLength"].SetValue(0.4f);
            e.Parameters["UVMult"].SetValue(new Vector2(.5f, .5f));
            e.CurrentTechnique.Passes[0].Apply();
            Color setColor = Color.Lerp(Color.Red, Color.WhiteSmoke, 0.4f);
            DrawBaseWave(tex, Color.White * .47f, 1f);
            DrawBaseWave(tex, Color.OrangeRed * .85f, 5f);
            DrawBaseWave(tex, Color.Red * .75f, 8f);
            DrawBaseWave(tex, Color.Orange * 0.5f, 12f);
            DrawBaseWave(tex, setColor * 0.45f, 15f);
            Texture2D texture2 = LostbeltModAssets.Noise_Misc2.Value;
            Vector4 vector4 = new(0.1f, 0.1f, 0.05f, 0.6f);
            LostbeltMethods.ApplyAlphaCut(vector4, new(0, -Main.GlobalTimeWrappedHourly * 0.79f + RandOffset1), new Vector2(4.6f, 0.07f), Color.DarkOrange);
            DrawBaseWave(texture2, Color.DarkOrange, 15f);

            LostbeltMethods.ApplyAlphaCut(vector4, new(0, -Main.GlobalTimeWrappedHourly * 0.65f + RandOffset2), new Vector2(2f, 0.1f), Color.Orange);
            DrawBaseWave(texture2, Color.Orange, 20f);

            texture2 = LostbeltModAssets.Noise_Misc.Value;
            setColor = Color.Lerp(Color.OrangeRed, Color.WhiteSmoke, 0.3f);
            LostbeltMethods.ApplyAlphaCut(vector4, new(0, -Main.GlobalTimeWrappedHourly * 0.85f + RandOffset3), new Vector2(2f, 0.15f), setColor);
            DrawBaseWave(texture2, setColor, 8f);


            setColor = Color.Lerp(Color.Orange, Color.WhiteSmoke, 0.4f);
            LostbeltMethods.ApplyAlphaCut(vector4, new(0, -Main.GlobalTimeWrappedHourly * 0.35f + RandOffset4), new Vector2(1.5f, 0.1f), setColor);
            DrawBaseWave(texture2, setColor, 6f);

            SB.EndShaderArea();
            return false;
        }
        public void DrawBaseWave(Texture2D tex, Color color, float v)
        {

            List<ScarletVertex> VertexList = [];
            Vector2 projVel = Projectile.velocity.SafeNormalize(Vector2.UnitX) * 20;
            for (int i = 0; i < CenterPosList.Count; i++)
            {
                float progress = (float)i / CenterPosList.Count;
                Vector2 posHead = CenterPosList[i] + Projectile.Center - Main.screenPosition;
                Vector2 posSrc = CenterPosList[i] + Projectile.Center - Main.screenPosition - projVel * v * Projectile.Opacity;
                VertexList.Add(new ScarletVertex(posHead, color, new Vector3(progress, 0, 0)));
                VertexList.Add(new ScarletVertex(posSrc, color, new Vector3(progress, 1, 0)));
            }
            if (VertexList.Count < 3)
                return;
            GD.Textures[0] = tex;
            GD.SamplerStates[0] = SamplerState.PointWrap;
            GD.DrawUserPrimitives(PrimitiveType.TriangleStrip, VertexList.ToArray(), 0, VertexList.Count - 2);
        }
    }
    public class DragonSwordFireball : LostbeltProjs
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public ref float Timer => ref Projectile.ai[0];
        public NPC CurTarget = null;
        public int BounceTime = 0;
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(16);
        }
        public override void ExSD()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = Projectile.height = 30;
            Projectile.SetupImmnuity(60);
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 2;
            Projectile.penetrate = 1;
            Projectile.timeLeft = GetSeconds(5);
        }
        public override bool? CanDamage() => Timer > 30f;
        public override void OnFirstFrame()
        {
            Vector2 firePos2 = Projectile.Center - Projectile.SafeDirByRot() * 30f;
            Vector2 dir2 = Projectile.SafeDirByRot() * -1;
        }
        public override void ProjAI()
        {
            Lighting.AddLight(Projectile.Center, TorchID.Orange);
            if (Projectile.GetTargetSafe(out NPC target, searchDistance: 1200f) && CurTarget is null)
            {
                CurTarget = target;
            }
            CurTarget = CurTarget.IsLegal() ? CurTarget : null;

            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Projectile.penetrate == -1 && Projectile.damage == 0)
            {
                Projectile.velocity *= 0.06f;
                Projectile.Opacity -= 0.1f;
                if (Projectile.Opacity <= 0.03f)
                {
                    Projectile.Kill();
                    return;
                }
            }
            else
            {
                Timer++;
                if (Timer < 30f)
                {
                    Projectile.velocity *= 0.9f;
                    return;
                }
                if (CurTarget.IsLegal())
                {
                    Projectile.timeLeft = GetSeconds(5);
                    Projectile.HomingTarget(CurTarget.Center, -1, 16f, 20f, 10);

                }
                else
                {
                    if (Projectile.velocity.LengthSquared() < 16f * 16f)
                        Projectile.velocity *= 1.1f;
                    else
                        Projectile.velocity *= 0.9f;
                }
            }
            if (Projectile.IsOutScreen() && Projectile.Opacity < Main.rand.NextFloat())
                return;
            if (Main.rand.NextBool(9))
                new ShinyCrossStar(Projectile.Center.ToRandCirclePosEdge(4f), Projectile.velocity / 8f, RandLerpColor(Color.Lerp(Color.DarkOrange, Color.Red, 0.64f), Color.OrangeRed), 40, RandRotTwoPi, 1f, 0.53f * Projectile.Opacity, false, ToRadians(1f)).Spawn();

            if (Main.rand.NextBool(12))
                new SmokeParticle(Projectile.Center.ToRandCirclePos(8f), Projectile.velocity / 8f, RandLerpColor(Color.DarkOrange, Color.OrangeRed), 20, RandRotTwoPi, 1f, Main.rand.NextFloat(0.12f, 0.16f) * 0.86f * Projectile.Opacity, true).SpawnToPriority();

        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 9; i++)
            {
                Vector2 vel = RandVelTwoPi(0.1f, 4.2f);
                Vector2 dir = Projectile.SafeDirByRot();
                Vector2 spawnpos = Projectile.Center.ToRandCirclePos(4f);
                new SmokeParticle(spawnpos, vel + dir * Main.rand.NextFloat(1f, 8f), RandLerpColor(Color.Lerp(Color.Orange, Color.Red, 0.50f), Color.Orange), 40, RandRotTwoPi, .35f, 0.30f * Main.rand.NextFloat(0.75f, 1.1f), true).SpawnToPriority();
                if (Main.rand.NextBool())
                {
                    vel = Projectile.oldVelocity.ToSafeNormalize() * Main.rand.NextFloat(-3f, 12f);
                    new SmokeParticle(spawnpos, vel, RandLerpColor(Color.Lerp(Color.Orange, Color.Red, 0.75f), Color.OrangeRed), 40, RandRotTwoPi, .35f, 0.30f * Main.rand.NextFloat(0.75f, 1.1f), true).SpawnToPriority();
                }
            }
            for (int j = 0; j < 10; j++)
            {
                Vector2 dir = Projectile.SafeDirByRot();
                new ShinyCrossStar(Projectile.Center.ToRandCirclePos(20f) + dir * Main.rand.NextFloat(0f, 6f), dir * 12f * Main.rand.NextFloat(), RandLerpColor(Color.Orange, Color.OrangeRed), 50, RandRotTwoPi, 1, 0.7f, false).Spawn();
            }
            if (BounceTime > 2)
            {
                Projectile.damage = 0;
                Projectile.penetrate = -1;

            }

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.Item45 with { MaxInstances = 1, Pitch = 0.3f }, Projectile.Center);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item45 with { MaxInstances = 1, Pitch = 0.3f }, Projectile.Center);
            for (int i = 0; i < 9; i++)
            {
                Vector2 vel = RandVelTwoPi(0.1f, 4.2f);
                Vector2 spawnpos = Projectile.Center.ToRandCirclePos(4f);
                new SmokeParticle(spawnpos, vel, RandLerpColor(Color.Lerp(Color.Orange, Color.Red, 0.50f), Color.Orange), 40, RandRotTwoPi, 1f, 0.30f * Main.rand.NextFloat(0.75f, 1.1f), true).SpawnToPriority();
                if (Main.rand.NextBool())
                {
                    vel = Projectile.oldVelocity.ToSafeNormalize() * Main.rand.NextFloat(-9f, 3f);
                    new SmokeParticle(spawnpos, vel, RandLerpColor(Color.Lerp(Color.Orange, Color.Red, 0.75f), Color.OrangeRed), 40, RandRotTwoPi, 1f, 0.30f * Main.rand.NextFloat(0.75f, 1.1f), true).SpawnToPriority();
                }
            }
            for (int j = 0; j < 10; j++)
            {
                Vector2 dir = -Projectile.SafeDirByRot();
                new ShinyCrossStar(Projectile.Center.ToRandCirclePos(20f) + dir * Main.rand.NextFloat(0f, 6f), dir * 12f * Main.rand.NextFloat(), RandLerpColor(Color.Orange, Color.OrangeRed), 50, RandRotTwoPi, 1, 0.7f, false).Spawn();
            }
            if (BounceTime > 2)
            {
                Projectile.damage = 0;
                Projectile.penetrate = -1;

            }
            Projectile.BounceOnTile(oldVelocity);
            BounceTime++;
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (!Projectile.Lostbelt().FirstFrame)
                return false;
            PixelatedRenderManager.BeginDrawProj = true;
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 projPos, out Vector2 ori);
            projTex = LostbeltModAssets.Particle_ShinyOrbHard.Value;
            ori = projTex.Size() / 2f;
            //绘制残影
            float oriScale = 0.64f;
            float scale = 1f;
            int length = (int)(8 * Projectile.Opacity);
            for (int i = 0; i < length; i++)
            {
                scale *= 0.975f;
                float rads = (float)i / length;
                Color edgeColor = Color.Lerp(Color.DarkOrange, Color.OrangeRed, 1 - rads).ToAddColor(10) * Clamp(Projectile.velocity.Length(), 0f, 1f) * (1 - rads) * Projectile.Opacity;
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], 0.25f);
                float rot = Lerp(Projectile.oldRot[i], Projectile.oldRot[0], 1f);
                SB.Draw(projTex, lerpPos + Projectile.PosToCenter(), null, edgeColor, rot, ori, oriScale * scale * Projectile.Opacity, 0, 0);
                edgeColor = Color.Lerp(Color.Orange, Color.White, 1 - rads).ToAddColor(10) * Clamp(Projectile.velocity.Length(), 0f, 1f) * (1 - rads) * Projectile.Opacity;
                SB.Draw(projTex, lerpPos + Projectile.PosToCenter(), null, edgeColor * 0.25f, rot, ori, oriScale * scale * Projectile.Opacity * 0.75f, 0, 0);
            }
            SB.EnterShaderArea();
            DrawTrails(LostbeltModAssets.Trail_ManaStreakTiny.Texture, Color.Red);
            DrawTrails(LostbeltModAssets.Trail_ManaStreakTiny.Texture, Color.DarkOrange, 0.75f);
            DrawTrails(LostbeltModAssets.Trail_ManaStreak.Texture, Color.OrangeRed, 0.25f);
            SB.EndShaderArea();

            SB.Draw(projTex, projPos, null, Color.DarkOrange.ToAddColor(75) * Projectile.Opacity, Projectile.rotation, ori, oriScale, 0, 0);
            SB.Draw(projTex, projPos, null, Color.White.ToAddColor(0) * Projectile.Opacity, Projectile.rotation, ori, oriScale * 0.75f, 0, 0);

            return false;
        }
        public void DrawTrails(Asset<Texture2D> useTex, Color drawColor, float multipleSize = 1f, float alphaValue = 1f, float offsetHeight = 1f)
        {
            if (!Projectile.Lostbelt().FirstFrame)
                return;

            if (Projectile.oldPos.Length < 3)
                return;
            Effect shader = LostbeltShaderAssets.StandardFlowShader;

            float laserLength = 50;
            shader.Parameters["LaserTextureSize"].SetValue(useTex.Size());
            shader.Parameters["targetSize"].SetValue(new Vector2(laserLength, useTex.Height()));
            shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * -24.2f);
            shader.Parameters["uColor"].SetValue(drawColor.ToVector4() * Projectile.Opacity * alphaValue * Clamp(Projectile.velocity.Length(), 0f, 1f));
            shader.Parameters["uFadeoutLength"].SetValue(0.8f);
            shader.Parameters["uFadeinLength"].SetValue(0.12f);
            shader.CurrentTechnique.Passes[0].Apply();

            DrawSetting drawSetting = new(useTex.Value);
            List<TrailDrawDate> trailDrawDates = [];
            int posCount = (int)(Projectile.oldPos.Length * Projectile.Opacity);
            for (int j = 0; j < posCount - 1; j++)
            {
                if (Projectile.oldPos[j] != Vector2.Zero)
                {
                    Vector2 drawPos = Projectile.oldPos[j] + new Vector2(Projectile.width / 2, Projectile.height / 2);
                    trailDrawDates.Add(new(drawPos, drawColor, new Vector2(0, 45 * multipleSize * Projectile.scale), Projectile.oldRot[j]));
                }
            }
            TrailRender.RenderTrail([.. trailDrawDates], drawSetting);
        }
    }

    public static class EasingHelper
    {
        /// <summary>缓动函数：Quadratic 缓入缓出 (Ease In Out Quad)。</summary>
        /// <remarks>
        /// 曲线呈 S 形，先加速后减速，全程速度变化平滑。
        /// 适合大多数 UI 动画、物体往返运动。
        /// </remarks>
        public static float EaseInOutQuad(float t)
        {
            if (!(t < 0.5f))
            {
                return 1f - (-2f * t + 2f) * (-2f * t + 2f) / 2f;
            }

            return 2f * t * t;
        }
        /// <summary>缓动函数：Exponential 缓出 (Ease Out Expo)。</summary>
        /// <remarks>
        /// 开始时极快减速，最后缓慢趋近终值。呈迅速停下、带有余韵的效果。
        /// 适合强调“快速结束”的动画，如飞入后停留。
        /// </remarks>
        public static float EaseOutExpo(float t)
        {
            if (t != 1f)
            {
                return 1f - MathF.Pow(2f, -10f * t);
            }

            return 1f;
        }
        /// <summary>缓动函数：Exponential 缓入缓出 (Ease In Out Expo)。</summary>
        /// <remarks>
        /// 两端缓慢，中间突变明显（相比 Quad 更陡峭）；开始和结束都极其平滑。
        /// 适合需要强烈“推进-减速”感的动画，如大型物体移动。
        /// </remarks>
        public static float EaseInOutExpo(float t)
        {
            if (!(t < 0.5f))
            {
                return 1f - MathF.Pow(-2f * t + 2f, 2f) / 2f;
            }

            return 2f * t * t;
        }
        /// <summary>缓动函数：Cubic 缓入 (Ease In Cubic)。</summary>
        /// <remarks>
        /// 起步非常慢，然后急剧加速，类似物体从静止被猛推出去。
        /// 适合表现“重力落下”或“突然发射”的效果。
        /// </remarks>
        public static float EaseInCubic(float t)
        {
            return t * t * t;
        }
        /// <summary>缓动函数：Cubic 缓出 (Ease Out Cubic)。</summary>
        /// <remarks>
        /// 快速启动，随后减速滑停，比 Quadratic 缓出更“柔和”。
        /// 适合物体弹出、旋转停止等自然阻尼运动。
        /// </remarks>
        public static float EaseOutCubic(float t)
        {
            return (float)(1.0 - Math.Pow(1f - t, 3.0));
        }
        /// <summary>缓动函数：Back 缓出 (Ease Out Back)。</summary>
        /// <remarks>
        /// 超过终值一点再弹回，产生“回弹/过冲”效果，结尾有振荡感。
        /// 适合模拟弹簧、橡皮筋拉长后回缩的动画。
        /// </remarks>
        public static float EaseOutBack(float t)
        {
            if (t == 1f)
            {
                return 1f;
            }

            return (float)(1.0 + 2.7015800476074219 * Math.Pow(t - 1f, 3.0) + 1.7015800476074219 * Math.Pow(t - 1f, 2.0));
        }
        /// <summary>缓动函数：Back 缓入 (Ease In Back)。</summary>
        /// <remarks>
        /// 开始前先回退一点再正向加速，产生“蓄力”感觉。
        /// 适合表现拉弓、蓄力攻击的预备动作。
        /// </remarks>
        public static float EaseInBack(float t)
        {
            if (t == 1f)
            {
                return 1f;
            }

            return 2.70158f * t * t * t - 1.70158f * t * t;
        }
        /// <summary>缓动函数：正弦缓入缓出 (Ease In Out Sin)。</summary>
        /// <remarks>
        /// 使用 sin(π * t) 曲线，全程平滑且对称，没有突变点。
        /// 适合呼吸灯、周期性摆动、淡入淡出等自然柔和的效果。
        /// </remarks>
        public static float EaseInOutSin(float t)
        {
            return (float)Math.Sin(MathF.PI * t);
        }
    }
    public static class GeneralMethods
    {
        /// <summary>
        /// 获取武器的攻击速度，适配攻速加成。
        /// 可以给手持射弹的攻击频率用
        /// </summary>
        /// <param name="player"></param>
        /// <param name="item">这个射弹对应的物品</param>
        /// <param name="time">基础攻击间隔</param>
        /// <param name="min">最低攻击间隔</param>
        /// <returns></returns>
        public static int ApplyWeaponAttackSpeed(this Player player, Item item, int time, int min)
        {
            float attackSpeed = player.GetWeaponAttackSpeed(item);
            float mult = 1f / attackSpeed;
            int realAttackSpeed = (int)(time * mult);
            return realAttackSpeed < min ? min : realAttackSpeed;
        }
        /// <summary>
        /// 获取武器的攻击速度，适配攻速加成。
        /// 可以给手持射弹的攻击频率用
        /// 重载方法，直接使用玩家手持武器 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="min"></param>
        /// <returns></returns>
        public static int ApplyWeaponAttackSpeed(this Player player, int min)
        {
            Item item = player.HeldItem;
            float attackSpeed = player.GetWeaponAttackSpeed(item);
            float mult = 1f / attackSpeed;
            int realAttackSpeed = (int)(item.useTime * mult);
            return realAttackSpeed < min ? min : realAttackSpeed;
        }
        /// <summary>
        /// 控制玩家的臂膀
        /// </summary>
        /// <param name="player"></param>
        /// <param name="armRot"></param>
        /// <param name="specificArm">指定要玩家哪一条手臂，默认值为0，即双手，低于<0的值，则为背部的手，高于>0的值，则为前部的手</param>
        public static void SetPlayerArm(this Player player, float armRot, int specificArm = 0, float customArmRot = PiOver2)
        {
            float armType = Math.Sign(specificArm);
            switch (armType)
            {
                case 1:
                    player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, armRot - customArmRot);
                    break;
                case -1:
                    player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, armRot - customArmRot);
                    break;
                default:
                    player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, armRot - customArmRot);
                    player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, armRot - customArmRot);
                    break;
            }
        }
        public static Vector2 GetToMouseVector2(this Player player, Vector2 BeginPos)
        {
            Vector2 vector = Main.MouseWorld - BeginPos;
            vector = vector.SafeNormalize(Vector2.UnitX);
            return vector;
        }
    }
}
