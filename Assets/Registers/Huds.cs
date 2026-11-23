using ReLogic.Content;
using Terraria.ModLoader;

namespace LostbeltTraveler.Assets.Registers
{
    public partial class LostbeltModAssets : ModSystem
    {
        private string Path_Huds => $"{TexPath}/Huds/";
        public static Asset<Texture2D> Hud_ExecutorCounter { get; set; }
        public static Asset<Texture2D> Hud_ExecutorColdSteel { get; set; }
        public static Asset<Texture2D> Hud_ExecutorMisc { get; set; }
        public static Asset<Texture2D> Hud_ExecutorAssist { get; set; }
        public static Asset<Texture2D> Hud_ExecutorFirearm { get; set; }
        public static Asset<Texture2D> Hud_ExecutorCaster { get; set; }
        public static Asset<Texture2D> Hud_ExecutorThrown { get; set; }
        public static Asset<Texture2D> Hud_DarkOverlayer { get; set; }
        public static Asset<Texture2D> Hud_Glow { get; set; }
        public static Asset<Texture2D> Hud_GlowNoEdge { get; set; }
        public static Asset<Texture2D> Hud_GlowNoEdgeAlt { get; set; }
        public static Asset<Texture2D> Hud_GlowEdge { get; set; }
        public static Asset<Texture2D> Hud_GlowEdgeAlt { get; set; }

        public void LoadHud()
        {
            Hud_ExecutorCounter = Request<Texture2D>($"{Path_Huds}{nameof(Hud_ExecutorCounter)}");
            Hud_DarkOverlayer = Request<Texture2D>($"{Path_Huds}{nameof(Hud_DarkOverlayer)}");
            Hud_Glow = Request<Texture2D>($"{Path_Huds}{nameof(Hud_Glow)}");
            Hud_GlowEdge = Request<Texture2D>($"{Path_Huds}{nameof(Hud_GlowEdge)}");
            Hud_GlowEdgeAlt = Request<Texture2D>($"{Path_Huds}{nameof(Hud_GlowEdgeAlt)}");
            Hud_GlowNoEdge = Request<Texture2D>($"{Path_Huds}{nameof(Hud_GlowNoEdge)}");
            Hud_GlowNoEdgeAlt = Request<Texture2D>($"{Path_Huds}{nameof(Hud_GlowNoEdgeAlt)}");
            Hud_ExecutorAssist = Request<Texture2D>($"{Path_Huds}{nameof(Hud_ExecutorAssist)}");
            Hud_ExecutorMisc = Request<Texture2D>($"{Path_Huds}{nameof(Hud_ExecutorMisc)}");
            Hud_ExecutorFirearm = Request<Texture2D>($"{Path_Huds}{nameof(Hud_ExecutorFirearm)}");
            Hud_ExecutorCaster = Request<Texture2D>($"{Path_Huds}{nameof(Hud_ExecutorCaster)}");
            Hud_ExecutorColdSteel = Request<Texture2D>($"{Path_Huds}{nameof(Hud_ExecutorColdSteel)}");
            Hud_ExecutorThrown = Request<Texture2D>($"{Path_Huds}{nameof(Hud_ExecutorThrown)}");
        }
        public static void UnloadHud()
        {
            Hud_ExecutorCounter = null;
            Hud_DarkOverlayer = null;
            Hud_Glow = null;
            Hud_GlowEdge = null;
            Hud_GlowEdgeAlt = null;
            Hud_GlowNoEdge = null;
            Hud_GlowNoEdgeAlt = null;
            Hud_ExecutorColdSteel = null;
            Hud_ExecutorCaster = null;
            Hud_ExecutorThrown = null;
            Hud_ExecutorAssist = null;
            Hud_ExecutorMisc = null;
            Hud_ExecutorFirearm = null;
            Hud_ExecutorThrown = null;
        }
    }
}
