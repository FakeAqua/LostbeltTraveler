using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Players.ConditionPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Scenes.Music
{
    public class LanternNightMusicScene:LostbeltMusicScene
    {
        public override string LoadMusic => "LanternNight";
        public override SceneEffectPriority Priority => ConditionModPlayer.Town ? SceneEffectPriority.Environment : SceneEffectPriority.BiomeMedium;
        public override float GetWeight(Player player)
        {
            return base.GetWeight(player) + .3f;
        }
        public override bool MusicCondtion(Player player) => ConditionModPlayer.LanternNightEvent;
    }
}
