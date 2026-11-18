using LostbeltTraveler.Globals.Classes;
using Terraria;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Scenes.Music
{
    public class BurialAgencyCelestialPillarFightMusicScene : LostbeltMusicScene
    {
        public override string LoadMusic => LostbeltModMusic.BurialAgencyCelestialPillarFight;
        public override SceneEffectPriority Priority => base.Priority;
        public override bool MusicCondtion(Player player)
        {
            return base.MusicCondtion(player);
        }
    }
}
