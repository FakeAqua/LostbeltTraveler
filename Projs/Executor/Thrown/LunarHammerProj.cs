using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.ScreenEffect;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Items.Weapons.Executor.Thrown;
using LostbeltTraveler.Projs.Melee;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Projs.Executor.Thrown
{
    public class LunarHammerProj: LostbeltProjs
    {
        public override string Texture => GetInstance<LunarHammer>().Texture;
        public override EnumDamageClass Category => EnumDamageClass.Executor;
        public ref float Timer => ref Projectile.ai[0];
        public override Vector2 TileHitbox => new Vector2(12,12);
        public enum State
        {
            Shoot,
            Return,
            Homing
        }
        public State AttackState
        {
            get => (State)Projectile.ai[1];
            set => Projectile.ai[1] = (float)value;
        }
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(8, 2);
        }
        public float LerpValue { 
            get
            {
                float distanceSquare = (Projectile.Center - Owner.Center).LengthSquared();
                float maxSquareDistance = 300f * 300f;
                float progress = Utils.GetLerpValue(0, maxSquareDistance, distanceSquare, true);
                return progress;
            }
        }
        public override void ExSD()
        {
            Projectile.width = Projectile.height = 120;
            Projectile.MaxUpdates = 3;
            Projectile.penetrate = -1;
            Projectile.SetupImmnuity(30);
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.noEnchantmentVisuals = true;

        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.BounceOnTile(oldVelocity,.3f);
            return false;
        }
        public override void OnFirstFrame()
        {
            base.OnFirstFrame();
        }
        public override void ProjAI()
        {
            Lighting.AddLight(Projectile.Center, Color.DarkSeaGreen.ToVector3() * 1f);
            Projectile.rotation += .15f;
            switch (AttackState)
            {
                case State.Shoot:
                    DoShoot();
                    break;
                case State.Return:
                    DoReturn();
                    break;
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter % (Projectile.MaxUpdates * 10) == 0&&AttackState != State.Homing)
            {
                float glowScale = Projectile.scale * .3f;
                //ECSParticle.CrossGlow(Projectile.Center, Color.DeepSkyBlue, 120, 0.85f, glowScale, 0.1f);
                //ECSParticle.CrossGlow(Projectile.Center, Color.White, 120, .85f, glowScale * .96f, 0.1f);

                Vector2 pos = Projectile.Center - Vector2.UnitY * 1200f;
                ScarletSound(SoundID.Item88, Projectile.Center);
                Vector2 normalVec = pos.GetNormalVector2(Owner.LocalMouseWorld());
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), pos, normalVec * 18f, ProjectileType<LunarHammerMissle>(), Projectile.damage, 1, Projectile.owner);
                proj.localAI[1] = 1f;
            }

            if (Projectile.IsOutScreen())
                return;
            if (Main.rand.NextBool(18))
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePos(65), -Vector2.UnitY, RandLerpColor(Color.DarkSeaGreen, Color.LightSkyBlue), 40, 1, Main.rand.NextFloat(.85f, 1.15f) * .8f, .2f);
            if (Main.rand.NextBool(18))
                ECSParticle.HRShinyOrb(Projectile.Center.ToRandCirclePosEdge(55), -Vector2.UnitY, RandLerpColor(Color.DarkSeaGreen, Color.LightSkyBlue), 60, 1, Main.rand.NextFloat(.95f, 1.15f)*.1f, .2f);

            base.ProjAI();
        }
        public void DoShoot()
        {
            Timer++;
            if(Timer > Projectile.MaxUpdates * 15f)
            {
                Projectile.netUpdate = true;
                Timer = 0;
                AttackState = State.Return;
                Projectile.ResetLocalNPCHitImmunity();
            }
        }
        public void DoReturn()
        {
            Projectile.tileCollide = false;
            if (Projectile.TooAwayFromOwner())
                Projectile.Kill();
            Projectile.HomingTarget(Owner.Center, -1, 16, 20);
            if (Projectile.IntersectOwnerByDistance(20))
            {
                if (Projectile.Lostbelt().ExecutionStrike)
                {
                    Projectile.netUpdate = true;
                    Timer = 0;
                    AttackState = State.Homing;
                    Projectile.velocity *= -1;
                    Projectile.extraUpdates += 1;

                }
                else
                    Projectile.Kill();
            }
        }
        public void DoHoming()
        {
            if (Projectile.GetTargetSafe(out NPC target))
                Projectile.HomingTarget(target.Center, 600f, 24f, 20f);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(Projectile.Lostbelt().ExecutionStrike&&AttackState == State.Homing)
            {
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ProjectileType<LunarHammerExecution>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                ScarletSound(LostbeltSoundAssets.Misc_SwordHit, Projectile.Center);
                ScreenShakeSystem.AddScreenShakes(Projectile.Center, 8f, 40, RandRotTwoPi);
                Projectile.timeLeft = 2;
            }
            if (!Owner.HasProj<LunarHammerExecution>())
                Projectile.AddExecutionTimeImmediate<LunarHammer>();
            ScarletSound(LostbeltSoundAssets.Smash_AirHeavy, target.Center,variantType:2);
            int dustSets = Main.rand.Next(5, 8);
            int dustRadius = 6;
            Vector2 corner = new(target.Center.X - dustRadius, target.Center.Y - dustRadius);
            for (int i = 0; i < dustSets; ++i)
            {
                float scaleOrb = 1.2f + Main.rand.NextFloat(1f);
                Dust orb = Dust.NewDustDirect(corner, 2 * dustRadius, 2 * dustRadius, DustID.Phantasmal);
                orb.noGravity = true;
                orb.velocity *= 4f;
                orb.scale = scaleOrb;
                for (int j = 0; j < 6; ++j)
                {
                    float scaleSparkle = 0.8f + Main.rand.NextFloat(1.1f);
                    Dust sparkle = Dust.NewDustDirect(corner, 2 * dustRadius, 2 * dustRadius, DustID.Vortex);
                    sparkle.noGravity = true;
                    float dustSpeed = Main.rand.NextFloat(10f, 18f);
                    sparkle.velocity = Main.rand.NextVector2Unit() * dustSpeed;
                    sparkle.scale = scaleSparkle;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            int length = Projectile.oldPos.Length;
            for (int i = 0; i < length; i++)
            {
                float progress = (float)i / length;
                Vector2 oldPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                float oldRot = Projectile.oldRot[i];
                Color c = Color.Lerp(Color.SkyBlue, Color.DarkSeaGreen, progress);
                float scale = (1f - (progress * 0.45f)) * Projectile.scale;
                float opac = Lerp(1f, 0f, progress);
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(3, 3), c.ToAddColor() * .75f * opac * opac, oldRot, ori, scale * 1.1f, 0);
                Color c2 = Color.Lerp(Color.CornflowerBlue, Color.DarkSeaGreen, progress);
                SB.FastDraw(projTex, oldPos, c2.ToAddColor(150) * .9f * opac * opac, oldRot, ori, scale, 0);
            }
            for (int i = 0; i < 3; i++)
            {
                SB.FastDraw(projTex, drawPos+Main.rand.NextVector2Circular(3,3), Color.LightSkyBlue.ToAddColor()*.5f, Projectile.rotation, ori, Projectile.scale*1.05f, 0);
            }
            SB.FastDraw(projTex, drawPos, lightColor, Projectile.rotation, ori, Projectile.scale, 0);
            float lerpRot = Lerp(-PiOver2, PiOver2, EaseOutCubic(LerpValue));
            float opac1 = EaseInCubic(LerpValue);
            GemYoyoProj.DrawAmethystSmear(Projectile, Color.Aquamarine*opac1, Color.LightSkyBlue*opac1, 3.5f*LerpValue, lerpRot);
            return false;
        }
    }
}
