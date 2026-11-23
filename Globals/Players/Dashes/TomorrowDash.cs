using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Globals.Systems;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Players.Dashes
{
    public class TomorrowDash : PlayerDashClass
    {
        public override int ImmuneTime(Player player) => 0;
        public override int DashTime(Player player) => 20;
        public override int DashDelay(Player player) => 16;
        public override DashEnum DashOnHitType => DashEnum.Slam;
        public override DashDamageInfo DashDamageInfo(Player player)
        {
            return new DashDamageInfo(50, 3f, DamageClass.Generic);
        }
        public override DashDirectionEnum DashDirection => DashDirectionEnum.VerticalAndHorizonal;
        public override float DashSpeed(Player player) => 26f;
        public override float DashEndSpeedMult(Player player) => 0.75f;
        public override void OnDashStart(Player player)
        {
            SoundEngine.PlaySound(LostbeltSoundAssets.GalvanizedHand_Charge with { Variants = [1], MaxInstances = 0, PitchVariance = .2f, Pitch = -.4f });
        }
        public override void OnDashEnd(Player player)
        {
            base.OnDashEnd(player);
        }
        public override void UpdateDash(Player player)
        {
            for (int i = 0; i < 4; i++)
            {
                Vector2 pos = player.ToRandRec();
                Vector2 dir = player.velocity.ToSafeNormalize() * -1f;
                Vector2 vel = dir * Main.rand.NextFloat(0.3f, 1.7f);
                int lifeTime = Main.rand.Next(30, 70);
                pos += player.velocity.ToSafeNormalize() * 5f * i;
                Vector2 stainPos = pos.ToRandCirclePos(6);
                float stainScale = Main.rand.NextFloat(.9f, 1.1f) * .4f;
                ECSParticle.HighResolutionThunder(stainPos - player.velocity, -player.velocity / 8f,
                    RandLerpColor(Color.Goldenrod, Color.DarkGoldenrod), 45, 1, player.velocity.ToRotation(), 0.14f, 1);
                //ECSParticle.SmokeParticle(stainPos, vel, RandLerpColor(Color.Goldenrod, Color.LightGoldenrodYellow),45, RandRotTwoPi, 1, stainScale, Main.rand.NextBool(), BlendState.Additive);

            }
            for (int j = 0; j < 3; j++)
            {
                for (int i = -1; i < 2; i += 2)
                {
                    Vector2 pos = new Vector2(0, 20 * i).RotatedBy(player.velocity.ToRotation()) + player.Center;
                    pos += player.velocity.ToSafeNormalize() * -1.5f * j;
                    //ECSParticle.ShrinkParticle(pos, -player.velocity / 8, Color.Lerp(Color.Goldenrod,Color.LightGoldenrodYellow,1-player.GetModPlayer<ScarletDashPlayer>().DashTime /DashTime(player)), 45, .97f, player.velocity.ToRotation() + PiOver2, 0.2f, 0, new Vector2(0.5f, 1.2f), 0, 0, BlendState.Additive);
                    ECSParticle.HighResolutionThunder(pos, -player.velocity / 8f,
                        RandLerpColor(Color.Goldenrod, Color.LightGoldenrodYellow), 45, 1, player.velocity.ToRotation() + PiOver2, 0.34f, 3);
                    ECSParticle.LightntingGlow(pos, -player.velocity / 8f, RandLerpColor(Color.Goldenrod, Color.LightGoldenrodYellow), 45, 1, 0.5f);
                }
            }
            for (int i = 0; i < 2; i++)
            {
                Vector2 pos2 = player.Center.ToRandCirclePos(8);
                Vector2 vel2 = player.velocity / 8f;
                Color color2 = RandLerpColor(Color.LightGoldenrodYellow, Color.Goldenrod);
                ECSParticle.TurbulenceShinyOrb(pos2, 2.6f, color2, 50, 1, 0.14f, glowMult: .5f);
                //ECSParticle.HRShinyOrb(pos2, vel2, color2, 60, .81f, 0.08f, 0.85f);
            }
        }
        public override void OnHitNPC(Player player, NPC target, int DamageDone)
        {
        }
    }

}
