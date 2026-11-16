using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Globals.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Players.Dashes
{
     public class FeatherDash : PlayerDashClass
    {
        public bool AllowWingTimeReset = false;
        public override int ImmuneTime(Player player) => 0;
        public override int DashTime(Player player) => 18;
        public override int DashDelay(Player player) => 12;
        public override DashEnum DashOnHitType => DashEnum.Slam;
        public override DashDamageInfo DashDamageInfo(Player player)
        {
            return new DashDamageInfo(100, 3f, DamageClass.Generic);
        }
        public override float DashSpeed(Player player) => 28f;
        public override float DashEndSpeedMult(Player player) => 0.5f;
        public override void OnDashStart(Player player)
        {
            AllowWingTimeReset = true;
            ScarletSound(LostbeltSoundAssets.TheSevenStar_Swing, player.Center, 1, 1, pitch: .1f, .1f, 1);
        }
        public override void OnDashEnd(Player player)
        {
            if (AllowWingTimeReset)
            {
                if (player.Lostbelt().chargeWingsRestore >0)
                {
                    player.wingTime = player.wingTimeMax * player.Lostbelt().chargeWingsRestore;
                }
                    AllowWingTimeReset = false;
            }
            base.OnDashEnd(player);
        }
        public override void UpdateDash(Player player)
        {
            //for (int i = 0; i < 4; i++)
            //{
            //    Vector2 pos2 = player.ToRandRec();
            //    Vector2 vel2 = player.velocity / 8f;
            //    Color color2 = RandLerpColor(Color.DarkRed, Color.Crimson);
            //    ECSParticle.SmokeParticle(pos2, vel2, color2, 40, 0, .55f, 0.65f);
            //}
                        for (int i = 0; i < 2; i++)
            {
                Vector2 pos = player.ToRandRec();
                Vector2 dir = player.velocity.ToSafeNormalize() * -1f;
                Vector2 vel = dir * Main.rand.NextFloat(0.3f, 1.7f);
                int lifeTime = Main.rand.Next(30, 70);
                pos += player.velocity.ToSafeNormalize() * 20f;
                //ECSParticle.SmokeParticle(pos, vel.RotatedBy(ToRadians(15f) * i), Color.White, lifeTime, RandRotTwoPi, 1, 0.3f, true,BlendState.Additive);
                Vector2 stainPos = pos.ToRandCirclePos(6);
                float stainScale = Main.rand.NextFloat(.9f, 1.1f) * .4f;
                ECSParticle.ShrinkParticle(stainPos, vel, Color.White, 45, 0.49f, dir.ToRotation(), stainScale, 1, new Vector2(2.0f, 1.4f), 0, 0, BlendState.Additive);
            }
            for (int i = -1; i < 2; i += 2)
            {
                Vector2 pos = new Vector2(0, 20 * i).RotatedBy(player.velocity.ToRotation()) + player.Center;
                pos += player.velocity.ToSafeNormalize() * -5f;
                ECSParticle.ShrinkParticle(pos, -player.velocity / 8, Color.White, 45, .7f, player.velocity.ToRotation() + PiOver2, 0.2f, 0, new Vector2(0.5f, 1.2f), 0, 0, BlendState.Additive);
            }
            for (int i = 0; i < 2; i++)
            {
                Vector2 pos = player.ToRandRec();
                pos += player.velocity.ToSafeNormalize() * 20f;
                //ECSParticle.TurbulenceShinyOrb(pos, 1.2f, Color.White, 45, 1, 0.15f, blendState: BlendState.Additive);
                ECSParticle.ShinyCrossStarECS(pos, player.velocity.ToSafeNormalize(), Color.White, 40, 1, 0.5f, 0.2f);
            }
        }
        public override void OnHitNPC(Player player, NPC target, int DamageDone)
        {
            Vector2 dir = player.velocity.ToSafeNormalize();
            ScarletSound(LostbeltSoundAssets.SodomsDisaster_BoomHit, target.Center);
            for (int i = 0; i < 32; i++)
            {
                ECSParticle.SmokeParticle(target.Center, dir.ToRandVelocity(ToRadians(35), 1.2f, 22.5f), RandLerpColor(Color.DarkRed, Color.Black), 40, RandRotTwoPi, 1, Main.rand.NextFloat(.9f, 1.2f) * .52f, blendstate: BlendState.AlphaBlend);
            }
            for (int i = 0; i < 32; i++)
            {
                ECSParticle.SmokeParticle(target.Center, RandVelTwoPi(1.2f, 8f), RandLerpColor(Color.Red, Color.Black), 40, RandRotTwoPi, 1, Main.rand.NextFloat(.9f, 1.2f) * .28f, Main.rand.NextBool(), blendstate: BlendState.NonPremultiplied);
            }
            for (int i = 0; i < 16; i++)
            {
                ECSParticle.LiliesFire(target.Center, RandVelTwoPi(1.2f, 8f), RandLerpColor(Color.Black, Color.Red), 40, RandRotTwoPi, 1, 0.2f * Main.rand.NextFloat(0.45f, 1.3f));
            }
            for (int i = 0; i < 16; i++)
            {
                ECSParticle.ShinyCrossStarECS(target.Center, RandVelTwoPi(1.2f, 8f), RandLerpColor(Color.DarkRed, Color.Red), 40, 1, 0.92f * Main.rand.NextFloat(0.95f, 1.3f), blendstate: BlendState.AlphaBlend);
            }

        }
    }
}
