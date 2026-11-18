using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.ScreenEffect;
using LostbeltTraveler.Globals.Classes;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Requirement
{
    public class TechCannon : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = Item.height = 32;
            Item.damage = 1234;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.buyPrice(1);
            Item.useTime = Item.useAnimation = 64;
            Item.knockBack = 5;
            Item.DamageType = DamageClass.Ranged;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ProjectileType<TechCannonHeldProj>();
            Item.shootSpeed = 16;
        }
        public override void HoldItem(Player player)
        {
            if (player.HasProj(Item.shoot))
                return;
            int projDamage = (int)player.GetTotalDamage<MagicDamageClass>().ApplyTo(Item.damage);
            Projectile proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, Item.shoot, 0, Item.knockBack, player.whoAmI);
            proj.originalDamage = projDamage;
            proj.netUpdate = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }
    }
    public class TechCannonHeldProj : LostbeltRangedWeaponoutClass
    {
        public override int OriginalItemID => ItemType<TechCannon>();
        public override string Texture => GetInstance<TechCannon>().Texture;
        public override float RecoilPower => 50;
        public override float RecoilWeaponPullbackRatios => base.RecoilWeaponPullbackRatios;
        public override float HoldoutDrawScale => 0.75f;
        public override Color HoldoutEdgeColor => base.HoldoutEdgeColor;
        public override bool HoldoutEdgeEnable => base.HoldoutEdgeEnable;
        public override Vector2 HoldoutOffset => new Vector2(35, -5);
        public override void ExSD()
        {
            base.ExSD();
            Projectile.DamageType = DamageClass.Magic;
        }
        protected override void OnAttack()
        {
            Vector2 offset = new Vector2(90, -10 * Projectile.direction).RotatedBy(Projectile.rotation);
            Vector2 pos = Projectile.Center + offset;
            Vector2 dir = Projectile.SafeDirByRot();
            int type = ProjectileType<TechCannonBullet>();
            for (int i = 0; i < 3; i++)
            {
                Vector2 vel = Projectile.rotation.ToRotationVector2().RotateRandom(ToRadians(15f)).ToSafeNormalize() * Main.rand.NextFloat(0.9f, 1.1f) * 16f;

                Projectile proj = Projectile.NewProjectileDirect(Owner.GetSource_ItemUse(Owner.HeldItem), pos, vel, type, Projectile.originalDamage, Projectile.knockBack, Projectile.owner);
            }
            ScreenShakeSystem.AddScreenShakes(pos, 40, 40, -Projectile.rotation, RandRotTwoPi, true, easingFunc: EaseOutExpo);
            for (int i = 0; i < 8; i++)
            {
                Vector2 vel = dir.ToRandVelocity(ToRadians(15f), 1).ToSafeNormalize();
                ECSParticle.HighResolutionThunder(pos, vel * .1f, RandLerpColor(Color.DodgerBlue, Color.White), 40, 1, vel.ToRotation() + PiOver2, Projectile.scale * .63f, 0);
            }
            Vector2 particleOffset = new Vector2(10, 0 * Projectile.direction).RotatedBy(Projectile.rotation);
            for (int i = 0; i < 36; i++)
            {
                Vector2 pos2 = pos.ToRandCirclePos(8) - particleOffset;
                Vector2 vel = Projectile.SafeDirByRot().ToRandVelocity(ToRadians(15), .1f, 11.6f);
                float scale = Projectile.scale * Main.rand.NextFloat(.95f, 1.15f) * 0.38f;
                int timeLeft = Main.rand.Next(30, 45);
                ECSParticle.ShinyCrossStarECS(pos2, vel, RandLerpColor(Color.CornflowerBlue, Color.White), timeLeft, 1, scale);
            }
            for (int i = 0; i < 36; i++)
            {
                Vector2 pos2 = pos.ToRandCirclePos(8) - particleOffset;
                Vector2 vel = Projectile.SafeDirByRot().ToRandVelocity(ToRadians(0), .1f, 19.6f);
                float scale = Projectile.scale * Main.rand.NextFloat(.95f, 1.15f) * 0.38f;
                int timeLeft = Main.rand.Next(30, 45);
                ECSParticle.LightntingGlow(pos2, vel, RandLerpColor(Color.CornflowerBlue, Color.White), timeLeft, 1, scale);
            }

            for (int i = 0; i < 8; i++)
            {
                Vector2 vel = dir.ToRandVelocity(ToRadians(15f), 1).ToSafeNormalize();
                ECSParticle.HighResolutionThunder(pos + vel.ToSafeNormalize() * 35, vel * .1f, RandLerpColor(Color.DodgerBlue, Color.White), 40, 1, vel.ToRotation(), Projectile.scale * .263f, 1);
            }

            base.OnAttack();
        }
        protected override void PreAttack()
        {
            ScarletSound(LostbeltSoundAssets.ASMD_ExecutionFire, Projectile.Center, 0.3f, 0, -.4f, 0.1f);

            base.PreAttack();
        }
    }
    public class TechCannonBullet : LostbeltProjs
    {
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(20);
        }
        public override void ExSD()
        {
            Projectile.SetupImmnuity(-1);
            Projectile.penetrate = 1;
            Projectile.MaxUpdates = 3;
            Projectile.width = Projectile.height = 32;
            Projectile.ignoreWater = true;
            Projectile.scale = 1;
            Projectile.noEnchantmentVisuals = true;
        }
        public override void ProjAI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.GetTargetSafe(out NPC target))
            {
                Projectile.HomingTarget(target.Center, -1, 19f, 20f);
            }
            else
            {
                if (Projectile.velocity.Length() < 20)
                    Projectile.velocity *= 1.1f;
            }
            if (Projectile.IsOutScreen())
                return;
            if (Main.rand.NextBool(3))
                ECSParticle.LightntingGlow(Projectile.Center.ToRandCirclePosEdge(16), Projectile.velocity / 8f, RandLerpColor(Color.LightSkyBlue, Color.RoyalBlue), 40, 1, Main.rand.NextFloat(.9f, 1.1f) * .46f);
            ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePosEdge(16), Projectile.velocity / 4f, RandLerpColor(Color.LightSkyBlue, Color.DodgerBlue), 40, 1, Main.rand.NextFloat(.9f, 1.1f) * .6f, .15f);
            ECSParticle.HighResolutionThunder(Projectile.Center.ToRandCirclePosEdge(16), Projectile.velocity / 6f, Color.SkyBlue, 40, 1, Projectile.velocity.ToRotation() + PiOver2, Projectile.scale * .3f, 3);
        }
        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return base.OnTileCollide(oldVelocity);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 pos = Projectile.Center.ToRandCirclePos(15);
                Vector2 vel = Projectile.Center.GetNormalVector2(pos) * Main.rand.NextFloat(1.2f, 12f);
                ECSParticle.LightntingGlow(pos, vel, RandLerpColor(Color.White, Color.SkyBlue), Main.rand.Next(40, 70), 1f, Main.rand.NextFloat(.75f, 1.1f) * .75f, Main.rand.Next(2, 5));
            }
            for (int i = 0; i < 16; i++)
            {
                Vector2 pos = Projectile.Center.ToRandCirclePos(3);
                Vector2 vel = Projectile.Center.GetNormalVector2(pos) * Main.rand.NextFloat(4.2f, 14.8f);
                bool value = Main.rand.NextBool();
                BlendState bs = value ? BlendState.Additive : BlendState.AlphaBlend;
                ECSParticle.SmokeParticle(pos, vel, RandLerpColor(Color.SkyBlue, Color.RoyalBlue), Main.rand.Next(35, 55), RandRotTwoPi, .95f, 0.40f * Main.rand.NextFloat(.7f, 1.1f), value, bs);
            }
            for (int i = 0; i < 16; i++)
            {
                Vector2 pos = Projectile.Center.ToRandCirclePos(10);
                Vector2 vel = Projectile.Center.GetNormalVector2(pos) * Main.rand.NextFloat(.42f, 14.8f);
                ECSParticle.ShinyCrossStarSmall(pos, vel, RandLerpColor(Color.RoyalBlue, Color.AliceBlue), 40, 0.7f, 1, 0);
            }
            for (int i = 0; i < 18; i++)
            {
                Vector2 pos = Projectile.Center.ToRandCirclePos(60);
                ECSParticle.TurbulenceShinyOrb(pos, 6.4f, RandLerpColor(Color.White, Color.RoyalBlue), 60, 1, .17f * Main.rand.NextFloat(.85f, 1.1f), glowMult: 0.5f);
            }
            for (int i = 0; i < 16; i++)
            {
                Vector2 pos = Projectile.Center.ToRandCirclePos(10);
                Vector2 vel = Projectile.Center.GetNormalVector2(pos) * Main.rand.NextFloat(1.2f, 9.8f);
                ECSParticle.ShinyCrossStarECS(pos, vel, RandLerpColor(Color.White, Color.RoyalBlue), 35, 1f, 0.95f * Main.rand.NextFloat(.75f, 1.15f), 0.2f);
            }
            ECSParticle.CrossGlow(Projectile.Center, Color.RoyalBlue, 30, .81f, .48f);
            ECSParticle.CrossGlow(Projectile.Center, Color.White, 30, .81f, .45f);

            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (!Projectile.Lostbelt().FirstFrame)
                return false;
            Texture2D fireball = LostbeltModAssets.Texture_FireBall.Value;
            Texture2D fireballPixel = LostbeltModAssets.Texture_FireBallPixel.Value;
            Texture2D glow = LostbeltModAssets.Particle_HRShinyOrbSmall.Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            int length = Projectile.oldPos.Length - 7;
            float overallScale = 0.85f;
            SB.EnterShaderArea();
            SB.FastDraw(glow, drawPos, Color.RoyalBlue, 0, glow.Size() / 2f, Projectile.scale * .5f * overallScale, 0);
            SB.EndShaderArea();
            SB.FastDraw(fireball, drawPos + Main.rand.NextVector2Circular(5, 5), Color.RoyalBlue.ToAddColor() * 1.5f, Projectile.rotation + PiOver2, fireball.Size() / 2f, Projectile.scale * new Vector2(1f, 1.2f) * overallScale, 0);
            SB.FastDraw(fireball, drawPos, Color.White.ToAddColor(0) * .855f, Projectile.rotation + PiOver2, fireball.Size() / 2f, Projectile.scale * .95f * new Vector2(1f, 1.2f) * overallScale, 0);

            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 oldPos = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2f;
                float oldRot = Projectile.oldRot[i] + PiOver2;
                //图竖直
                float progress = i / (float)length;
                float xMult = Lerp(1f, .1f, (progress));
                float yMult = Lerp(1.2f, .55f, progress);
                Vector2 scale = new Vector2(xMult, yMult) * Projectile.scale * overallScale;
                Color c = Color.Lerp(Color.DodgerBlue, Color.Lerp(Color.RoyalBlue, Color.DeepSkyBlue, .5f), EaseInCubic(progress));
                float opac = Lerp(1f, .6f, EaseInCubic(progress));
                Color pixelColor = Color.Lerp(Color.White, Color.Lerp(Color.RoyalBlue, Color.DodgerBlue, .93f), EaseInOutExpo(progress));
                SB.FastDraw(fireball, oldPos + Main.rand.NextVector2Circular(5, 5) * (1 - progress),
                    c.ToAddColor() * opac * 1.15f * (1 - progress), oldRot, fireball.Size() / 2f, scale, 0);
                SB.FastDraw(fireballPixel, oldPos, pixelColor.ToAddColor(100) * opac * 0.815f, oldRot, fireballPixel.Size() / 2f, scale * .90f, 0);
            }
            return false;
        }
    }
}
