using ContinentOfJourney;
using ContinentOfJourney.Items.FielderSentries;
using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Buffs.GoodBuffs;
using LostbeltTraveler.Buffs.Status;
using LostbeltTraveler.Core;
using LostbeltTraveler.Core.NetCode;
using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.ScreenEffect;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Globals.Graphics.Particles;
using LostbeltTraveler.Globals.Keybinds;
using LostbeltTraveler.Globals.Methods;
using LostbeltTraveler.Globals.Players.Dashes;
using LostbeltTraveler.Globals.Players.InnerCooldown.Methods;
using LostbeltTraveler.Globals.Systems;
using LostbeltTraveler.Items.Accessories;
using LostbeltTraveler.Items.Armor.ExecutorVanillaHead;
using LostbeltTraveler.Items.Armor.Reaper;
using LostbeltTraveler.Items.Armor.SaintChurch;
using LostbeltTraveler.Items.Useables;
using LostbeltTraveler.Items.Weapons.Executor.Assistance;
using LostbeltTraveler.Items.Weapons.Executor.ColdSteel;
using LostbeltTraveler.Projs.Executor.Assistance;
using LostbeltTraveler.Projs.General;
using LostbeltTraveler.Projs.Melee;
using rail;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Players
{
    public partial class LostbeltPlayer : ModPlayer
    {
        // ==================== PostUpdateEquips ====================
        public override void PostUpdateEquips()
        {
            HandleTerraRecipe();
            HandleLoveRing();
            UpdateFloretProtectorHerbSpawn();
            UpdateHerbBuff();
            UpdateDesterrannacht();
            UpdateDiverArmorJellyfishSpawn();
            UpdateHeadExecutor();
            UpdateMaidReaper();
            UpdateExecutorKnifeMark();
            UpdateShorthandFunction();
            UpdateDrowingEffect();
            UpdatePocketMirror();
            UpdateSaintChurch();
            UpdateBloodThornCrown();
            UpdateChargeFeather();
        }

        /// <summary>
        /// 似乎是更新顺序的问题，导致这里必须得使用后于updateaccessories的钩子去更新充能羽翼系列饰品的效果
        /// </summary>
        public void UpdateChargeFeather()
        {
            if (wingsType <= 0)
                return;
            bool isCorrectBoth(int wing, int feather) => wingsType == wing && wingsChargeType == feather;
            //雏鸟
            if (isCorrectBoth(ItemID.CreativeWings, ItemType<ChargeFeatherBroken>()))
            {
                Player.wingTimeMax += GetSeconds(ChargeFeatherBroken.WingTimeMax);
            }
            //鱼鳍
            if (wingsChargeType == ItemType<ChargeFeatherFish>())
            {
                Player.fishingSkill += ChargeFeatherFish.FishingPower;
            }
            //天使
            if (wingsChargeType == ItemType<ChargeFeatherAngel>())
            {
                Player.statLifeMax2 += ChargeFeatherAngel.MaxLife;
                if (wingsType == ItemID.AngelWings)
                {
                    Player.statDefense += ChargeFeatherAngel.Defense;
                }
            }
            //恶魔
            if (wingsChargeType == ItemType<ChargeFeatherDemon>())
            {
                Player.maxRunSpeed *= (1 + ChargeFeatherDemon.RunSpeedAcce);
                if (wingsType == ItemID.DemonWings)
                {
                    Player.GetDamage<GenericDamageClass>() += ChargeFeatherDemon.Damage;
                    Player.GetCritChance<GenericDamageClass>() += ChargeFeatherDemon.Crit;
                }
            }
            //节日
            if (wingsChargeType == ItemType<ChargeFeatherFestival>())
            {
                Player.statLifeMax2 += ChargeFeatherFestival.MaxLife;
                if (wingsType == ItemID.FestiveWings)
                    Player.statDefense += ChargeFeatherFestival.Defense;
            }
            if (wingsChargeType == ItemType<ChargeFeatherTattered>())
            {
                Player.maxRunSpeed *= (1 + ChargeFeatherTattered.RunSpeedAcce);
                if (wingsType == ItemID.TatteredFairyWings)
                {
                    Player.GetDamage<GenericDamageClass>() += ChargeFeatherTattered.Damage;
                    Player.GetCritChance<GenericDamageClass>() += ChargeFeatherTattered.Crit;

                }
            }
            if (wingsChargeType == ItemType<ChargeFeatherButterfly>())
            {
                Player.wingTimeMax += GetSeconds(ChargeFeatherButterfly.WingTime);
                if (wingsType == ItemID.ButterflyWings)
                {
                    float lerpProgress = Player.wingTime / (float)Player.wingTimeMax;
                    if (lerpProgress < .05f)
                    {
                        Player.wingTime = (Player.wingTimeMax * .05f);
                    }
                }
            }
            if (wingsChargeType == ItemType<ChargeFeatherLeafs>())
            {
                Player.maxRunSpeed *= (1 + ChargeFeatherLeafs.RunSpeedAcc);
                if (wingsType == ItemID.LeafWings)
                {
                    Player.whipRangeMultiplier += ChargeFeatherLeafs.WhipRange;
                }

            }
            if (isCorrectBoth(ItemID.BetsyWings, ItemType<ChargeFeatherBetsy>()))
            {
                if (LostbeltNetInput.ActionSkill && Player.miscCounter % 3 == 0)
                {
                    if (Player.miscCounter % 9 == 0)
                        SoundEngine.PlaySound(LostbeltSoundAssets.HymnFireball_Release, Player.Center);
                    Vector2 position = Player.Center - 10 * Vector2.UnitY;
                    Vector2 dir = position.GetNormalVector2(Player.LocalMouseWorld());
                    Vector2 pos = position + dir * 10 * Player.direction;
                    Player.ChangeDir((dir.X > 0).ToDirectionInt());
                    float randRot = ToRadians(7.5f);
                    ScreenShakeSystem.AddScreenShakes(position, 4, 4, Main.rand.NextFloat(TwoPi));
                    int damage = 150;
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 randomVelocity = dir.RotatedByRandom(randRot) * Main.rand.NextFloat(0.88f, 1.12f);
                        Projectile proj = Projectile.NewProjectileDirect(Player.GetSource_FromThis(), pos, randomVelocity * 15f, ProjectileType<BetsySuperFlame>(), damage, 1, Player.whoAmI);
                    }
                }
            }
        }
        public void UpdateBloodThornCrown()
        {
            if (!bloodThronCrown)
                return;
            if (LostbeltMethods.AnyBossNPCS())
            {
                Player.GetDamage<GenericDamageClass>() += BloodThornCrown.DamageMult;
            }
            else
            {
                Player.GetDamage<GenericDamageClass>() -= BloodThornCrown.DamageReduce;
                Player.GetCritChance<GenericDamageClass>() -= BloodThornCrown.Crit;
                Player.endurance += BloodThornCrown.DR;
            }
        }

        public void UpdateSaintChurch()
        {
            if (!Player.HasBuff<SaintChurchBuff>())
                saintChurchLastStanding = 0;
            if (!(saintChurch && saintChurchLastStanding > 0))
                return;
            foreach (var p in Main.ActivePlayers)
            {
                if (p.team == Player.team && p.whoAmI != Player.whoAmI)
                {
                    p.GetDamage<GenericDamageClass>() += SaintChurchHead.DamageBonus * saintChurchLastStanding;
                    p.GetCritChance<GenericDamageClass>() += SaintChurchHead.CritBonus * saintChurchLastStanding;
                }
            }
            Player.aggro += SaintChurchHead.Aggro;
            Player.GetDamage<GenericDamageClass>() += SaintChurchHead.DamageBonus * saintChurchLastStanding;
            Player.GetCritChance<GenericDamageClass>() += SaintChurchHead.CritBonus * saintChurchLastStanding;
            Player.AddBuff(BuffID.PotionSickness, 2);
        }

        public void UpdatePocketMirror()
        {
            if (!pocketMirror)
                return;
            if (LostbeltKeybinds.GeneralActionKeybind.JustPressed)
            {
                Vector2 spawnPoint = new(Main.spawnTileX * 16, Main.spawnTileY * 16);
                if (Player.SpawnX != -1 && Player.SpawnY != -1)
                    spawnPoint = new Vector2(Player.SpawnX * 16, Player.SpawnY * 16 - 32);
                Player.UnityTeleport(spawnPoint);
            }
        }

        public void HandleTerraRecipe()
        {
            //由于需要提供血上限，这里基本上得往reset这里写内容。
            if (!terraRecipe)
                return;
            if (terrariaRecipeEatenFoodCounts >= 5)
            {
                terrariaRecipeEatenFoodCounts = 0;
                ScarletSound(SoundID.ResearchComplete, Player.Center);
            }
            //全局常态提供血上限。
            Player.statLifeMax2 += (terrariaRecipeEatenList.Count / 5) * TerrariaRecipe.StatLifePerAdd;
        }
        public void HandleLoveRing()
        {
            if (!loveRing)
                return;
            foreach (var player in Main.ActivePlayers)
            {
                bool isLegalPlayer = player.whoAmI != Player.whoAmI && player.active;
                bool maleFemale = (player.Male && !player.Male) || (!player.Male && player.Male);
                float distance = Vector2.Distance(player.Center, Player.Center);
                if (isLegalPlayer && distance < 450f && maleFemale)
                {
                    player.Lostbelt().isBeingLove = true;

                }
            }

        }

        public void UpdateFloretProtectorHerbSpawn()
        {
            if (!Player.HasInnerCooldown(InnerCooldownType.BeniEmmaRice) && floretProtectorExecutor && Player.IsOwnerSide())
            {
                if (Main.rand.NextBool())
                    return;
                bool collision = false;
                Vector2 spawnPos = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(Player.Center, new Vector2(1300f, 700f)));
                float recDistanceMult = 1f;
                while (!collision)
                {
                    //添加一个安全性的收缩倍率检查，如果收缩的倍率已经少于0.5f,立刻跳出去避免出现可能的死生成
                    //也就是说我们会确保其生成一个，但只会进行一定程度的安全检查
                    if (Collision.SolidCollision(spawnPos, 100, 100) && recDistanceMult > 0.5f)
                    {
                        recDistanceMult -= 0.1f;
                        //一定程度上收缩倍率以查看是否可能玩家处于一些物块内的情况，如洞穴层
                        //这里有个问题是，可能不会很完美地检测所有情况，如玩家处于地表站立在地面上时，有草药生成在了地下，则重新取位时可能会因此收缩了一定的距离
                        //但应该问题不大。
                        spawnPos = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(Player.Center, new Vector2(1300f * recDistanceMult, 700f * recDistanceMult)));
                    }
                    else
                    {
                        //在最后我们在推开这个草药一定距离。
                        if ((spawnPos - Player.Center).LengthSquared() < 50f * 50f)
                            spawnPos += RandVelTwoPi(30f, 70f);
                        break;
                    }
                }
                Projectile proj = Projectile.NewProjectileDirect(Player.GetSource_FromThis(), spawnPos, RandVelTwoPi(2f, 6f), ProjectileType<FloatingPlants>(), 0, 0, Player.whoAmI);
                proj.rotation = RandRotTwoPi;
                proj.ai[1] = Main.rand.Next(0, 7);
                Player.AddInnerCooldown(InnerCooldownType.BeniEmmaRice, 40);
            }
        }

        public void UpdateHerbBuff()
        {
            if (!floretProtectorExecutor)
                return;
            if (!Player.HasBuff<HerbBagBuff>())
                return;
            //遍历所有的目标准备赋效果
            //妈的，天塌下来了你也只能这么打表
            //太阳花，已有buff
            if (protectorHerbTimerList[0] > 0)
            {
                Player.lifeRegen += 2;
                Player.statDefense += 10;
            }
            //月光花，已有buff
            if (protectorHerbTimerList[1] > 0)
            {
                Player.endurance += 0.08f;
            }
            //闪耀根，已有buff
            if (protectorHerbTimerList[2] > 0)
            {
                Player.pickSpeed -= 0.3f;

            }
            //水叶草，已有Buff
            if (protectorHerbTimerList[3] > 0)
            {
                Player.luck += 25;
            }
            //死亡草，已有Buff
            if (protectorHerbTimerList[4] > 0)
            {
                Player.GetDamage<ExecutorDamageClass>() += 0.10f;
                Player.GetCritChance<ExecutorDamageClass>() += 10f;
            }
            //火焰花，已有Buff
            if (protectorHerbTimerList[6] > 0)
            {
                if (Collision.LavaCollision(Player.Center, Player.width, Player.height))
                {
                    Player.GetDamage<ExecutorDamageClass>() += 0.15f;
                    Player.GetCritChance<ExecutorDamageClass>() += 15f;
                }
            }
        }
        public void UpdateDesterrannacht()
        {
            if (!souloftheTidalMark)
                return;
            int minLife = desterrennacht ? Desterrennacht.LifeThreshold : 50;
            if (Player.statLife < minLife)
                Player.statLife = minLife;
        }
             public void UpdateDiverArmorJellyfishSpawn()
        {
            if (!diverArmor)
                return;
            if (Player.miscCounter % 45 == 0 && Player.velocity.LengthSquared() > 2f * 2f && Player.IsOwnerSide())
            {
                int damage = (int)Player.GetTotalDamage<ExecutorDamageClass>().ApplyTo(50);
                Projectile proj = Projectile.NewProjectileDirect(Player.GetSource_FromThis(), Player.Center, Player.velocity.ToSafeNormalize() * -3f, ProjectileType<DiverJellyFish>(), damage, 0f, Player.whoAmI);
                proj.timeLeft = GetSeconds(10);

            }
        }
        #region 代行者的矿石套
        public void UpdateHeadExecutor()
        {
            if (Main.myPlayer != Player.whoAmI)
                return;
            TitaniumHeadExecutorShard();
            AdamantiteHeadExecutorThunder();
            ChlorophyteHeadExecutorCrystal();
        }

        public void TitaniumHeadExecutorShard()
        {
            bool canUseTitan = titaniumHeadExecutor && Player.HeldItem.IsExecutorWeapon() && Player.MouseLeftOf() && Player.miscCounter % 9 == 0 && !Player.IsInInventory();
            if (!canUseTitan)
                return;
            int damage = (int)Player.GetTotalDamage<ExecutorDamageClass>().ApplyTo(TitaniumHeadExecutor.ShardDamage);
            Vector2 dir = Player.Center.GetNormalVector2(Main.MouseWorld);
            Vector2 off = dir.RotatedByRandom(PiOver4).ToSafeNormalize();
            Vector2 pos = Player.Center - off * Main.rand.NextFloat(0.7f, 1.1f) * 120f;
            Projectile.NewProjectileDirect(Player.GetSource_FromThis(), pos, dir * 14f, ProjectileType<TitaniumShardHoming>(), damage, 1f, Player.whoAmI);
        }

        public void AdamantiteHeadExecutorThunder()
        {
            if (!(adamantiteHeadExecutor && Player.HasInnerCooldown(InnerCooldownType.AdamantiteExecutorThunder)))
                return;
            float searchDist = 1100f;
            List<NPC> availableTarget = [];
            foreach (NPC needTar in Main.ActiveNPCs)
            {
                if (availableTarget.Count >= AdamantiteHeadExecutor.ThunderCount)
                    break;
                bool legalTarget = needTar.CanBeChasedBy();
                float distPerTar = Vector2.Distance(needTar.Center, Player.Center);
                if (legalTarget && distPerTar < searchDist)
                {
                    availableTarget.Add(needTar);
                }
            }
            if (availableTarget.Count == 0)
            {
                return;
            }
            for (int i = 0; i < availableTarget.Count; i++)
            {
                NPC target = availableTarget[i];
                if (!target.IsLegal())
                    continue;

                ScarletSound(LostbeltSoundAssets.Lightning_Strike, Player.Center, 0.4f, 1, 0.35f);
                Vector2 pos = Player.Center - Vector2.UnitY * Main.rand.NextFloat(800f, 900f) + Vector2.UnitX * Main.rand.NextFloat(0f, 20f) * Main.rand.NextBool().ToDirectionInt();
                Vector2 vel = (target.Center - pos).ToSafeNormalize() * Main.rand.NextFloat(4f, 9f);
                int damage = (int)Player.GetTotalDamage<ExecutorDamageClass>().ApplyTo(AdamantiteHeadExecutor.ThunderDamage);
                Projectile proj = Projectile.NewProjectileDirect(Player.GetSource_FromThis(), pos, vel, ProjectileType<AdamantiteThunder>(), damage, 3f, Player.whoAmI);
                ((AdamantiteThunder)proj.ModProjectile).CurTarget = target;
            }
            Player.AddInnerCooldown(InnerCooldownType.AdamantiteExecutorThunder, AdamantiteHeadExecutor.StrikeChance);
        }

        public void ChlorophyteHeadExecutorCrystal()
        {
            if (chlorophyteHeadExecutor && !Player.HasProj<ChlorophyteCrystalExecutor>(out int crystalLeaf))
            {
                int damage = (int)Player.GetTotalDamage<ExecutorDamageClass>().ApplyTo(ChlorophyteHeadExecutor.BoltDamage);
                Projectile proj = Projectile.NewProjectileDirect(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, crystalLeaf, damage, 2, Player.whoAmI);
                proj.originalDamage = damage;
            }

        }
        #endregion

        public void UpdateMaidReaper()
        {
            if (!maidReaperArmor)
                return;
            if (Player.IsHolding<CrimsonScythe>())
            {
                infiniteFlightTime = true;
                Player.ApplyDash(ScarletContent.DashType<CrimsonScytheDash>());
                Player.jumpSpeed += 1.6f;
                Player.runAcceleration *= 1.40f;
                Player.moveSpeed += .35f;
                Player.GetDamage<ExecutorDamageClass>() += ReaperHead.TlipocaScytheDamage;
                Player.GetCritChance<ExecutorDamageClass>() += ReaperHead.TlipocaScytheCrits;
                Player.statDefense += ReaperHead.TlipocaDefense;
                critDamageExecutor += ReaperHead.TlipocaCritDamage;
            }

            bool checkExecution = Player.CheckCurWeaponExecution(Player.HeldItem.type);
            if (checkExecution)
                return;
            if (LostbeltKeybinds.GeneralActionKeybind.JustPressed && maidReaperIndex != -1 &&
                Player.HeldItem.DamageType.CountsAsClass<ExecutorDamageClass>() && !Player.HasInnerCooldown(InnerCooldownType.TlipocaHeal) && Player.IsOwnerSide())
            {
                NPC npc = Main.npc[maidReaperIndex];
                if (npc.IsLegal())
                {
                    ScarletSound(LostbeltSoundAssets.Tlipoca_SoulAbsorb, Player.Center, 0.85f, 1, 0.3f);
                    float ratios = Clamp((float)ExecutionListStored[Player.HeldItem.type] / (float)LostbeltList.ExecuteRequests[Player.HeldItem.type], 0, 1);
                    Projectile proj = Projectile.NewProjectileDirect(Player.GetSource_FromThis(), npc.Center, RandVelTwoPi(6, 9), ProjectileType<MaidReaperHeal>(), 0, 0, Player.whoAmI);
                    proj.ai[2] = ratios;
                    ((MaidReaperHeal)proj.ModProjectile).CurTarget = npc;
                    Player.RemoveExecutionProgress(Player.HeldItem.type);
                    Player.AddInnerCooldown(InnerCooldownType.TlipocaHeal, GetSeconds((int)Lerp(0, ReaperHead.MaidReaperMaxHealCooldown, ratios)));
                }
            }
        }

        public void UpdateExecutorKnifeMark()
        {
            if (!Player.HeldItem.IsExecutorWeapon())
                return;
            //希望之星
            if (KnifeMarkIndex == ProjectileType<StarofHoperMark>()
                && !Player.IsInInventory() && Main.mouseLeft && Player.miscCounter % 25f == 0 && Player.IsOwnerSide())
            {
                for (int i = 0; i < 1; i++)
                {
                    Vector2 pos = Player.Center - Vector2.UnitY * Main.rand.NextFloat(600, 700) + Main.rand.NextFloat(80, 120) * i * Vector2.UnitX * Main.rand.NextBool().ToDirectionInt() - ((Main.MouseWorld.X - Player.Center.X) > 0).ToDirectionInt() * Vector2.UnitX * 100;
                    int dmg = (int)Player.GetTotalDamage<ExecutorDamageClass>().ApplyTo(20);
                    Projectile proj = Projectile.NewProjectileDirect(Player.GetSource_FromThis(), pos, pos.GetNormalVector2(Main.MouseWorld) * 30f, ProjectileType<StarofHopeStar>(), dmg, 1f, Player.whoAmI);
                    proj.Lostbelt().HasExecutionMechanic = true;
                }

            }
            //克苏鲁的眼泪
            if (tearEyeBuff > 0)
            {
                Player.AddBuff(BuffID.Wet, GetSeconds(1));
                Player.endurance += (1 + Condition.Hardmode.IsMet().ToInt() + DownedBossSystem.downedBarrier.ToInt()) * .05f;
            }
            //幽灵短匕
            if (Player.MouseLeftOf() && !Player.IsInInventory()
                && Player.miscCounter % GhostKnife.MarkGhostKnifeAttackSpeed == 0f
                && KnifeMarkIndex == GetInstance<GhostKnifeMark>().Type
                && Player.IsOwnerSide())
            {
                int applyDamage = (int)Player.GetTotalDamage<ExecutorDamageClass>().ApplyTo(GhostKnife.MarkGhostKnifeAttackDamage);
                Vector2 dir = Player.Center.GetNormalVector2(Main.MouseWorld);
                Vector2 off = dir.RotatedBy(PiOver2 * Main.rand.NextBool().ToDirectionInt()).ToSafeNormalize();
                Vector2 pos = Player.Center - dir * 60f + off * 60;
                Projectile proj = Projectile.NewProjectileDirect(Player.GetSource_FromThis(), pos, pos.GetNormalVector2(Main.MouseWorld) * 14f, ProjectileType<GhostKnifeProj>(), applyDamage, 1f, Player.whoAmI);
                proj.ai[1] = 1;
                pos = proj.Center + proj.Size / 2;
                for (int i = 0; i < 8; i++)
                {
                    ECSParticle.ShinyCrossStarECS(pos, RandVelTwoPi(1.2f, 2.2f), Color.White, 40, 1, 0.4f);
                }
                for (int i = 0; i < 6; i++)
                {
                    ECSParticle.SmokeParticle(pos, RandVelTwoPi(1.2f, 3.2f), RandLerpColor(Color.White, Color.LightSkyBlue), 40, 1, 1, 0.21f, blendstate: BlendState.Additive);
                }
                ECSParticle.StarShape(pos, proj.velocity.ToSafeNormalize() * .01f, Color.LightBlue, 40, 1, 0.94f);
                ECSParticle.StarShape(pos, proj.velocity.RotatedBy(PiOver2).ToSafeNormalize() * .01f, Color.LightBlue, 40, 1, 0.94f);
            }
            //公爵长牙
            if (KnifeMarkIndex == ProjectileType<FishronKnifeMark>())
            {
                Player.GetDamage<ExecutorDamageClass>() += FishronKnife.DamageBuff;
            }
            //钢制匕首
            if (KnifeMarkIndex == ProjectileType<DungeonKnifeMark>())
            {
                Player.statDefense += 8;

            }
        }

        public void UpdateShorthandFunction()
        {
            if (infiniteBreath)
            {
                Player.breathCD = 0;
            }
            if (ankhShieldImmnue)
            {
                Player.buffImmune[67] = true;
                Player.buffImmune[24] = true;
                Player.buffImmune[323] = true;
                Player.buffImmune[33] = true;
                Player.buffImmune[30] = true;
                Player.buffImmune[20] = true;
                Player.buffImmune[36] = true;
                Player.buffImmune[31] = true;
                Player.buffImmune[22] = true;
                Player.buffImmune[35] = true;
                Player.buffImmune[46] = true;
                Player.buffImmune[32] = true;
                Player.buffImmune[23] = true;
                Player.buffImmune[156] = true;
                Player.buffImmune[70] = true;
                Player.buffImmune[39] = true;
                Player.buffImmune[69] = true;
            }
            if (terraSparkBoostImmnue)
            {
                Player.iceSkate = true;
                Player.waterWalk = true;
                Player.lavaImmune = true;
                Player.lavaRose = true;
            }
            if (celesitalShellEffect)
            {
                Player.GetAttackSpeed(DamageClass.Melee) += 0.1f;
                Player.GetDamage(DamageClass.Generic) += 0.1f;
                Player.GetCritChance(DamageClass.Generic) += 3f;
                Player.GetKnockback(DamageClass.Summon) += 0.5f;
                Player.pickSpeed += 0.15f;
            }
        }

        public void UpdateDrowingEffect()
        {
            if (Player.IsInwater())
            {
                if (LightofHorizon)
                {
                    Lighting.AddLight(Player.Center, new Vector3(255, 255, 255) / 35f);
                }
            }
            else
            {
                if (LightofHorizon)
                    Lighting.AddLight(Player.Center, new Vector3(255, 255, 255) / 70f);

            }
        }
    }
}
