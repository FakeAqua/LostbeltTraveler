using ContinentOfJourney.Projectiles;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;

namespace LostbeltTraveler.Projs.Ranged
{
    public class PropellerRapierRangedTornado : LostbeltProjs
    {
        public override string Texture => GetInstance<Tornado_Type2>().Texture;
        public override EnumDamageClass Category => EnumDamageClass.Ranged;
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(4);
        }
        public override void ExSD()
        {
            base.ExSD();
        }
        public override void OnFirstFrame()
        {
            base.OnFirstFrame();
        }
        public override void ProjAI()
        {
            base.ProjAI();
        }
        public override bool? CanDamage()
        {
            return base.CanDamage();
        }

        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}
