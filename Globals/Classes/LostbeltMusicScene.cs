using Terraria;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Classes
{
    public class LostbeltMusicScene : ModSceneEffect
    {
        public virtual string LoadMusic { get; }
        public virtual bool MusicCondtion(Player player) => false;
        public override int Music => LostbeltModMusic.ApplyMusic(LoadMusic);
        public override float GetWeight(Player player)
        {
            return .7f;
        }
        public override SceneEffectPriority Priority => base.Priority;
        public override bool IsSceneEffectActive(Player player)
        {
            return MusicCondtion(player);
        }
    }
}
