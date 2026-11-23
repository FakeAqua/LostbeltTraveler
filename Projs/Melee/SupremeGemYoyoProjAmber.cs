using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Items.Weapons.Melee;
using System;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Projs.Melee
{
    public class SupremeGemYoyoProjAmber : LostbeltProjs
    {
        public override string Texture => GetVanillaAssetPath(VanillaAsset.Item, ItemID.Amber);
        public override EnumDamageClass Category => EnumDamageClass.Melee;
        /// <summary>
        /// 这个用于存放当前匕首在哪个位置（即排序）
        /// </summary>
        public int DaggerIndex => (int)Projectile.ai[2];
        /// <summary>
        /// 存储当前匕首的“额外角度”，用于悬挂在后方
        /// </summary>
        public ref float RotationOffset => ref Projectile.localAI[0];
        public ref float Timer => ref Projectile.ai[0];
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(32);
        }
        public override void ExSD()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 1000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 2;
            Projectile.localNPCHitCooldown = 8 * Projectile.MaxUpdates;
        }
        public override bool? CanDamage() => false;
        public override void ProjAI()
        {
            if (Owner.IsHolding<SupremeGem>() && !Owner.dead)
                Projectile.timeLeft = 2;
            else
                Projectile.Kill();
            DoIdle();
        }
        public void BonkHostileProjectile()
        {
            Rectangle hitbox = Utils.CenteredRectangle(Projectile.Center, new Vector2(40));
            foreach (var p in Main.ActiveProjectiles)
            {
                if (!p.hostile)
                    continue;
                if (p.damage > 150)
                    continue;
                if (p.Lostbelt().isBounceByAmber)
                    continue;
                if (p.Hitbox.Intersects(hitbox))
                {
                    p.velocity = -p.velocity;
                    p.Lostbelt().isBounceByAmber = true;
                    Projectile.Kill();
                }
            }
        }
        public void DoIdle()
        {
            if (Projectile.Center.HasNaNs())
                Projectile.Center = Owner.MountedCenter;
            if (Main.rand.NextBool(16))
                ECSParticle.ShinyCrossStarSmall(Projectile.Center.ToRandCirclePos(16), RandVelTwoPi(1, 1.1f) * .5f, RandLerpColor(Color.Orange, Color.OrangeRed), 40, 1, .225f, 0);
            BonkHostileProjectile();
            Vector2 extraPos = (Vector2.UnitY * 3).RotatedBy((Main.GlobalTimeWrappedHourly * .5f + DaggerIndex) * 7);
            Vector2 destination = Owner.Center + extraPos + Vector2.UnitY * 35f + (Vector2.UnitX * Owner.direction).RotatedBy(RotationOffset * Owner.direction / 4f) * (-30 - DaggerIndex * 15);
            Projectile.velocity = (destination - Projectile.Center) / 18f;

        }
        public override void OnKill(int timeLeft)
        {
            Vector2 spawnPos = Projectile.Center;
            ECSParticle.ShinyCrossStarSmall(spawnPos, Vector2.Zero, Color.Orange, 40, 1, 0.82f, 0);
            ECSParticle.HRShinyOrb(spawnPos, Vector2.Zero, Color.OrangeRed, 40, 1, 0.2f, 0.75f);
            for (int i = 0; i < 5; i++)
            {
                ECSParticle.ShinyCrossStarECS(spawnPos, RandVelTwoPi(4), RandLerpColor(Color.Orange, Color.OrangeRed), 45, 1, 0.2f, 0.2f);
            }
            ScarletSound(SoundID.DD2_CrystalCartImpact, Projectile.Center);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.GetProjDrawData(out Texture2D tex, out Vector2 drawPos, out Vector2 ori);
            float edgeShiningProgress = Math.Abs((float)Math.Sin(Main.timeForVisualEffects * .01f));
            SB.EnterShaderArea();
            Vector2 tipPos = Projectile.Center - Main.screenPosition;

            Texture2D orb = LostbeltModAssets.Particle_HRShinyOrbSmall.Value;
            Vector2 vec = new Vector2(1f, 1f);
            SB.FastDraw(orb, tipPos, Color.OrangeRed * .85f * edgeShiningProgress, Projectile.rotation, orb.Size() / 2f, vec * .45f);

            SB.EndShaderArea();
            Vector2 pos = Projectile.Center - Main.screenPosition;
            float rot = Projectile.rotation + PiOver4;

            SB.FastDraw(tex, pos, Color.White, rot, ori, Projectile.scale);
            return false;
        }
    }
}
