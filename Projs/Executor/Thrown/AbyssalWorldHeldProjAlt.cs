using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Globals.Executor;
using Terraria.ModLoader;

namespace LostbeltTraveler.Projs.Executor.Thrown
{
    /// <summary>
    /// 这个射弹用于高举
    /// </summary>
    public class AbyssalWorldHeldProjAlt : ExecutorHeldProj
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        public override string Texture => GetInstance<AbyssalWorldHeldProj>().Texture;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
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
