using Terraria.ModLoader;

namespace LostbeltTraveler.Assets.Registers
{
    public partial class LostbeltModAssets : ModSystem
    {
        public static Tex2DWithPath Noise_Misc { get; set; }
        public static Tex2DWithPath Noise_Misc2 { get; set; }
        public static Tex2DWithPath Noise_Aura { get; set; }
        public static Tex2DWithPath Noise_EmptyAura { get; set; }
        public static Tex2DWithPath Noise_HeavyAura { get; set; }
        public static Tex2DWithPath Noise_Smoke { get; set; }
        public static Tex2DWithPath Noise_WaterFlow { get; set; }
        public static Tex2DWithPath Noise_BlackGalaxy1 { get; set; }
        public static Tex2DWithPath Noise_BlackGalaxy2 { get; set; }
        public static Tex2DWithPath Noise_Distortion { get; set; }
        public static Tex2DWithPath Noise_MiscNoise { get; set; }

        public void LoadNoise()
        {
            Noise_Misc = new Tex2DWithPath($"{Path_Noise}{nameof(Noise_Misc)}");
            Noise_Misc2 = new Tex2DWithPath($"{Path_Noise}{nameof(Noise_Misc2)}");
            Noise_Aura = new Tex2DWithPath($"{Path_Noise}{nameof(Noise_Aura)}");
            Noise_EmptyAura = new Tex2DWithPath($"{Path_Noise}{nameof(Noise_EmptyAura)}");
            Noise_HeavyAura = new Tex2DWithPath($"{Path_Noise}{nameof(Noise_HeavyAura)}");
            Noise_Smoke = new Tex2DWithPath($"{Path_Noise}{nameof(Noise_Smoke)}");
            Noise_WaterFlow = new Tex2DWithPath($"{Path_Noise}{nameof(Noise_WaterFlow)}");
            Noise_BlackGalaxy1 = new Tex2DWithPath($"{Path_Noise}{nameof(Noise_BlackGalaxy1)}");
            Noise_BlackGalaxy2 = new Tex2DWithPath($"{Path_Noise}{nameof(Noise_BlackGalaxy2)}");
            Noise_Distortion = new Tex2DWithPath($"{Path_Noise}{nameof(Noise_Distortion)}");
            Noise_MiscNoise = new Tex2DWithPath($"{Path_Noise}{nameof(Noise_MiscNoise)}");


        }
        public void UnloadNoise()
        {
            Noise_Misc = null;
            Noise_Misc2 = null;
            Noise_Aura = null;
            Noise_EmptyAura = null;
            Noise_Smoke = null;
            Noise_HeavyAura = null;
            Noise_WaterFlow = null;
            Noise_BlackGalaxy2 = null;
            Noise_BlackGalaxy1 = null;
            Noise_Distortion = null;
            Noise_MiscNoise = null;
        }
    }
}
