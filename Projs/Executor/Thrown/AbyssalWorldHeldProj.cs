using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Items.Weapons.Executor.Caster;
using Terraria.ModLoader;

namespace LostbeltTraveler.Projs.Executor.Thrown
{
    public class AbyssalWorldHeldProj : ExecutorHeldProj
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        public override int OriginalItemID => ItemType<AbyssalWorld>();
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting();
        }
        public override void ExSD()
        {
            Projectile.SetUpHeldProj(10);
        }
        public override void OnFirstFrame()
        {
            base.OnFirstFrame();
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
