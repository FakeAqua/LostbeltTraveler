using System.Collections.Generic;
using Terraria.ModLoader;

namespace LostbeltTraveler.Assets.Registers
{
    public class LostbeltModMusic : ModSystem
    {
        public static string BurialAgencyCelestialPillarFight => "BurialAgencyCelestialPillarFight";
        public static string LostbeltFatalBattle => "LostbeltFatalBattle";

        public static Dictionary<string, int> MusicDictionary;
        public static int ApplyMusic(string keys)
        {
            if (MusicDictionary.TryGetValue(keys, out int slot))
                return slot;
            return -1;
        }
        public override void Load()
        {
            static string paths(string key)
            {
                return "Assets/Music/" + key;
            }
            MusicDictionary = new Dictionary<string, int>
            {
                {BurialAgencyCelestialPillarFight, MusicLoader.GetMusicSlot(Mod, paths(BurialAgencyCelestialPillarFight)) },
                {LostbeltFatalBattle, MusicLoader.GetMusicSlot(Mod, paths(LostbeltFatalBattle)) },
                {"LanternNight", MusicLoader.GetMusicSlot(Mod, paths("LanternNight")) }
            };
        }
        public override void Unload()
        {
            base.Unload();
        }
    }
}
