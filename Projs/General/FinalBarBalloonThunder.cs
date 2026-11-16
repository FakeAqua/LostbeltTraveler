using LostbeltTraveler.Core.Lightning;
using LostbeltTraveler.Core.ScreenEffect;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace LostbeltTraveler.Projs.General
{
    public class FinalBarBalloonThunder:LostbeltProjs
    {
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public override EnumDamageClass Category => EnumDamageClass.Typeless;
                public Vector2 beginPos;
        public Vector2 endPos;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
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

            Color color = Main.rand.NextFromList(Color.Goldenrod, Color.Orange, Color.DarkGoldenrod);
            LightningSetting setting = new LightningSetting(BeginPos, EndPos, color*1.2f, 50, 60, 45, 6, 0.5f, 3, 100, 0.6f, 30);
            LightningBuilder.SpawnLightning(setting);

            beginPos = BeginPos;
            endPos = EndPos;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
    }
}
