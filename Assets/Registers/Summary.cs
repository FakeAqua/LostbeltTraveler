using Terraria.ModLoader;

namespace LostbeltTraveler.Assets.Registers
{
    public partial class LostbeltModAssets : ModSystem
    {
        public static Tex2DWithPath ScarletGhost { get; set; }
        public static Tex2DWithPath LostbeltJourneyIcon { get; set; }
        public static Tex2DWithPath LostbeltJourneyIconLegacy { get; set; }
        public static Tex2DWithPath InvisAsset { get; private set; }
        private string TexPath => "LostbeltTraveler/Assets/Texture";
        private string Path_Particle => $"{TexPath}/Particles/";
        private string Path_General => $"{TexPath}/General/";
        private string Path_Metaball => $"{TexPath}/Metaball/";
        private string Path_Noise => $"{TexPath}/Noises/";
        private static string InvisAssetPath => "LostbeltTraveler/Assets/Texture/InvisibleProj";


        public override void Load()
        {
            InvisAsset = new Tex2DWithPath(InvisAssetPath);
            ScarletGhost = new Tex2DWithPath($"{TexPath}/{nameof(ScarletGhost)}");
            LostbeltJourneyIcon = new Tex2DWithPath("LostbeltTraveler/icon_small");
            LostbeltJourneyIconLegacy = new Tex2DWithPath("LostbeltTraveler/Assets/LegacyContent/icon_small");
            LoadParticle();
            LoadTrail();
            LoadTexture();
            LoadMisc();
            LoadHud();
            LoadNoise();
        }
        public override void Unload()
        {
            InvisAsset = null;
            ScarletGhost = null;
            LostbeltJourneyIcon = null;
            LostbeltJourneyIconLegacy = null;
            UnLoadParticle();
            UnloadTrail();
            UnloadTexture();
            UnloadMisc();
            UnloadHud();
            UnloadNoise();
        }
    }
}
