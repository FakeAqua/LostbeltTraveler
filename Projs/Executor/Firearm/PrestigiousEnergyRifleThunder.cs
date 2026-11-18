using LostbeltTraveler.Core.Lightning;
using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.ScreenEffect;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using Terraria;

namespace LostbeltTraveler.Projs.Executor.Firearm
{
    public class PrestigiousEnergyRifleThunder : LostbeltProjs
    {
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public override EnumDamageClass Category => EnumDamageClass.Executor;
        public Vector2 beginPos;
        public Vector2 endPos;

        public override void ExSD()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.extraUpdates = 0;
            Projectile.timeLeft = 30;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.SetupImmnuity(10);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return base.Colliding(projHitbox, targetHitbox);
        }
        public override void OnFirstFrame()
        {
            ScreenShakeSystem.AddScreenShakes(Owner.Center, 8, 8, RandRotTwoPi);
            Vector2 NowBeginPos = new Vector2(Main.rand.NextFloat(-200, 200), -700);
            Vector2 BeginPos = Projectile.Center + NowBeginPos;
            Vector2 fireVel = BeginPos.GetNormalVector2(Projectile.Center);
            Vector2 EndPos = BeginPos + fireVel * 1400;

            Color c1 = Color.Lerp(Color.MidnightBlue, Color.SkyBlue, .5f);
            Color c2 = Color.Lerp(Color.RoyalBlue, Color.SkyBlue, .5f);
            Color color = Main.rand.NextFromList(c1, c2);
            LightningSetting setting = new LightningSetting(BeginPos, EndPos, color * 1.2f, 45, 60, Main.rand.Next(10, 15), 6, 0.5f, 3, 100, 0.6f, 30);
            LostbeltLightning lostbeltLightning = LightningBuilder.SpawnLightning(setting);
            int c = lostbeltLightning.Nodes.Count;
            for (int i = 0; i < c / 3; i++)
            {
                LightningNode node = lostbeltLightning.Nodes[Main.rand.Next(c)];
                Vector2 p = node.Position;
                for (int j = 0; j < 4; j++)
                    ECSParticle.ShinyCrossStarSmall(p.ToRandCirclePos(32) + fireVel * Main.rand.NextFloat() * 150,
                        fireVel * Main.rand.NextFloat(1f, 1.2f) * 2f, RandLerpColor(c1, c2), 20, 1, 0.25f, Main.rand.NextFloat(-.05f, .05f));
            }

            beginPos = BeginPos;
            endPos = EndPos;

            base.OnFirstFrame();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}
