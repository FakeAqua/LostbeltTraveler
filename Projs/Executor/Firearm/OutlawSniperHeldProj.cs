using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.ScreenEffect;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Items.Weapons.Executor.Firearm;
using Terraria;

namespace LostbeltTraveler.Projs.Executor.Firearm
{
    public class OutlawSniperHeldProj : LostbeltRangedWeaponoutClass
    {
        public override EnumDamageClass Category => EnumDamageClass.Executor;
        public override int OriginalItemID => ItemType<OutlawSniper>();
        public override string Texture => GetInstance<OutlawSniper>().Texture;
        public override float RecoilPower => 15;
        public override int ProjExtraUpdates => 2;
        public override float HoldoutDrawScale => base.HoldoutDrawScale;
        public override Vector2 HoldoutOffset => new Vector2(20, 0);
        public override Color HoldoutEdgeColor => base.HoldoutEdgeColor;
        protected override void PreAttack()
        {
            if (!Owner.GetExecutionSrike())
                Projectile.Lostbelt().ExecutionStrike = false;
            if (Owner.GetExecutionSrike() && !Projectile.Lostbelt().ExecutionStrike)
            {
                Projectile.Lostbelt().ExecutionStrike = true;
                Owner.RemoveExecutionProgress(OriginalItemID);
            }
        }
        protected override void OnAttack()
        {
            Vector2 dir = Projectile.SafeDirByRot();
            int type = ProjectileType<OutlawSniperBullet>();
            int projDamage = (int)Owner.GetTotalDamage<ExecutorDamageClass>().ApplyTo(Owner.HeldItem.damage);
            Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(),
                Projectile.Center, dir * 17f, type, projDamage, Projectile.knockBack, Projectile.owner);
            proj.Lostbelt().HasExecutionMechanic = true;
            proj.Lostbelt().ExecutionStrike = Projectile.Lostbelt().ExecutionStrike;
            proj.MaxUpdates = Projectile.Lostbelt().ExecutionStrike ? 4 : 3;
            if (proj.Lostbelt().ExecutionStrike)
            {
                ScreenShakeSystem.AddScreenShakes(Projectile.Center, 20f, 20, Projectile.rotation, 0.1f);
                ScarletSound(LostbeltSoundAssets.Misc_PistolClear, Projectile.Center, 0.20f, 1, -.3f, 0.1f);
            }
            else
            {
                ScreenShakeSystem.AddScreenShakes(Projectile.Center, 10f, 10, Projectile.rotation, 0.1f);
                ScarletSound(LostbeltSoundAssets.Misc_Pistol, Projectile.Center, 0.20f, 1, -.3f, 0.1f);
            }
            Vector2 offset = new Vector2(45, -5 * Projectile.direction).RotatedBy(Projectile.rotation);
            Vector2 pos = Projectile.Center + offset;
            Vector2 particleOffset = new Vector2(10, 0 * Projectile.direction).RotatedBy(Projectile.rotation);
            for (int i = 0; i < 12; i++)
            {
                Vector2 pos2 = pos.ToRandCirclePos(8) - particleOffset;
                Vector2 vel = Projectile.SafeDirByRot().ToRandVelocity(ToRadians(15), .1f, 11.6f);
                float scale = Projectile.scale * Main.rand.NextFloat(.95f, 1.15f) * 0.38f;
                int timeLeft = Main.rand.Next(30, 45);
                ECSParticle.ShinyCrossStarECS(pos2, vel, RandLerpColor(Color.OrangeRed, Color.Goldenrod), timeLeft, 1, scale);
            }
            for (int i = 0; i < 12; i++)
            {
                Vector2 pos2 = pos.ToRandCirclePos(8) - particleOffset;
                Vector2 vel = Projectile.SafeDirByRot().ToRandVelocity(ToRadians(0), .1f, 12.6f);
                float scale = Projectile.scale * Main.rand.NextFloat(.95f, 1.15f) * 0.38f;
                int timeLeft = Main.rand.Next(30, 45);
                ECSParticle.LightntingGlow(pos2, vel, RandLerpColor(Color.OrangeRed, Color.Goldenrod), timeLeft, 1, scale);
            }

            base.OnAttack();
        }
    }
}
