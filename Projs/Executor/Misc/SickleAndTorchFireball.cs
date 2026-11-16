using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;

namespace LostbeltTraveler.Projs.Executor.Misc
{
    public class SickleAndTorchFireball : LostbeltProjs
    {
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public override EnumDamageClass Category => EnumDamageClass.Executor;
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(8);
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
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return base.OnTileCollide(oldVelocity);
        }
        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return base.PreDraw(ref lightColor);
        }
    }
}
