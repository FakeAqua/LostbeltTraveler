using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Graphics.Particles;
using LostbeltTraveler.Globals.Methods;
using System.Xml.Schema;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Projs.Melee
{
    public abstract class GemYoyoProj : LostbeltProjs
    {
        public override EnumDamageClass Category => EnumDamageClass.Melee;
        protected virtual float YoyoTopSpeed => 10f;
        protected virtual float YoyoLength => LostbeltMethods.TilePixel(13.4375f);
        protected virtual float YoyoLifeTime => 300;
        protected virtual int YoyoMaxUpdates => 1;
        protected string CurrentyGemString => Owner.Lostbelt().gemYoyoStrings;
        #region 一些枚举
        public enum GemYoyoTypes
        {
            Amethyst,
            Topaz,
            Sapphire,
            Emerald,
            Amber,
            Ruby,
            Diamond,
            Onyx
        }
        #endregion 

        #region 快捷判定
        public bool GemStringTopaz => CurrentyGemString.Equals("Topaz");
        public bool GemStringAmethyst => CurrentyGemString.Equals("Amethyst");
        public bool GemStringSapphire => CurrentyGemString.Equals("Sapphire");
        public bool GemStringEmerald => CurrentyGemString.Equals("Emerald");
        public bool GemStringRuby => CurrentyGemString.Equals("Ruby");
        public bool GemStringAmber => CurrentyGemString.Equals("Amber");
        public bool GemStringOnyx => CurrentyGemString.Equals("Onyx");
        public bool GemStringDiamond => CurrentyGemString.Equals("Diamond");
        #endregion
        public GemYoyoTypes CurGemType()
        {
            GemYoyoTypes t = GemYoyoTypes.Amethyst;
            if (GemStringTopaz)
                t = GemYoyoTypes.Topaz;
            if (GemStringSapphire)
                t = GemYoyoTypes.Sapphire;
            if (GemStringEmerald)
                t = GemYoyoTypes.Emerald;
            if (GemStringRuby)
                t = GemYoyoTypes.Ruby;
            if (GemStringAmber)
                t = GemYoyoTypes.Amber;
            if (GemStringOnyx)
                t = GemYoyoTypes.Onyx;
            if (GemStringDiamond)
                t = GemYoyoTypes.Diamond;
            return t;
        }
        #region 快捷强制
        public virtual bool ForceRuby => false;
        public virtual bool ForceDiamond => false;
        public virtual bool ForceEmerald => false;
        public virtual bool ForceTopaz => false;
        public virtual bool ForceAmethyst => false;
        public virtual bool ForceAmber => false;
        public virtual bool ForceSapphire => false;
        public virtual bool ForceOnyx => false;
        #endregion
        public float TimerEmerald = 0;
        public float TimerDiamond = 0;
        public float TimerRuby = 0;
        public float TimerSapphire = 0;
        public float TimerAmber = 0;
        public float TimerOnyx = 0;
        public bool SetBeginPos = false;
        public Vector2 BeginPos = Vector2.Zero;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Type] = YoyoLifeTime;
            ProjectileID.Sets.YoyosMaximumRange[Type] = YoyoLength;
            ProjectileID.Sets.YoyosTopSpeed[Type] = YoyoTopSpeed / YoyoMaxUpdates;
        }
        public override void ExSD()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = ProjAIStyleID.Yoyo;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = YoyoMaxUpdates;
        }
        public override void ProjAI()
        {
            DoBehaviourEmerald();
            DoBehaviourTopaz();
            DoBehaviourRuby();
            DoBehaviourSapphire();
            DoBehaviourAmber();
            DoBehaviourDiamond();
            base.ProjAI();
        }
        #region 宝石悠悠球的行为逻辑
        public void DoBehaviourOnyx()
        {
            if (!(GemStringOnyx || ForceOnyx))
                return;
            if (TimerOnyx > 0)
                TimerOnyx--;
        }
        public void DoBehaviourRuby()
        {
            if (!(GemStringRuby || ForceRuby))
                return;
            TimerRuby += 1;
        }
        public void DoBehaviourAmber()
        {
            if (!(GemStringAmber || ForceAmber))
                return;
            TimerAmber++;
        }
        public void DoBehaviourSapphire()
        {
            if (!(GemStringSapphire || ForceSapphire))
                return;
            if (TimerSapphire > 0)
            {
                Projectile.MaxUpdates = YoyoMaxUpdates + 1;
                Projectile.SetupImmnuity(8, ImmnuityType.Static);
            }
            else
            {
                Projectile.SetupImmnuity(10, ImmnuityType.Static);
                Projectile.MaxUpdates = YoyoMaxUpdates;
            }

        }
        public void DoBehaviourTopaz()
        {
            if (!(GemStringTopaz || ForceTopaz))
                return;
            Projectile.tileCollide = false;
        }
        public void DoBehaviourEmerald()
        {
            if (!(GemStringEmerald || ForceEmerald))
                return;
            TimerEmerald++;
        }
        #endregion
        #region 钻石生成
        public void DoBehaviourDiamond()
        {
            if (!(GemStringDiamond || ForceDiamond))
                return;
            TimerDiamond++;
            if (TimerDiamond > Projectile.MaxUpdates * 60)
            {
                ScarletSound(LostbeltSoundAssets.Misc_Spell, Projectile.Center, 0.45f);
                GemYoyoTypes t = ForceDiamond ? GemYoyoTypes.Diamond : CurGemType();
                switch (t)
                {
                    case GemYoyoTypes.Amethyst:
                        DoBehaviourDiamond_ParticleAmethyst();
                        break;
                    case GemYoyoTypes.Topaz:
                        DoBehaviourDiamond_ParticleTopaz();
                        break;
                    case GemYoyoTypes.Onyx:
                        DoBehaviourDiamond_ParticleOnyx();
                        break;
                    case GemYoyoTypes.Emerald:
                        DoBehaviourDiamond_ParticleEmerald();
                        break;
                    case GemYoyoTypes.Amber:
                        DoBehaviourDiamond_ParticleAmber();
                        break;
                    case GemYoyoTypes.Ruby:
                        DoBehaviourDiamond_ParticleRuby();
                        break;
                    case GemYoyoTypes.Sapphire:
                        DoBehaviourDiamond_ParticleSapphire();
                        break;
                    case GemYoyoTypes.Diamond:
                        DoBehaviourDiamond_ParticleDiamond();
                        break;
                    default:
                        DoBehaviourDiamond_ParticleDiamond();
                        break;
                }

                float rod = RandRotTwoPi;
                for (int i = 0; i < 3; i++)
                {
                    Vector2 dir = (rod + TwoPi / 3 * i).ToRotationVector2();
                    Projectile p= Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, dir * 6f, ProjectileType<DiamondYoyoDiamond>(), Projectile.damage, Projectile.knockBack, Owner.whoAmI);
                    ((DiamondYoyoDiamond)p.ModProjectile).GemTypes = t;
                }
                TimerDiamond = 0;
            }
        }

        public void DoBehaviourDiamond_ParticleAmethyst()
        {
            Vector2 spawnPos = Projectile.Center;
            for (int i = 0; i < 6; i++)
            {
                Color color = RandLerpColor(Color.DarkViolet, Color.Violet);
                new NoiseShockRing(spawnPos, Vector2.Zero, color * 1.2f, 45, 1f, .6f + i * 0.031f, -1, Vector2.Zero, false).Spawn();
            }
            for (int i = 0; i < 15; i++)
                ECSParticle.TurbulenceShinyOrb(spawnPos.ToRandCirclePosEdge(30), Main.rand.NextFloat(1.2f, 2.4f) * 2,
                    Color.Violet, 120, 1, Main.rand.NextFloat(.9f, 1.15f) * .13f);
        }

        public void DoBehaviourDiamond_ParticleTopaz()
        {
            Vector2 spawnPos = Projectile.Center;
            for (int i = 0; i < 6; i++)
            {
                Color color = RandLerpColor(Color.Goldenrod, Color.DarkGoldenrod);
                new NoiseShockRing(spawnPos, Vector2.Zero, color * 1.2f, 45, 1f, .6f + i * 0.031f, -1, Vector2.Zero, false).Spawn();
            }
            for (int i = 0; i < 15; i++)
                ECSParticle.TurbulenceShinyOrb(spawnPos.ToRandCirclePosEdge(30), Main.rand.NextFloat(1.2f, 2.4f) * 2,
                    Color.Gold, 120, 1, Main.rand.NextFloat(.9f, 1.15f) * .13f);
        }

        public void DoBehaviourDiamond_ParticleSapphire()
        {
            Vector2 spawnPos = Projectile.Center;
            for (int i = 0; i < 6; i++)
            {
                Color color = RandLerpColor(Color.DodgerBlue, Color.RoyalBlue);
                new NoiseShockRing(spawnPos, Vector2.Zero, color * 1.2f, 45, 1f, .6f + i * 0.031f, -1, Vector2.Zero, false).Spawn();
            }
            for (int i = 0; i < 15; i++)
                ECSParticle.TurbulenceShinyOrb(spawnPos.ToRandCirclePosEdge(30), Main.rand.NextFloat(1.2f, 2.4f) * 2,
                    Color.LightSkyBlue, 120, 1, Main.rand.NextFloat(.9f, 1.15f) * .13f);
        }

        public void DoBehaviourDiamond_ParticleEmerald()
        {
            Vector2 spawnPos = Projectile.Center;
            for (int i = 0; i < 6; i++)
            {
                Color color = RandLerpColor(Color.ForestGreen, Color.Lime);
                new NoiseShockRing(spawnPos, Vector2.Zero, color * 1.2f, 45, 1f, .6f + i * 0.031f, -1, Vector2.Zero, false).Spawn();
            }
            for (int i = 0; i < 15; i++)
                ECSParticle.TurbulenceShinyOrb(spawnPos.ToRandCirclePosEdge(30), Main.rand.NextFloat(1.2f, 2.4f) * 2,
                    Color.LimeGreen, 120, 1, Main.rand.NextFloat(.9f, 1.15f) * .13f);
        }

        public void DoBehaviourDiamond_ParticleAmber()
        {
            Vector2 spawnPos = Projectile.Center;
            for (int i = 0; i < 6; i++)
            {
                Color color = RandLerpColor(Color.White, Color.OrangeRed);
                new NoiseShockRing(spawnPos, Vector2.Zero, color * 1.2f, 45, 1f, .6f + i * 0.031f, -1, Vector2.Zero, false).Spawn();
            }
            for (int i = 0; i < 15; i++)
                ECSParticle.TurbulenceShinyOrb(spawnPos.ToRandCirclePosEdge(30), Main.rand.NextFloat(1.2f, 2.4f) * 2,
                    Color.DarkOrange, 120, 1, Main.rand.NextFloat(.9f, 1.15f) * .13f);
        }

        public void DoBehaviourDiamond_ParticleRuby()
        {
            Vector2 spawnPos = Projectile.Center;
            for (int i = 0; i < 6; i++)
            {
                Color color = RandLerpColor(Color.Red, Color.Crimson);
                new NoiseShockRing(spawnPos, Vector2.Zero, color * 1.2f, 45, 1f, .6f + i * 0.031f, -1, Vector2.Zero, false).Spawn();
            }
            for (int i = 0; i < 15; i++)
                ECSParticle.TurbulenceShinyOrb(spawnPos.ToRandCirclePosEdge(30), Main.rand.NextFloat(1.2f, 2.4f) * 2,
                    Color.Red, 120, 1, Main.rand.NextFloat(.9f, 1.15f) * .13f);
        }

        public void DoBehaviourDiamond_ParticleOnyx()
        {
            Vector2 spawnPos = Projectile.Center;
            for (int i = 0; i < 6; i++)
            {
                Color color = RandLerpColor(Color.White, Color.WhiteSmoke);
                new NoiseShockRing(spawnPos, Vector2.Zero, color * 1.2f, 45, 1f, .6f + i * 0.031f, -1, Vector2.Zero, false).Spawn();
            }
            for (int i = 0; i < 15; i++)
                ECSParticle.TurbulenceShinyOrb(spawnPos.ToRandCirclePosEdge(30), Main.rand.NextFloat(1.2f, 2.4f) * 2,
                    RandLerpColor(Color.Silver, Color.White), 120, 1, Main.rand.NextFloat(.9f, 1.15f) * .13f);
        }

        public void DoBehaviourDiamond_ParticleDiamond()
        {
            Vector2 spawnPos = Projectile.Center;
            for (int i = 0; i < 6; i++)
            {
                Color color = RandLerpColor(Color.White, Color.WhiteSmoke);
                new NoiseShockRing(spawnPos, Vector2.Zero, color * 1.2f, 45, 1f, .6f + i * 0.031f, -1, Vector2.Zero, false).Spawn();
            }
            for (int i = 0; i < 15; i++)
                ECSParticle.TurbulenceShinyOrb(spawnPos.ToRandCirclePosEdge(30), Main.rand.NextFloat(1.2f, 2.4f) * 2,
                    RandLerpColor(Color.Silver, Color.White), 120, 1, Main.rand.NextFloat(.9f, 1.15f) * .13f);
        }

        #endregion
        #region 蓝玉命中
        public void DoBehaviourSapphire_OnHit()
        {
            if (!(GemStringSapphire || ForceSapphire))
                return;
            if (TimerSapphire == 0)
            {
                GemYoyoTypes t = ForceSapphire ? GemYoyoTypes.Sapphire : CurGemType();
                switch (t)
                {
                    case GemYoyoTypes.Amethyst:
                        DoBehaviourSapphire_OnHit_ParticleAmethyst();
                        break;
                    case GemYoyoTypes.Topaz:
                        DoBehaviourSapphire_OnHit_ParticleTopaz();
                        break;
                    case GemYoyoTypes.Onyx:
                        DoBehaviourSapphire_OnHit_ParticleOnyx();
                        break;
                    case GemYoyoTypes.Emerald:
                        DoBehaviourSapphire_OnHit_ParticleEmerald();
                        break;
                    case GemYoyoTypes.Amber:
                        DoBehaviourSapphire_OnHit_ParticleAmber();
                        break;
                    case GemYoyoTypes.Ruby:
                        DoBehaviourSapphire_OnHit_ParticleRuby();
                        break;
                    case GemYoyoTypes.Diamond:
                        DoBehaviourSapphire_OnHit_ParticleDiamond();
                        break;
                    case GemYoyoTypes.Sapphire:
                        DoBehaviourSapphire_OnHit_ParticleSapphire();
                        break;
                    default:
                        DoBehaviourSapphire_OnHit_ParticleSapphire();
                        break;
                }
                ScarletSound(LostbeltSoundAssets.GrabCharge, Projectile.Center);
                TimerSapphire = 60;
            }

        }
        public void DoBehaviourSapphire_OnHit_ParticleAmethyst()
        {
            for (int i = 0; i < 16; i++)
                ECSParticle.TurbulenceShinyOrb(Projectile.Center, 1.2f, RandLerpColor(Color.AliceBlue, Color.LightSkyBlue), 45, 1, 0.1f, glowMult: .4f);
            for (int i = 0; i < 8; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center, RandVelTwoPi(3), RandLerpColor(Color.AliceBlue, Color.LightSkyBlue), 45, 1, Main.rand.NextFloat(.9f, 1.1f) * .4f, .2f);
        }

        public void DoBehaviourSapphire_OnHit_ParticleTopaz()
        {
            for (int i = 0; i < 16; i++)
                ECSParticle.TurbulenceShinyOrb(Projectile.Center, 1.2f, RandLerpColor(Color.AliceBlue, Color.LightSkyBlue), 45, 1, 0.1f, glowMult: .4f);
            for (int i = 0; i < 8; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center, RandVelTwoPi(3), RandLerpColor(Color.AliceBlue, Color.LightSkyBlue), 45, 1, Main.rand.NextFloat(.9f, 1.1f) * .4f, .2f);
        }

        public void DoBehaviourSapphire_OnHit_ParticleEmerald()
        {
            for (int i = 0; i < 16; i++)
                ECSParticle.TurbulenceShinyOrb(Projectile.Center, 1.2f, RandLerpColor(Color.AliceBlue, Color.LightSkyBlue), 45, 1, 0.1f, glowMult: .4f);
            for (int i = 0; i < 8; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center, RandVelTwoPi(3), RandLerpColor(Color.AliceBlue, Color.LightSkyBlue), 45, 1, Main.rand.NextFloat(.9f, 1.1f) * .4f, .2f);
        }

        public void DoBehaviourSapphire_OnHit_ParticleSapphire()
        {
            for (int i = 0; i < 16; i++)
                ECSParticle.TurbulenceShinyOrb(Projectile.Center, 1.2f, RandLerpColor(Color.AliceBlue, Color.LightSkyBlue), 45, 1, 0.1f, glowMult: .4f);
            for (int i = 0; i < 8; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center, RandVelTwoPi(3), RandLerpColor(Color.AliceBlue, Color.LightSkyBlue), 45, 1, Main.rand.NextFloat(.9f, 1.1f) * .4f, .2f);
        }

        public void DoBehaviourSapphire_OnHit_ParticleAmber()
        {
            for (int i = 0; i < 16; i++)
                ECSParticle.TurbulenceShinyOrb(Projectile.Center, 1.2f, RandLerpColor(Color.AliceBlue, Color.LightSkyBlue), 45, 1, 0.1f, glowMult: .4f);
            for (int i = 0; i < 8; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center, RandVelTwoPi(3), RandLerpColor(Color.AliceBlue, Color.LightSkyBlue), 45, 1, Main.rand.NextFloat(.9f, 1.1f) * .4f, .2f);
        }

        public void DoBehaviourSapphire_OnHit_ParticleRuby()
        {
            for (int i = 0; i < 16; i++)
                ECSParticle.TurbulenceShinyOrb(Projectile.Center, 1.2f, RandLerpColor(Color.AliceBlue, Color.LightSkyBlue), 45, 1, 0.1f, glowMult: .4f);
            for (int i = 0; i < 8; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center, RandVelTwoPi(3), RandLerpColor(Color.AliceBlue, Color.LightSkyBlue), 45, 1, Main.rand.NextFloat(.9f, 1.1f) * .4f, .2f);
        }

        public void DoBehaviourSapphire_OnHit_ParticleDiamond()
        {
            for (int i = 0; i < 16; i++)
                ECSParticle.TurbulenceShinyOrb(Projectile.Center, 1.2f, RandLerpColor(Color.AliceBlue, Color.LightSkyBlue), 45, 1, 0.1f, glowMult: .4f);
            for (int i = 0; i < 8; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center, RandVelTwoPi(3), RandLerpColor(Color.AliceBlue, Color.LightSkyBlue), 45, 1, Main.rand.NextFloat(.9f, 1.1f) * .4f, .2f);
        }

        public void DoBehaviourSapphire_OnHit_ParticleOnyx()
        {
            for (int i = 0; i < 16; i++)
                ECSParticle.TurbulenceShinyOrb(Projectile.Center, 1.2f, RandLerpColor(Color.AliceBlue, Color.LightSkyBlue), 45, 1, 0.1f, glowMult: .4f);
            for (int i = 0; i < 8; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center, RandVelTwoPi(3), RandLerpColor(Color.AliceBlue, Color.LightSkyBlue), 45, 1, Main.rand.NextFloat(.9f, 1.1f) * .4f, .2f);
        }
        #endregion
        #region 红玉命中
        public void DoBehaviourRuby_OnHit()
        {
            if (!(GemStringRuby || ForceRuby))
                return;
            if (TimerRuby > Projectile.MaxUpdates * 60)
            {
                GemYoyoTypes t = ForceRuby ? GemYoyoTypes.Ruby : CurGemType();
                switch (t)
                {
                    case GemYoyoTypes.Amethyst:
                        DoBehaviourRuby_OnHit_ParticleAmethyst();
                        break;
                    case GemYoyoTypes.Topaz:
                        DoBehaviourRuby_OnHit_ParticleTopaz();
                        break;
                    case GemYoyoTypes.Sapphire:
                        DoBehaviourRuby_OnHit_ParticleSapphire();
                        break;
                    case GemYoyoTypes.Emerald:
                        DoBehaviourRuby_OnHit_ParticleEmerald();
                        break;
                    case GemYoyoTypes.Amber:
                        DoBehaviourRuby_OnHit_ParticleAmber();
                        break;
                    case GemYoyoTypes.Ruby:
                        DoBehaviourRuby_OnHit_ParticleRuby();
                        break;
                    case GemYoyoTypes.Diamond:
                        DoBehaviourRuby_OnHit_ParticleDiamond();
                        break;
                    case GemYoyoTypes.Onyx:
                        DoBehaviourRuby_OnHit_ParticleOnyx();
                        break;
                    default:
                        DoBehaviourRuby_OnHit_ParticleRuby();
                        break;
                }
                ScarletSound(LostbeltSoundAssets.Misc_Spell, Projectile.Center, 0.45f);
                float rod = RandRotTwoPi;
                for (int i = 0; i < 3; i++)
                {
                    Vector2 dir = (rod + TwoPi / 3 * i).ToRotationVector2();
                    Projectile p= Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, dir * 6f, ProjectileType<RubyYoyoRuby>(), Projectile.damage, Projectile.knockBack, Owner.whoAmI);
                    ((RubyYoyoRuby)p.ModProjectile).GemTypes = t;
                }
                TimerRuby = 0;
            }
        }
        public void DoBehaviourRuby_OnHit_ParticleAmethyst()
        {
            for (int i = 0; i < 16; i++)
            {
                Color color = RandLerpColor(Color.Red, Color.Crimson);
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(6f), color, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
            }
            for (int i = 0; i < 15; i++)
                ECSParticle.TurbulenceShinyOrb(Projectile.Center.ToRandCirclePosEdge(30), Main.rand.NextFloat(1.2f, 2.4f) * 2,
                    RandLerpColor(Color.Crimson, Color.Red), 120, 1, Main.rand.NextFloat(.9f, 1.15f) * .13f);
        }

        public void DoBehaviourRuby_OnHit_ParticleTopaz()
        {
            for (int i = 0; i < 16; i++)
            {
                Color color = RandLerpColor(Color.Red, Color.Crimson);
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(6f), color, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
            }
            for (int i = 0; i < 15; i++)
                ECSParticle.TurbulenceShinyOrb(Projectile.Center.ToRandCirclePosEdge(30), Main.rand.NextFloat(1.2f, 2.4f) * 2,
                    RandLerpColor(Color.Crimson, Color.Red), 120, 1, Main.rand.NextFloat(.9f, 1.15f) * .13f);
        }

        public void DoBehaviourRuby_OnHit_ParticleSapphire()
        {
            for (int i = 0; i < 16; i++)
            {
                Color color = RandLerpColor(Color.Red, Color.Crimson);
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(6f), color, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
            }
            for (int i = 0; i < 15; i++)
                ECSParticle.TurbulenceShinyOrb(Projectile.Center.ToRandCirclePosEdge(30), Main.rand.NextFloat(1.2f, 2.4f) * 2,
                    RandLerpColor(Color.Crimson, Color.Red), 120, 1, Main.rand.NextFloat(.9f, 1.15f) * .13f);
        }

        public void DoBehaviourRuby_OnHit_ParticleEmerald()
        {
            for (int i = 0; i < 16; i++)
            {
                Color color = RandLerpColor(Color.Red, Color.Crimson);
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(6f), color, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
            }
            for (int i = 0; i < 15; i++)
                ECSParticle.TurbulenceShinyOrb(Projectile.Center.ToRandCirclePosEdge(30), Main.rand.NextFloat(1.2f, 2.4f) * 2,
                    RandLerpColor(Color.Crimson, Color.Red), 120, 1, Main.rand.NextFloat(.9f, 1.15f) * .13f);
        }

        public void DoBehaviourRuby_OnHit_ParticleAmber()
        {
            for (int i = 0; i < 16; i++)
            {
                Color color = RandLerpColor(Color.Red, Color.Crimson);
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(6f), color, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
            }
            for (int i = 0; i < 15; i++)
                ECSParticle.TurbulenceShinyOrb(Projectile.Center.ToRandCirclePosEdge(30), Main.rand.NextFloat(1.2f, 2.4f) * 2,
                    RandLerpColor(Color.Crimson, Color.Red), 120, 1, Main.rand.NextFloat(.9f, 1.15f) * .13f);
        }

        public void DoBehaviourRuby_OnHit_ParticleDiamond()
        {
            for (int i = 0; i < 16; i++)
            {
                Color color = RandLerpColor(Color.Red, Color.Crimson);
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(6f), color, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
            }
            for (int i = 0; i < 15; i++)
                ECSParticle.TurbulenceShinyOrb(Projectile.Center.ToRandCirclePosEdge(30), Main.rand.NextFloat(1.2f, 2.4f) * 2,
                    RandLerpColor(Color.Crimson, Color.Red), 120, 1, Main.rand.NextFloat(.9f, 1.15f) * .13f);
        }

        public void DoBehaviourRuby_OnHit_ParticleOnyx()
        {
            for (int i = 0; i < 16; i++)
            {
                Color color = RandLerpColor(Color.Red, Color.Crimson);
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(6f), color, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
            }
            for (int i = 0; i < 15; i++)
                ECSParticle.TurbulenceShinyOrb(Projectile.Center.ToRandCirclePosEdge(30), Main.rand.NextFloat(1.2f, 2.4f) * 2,
                    RandLerpColor(Color.Crimson, Color.Red), 120, 1, Main.rand.NextFloat(.9f, 1.15f) * .13f);
        }

        public void DoBehaviourRuby_OnHit_ParticleRuby()
        {
            for (int i = 0; i < 16; i++)
            {
                Color color = RandLerpColor(Color.Red, Color.Crimson);
                ECSParticle.GlowSquare(Projectile.Center.ToRandCirclePosEdge(6), RandVelTwoPi(6f), color, 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
            }
            for (int i = 0; i < 15; i++)
                ECSParticle.TurbulenceShinyOrb(Projectile.Center.ToRandCirclePosEdge(30), Main.rand.NextFloat(1.2f, 2.4f) * 2,
                    RandLerpColor(Color.Crimson, Color.Red), 120, 1, Main.rand.NextFloat(.9f, 1.15f) * .13f);
        }
        #endregion
        #region 琥珀命中
        public void DoBehaivourAmber_OnHit(NPC target, NPC.HitInfo hit, int damageDone, Color squareColor, Color orbColor)
        {
            if (!(GemStringAmber || ForceAmber))
                return;

            if (TimerAmber > Projectile.MaxUpdates * 10)
            {
                Vector2 pos = Owner.Center.ToRandCirclePosEdge(5, 50) + Owner.ToMouseVector2() * -100f;
                GemYoyoTypes t = ForceAmber ? GemYoyoTypes.Amber : CurGemType();
                switch (t)
                {
                    case GemYoyoTypes.Amethyst:
                        DoBehaviourAmber_OnHit_ParticleAmethyst(pos);
                        break;
                    case GemYoyoTypes.Topaz:
                        DoBehaviourAmber_OnHit_ParticleTopaz(pos);
                        break;
                    case GemYoyoTypes.Sapphire:
                        DoBehaviourAmber_OnHit_ParticleSapphire(pos);
                        break;
                    case GemYoyoTypes.Emerald:
                        DoBehaviourAmber_OnHit_ParticleEmerald(pos);
                        break;
                    case GemYoyoTypes.Ruby:
                        DoBehaviourAmber_OnHit_ParticleRuby(pos);
                        break;
                    case GemYoyoTypes.Diamond:
                        DoBehaviourAmber_OnHit_ParticleDiamond(pos);
                        break;
                    case GemYoyoTypes.Onyx:
                        DoBehaviourAmber_OnHit_ParticleOnyx(pos);
                        break;
                    case GemYoyoTypes.Amber:
                        DoBehaviourAmber_OnHit_ParticleAmber(pos);
                        break;
                    default:
                        DoBehaviourAmber_OnHit_ParticleAmber(pos);
                        break;
                }
                Vector2 dir = pos.ToMouseVector2();
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), pos, dir * 10f, ProjectileType<AmberYoyoAmber>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                ((AmberYoyoAmber)proj.ModProjectile).GemTypes = t;
                if (target.IsLegal())
                    proj.Lostbelt().CurStoredTarget = target;
                TimerAmber = 0;
            }
        }
        public void DoBehaviourAmber_OnHit_ParticleAmethyst(Vector2 pos)
        {
            for (int i = 0; i < 8; i++)
            {
                ECSParticle.GlowSquare(pos.ToRandCirclePosEdge(6), RandVelTwoPi(3.5f), RandLerpColor(Color.Orange, Color.DarkOrange),
                    40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
            }
            ECSParticle.HRShinyOrb(pos, Vector2.Zero, Color.Orange, 40, 1, 0.2f, 0.4f);

        }

        public void DoBehaviourAmber_OnHit_ParticleTopaz(Vector2 pos)
        {
            for (int i = 0; i < 8; i++)
            {
                ECSParticle.GlowSquare(pos.ToRandCirclePosEdge(6), RandVelTwoPi(3.5f), RandLerpColor(Color.Orange, Color.DarkOrange),
                    40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
            }
            ECSParticle.HRShinyOrb(pos, Vector2.Zero, Color.Orange, 40, 1, 0.2f, 0.4f);

        }

        public void DoBehaviourAmber_OnHit_ParticleSapphire(Vector2 pos)
        {
            for (int i = 0; i < 8; i++)
            {
                ECSParticle.GlowSquare(pos.ToRandCirclePosEdge(6), RandVelTwoPi(3.5f), RandLerpColor(Color.Orange, Color.DarkOrange),
                    40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
            }
            ECSParticle.HRShinyOrb(pos, Vector2.Zero, Color.Orange, 40, 1, 0.2f, 0.4f);

        }

        public void DoBehaviourAmber_OnHit_ParticleEmerald(Vector2 pos)
        {
            for (int i = 0; i < 8; i++)
            {
                ECSParticle.GlowSquare(pos.ToRandCirclePosEdge(6), RandVelTwoPi(3.5f), RandLerpColor(Color.Orange, Color.DarkOrange),
                    40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
            }
            ECSParticle.HRShinyOrb(pos, Vector2.Zero, Color.Orange, 40, 1, 0.2f, 0.4f);

        }

        public void DoBehaviourAmber_OnHit_ParticleRuby(Vector2 pos)
        {
            for (int i = 0; i < 8; i++)
            {
                ECSParticle.GlowSquare(pos.ToRandCirclePosEdge(6), RandVelTwoPi(3.5f), RandLerpColor(Color.Orange, Color.DarkOrange),
                    40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
            }
            ECSParticle.HRShinyOrb(pos, Vector2.Zero, Color.Orange, 40, 1, 0.2f, 0.4f);

        }

        public void DoBehaviourAmber_OnHit_ParticleDiamond(Vector2 pos)
        {
            for (int i = 0; i < 8; i++)
            {
                ECSParticle.GlowSquare(pos.ToRandCirclePosEdge(6), RandVelTwoPi(3.5f), RandLerpColor(Color.Orange, Color.DarkOrange),
                    40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
            }
            ECSParticle.HRShinyOrb(pos, Vector2.Zero, Color.Orange, 40, 1, 0.2f, 0.4f);

        }

        public void DoBehaviourAmber_OnHit_ParticleOnyx(Vector2 pos)
        {
            for (int i = 0; i < 8; i++)
            {
                ECSParticle.GlowSquare(pos.ToRandCirclePosEdge(6), RandVelTwoPi(3.5f), RandLerpColor(Color.Orange, Color.DarkOrange),
                    40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
            }
            ECSParticle.HRShinyOrb(pos, Vector2.Zero, Color.Orange, 40, 1, 0.2f, 0.4f);

        }

        public void DoBehaviourAmber_OnHit_ParticleAmber(Vector2 pos)
        {
            for (int i = 0; i < 8; i++)
            {
                ECSParticle.GlowSquare(pos.ToRandCirclePosEdge(6), RandVelTwoPi(3.5f), RandLerpColor(Color.Orange, Color.DarkOrange),
                    40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .75f, 0, 0.1f);
            }
            ECSParticle.HRShinyOrb(pos, Vector2.Zero, Color.Orange, 40, 1, 0.2f, 0.4f);

        }
        #endregion
        #region 玛瑙命中
        public void DoBehaviourOnyx_OnHit(NPC target)
        {
            if (!(GemStringOnyx || ForceOnyx))
                return;
            if (TimerOnyx == 0)
            {
                
                GemYoyoTypes t = ForceOnyx ? GemYoyoTypes.Onyx : CurGemType();
for (int i = 0; i < 5; i++)
                {
                    Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), target.Center, RandVelTwoPi(12f, 15f) * .85f, ProjectileType<OnyxYoyoOnyx>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    p.rotation = RandRotTwoPi;
                    ((OnyxYoyoOnyx)p.ModProjectile).GemTypes = t;
                }
                switch (t)
                {
                    case GemYoyoTypes.Amethyst:
                        DoBehaviourOnyx_OnHit_ParticleAmethyst(target);
                        break;
                    case GemYoyoTypes.Topaz:
                        DoBehaviourOnyx_OnHit_ParticleTopaz(target);
                        break;
                    case GemYoyoTypes.Sapphire:
                        DoBehaviourOnyx_OnHit_ParticleSapphire(target);
                        break;
                    case GemYoyoTypes.Emerald:
                        DoBehaviourOnyx_OnHit_ParticleEmerald(target);
                        break;
                    case GemYoyoTypes.Amber:
                        DoBehaviourOnyx_OnHit_ParticleAmber(target);
                        break;
                    case GemYoyoTypes.Ruby:
                        DoBehaviourOnyx_OnHit_ParticleRuby(target);
                        break;
                    case GemYoyoTypes.Diamond:
                        DoBehaviourOnyx_OnHit_ParticleDiamond(target);
                        break;
                    case GemYoyoTypes.Onyx:
                        DoBehaviourOnyx_OnHit_ParticleOnyx(target);
                        break;
                    default:
                        DoBehaviourOnyx_OnHit_ParticleOnyx(target);
                        break;
                }
                TimerOnyx = Projectile.MaxUpdates * 80;
                ScarletSound(LostbeltSoundAssets.Dialectics_Throw, target.Center, .7f, 1, .3f);
            }
        }
        public void DoBehaviourOnyx_OnHit_ParticleAmethyst(NPC target)
        {
            float scale = .42f;
            ECSParticle.CrossGlow(target.Center, Color.DarkViolet, 45, 1, scale, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(target.Center, Color.Black, 45, 1, scale * .95f, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(target.Center, Color.Violet, 45, 1, scale * .9f, 0.1f, BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.TurbulenceShinyOrb(target.Center.ToRandCirclePos(60), 9.4f, RandLerpColor(Color.DarkViolet, Color.Black), 75, 1, 0.15f * Main.rand.NextFloat(.8f, 1.1f), Pi, 0, BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.ShinyCrossStarECS(target.Center.ToRandCirclePos(1.2f), RandVelTwoPi(0f, 9f), RandLerpColor(Color.Violet, Color.DarkViolet), 45, 1, Main.rand.NextFloat(.85f, 1.15f) * .64f, 0.2f, BlendState.AlphaBlend);
            for (int i = 0; i < 16; i++)
                ECSParticle.SmokeParticle(target.Center.ToRandCirclePos(2f), RandVelTwoPi(9f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .41f, Main.rand.NextBool(), BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.LiliesFire(target.Center.ToRandCirclePos(2f), RandVelTwoPi(13f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .31f, true, BlendState.Additive);
        }
        public void DoBehaviourOnyx_OnHit_ParticleTopaz(NPC target)
        {
            float scale = .42f;
            ECSParticle.CrossGlow(target.Center, Color.DarkViolet, 45, 1, scale, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(target.Center, Color.Black, 45, 1, scale * .95f, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(target.Center, Color.Violet, 45, 1, scale * .9f, 0.1f, BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.TurbulenceShinyOrb(target.Center.ToRandCirclePos(60), 9.4f, RandLerpColor(Color.DarkViolet, Color.Black), 75, 1, 0.15f * Main.rand.NextFloat(.8f, 1.1f), Pi, 0, BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.ShinyCrossStarECS(target.Center.ToRandCirclePos(1.2f), RandVelTwoPi(0f, 9f), RandLerpColor(Color.Violet, Color.DarkViolet), 45, 1, Main.rand.NextFloat(.85f, 1.15f) * .64f, 0.2f, BlendState.AlphaBlend);
            for (int i = 0; i < 16; i++)
                ECSParticle.SmokeParticle(target.Center.ToRandCirclePos(2f), RandVelTwoPi(9f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .41f, Main.rand.NextBool(), BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.LiliesFire(target.Center.ToRandCirclePos(2f), RandVelTwoPi(13f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .31f, true, BlendState.Additive);
        }
        public void DoBehaviourOnyx_OnHit_ParticleSapphire(NPC target)
        {
            float scale = .42f;
            ECSParticle.CrossGlow(target.Center, Color.DarkViolet, 45, 1, scale, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(target.Center, Color.Black, 45, 1, scale * .95f, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(target.Center, Color.Violet, 45, 1, scale * .9f, 0.1f, BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.TurbulenceShinyOrb(target.Center.ToRandCirclePos(60), 9.4f, RandLerpColor(Color.DarkViolet, Color.Black), 75, 1, 0.15f * Main.rand.NextFloat(.8f, 1.1f), Pi, 0, BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.ShinyCrossStarECS(target.Center.ToRandCirclePos(1.2f), RandVelTwoPi(0f, 9f), RandLerpColor(Color.Violet, Color.DarkViolet), 45, 1, Main.rand.NextFloat(.85f, 1.15f) * .64f, 0.2f, BlendState.AlphaBlend);
            for (int i = 0; i < 16; i++)
                ECSParticle.SmokeParticle(target.Center.ToRandCirclePos(2f), RandVelTwoPi(9f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .41f, Main.rand.NextBool(), BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.LiliesFire(target.Center.ToRandCirclePos(2f), RandVelTwoPi(13f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .31f, true, BlendState.Additive);
        }
        public void DoBehaviourOnyx_OnHit_ParticleEmerald(NPC target)
        {
            float scale = .42f;
            ECSParticle.CrossGlow(target.Center, Color.DarkViolet, 45, 1, scale, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(target.Center, Color.Black, 45, 1, scale * .95f, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(target.Center, Color.Violet, 45, 1, scale * .9f, 0.1f, BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.TurbulenceShinyOrb(target.Center.ToRandCirclePos(60), 9.4f, RandLerpColor(Color.DarkViolet, Color.Black), 75, 1, 0.15f * Main.rand.NextFloat(.8f, 1.1f), Pi, 0, BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.ShinyCrossStarECS(target.Center.ToRandCirclePos(1.2f), RandVelTwoPi(0f, 9f), RandLerpColor(Color.Violet, Color.DarkViolet), 45, 1, Main.rand.NextFloat(.85f, 1.15f) * .64f, 0.2f, BlendState.AlphaBlend);
            for (int i = 0; i < 16; i++)
                ECSParticle.SmokeParticle(target.Center.ToRandCirclePos(2f), RandVelTwoPi(9f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .41f, Main.rand.NextBool(), BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.LiliesFire(target.Center.ToRandCirclePos(2f), RandVelTwoPi(13f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .31f, true, BlendState.Additive);
        }
        public void DoBehaviourOnyx_OnHit_ParticleAmber(NPC target)
        {
            float scale = .42f;
            ECSParticle.CrossGlow(target.Center, Color.DarkViolet, 45, 1, scale, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(target.Center, Color.Black, 45, 1, scale * .95f, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(target.Center, Color.Violet, 45, 1, scale * .9f, 0.1f, BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.TurbulenceShinyOrb(target.Center.ToRandCirclePos(60), 9.4f, RandLerpColor(Color.DarkViolet, Color.Black), 75, 1, 0.15f * Main.rand.NextFloat(.8f, 1.1f), Pi, 0, BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.ShinyCrossStarECS(target.Center.ToRandCirclePos(1.2f), RandVelTwoPi(0f, 9f), RandLerpColor(Color.Violet, Color.DarkViolet), 45, 1, Main.rand.NextFloat(.85f, 1.15f) * .64f, 0.2f, BlendState.AlphaBlend);
            for (int i = 0; i < 16; i++)
                ECSParticle.SmokeParticle(target.Center.ToRandCirclePos(2f), RandVelTwoPi(9f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .41f, Main.rand.NextBool(), BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.LiliesFire(target.Center.ToRandCirclePos(2f), RandVelTwoPi(13f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .31f, true, BlendState.Additive);
        }
        public void DoBehaviourOnyx_OnHit_ParticleRuby(NPC target)
        {
            float scale = .42f;
            ECSParticle.CrossGlow(target.Center, Color.DarkViolet, 45, 1, scale, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(target.Center, Color.Black, 45, 1, scale * .95f, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(target.Center, Color.Violet, 45, 1, scale * .9f, 0.1f, BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.TurbulenceShinyOrb(target.Center.ToRandCirclePos(60), 9.4f, RandLerpColor(Color.DarkViolet, Color.Black), 75, 1, 0.15f * Main.rand.NextFloat(.8f, 1.1f), Pi, 0, BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.ShinyCrossStarECS(target.Center.ToRandCirclePos(1.2f), RandVelTwoPi(0f, 9f), RandLerpColor(Color.Violet, Color.DarkViolet), 45, 1, Main.rand.NextFloat(.85f, 1.15f) * .64f, 0.2f, BlendState.AlphaBlend);
            for (int i = 0; i < 16; i++)
                ECSParticle.SmokeParticle(target.Center.ToRandCirclePos(2f), RandVelTwoPi(9f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .41f, Main.rand.NextBool(), BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.LiliesFire(target.Center.ToRandCirclePos(2f), RandVelTwoPi(13f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .31f, true, BlendState.Additive);
        }
        public void DoBehaviourOnyx_OnHit_ParticleDiamond(NPC target)
        {
            float scale = .42f;
            ECSParticle.CrossGlow(target.Center, Color.DarkViolet, 45, 1, scale, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(target.Center, Color.Black, 45, 1, scale * .95f, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(target.Center, Color.Violet, 45, 1, scale * .9f, 0.1f, BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.TurbulenceShinyOrb(target.Center.ToRandCirclePos(60), 9.4f, RandLerpColor(Color.DarkViolet, Color.Black), 75, 1, 0.15f * Main.rand.NextFloat(.8f, 1.1f), Pi, 0, BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.ShinyCrossStarECS(target.Center.ToRandCirclePos(1.2f), RandVelTwoPi(0f, 9f), RandLerpColor(Color.Violet, Color.DarkViolet), 45, 1, Main.rand.NextFloat(.85f, 1.15f) * .64f, 0.2f, BlendState.AlphaBlend);
            for (int i = 0; i < 16; i++)
                ECSParticle.SmokeParticle(target.Center.ToRandCirclePos(2f), RandVelTwoPi(9f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .41f, Main.rand.NextBool(), BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.LiliesFire(target.Center.ToRandCirclePos(2f), RandVelTwoPi(13f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .31f, true, BlendState.Additive);
        }

        public void DoBehaviourOnyx_OnHit_ParticleOnyx(NPC target)
        {
            float scale = .42f;
            ECSParticle.CrossGlow(target.Center, Color.DarkViolet, 45, 1, scale, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(target.Center, Color.Black, 45, 1, scale * .95f, 0.1f, BlendState.NonPremultiplied);
            ECSParticle.CrossGlow(target.Center, Color.Violet, 45, 1, scale * .9f, 0.1f, BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.TurbulenceShinyOrb(target.Center.ToRandCirclePos(60), 9.4f, RandLerpColor(Color.DarkViolet, Color.Black), 75, 1, 0.15f * Main.rand.NextFloat(.8f, 1.1f), Pi, 0, BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.ShinyCrossStarECS(target.Center.ToRandCirclePos(1.2f), RandVelTwoPi(0f, 9f), RandLerpColor(Color.Violet, Color.DarkViolet), 45, 1, Main.rand.NextFloat(.85f, 1.15f) * .64f, 0.2f, BlendState.AlphaBlend);
            for (int i = 0; i < 16; i++)
                ECSParticle.SmokeParticle(target.Center.ToRandCirclePos(2f), RandVelTwoPi(9f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .41f, Main.rand.NextBool(), BlendState.NonPremultiplied);
            for (int i = 0; i < 16; i++)
                ECSParticle.LiliesFire(target.Center.ToRandCirclePos(2f), RandVelTwoPi(13f), RandLerpColor(Color.DarkViolet, Color.Black), 45, RandRotTwoPi, .81f, .31f, true, BlendState.Additive);
        }
        #endregion
        public void DoBehaviourEmerald_OnHit()
        {
            if (!(GemStringEmerald || ForceEmerald))
                return;
            if (TimerEmerald > Projectile.MaxUpdates * 90)
            {
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, RandVelTwoPi(6, 8f), ProjectileType<EmeraldYoyoEmerald>(), 0, 0, Projectile.owner);
                ScarletSound(SoundID.DD2_DarkMageAttack, Projectile.Center);
                TimerEmerald = 0;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)

        {
            DoBehaviourEmerald_OnHit();
            DoBehaviourOnyx_OnHit(target);
            DoBehaviourSapphire_OnHit();
            DoBehaviourRuby_OnHit();

            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (!(GemStringAmethyst || ForceAmethyst))
                return null;
            Rectangle rec = Utils.CenteredRectangle(Projectile.Center, new Vector2(16 * 8));
            return rec.Intersects(targetHitbox);
        }
        public static void DrawAmethystSmear(Projectile proj, Color outerColor, Color innerColor, float overallScale = 1, float rotFix = 0)
        {
            Main.spriteBatch.EnterShaderArea();
            Texture2D glowTex = LostbeltModAssets.Particle_Smear.Value;
            Vector2 drawPos = proj.Center - Main.screenPosition;
            Main.spriteBatch.FastDraw(glowTex, drawPos.ToRandCirclePos(1.3f), outerColor, proj.rotation + rotFix, glowTex.Size() / 2f, .23f * overallScale, 0);
            Main.spriteBatch.FastDraw(glowTex, drawPos, innerColor, proj.rotation + rotFix, glowTex.Size() / 2f, .21f * overallScale, 0);
            Main.spriteBatch.EndShaderArea();
            Main.spriteBatch.EndShaderArea();
        }
    }
    public class TopazYoyoProj : GemYoyoProj
    {
        public ref float BoosterTime => ref Projectile.ai[2];
        protected override float YoyoLifeTime => 5;
        protected override int YoyoMaxUpdates => 1;
        protected override float YoyoTopSpeed => 16;
        public override bool ForceTopaz => true;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Projectile.ToTrailSetting(6);
        }
        public override void ExSD()
        {
            base.ExSD();
            Projectile.tileCollide = false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Rectangle rec = Utils.CenteredRectangle(Projectile.Center, new Vector2(64));
            return rec.Intersects(targetHitbox);
        }
        public override void ProjAI()
        {
            base.ProjAI();
            Lighting.AddLight(Projectile.Center, TorchID.Yellow);
            ECSParticle.ShrinkParticle(Projectile.Center.ToRandCirclePos(8), RandVelTwoPi(1), RandLerpColor(Color.Yellow, Color.Gold), 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .13f, 1);
            if (Projectile.velocity.LengthSquared() > 2.5f * 2.5f)
            {
                ECSParticle.Stain(Projectile.Center.ToRandCirclePosEdge(8), Projectile.velocity / 8f, RandLerpColor(Color.Yellow, Color.Gold), 40, 1, Projectile.velocity.ToRotation(), 0.16f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            ECSParticle.LightntingGlow(target.Center, Projectile.rotation.ToRotationVector2() * .01f, Color.Gold, 40, 1, 0.6f);
            for (int i = 0; i < 8; i++)
            {
                ECSParticle.ShinyCrossStarECS(target.Center, RandVelTwoPi(3), RandLerpColor(Color.Yellow, Color.Gold), 45, 1, Main.rand.NextFloat(.9f, 1.1f) * .4f, .2f);

            }
            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            if (GemStringAmethyst)
            {
                DrawAmethystSmear(Projectile, Color.Goldenrod, Color.LightGoldenrodYellow * .5f, 1);
                DrawAmethystSmear(Projectile, Color.Goldenrod, Color.LightGoldenrodYellow * .5f, 1.5f, Pi);
                DrawAmethystSmear(Projectile, Color.Goldenrod, Color.LightGoldenrodYellow * .5f, 2f, PiOver2);
                DrawAmethystSmear(Projectile, Color.Goldenrod, Color.LightGoldenrodYellow * .5f, 2.5f, -PiOver2);
            }
            int length = Projectile.oldPos.Length;
            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 lerpPos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[0], .2f);
                Vector2 oldPos = lerpPos - Main.screenPosition + Projectile.Size / 2f;
                float oldRot = Projectile.oldRot[i];
                //图竖直
                float progress = i / (float)length;
                float xMult = Lerp(1f, .55f, (progress));
                float yMult = Lerp(1f, .55f, progress);
                Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale;
                Color c = Color.Lerp(Color.White, Color.Lerp(Color.DarkGoldenrod, Color.Gold, .63f), EaseInOutQuad(progress));
                float opac = Lerp(1f, .59f, EaseInOutExpo(progress));
                Color pixelColor = Color.Lerp(Color.White, Color.Lerp(Color.DarkGoldenrod, Color.Gold, .63f), EaseInOutQuad(progress));
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(5, 5), pixelColor.ToAddColor(50) * opac * 0.815f, oldRot, projTex.Size() / 2f, scale * .95f, 0);
                SB.FastDraw(projTex, oldPos, c.ToAddColor(0) * opac * .315f, oldRot, projTex.Size() / 2f, scale * .98f, 0);
            }

            for (int i = 0; i < 8; i++)
                SB.FastDraw(projTex, drawPos + Main.rand.NextVector2Circular(1.5f, 1.5f) + (TwoPi / 8f * i).ToRotationVector2() * 1.5f, Color.Gold.ToAddColor(), Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
            return false;
        }
    }
}
