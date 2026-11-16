using ContinentOfJourney;
using LostbeltTraveler.Buffs.Debuffs;
using LostbeltTraveler.Buffs.GoodBuffs;
using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Methods;
using LostbeltTraveler.Globals.Players;
using LostbeltTraveler.Items.Accessories;
using LostbeltTraveler.Items.Armor.WesternBoy;
using LostbeltTraveler.Items.Weapons.Executor.Assistance;
using LostbeltTraveler.Projs;
using LostbeltTraveler.Projs.Executor.Assistance;
using LostbeltTraveler.Projs.General;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Instances.Projs
{
    public partial class LostbeltGlobalProjs : GlobalProjectile
    {
        public bool HasCreatedProj = false;
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (ScarletProjIDSets.GiantKiller[projectile.type] && ScarletNPCIDSets.Giant[target.type])
            {
                modifiers.FinalDamage *= BalacingHandler.GiantKillerDamageMult;
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player Owner = Main.player[projectile.owner];
            switch (projectile.type)
            {
                case ProjectileID.NightsEdge:
                case ProjectileID.TrueNightsEdge:
                    target.AddBuff(BuffType<ForeverNightBuff>(), 60);
                    break;
            }
            //判定是否为悠悠球
            bool isYoyo = projectile.aiStyle == ProjAIStyleID.Yoyo && projectile.DamageType.CountsAsClass<ExecutorDamageClass>();
            if (HasExecutionMechanic && ((!AddExecutionHit && projectile.numHits < 1) || isYoyo))
            {
                HandleCowboy(Owner, target);
                HandlePendant(Owner, target);
                if (!isYoyo)
                    AddExecutionHit = true;
            }
            HandleMaidReaperOnHit(Owner, projectile, target);
            HandleBlackKeyOnHit(Owner);
            if (projectile.DamageType.CountsAsClass<ExecutorDamageClass>())
            {
                if (Owner.Lostbelt().KnifeMarkIndex == ProjectileType<TearEyeMark>())
                {
                    Owner.Lostbelt().tearEyeBuff = GetSeconds(1);
                }
                if (Owner.Lostbelt().KnifeMarkIndex == ProjectileType<MoltenKnifeMark>() && projectile.type != ProjectileType<MoltenKnifeBoom>() && projectile.type != ProjectileType<MoltenKnifeProj>() && Owner.Lostbelt().markProjTimer == 0 && HasExecutionMechanic)
                {
                    int dmg = (int)(projectile.originalDamage * MoltenKnife.BoomDamageMult);
                    Owner.Lostbelt().markProjTimer = GetInstance<MoltenKnife>().Item.useTime;
                    Projectile proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), target.Center, Vector2.Zero, ProjectileType<MoltenKnifeBoom>(), dmg, 1f, Owner.whoAmI);
                }
                if (Owner.Lostbelt().KnifeMarkIndex == ProjectileType<GrassKnifeMark>() && projectile.type != ProjectileType<GrassKnifePoisonProj>() && projectile.type != ProjectileType<InvisBoom>() && Owner.Lostbelt().markProjTimer == 0)
                {
                    Owner.Lostbelt().markProjTimer = GetInstance<GrassKnife>().Item.useTime;
                    if (target.HasBuff<GrassPoison>())
                    {
                        foreach (var proj in Main.ActiveProjectiles)
                        {
                            if (proj.type != ProjectileType<GrassKnifePoisonProj>())
                                continue;
                            if (proj.owner != Owner.whoAmI)
                                continue;
                            ((GrassKnifePoisonProj)proj.ModProjectile).StackLevel += 1;
                            target.AddBuff(BuffType<GrassPoison>(), GetSeconds(10));
                            proj.timeLeft = GetSeconds(10);
                        }
                    }
                    else
                    {
                        if (!HasCreatedProj)
                        {
                            target.AddBuff(BuffType<GrassPoison>(), GetSeconds(10));
                            int damageValueInstance = 25 * (1 + DownedBossSystem.downedBarrier.ToInt() * 2 + Condition.Hardmode.IsMet().ToInt());
                            Projectile proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), target.Center, Vector2.Zero, ProjectileType<GrassKnifePoisonProj>(), damageValueInstance / 5, 0, Owner.whoAmI);
                            HasCreatedProj = true;
                            proj.originalDamage = damageValueInstance;
                            ((GrassKnifePoisonProj)proj.ModProjectile).CurTarget = target;
                        }
                    }
                }
            }
            if (Owner.Lostbelt().KnifeMarkIndex != -1)
                Owner.Lostbelt().KnifeMarkIndex = -1;
        }

        public void HandlePendant(Player owner, NPC target)
        {
            LostbeltPlayer usPlayer = owner.Lostbelt();
            if (owner.Lostbelt().pendantLevel < 1)
                return;
            if (!owner.HeldItem.IsExecutorWeapon())
                return;

            foreach (var keys in usPlayer.ExecutionListStored.Keys.ToList())
            {
                if (LostbeltMethods.CheckExecuteTypes(keys, ExecutorWeaponType.Assistance))
                {
                    owner.AddExecutionTimeDirectly(keys, 1 * usPlayer.pendantLevel);
                }
            }
        }

        public void HandleCowboy(Player Owner, NPC target)
        {
            if (Owner.Lostbelt().cowboyExecutor && Owner.Lostbelt().cowboyRevolverTimer == 0)
            {
                int revolverDamage = (int)Owner.GetTotalDamage<ExecutorDamageClass>().ApplyTo(WesternBoyHead.RevolerDamage);
                Projectile proj2 = Projectile.NewProjectileDirect(Owner.GetSource_FromThis(), target.Center, (-Vector2.UnitY).ToRandVelocity(ToRadians(35f), 9f, 11f), ProjectileType<CowboyRevolverProj>(), revolverDamage, 0f, Owner.whoAmI);
                proj2.timeLeft = 300;
                if (target.CanBeChasedBy())
                    ((CowboyRevolverProj)proj2.ModProjectile).CurTarget = target;
                Owner.Lostbelt().cowboyRevolverTimer = 30;
            }
        }

        public void HandleBlackKeyOnHit(Player owner)
        {
            bool isBlackKey = owner.Lostbelt().blackKeyDoT;
            if (!ExecutionStrike)
                return;
            if (isBlackKey)
            {
                if (!owner.HasBuff<BlackKeyExecutionBuff>())
                    owner.AddBuff(BuffType<BlackKeyExecutionBuff>(), GetSeconds(ConceptualBlackKey.BuffTime));
            }
            else
            {
                if (owner.Lostbelt().arcBowLevel > 0)
                {
                    if (!owner.HasBuff<ArcBowBuff>())
                        owner.AddBuff(BuffType<ArcBowBuff>(), GetSeconds(ConceptualBlackKey.BuffTime));
                    if (owner.Lostbelt().blackKeyDefenseBuff != 0)
                        //对的没错，这个鬼东西的减防数据存在了玩家类里面。
                        owner.Lostbelt().blackKeyDefenseTrigger = true;
                }
                if (owner.Lostbelt().rocketCharmLevel > 0)
                {
                    if (!owner.HasBuff<RocketCharmBuff>())
                        owner.AddBuff(BuffType<RocketCharmBuff>(), GetSeconds(ConceptualBlackKey.BuffTime));
                }
            }
        }
        public void HandleMaidReaperOnHit(Player owner, Projectile proj, NPC target)
        {
            if (owner.Lostbelt().maidReaperArmor && target.IsLegal())
            {
                owner.Lostbelt().maidReaperIndex = target.whoAmI;
            }
        }

    }
}
