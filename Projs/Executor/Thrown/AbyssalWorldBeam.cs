using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;

namespace LostbeltTraveler.Projs.Executor.Thrown
{
    public class AbyssalWorldBeam : LostbeltProjs
    {
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public override EnumDamageClass Category => EnumDamageClass.Executor;
        public override void ExSD()
        {
            base.ExSD();
        }
        public override void ProjAI()
        {
            base.ProjAI();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}
