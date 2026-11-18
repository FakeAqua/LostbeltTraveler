using LostbeltTraveler.Globals.Executor;

namespace LostbeltTraveler.Projs.Executor.ColdSteel
{
    public class CrescentRoseHeldSniper : ExecutorHeldProj
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
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
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}
