using System.Collections.Generic;
using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.Primitives.Trail;
using LostbeltTraveler.Core.ScreenEffect;
using LostbeltTraveler.Globals.Graphics.Particles;
using LostbeltTraveler.Globals.Handlers;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Requirement
{
    public class DragonSniper : ModItem, ILocalizedModType
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public new string LocalizationCategory => "Weapons.Magic";
        public override void SetDefaults()
        {
            Item.damage = 64;
            Item.noMelee = true;
            Item.shootSpeed = 16;
            Item.useAnimation = Item.useTime = 70;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = null;
            Item.DamageType = DamageClass.Ranged;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.sellPrice(1);
        }
        public override bool CanUseItem(Player player)
        {
            return !player.HasProj(ProjectileType<DragonSniperHeldProj>());
        }
        public override void HoldItem(Player player)
        {
            int projID = ProjectileType<DragonSniperHeldProj>();
            if (player.ownedProjectileCounts[projID] > 0)
                return;
            Vector2 dir = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);
            int damage = (int)player.GetTotalDamage<RangedDamageClass>().ApplyTo(Item.damage);
            Projectile proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, projID, 0, 0, player.whoAmI);
            proj.originalDamage = damage;
            proj.netUpdate = true;
            proj.ai[0] = 30;


        }
    }
    public class DragonSniperNuke : ModProjectile, ILocalizedModType
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public Player Owner => Main.player[Projectile.owner];
        public NPC CurTaget = null;
        public override string Texture => DragonMethods.GetItemTexturePath(ItemID.MiniNukeII);
        public enum State
        {
            Shoot,
            Lock
        }
        public ref float Timer => ref Projectile.ai[0];
        public State AttackState
        {
            get => (State)Projectile.ai[1];
            set => Projectile.ai[1] = (float)value;
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 16;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            //初始化的值随意，因为下面会被重设定
            Projectile.localNPCHitCooldown = 60;
            Projectile.noEnchantmentVisuals = true;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
        }
        public override void AI()
        {
            switch (AttackState)
            {
                case State.Shoot:
                    DoShoot();
                    break;
                case State.Lock:
                    DoLock();
                    break;
            }
        }

        public void DoLock()
        {
            Projectile.Opacity = Lerp(Projectile.Opacity, 0, 0.1f);
            if (CurTaget.IsSafe() && Timer < 11)
            {
                Projectile.Center = CurTaget.Center;
            }
            else
            {
                //这里会记录实际上总共攻击了多少次
                //如果处死的时候没有满足足够的次数，会直接尝试生成所有的剩余次数
                Projectile.Kill();
            }
        }

        public void DoShoot()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Main.rand.NextBool())
            {
                for (int i = 0; i < 2; i++)
                    ECSParticle.ShinyCrossStarECS(Main.rand.NextVector2CircularEdge(4, 4) + Projectile.Center, Projectile.velocity / 8f + Main.rand.NextFloat(TwoPi).ToRotationVector2() * Main.rand.NextFloat(-1f, 1f) * 1.1f, Color.Lerp(Color.Orange, Color.OrangeRed, Main.rand.NextFloat()), Main.rand.Next(30, 60), 1, Main.rand.NextFloat(0.7f, 1.1f) * 0.5f, 0.2f);
            }
            if (Main.rand.NextBool())
            {
                for (int i = 0; i < 2; i++)
                    ECSParticle.SmokeParticle(Main.rand.NextVector2Circular(4, 4) + Projectile.Center, Projectile.velocity / 8f, Color.Lerp(Color.Orange, Color.OrangeRed, Main.rand.NextFloat()), Main.rand.Next(30, 74), Main.rand.NextFloat(TwoPi), .65f, Main.rand.NextFloat(0.7f, 1.1f) * .15f, true);
            }
            if (Main.rand.NextBool())
            {
                for (int i = 0; i < 2; i++)
                {
                    Dust d = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2CircularEdge(4, 4), DustID.Torch);
                    d.velocity = Projectile.velocity / 8f + Main.rand.NextFloat(TwoPi).ToRotationVector2() * Main.rand.NextFloat(-1f, 1f) * 1.2f;
                    d.scale = Main.rand.NextFloat(.7f, 1.3f);
                    d.noGravity = true;
                }
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            bool canHit = AttackState == State.Shoot ||
                         AttackState == State.Lock && CurTaget.IsSafe() && CurTaget.Equals(target);
            if (canHit)
                return null;
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (AttackState == State.Shoot)
            {
                AttackState = State.Lock;
                CurTaget = target;
                Timer = 0;
                //这样的话会有一个双判的可能性，不过不关我事
                Projectile.extraUpdates = 1;
                Projectile.localNPCHitCooldown = 50;
                Projectile.ResetLocalNPCHitImmunity();
                return;
            }
            if (AttackState == State.Lock)
            {
                float progress = Timer / 6f;
                Projectile.localNPCHitCooldown -= 5;
                SoundEngine.PlaySound(LostbeltSoundAssets.SodomsDisaster_BoomHit with { Pitch = Lerp(0f, 0.4f, progress) }, Projectile.Center);
                Timer += 1;
                //什么叫你写了这么多就为了处理这个特效爆炸？
                for (int i = 0; i < 30; i++)
                {
                    Vector2 vel = (TwoPi / 45f * i).ToRotationVector2() * Lerp(8f, 15f, progress) * Main.rand.NextFloat(0f, 1f);
                    Vector2 spawnpos = Projectile.Center.ToRandCirclePos(4f) + vel.ToSafeNormalize() * Main.rand.NextFloat() * 2f;
                    Color color = RandLerpColor(Color.Lerp(Color.Orange, Color.Red, 0.50f), Color.Orange);
                    float scale = 0.40f * Main.rand.NextFloat(0.55f, 1.1f);
                    ECSParticle.SmokeParticle(spawnpos, vel, color, Main.rand.Next(10, 41), RandRotTwoPi, Main.rand.NextFloat(.75f, 1f) * .97f, scale, true, BlendState.Additive);

                    vel = RandVelTwoPi(.4f, 15.5f);
                    color = RandLerpColor(Color.Lerp(Color.Orange, Color.Red, 0.55f), Color.OrangeRed);
                    scale = .35f * Main.rand.NextFloat(.65f, 1.1f);
                    ECSParticle.SmokeParticle(spawnpos, vel, color, Main.rand.Next(10, 41), RandRotTwoPi, Main.rand.NextFloat(.75f, 1f) * .97f, scale, true, BlendState.Additive);
                }
                for (int j = 0; j < 10; j++)
                {
                    Vector2 dir = Vector2.UnitX.RotatedByRandom(TwoPi) * Main.rand.NextFloat(0f, Lerp(3f, 15.9f, progress));
                    Vector2 pos = Projectile.Center.ToRandCirclePos(3f);
                    ECSParticle.ShinyCrossStarECS(pos, dir, RandLerpColor(Color.Orange, Color.OrangeRed), Main.rand.Next(15, 50), 1f, 1f * Main.rand.NextFloat(.7f, .9f), .2f);
                }
                for (int i = 0; i < 10; i++)
                {
                    Vector2 pos = Projectile.Center.ToRandCirclePos(2f);
                    Vector2 vel = RandVelTwoPi(.1f, Lerp(1.2f, 11.9f, progress));
                    ECSParticle.ShinyCrossStarECS(pos, vel, RandLerpColor(Color.Lerp(Color.Red, Color.Orange, .5f), Color.OrangeRed), Main.rand.Next(15, 50), 1f, .99f * Main.rand.NextFloat(.6f, 1f), .2f);
                }
                for (int i = 0; i < 20; i++)
                {
                    Vector2 pos = Projectile.Center.ToRandCirclePos(2f);
                    Vector2 vel = RandVelTwoPi(.1f, Lerp(1.2f, 14.9f, progress));
                    ECSParticle.HRShinyOrb(pos, vel, RandLerpColor(Color.Lerp(Color.Red, Color.Orange, .5f), Color.OrangeRed), Main.rand.Next(15, 50), 1f, .15f * Main.rand.NextFloat(.6f, 1f), .5f);
                }
                if (Timer < 11)
                    return;
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(LostbeltSoundAssets.SodomsDisaster_BoomHit with { Pitch = 0.5f }, Projectile.Center);
            SoundEngine.PlaySound(LostbeltSoundAssets.Sundowner_Fire with { Pitch = 0.5f }, Projectile.Center);
            SoundEngine.PlaySound(LostbeltSoundAssets.Misc_Boom with { Pitch = 0.5f }, Projectile.Center);

            ScreenShakeSystem.AddScreenShakes(Projectile.Center, 16f, 20, Projectile.rotation);
            //强行用粉色的烟来把这里的范围覆盖起来
            Vector2 safeDir = Projectile.rotation.ToRotationVector2();
            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 4; j++)
                    ECSParticle.StarShape(Projectile.Center.ToRandCirclePos(3f), safeDir.RotatedBy(PiOver2 * j + PiOver4) * Main.rand.NextFloat(.1f, 19f), RandLerpColor(Color.OrangeRed, Color.WhiteSmoke), 30, 1f, 1.091f);
            }
            for (int i = 0; i < 80; i++)
            {
                if (i % 2 == 0)
                    ECSParticle.HRShinyOrb(Projectile.Center.ToRandCirclePos(3f), RandVelTwoPi(1f, 24f), RandLerpColor(Color.Orange, Color.OrangeRed), 30, 1f, Main.rand.NextFloat(.7f, 1.3f) * .1f, 0.45f);
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePos(5f), RandVelTwoPi(2, 16f), RandLerpColor(Color.Orange, Color.OrangeRed), Main.rand.Next(35, 50), 1f, Main.rand.NextFloat(.9f, 1.1f), 0.2f);
                //ECSParticle.StarShape(Projectile.Center.ToRandCirclePos(5f), RandVelTwoPi(2, 17), RandLerpColor(Color.Orange, Color.OrangeRed), Main.rand.Next(35, 50), 1, Main.rand.NextFloat(1.1f,1.3f), glowMult: 0.855f);
                Dust d = Dust.NewDustPerfect(Projectile.Center.ToRandCirclePos(5f), Main.rand.NextBool() ? DustID.Torch : DustID.OrangeTorch, RandVelTwoPi(2f, 21f));
                d.velocity = RandVelTwoPi(2, 17);
                d.scale = Main.rand.NextFloat(.9f, 1.2f) * 1.5f;
                d.noGravity = true;
            }
            for (int i = 0; i < 60; i++)
            {
                Color color = RandLerpColor(Color.Lerp(Color.DarkOrange, Color.Red, 0.50f), Color.Orange);
                Color color2 = RandLerpColor(Color.DarkOrange, Color.OrangeRed);
                new SmokeParticle(Projectile.Center.ToRandCirclePos(5f), RandVelTwoPi(2.7f, 28), color, 40, RandRotTwoPi, .45f, 0.87f * Main.rand.NextFloat(0.8f, 1.1f), false).Spawn();
                new SmokeParticle(Projectile.Center.ToRandCirclePos(5f), RandVelTwoPi(2.7f, 20), color2, 40, RandRotTwoPi, .45f, 0.77f * Main.rand.NextFloat(0.8f, 1.1f), true).SpawnToPriority();
                new SmokeParticle(Projectile.Center.ToRandCirclePos(3f), RandVelTwoPi(2.7f, 18), RandLerpColor(Color.DarkGray, Color.DarkOrange), 40, RandRotTwoPi, .40f, 0.78f * Main.rand.NextFloat(0.8f, 1.1f), false).SpawnToPriorityNonPreMult();
            }
            float starScale = 1.60f * .4f;
            new KiraStar(Projectile.Center, Vector2.Zero, RandLerpColor(Color.Orange, Color.Red), 20, safeDir.ToRotation() + PiOver4, 0.78f, starScale * .95F, 0, true, useAlt: true).Spawn();
            new KiraStar(Projectile.Center, Vector2.Zero, Color.White, 20, safeDir.ToRotation() + PiOver4, 0.78f, starScale * 0.90f, 0, true, useAlt: true).Spawn();
            base.OnKill(timeLeft);
        }
        public override bool ShouldUpdatePosition()
        {
            return AttackState == State.Shoot;
        }
        public SpriteBatch SB { get => Main.spriteBatch; }
        public float TrailRatios = 0;
        public override bool PreDraw(ref Color lightColor)
        {
            //特效复用
            Texture2D tex = Projectile.GetProjTexture();
            Vector2 pos = Projectile.Center - Main.screenPosition + Main.rand.NextVector2Circular(5, 5);
            Vector2 ori = tex.Size() / 2;
            float progress = Timer / 11f;
            for (int i = 0; i < 8; i++)
                SB.Draw(tex, pos + ToRadians(360f / 8 * i).ToRotationVector2() * 2, null, Color.White.ToAddColor(), Projectile.rotation, ori, Projectile.scale, 0, 0);
            SB.Draw(tex, pos, null, Color.Lerp(Color.White, Color.White.ToAddColor(), progress), Projectile.rotation, ori, Projectile.scale, 0, 0);
            DrawShaderTrail();
            return false;
        }
        public void DrawShaderTrail()
        {
            SB.EnterShaderArea();
            DrawTrails(LostbeltModAssets.Trail_ManaStreak.Texture, Color.Orange);
            DrawTrails(LostbeltModAssets.Trail_ManaStreak.Texture, Color.Red, 0.85f);
            DrawTrails(LostbeltModAssets.Trail_ManaStreak.Texture, Color.OrangeRed, 0.80f);
            DrawTrails(LostbeltModAssets.Trail_ManaStreak.Texture, Color.White, 0.40f);
            SB.EndShaderArea();
        }
        public void DrawTrails(Asset<Texture2D> useTex, Color drawColor, float multipleSize = 1f, float alphaValue = 1f, float offsetHeight = 1f)
        {
            if (Projectile.oldPos.Length < 3)
                return;
            Effect shader = LostbeltShaderAssets.StandardFlowShader;
            float laserLength = 150;
            shader.Parameters["LaserTextureSize"].SetValue(useTex.Size());
            shader.Parameters["targetSize"].SetValue(new Vector2(laserLength, useTex.Height()));
            shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * -120.2f);
            shader.Parameters["uColor"].SetValue(drawColor.ToVector4() * alphaValue);
            shader.Parameters["uFadeoutLength"].SetValue(1.1f);
            shader.Parameters["uFadeinLength"].SetValue(0.1f);
            shader.CurrentTechnique.Passes[0].Apply();

            DrawSetting drawSetting = new(useTex.Value);
            List<TrailDrawDate> trailDrawDates = [];
            int posCount = (int)(Projectile.oldPos.Length * Projectile.Opacity);
            for (int j = 0; j < posCount - 1; j++)
            {
                if (Projectile.oldPos[j] != Vector2.Zero)
                {
                    Vector2 drawPos = Projectile.oldPos[j] + new Vector2(Projectile.width / 2, Projectile.height / 2) + Projectile.SafeDir() * 5f + Main.rand.NextVector2Circular(5, 5);
                    trailDrawDates.Add(new(drawPos, drawColor, new Vector2(0, 15 * multipleSize * Projectile.scale), Projectile.oldRot[j]));
                }
            }
            TrailRender.RenderTrail([.. trailDrawDates], drawSetting);
        }
    }
    public class DragonSniperHeldProj : ModProjectile, ILocalizedModType
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public Player Owner => Main.player[Projectile.owner];
        public int AttackSpeed => Owner.ApplyWeaponAttackSpeed(Owner.HeldItem, Owner.HeldItem.useTime * Projectile.MaxUpdates, 5 * Projectile.MaxUpdates);
        public ref float Timer => ref Projectile.ai[0];
        public AnimationStruct Helper = new(3);
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Projectile.SetDefaultsHeldProj(10);
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override void AI()
        {
            if (Owner.HeldItem.type != ItemType<DragonSniper>())
            {
                Projectile.Kill();
                return;
            }

            Projectile.rotation = (Main.MouseWorld - Owner.Center).SafeNormalize(Vector2.UnitX).ToRotation();
            Projectile.spriteDirection = Projectile.direction = (Main.MouseWorld.X > Owner.Center.X).ToDirectionInt();
            Owner.ChangeDir(Projectile.direction);
            Owner.heldProj = Projectile.whoAmI;
            Owner.SetPlayerArmRotation(Projectile.rotation);
            Projectile.Center = Owner.MountedCenter;
            Projectile.position.Y += Owner.gfxOffY;
            if (Timer > 0)
            {
                float pro = Timer / AttackSpeed;
                float pullback = 9f;
                float rotation = (Owner.Center - Main.MouseWorld).ToRotation() * Owner.gravDir;
                if (pro <= 0.7f)
                {
                    float progress = 1 - pro / 0.7f;
                    pullback = Lerp(5f, 0f, EaseOutExpo(progress));
                    //pullback -= (9f) * (float)Math.Pow((0.6f - animProgress) / 0.6f, 2);
                }
                else
                {
                    float progress = (1 - pro) / 0.3f;
                    pullback = Lerp(0f, 7.5f, EaseOutExpo(progress));
                }
                Projectile.Center += Main.rand.NextVector2Circular(3 * pro, 3 * pro) + rotation.ToRotationVector2() * pullback;
                Timer--;
            }

            //Vector2 itemPosition = player.MountedCenter + itemRotation.ToRotationVector2() * pullback;
            //实际控制攻击

            if (Timer == 0 && Projectile.owner == Main.myPlayer && Main.mouseLeft)
            {
                Vector2 pos = Projectile.Center + Projectile.rotation.ToRotationVector2() * 75f * Projectile.scale + Projectile.rotation.ToRotationVector2().RotatedBy(PiOver2 * Owner.direction) * -7f * Projectile.scale;
                Vector2 vel = Projectile.rotation.ToRotationVector2().SafeNormalize(Vector2.UnitX) * 20f;
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), pos, vel, ProjectileType<DragonSniperNuke>(), Projectile.originalDamage, Projectile.knockBack, Projectile.owner);
                SoundEngine.PlaySound(LostbeltSoundAssets.Frostwave_Boom with { MaxInstances = 0, Pitch = -0.4f }, pos);
                ScreenShakeSystem.AddScreenShakes(pos, 16, 30, vel.ToRotation(), 0.1f);
                HandleBoomUp(pos);

                Timer = AttackSpeed;
            }
            Projectile.timeLeft = 2;
            Projectile.netUpdate = true;
            HandleFrames();
        }

        public void HandleBoomUp(Vector2 pos)
        {
            Vector2 vel = Vector2.Zero;
            //什么叫你写了这么多就为了处理这个特效爆炸？
            for (int i = 0; i < 35; i++)
            {
                vel = (TwoPi / 45f * i).ToRotationVector2() * 1.6f * Main.rand.NextFloat(0f, 1f) + Projectile.SafeDirByRot() * Main.rand.NextFloat(1f, 14f);
                Vector2 spawnpos = pos.ToRandCirclePos(4f) + vel.ToSafeNormalize() * Main.rand.NextFloat() * 2f;
                Color color = RandLerpColor(Color.Lerp(Color.Orange, Color.Red, 0.50f), Color.Orange);
                float scale = 0.40f * Main.rand.NextFloat(0.55f, 1.1f);
                ECSParticle.SmokeParticle(spawnpos, vel, color, Main.rand.Next(10, 41), RandRotTwoPi, Main.rand.NextFloat(.75f, 1f), scale, true, BlendState.Additive);

                vel += RandVelTwoPi(.4f, 3.5f);
                color = RandLerpColor(Color.Lerp(Color.Orange, Color.Red, 0.55f), Color.OrangeRed);
                scale = .35f * Main.rand.NextFloat(.65f, 1.1f);
                ECSParticle.SmokeParticle(spawnpos, vel, color, Main.rand.Next(10, 41), RandRotTwoPi, Main.rand.NextFloat(.75f, 1f), scale, true, BlendState.Additive);
            }
            for (int j = 0; j < 30; j++)
            {
                Vector2 dir = Projectile.SafeDirByRot();
                Vector2 pos2 = pos.ToRandCirclePos(3f) + dir * Main.rand.NextFloat(0f, 3f);
                ECSParticle.ShinyCrossStarECS(pos2, dir.ToRandVelocity(ToRadians(20f), 0f, 11f), RandLerpColor(Color.Orange, Color.OrangeRed), Main.rand.Next(15, 50), 1f, 1f * Main.rand.NextFloat(.7f, .9f), .2f);
            }
            for (int i = 0; i < 20; i++)
            {
                Vector2 pos2 = pos.ToRandCirclePos(2f);
                vel = RandVelTwoPi(.1f, 4.9f) + Projectile.SafeDirByRot() * Main.rand.NextFloat(1f, 11f);
                ECSParticle.ShinyCrossStarECS(pos2, vel, RandLerpColor(Color.Lerp(Color.Red, Color.Orange, .5f), Color.OrangeRed), Main.rand.Next(15, 50), 1f, .99f * Main.rand.NextFloat(.6f, 1f), .2f);
            }
            for (int i = 0; i < 20; i++)
            {
                Vector2 pos2 = pos.ToRandCirclePos(2f);
                vel = RandVelTwoPi(.1f, 4.9f) + Projectile.SafeDirByRot() * Main.rand.NextFloat(1f, 12f);
                ECSParticle.HRShinyOrb(pos2, vel, RandLerpColor(Color.Lerp(Color.Red, Color.Orange, .5f), Color.OrangeRed), Main.rand.Next(15, 50), 1f, .15f * Main.rand.NextFloat(.6f, 1f), .5f);
            }
        }

        public void HandleFrames()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 5)
            {
                Projectile.frame += 1;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 1)
            {
                Projectile.frame = 0;
            }
        }
        public override bool ShouldUpdatePosition() => false;
        public override bool? CanDamage() => false;
        public SpriteBatch SB { get => Main.spriteBatch; }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 offset = new(25 * Owner.direction, -20f);
            Texture2D tex = TextureAssets.Item[ItemType<DragonSniper>()].Value;
            //帧图
            Rectangle frame = tex.Frame(1, 2, 0, Projectile.frame);
            Vector2 origin = frame.Size() / 2;
            float rot = Projectile.rotation + (Projectile.spriteDirection == -1 ? Pi : 0);
            SpriteEffects flipSprite = Projectile.spriteDirection * Owner.gravDir == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;


            Vector2 pos = Projectile.Center - Main.screenPosition + offset.RotatedBy(rot) /*+ rotation.ToRotationVector2() * pullback*/;
            //这枪太他妈大了，得缩小一点
            float pro = Timer / AttackSpeed;
            float pullback;
            if (pro <= 0.7f)
            {
                float progress = 1 - pro / 0.7f;
                pullback = Lerp(2f, 0f, EaseOutExpo(progress));
                //pullback -= (9f) * (float)Math.Pow((0.6f - animProgress) / 0.6f, 2);
            }
            else
            {
                float progress = (1 - pro) / 0.3f;
                pullback = Lerp(0f, 2f, EaseOutExpo(progress));
            }

            for (int i = 0; i < 16; i++)
            {
                SB.Draw(tex, pos + (TwoPi / 16f * i).ToRotationVector2() * pullback, null, Color.White.ToAddColor(50) * pullback, rot, origin, Projectile.scale * .85f, flipSprite, 0);
            }
            SB.Draw(tex, pos, null, Color.White, rot, origin, Projectile.scale * .85f, flipSprite, 0);
            return false;
        }
    }
}
