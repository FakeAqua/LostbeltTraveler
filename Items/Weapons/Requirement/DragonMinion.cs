using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.Primitives.Trail;
using LostbeltTraveler.Globals.Graphics.Particles;
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
    public class DragonMinionBuff : ModBuff
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            bool minion = player.GetModPlayer<DragonPlayer>().dragonMinion = true;
            if (player.ownedProjectileCounts[ProjectileType<DragonMinionProj>()] > 0)
                player.GetModPlayer<DragonPlayer>().dragonMinion = true;
            if (!minion)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
                player.buffTime[buffIndex] = 18000;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            base.Update(npc, ref buffIndex);
        }
    }
    public class DragonPlayer : ModPlayer
    {
        public bool dragonMinion = false;
        public override void UpdateDead()
        {
            dragonMinion = false;
        }
    }
    public class DragonMinion : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public override void SetDefaults()
        {
            Item.width = 170;
            Item.height = 164;
            Item.damage = 560;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 10;
            Item.useAnimation = Item.useTime = 24;
            Item.knockBack = 0.25f;
            Item.buffType = BuffType<DragonMinionBuff>();
            Item.shoot = ProjectileType<DragonMinionProj>();

            Item.UseSound = SoundID.Item60;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;

            Item.rare = ItemRarityID.Red;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);
            var minion = Projectile.NewProjectileDirect(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI);
            minion.originalDamage = Item.damage;
            return false;
        }
    }
    public class DragonMinionProj : ModProjectile
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public bool justSpawned = false;
        public bool MinionReturnToOwner
        {
            get => Projectile.ai[0] == 1;
            set => Projectile.ai[0] = value ? 1 : 0;
        }
        public ref float AttackRates => ref Projectile.ai[1];
        public ref float FlameTimer => ref Projectile.ai[2];
        public ref float LerpValue => ref Projectile.localAI[0];
        public bool GrowUp = false;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionSacrificable[Type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 1f;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
        }
        public void MinionAntiClump(float pushForce = 0.05f)
        {
            for (int k = 0; k < Main.maxProjectiles; k++)
            {
                Projectile otherProj = Main.projectile[k];
                // Short circuits to make the loop as fast as possible
                if (!otherProj.active || otherProj.owner != Projectile.owner || !otherProj.minion || k == Projectile.whoAmI)
                    continue;

                // If the other Projectile is indeed the same owned by the same player and they're too close, nudge them away.
                bool sameProjType = otherProj.type == Projectile.type;
                float taxicabDist = Math.Abs(Projectile.position.X - otherProj.position.X) + Math.Abs(Projectile.position.Y - otherProj.position.Y);
                if (sameProjType && taxicabDist < Projectile.width)
                {
                    if (Projectile.position.X < otherProj.position.X)
                        Projectile.velocity.X -= pushForce;
                    else
                        Projectile.velocity.X += pushForce;

                    if (Projectile.position.Y < otherProj.position.Y)
                        Projectile.velocity.Y -= pushForce;
                    else
                        Projectile.velocity.Y += pushForce;
                }
            }
        }
        public override void AI()
        {
            //Set namespaces
            Player player = Main.player[Projectile.owner];
            var modPlayer = player.GetModPlayer<DragonPlayer>();

            //On spawn effects
            if (!justSpawned)
            {
                //Spawn dust
                int dustAmt = 36;
                for (int dustIndex = 0; dustIndex < dustAmt; dustIndex++)
                {
                    int randomDust = Utils.SelectRandom(Main.rand, new int[]
                    {
                        33,
                        89
                    });
                    Vector2 direction = Vector2.Normalize(Projectile.velocity) * new Vector2(Projectile.width / 2f, Projectile.height) * 0.75f;
                    direction = direction.RotatedBy((double)((dustIndex - (dustAmt / 2f - 1f)) * MathHelper.TwoPi / dustAmt), default) + Projectile.Center;
                    Vector2 dustVel = direction - Projectile.Center;
                    int water = Dust.NewDust(direction + dustVel, 0, 0, randomDust, dustVel.X * 1.75f, dustVel.Y * 1.75f, 100, default, 1.1f);
                    Main.dust[water].noGravity = true;
                    Main.dust[water].velocity = dustVel;
                }
                justSpawned = true;
            }
            //Set up buff and timeLeft
            bool typeCheck = Projectile.type == Type;
            player.AddBuff(BuffType<DragonMinionBuff>(), 3600);
            if (typeCheck)
            {
                if (player.dead)
                {
                    modPlayer.dragonMinion = false;
                }
                if (modPlayer.dragonMinion)
                {
                    Projectile.timeLeft = 2;
                }
            }

            //Anti sticky movement to prevent overlapping minions
            MinionAntiClump();

            //搜索敌对单位
            //这里没有用封装的方法，单独写的
            float maxDistance = 700f;
            Vector2 targetVec = Projectile.position;
            bool foundTarget = false;
            //If targeting something, prioritize that enemy
            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];
                if (npc.CanBeChasedBy(Projectile, false))
                {
                    float extraDist = (npc.width / 2) + (npc.height / 2);
                    float targetDist = Vector2.Distance(npc.Center, Projectile.Center);
                    bool canHit = true;
                    if (extraDist < maxDistance)
                        canHit = Collision.CanHit(Projectile.Center, 1, 1, npc.Center, 1, 1);
                    if (!foundTarget && targetDist < (maxDistance + extraDist) && canHit)
                    {
                        maxDistance = targetDist;
                        targetVec = npc.Center;
                        foundTarget = true;
                    }
                }
            }
            if (!foundTarget)
            {
                for (int npcIndex = 0; npcIndex < Main.maxNPCs; npcIndex++)
                {
                    NPC npc = Main.npc[npcIndex];
                    if (npc.CanBeChasedBy(Projectile, false))
                    {
                        float extraDist = (npc.width / 2) + (npc.height / 2);
                        float targetDist = Vector2.Distance(npc.Center, Projectile.Center);
                        bool canHit = true;
                        if (extraDist < maxDistance)
                            canHit = Collision.CanHit(Projectile.Center, 1, 1, npc.Center, 1, 1);
                        if (!foundTarget && targetDist < (maxDistance + extraDist) && canHit)
                        {
                            maxDistance = targetDist;
                            targetVec = npc.Center;
                            foundTarget = true;
                        }
                    }
                }
            }

            //距离玩家多少距离才让仆从发起传送。
            float returnDist = 1000f;
            if (foundTarget)
            {
                //有敌人的情况下索敌更广一点点。
                returnDist = 2200f;
            }
            if (Vector2.Distance(player.Center, Projectile.Center) > returnDist)
            {
                MinionReturnToOwner = true;
                Projectile.netUpdate = true;
            }
            //If a target is found, move toward it
            if (foundTarget && !MinionReturnToOwner)
            {
                Vector2 vecToTarget = targetVec - Projectile.Center;
                float targetDist = vecToTarget.Length();
                vecToTarget.Normalize();
                //If farther than 200 pixels, move toward it
                if (targetDist > 200f)
                {
                    float speedMult = (targetDist > 400f) ? 16f : (targetDist > 250) ? 9f : 5f;
                    vecToTarget *= speedMult;
                    Projectile.velocity = (Projectile.velocity * 40f + vecToTarget) / 41f;
                }
                //Otherwise, back it up slowly
                else
                {
                    float speedMult = -3f;
                    vecToTarget *= speedMult;
                    Projectile.velocity = (Projectile.velocity * 40f + vecToTarget) / 41f;
                }
            }

            //If not targeting something, act passively
            else
            {
                //Move faster if actively returning to the player
                float speedMult = 10f;
                if (MinionReturnToOwner)
                {
                    speedMult = 21f;
                }
                Vector2 vecToPlayer = player.Center - Projectile.Center + new Vector2(0f, -60f);
                float playerDist = vecToPlayer.Length();
                //Slow down if near the player
                if (playerDist < 200f && speedMult > 8f)
                {
                    speedMult = 1f;
                }
                //If close enough to the player, return to normal
                if (playerDist < 150f && MinionReturnToOwner && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                {
                    MinionReturnToOwner = false;
                    Projectile.netUpdate = true;
                }
                //If abnormally far, teleport to the player
                if (playerDist > 2000f)
                {
                    Projectile.Center = player.MountedCenter;
                    Projectile.netUpdate = true;
                }
                //Move toward player if more than 70 pixels away
                if (playerDist > 70f)
                {
                    vecToPlayer.Normalize();
                    vecToPlayer *= speedMult;
                    Projectile.velocity = (Projectile.velocity * 40f + vecToPlayer) / 41f;
                }
                //Move if still
                else if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f)
                {
                    Projectile.velocity.X = -0.15f;
                    Projectile.velocity.Y = -0.05f;
                }
            }

            //Update sprite direction and rotation
            if (foundTarget)
            {
                Projectile.spriteDirection = Projectile.direction = ((targetVec.X - Projectile.Center.X) > 0).ToDirectionInt();
                Projectile.rotation = Projectile.rotation.AngleTowards(Projectile.AngleTo(targetVec) + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi), 0.1f);
            }
            else
            {
                Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
                Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);
            }

            //Increment attack cooldown
            if (!MinionReturnToOwner && foundTarget)
            {
                if (Projectile.frame == 0)
                {
                    LerpValue = Lerp(LerpValue, 1.1f, 0.3f);
                    if (LerpValue >= 1.08f)
                    {
                        LerpValue = 1.1f;
                        Projectile.frame = 1;
                    }
                }
                else
                {
                    LerpValue = Lerp(LerpValue, 0, 0.2f);
                    if (LerpValue <= .02f)
                    {
                        LerpValue = 0;
                    }

                }
                if (Vector2.DistanceSquared(targetVec, Projectile.Center) < 600 * 600f)
                {
                    FlameTimer += 1;
                    if (FlameTimer > 3)
                    {
                        Vector2 velocity = targetVec - Projectile.Center;
                        velocity.Normalize();

                        //Add some inaccuracy for the vomit projectiles
                        float vomitSpeedMult = 8f;
                        Vector2 vomitVel = velocity * vomitSpeedMult;
                        vomitVel.Y += Main.rand.NextFloat(-3f, 3f) * 0.05f;
                        vomitVel.X += Main.rand.NextFloat(-3f, 3f) * 0.05f;
                        Vector2 yOffset = Projectile.rotation.ToRotationVector2().RotatedBy(PiOver2) * 5f + Projectile.rotation.ToRotationVector2() * 20f * Projectile.spriteDirection;
                        Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center.ToRandCirclePos(2) + yOffset, vomitVel, ProjectileType<DragonMinionFlame>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                        FlameTimer = 0;
                        if (Main.rand.NextBool(3))
                            new SmokeParticle(Projectile.Center.ToRandCirclePos(4) + yOffset, RandVelTwoPi(0.1f, 3.45f), RandLerpColor(Color.Orange, Color.White), 40, RandRotTwoPi, .68f, 0.22f, true).Spawn();
                    }
                    AttackRates += Main.rand.Next(1, 6);
                    if (AttackRates > 50f)
                    {
                        Vector2 velocity = targetVec - Projectile.Center;
                        velocity.Normalize();

                        //Add some inaccuracy for the vomit projectiles
                        float vomitSpeedMult = 15f;
                        Vector2 vomitVel = velocity.RotatedByRandom(PiOver4) * vomitSpeedMult;
                        vomitVel.Y += Main.rand.NextFloat(-3f, 3f) * 0.05f;
                        vomitVel.X += Main.rand.NextFloat(-3f, 3f) * 0.05f;
                        Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center.ToRandCirclePos(2) + vomitVel, vomitVel, ProjectileType<DragonSwordFireball>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                        AttackRates = 0;
                        Projectile.netUpdate = true;
                        for (int i = 0; i < 16; i++)
                        {
                            Vector2 yOffset = Projectile.rotation.ToRotationVector2().RotatedBy(PiOver2) * 5f + Projectile.rotation.ToRotationVector2() * 20f * Projectile.spriteDirection;
                            new SmokeParticle(Projectile.Center.ToRandCirclePos(4) + yOffset, vomitVel.ToRandVelocity(ToRadians(5), 0.3f, 17f) + RandVelTwoPi(0.1f, 0.45f), RandLerpColor(Color.Orange, Color.White), 40, RandRotTwoPi, .68f, 0.22f, true).Spawn();
                        }
                    }

                }
            }
            else
            {
                Projectile.frame = 0;
            }
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Rectangle frame = tex.Frame(1, 2, 0, Projectile.frame);
            Vector2 ori = frame.Size() / 2;
            float rot = Projectile.spriteDirection > 0 ? -PiOver4 : PiOver2 - PiOver4;
            if (Projectile.frame == 1)
                rot = Projectile.spriteDirection > 0 ? -ToRadians(25) : PiOver2 - ToRadians(70);
            SpriteEffects se = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 pos = Projectile.Center - Main.screenPosition;
            float scaleMult = 1 + LerpValue * .34f;
            if (Projectile.frame == 1)
            {
                float ratios = 1 - LerpValue;
                for (int i = 0; i < 16f; i++)
                {
                    sb.Draw(tex, pos + (TwoPi / 16f).ToRotationVector2() * 1.5f * ratios, frame, Color.Orange.ToAddColor() * ratios, Projectile.rotation + rot, ori, Projectile.scale * scaleMult, se, 0);

                }
            }
            sb.Draw(tex, pos, frame, Color.White, Projectile.rotation + rot, ori, Projectile.scale * scaleMult, se, 0);
            if (Projectile.frame == 1)
            {
                sb.EnterShaderArea();
                Vector2 offset = Projectile.rotation.ToRotationVector2() * -20f * scaleMult * Projectile.spriteDirection + Projectile.rotation.ToRotationVector2().RotatedBy(PiOver2) * -5f;
                Texture2D crossGlow = LostbeltModAssets.Particle_CrossGlow.Value;
                float scale = Projectile.scale * (1f - LerpValue) * .85f;
                sb.Draw(crossGlow, pos - offset, null, Color.OrangeRed, 0, crossGlow.ToOrigin(), scale * .30f, 0, 0);
                sb.Draw(crossGlow, pos - offset, null, Color.Orange, 0, crossGlow.ToOrigin(), scale * .28f, SpriteEffects.FlipHorizontally, 0);
                sb.Draw(crossGlow, pos - offset, null, Color.White, 0, crossGlow.ToOrigin(), scale * .26f, 0, 0);
                sb.EndShaderArea();
            }
            return false;
        }
    }
    public class DragonminionFireball : ModProjectile
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
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Summon;
            Projectile.friendly = true;
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
        public void OnFirstFrame()
        {
            Vector2 firePos2 = Projectile.Center - Projectile.SafeDirByRot() * 30f;
            Vector2 dir2 = Projectile.SafeDirByRot() * -1;
        }
        public override void AI()
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
            SpriteBatch SB = Main.spriteBatch;
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
    public class DragonMinionFlame : ModProjectile
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public override string Texture => GetInstance<DragonMinion>().Texture;
        public ref float Timer => ref Projectile.ai[0];
        public int LifeTime = 100;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 3;
            Projectile.MaxUpdates = 6;
            Projectile.timeLeft = LifeTime;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 8;
            base.SetDefaults();
        }
        public bool IsFristFrame = false;
        public float OriginalSpeed = 0;
        public override void AI()
        {
            if (!IsFristFrame)
            {
                OriginalSpeed = Projectile.velocity.Length();
                IsFristFrame = true;
            }

            DrawShinyParticle();
            DrawShinyFire();
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.SafeDir() * OriginalSpeed * .50f, 0.02f);
            Timer++;
        }
        public void DrawShinyParticle()
        {
            if (Main.rand.NextBool(5))
            {
                float lifeTimeRatios = Timer / LifeTime;
                Vector2 pos = Projectile.Center + Projectile.SafeDir().RotatedBy(PiOver2 * Main.rand.NextBool().ToDirectionInt()) + Projectile.SafeDir() * 5f;
                pos.Y += Main.rand.NextFloat(-15f, 15f);
                Vector2 offsetvec = (pos - Projectile.Center).ToSafeNormalize();
                Vector2 vel = offsetvec * OriginalSpeed * Lerp(.15f, .20f, lifeTimeRatios) * 1f + Projectile.SafeDir() * (Lerp(1f, 10f, lifeTimeRatios));
                Vector2 spawnPos = Projectile.Center + offsetvec * Main.rand.NextFloat(lifeTimeRatios * 50);
                spawnPos -= Projectile.SafeDir() * lifeTimeRatios * 100f;
                ECSParticle.ShinyCrossStarECS(spawnPos, vel, RandLerpColor(RandLerpColor(Color.DarkOrange, Color.Orange), Color.OrangeRed), 40, 1, .8f * Main.rand.NextFloat(0.5f, 0.75f), 0.2f);
            }
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }
        private void DrawShinyFire()
        {
            float lifetimeInterpolant = Timer / LifeTime;
            float particleScale = Lerp(0.10f, 1.24f, (float)Math.Pow(lifetimeInterpolant, 0.79f)) * 0.8f;
            particleScale *= (1f + lifetimeInterpolant * Main.rand.NextFloat(0.073f, .25f));
            float opacity = Utils.GetLerpValue(0.9f, 0.57f, lifetimeInterpolant, true);
            float fadeToBlack = Utils.GetLerpValue(0.67f, 0.92f, lifetimeInterpolant, true);
            Color fireColor = Color.Lerp(Color.OrangeRed, Color.Orange, Main.rand.NextFloat(0.2f, 0.9f));
            fireColor = Color.Lerp(fireColor, Color.White, fadeToBlack) with { A = 80 };
            Color brightColor = RandLerpColor(RandLerpColor(Color.DarkOrange, Color.OrangeRed), Color.WhiteSmoke);
            brightColor = Color.Lerp(brightColor, Color.Orange, fadeToBlack);

            for (int i = 0; i < 2; i++)
            {
                Vector2 vel = Projectile.velocity * .1f * i + Projectile.velocity.ToSafeNormalize() * lifetimeInterpolant * Main.rand.NextFloat(.5f, 4f);
                if (Main.rand.NextBool(3))
                {
                    vel.X *= 1f;
                    vel.Y *= 2f;
                }
                float brigherScale = particleScale * .90f;
                if (Main.rand.NextBool(3))
                    brigherScale *= Main.rand.NextFloat(1.1f, 1.3f);
                new SmokeParticle(Projectile.Center, vel, brightColor, 12, RandRotTwoPi, opacity * .7f, brigherScale * .9f, false).Spawn();
                new SmokeParticle(Projectile.Center, vel, fireColor, 12, RandRotTwoPi, opacity * .7f, particleScale * 1.1f, true).SpawnToPriorityNonPreMult();
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}
