using LostbeltTraveler.Globals.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Scenes.Music
{
    public class LostbeltFatalBattleMusicScene:LostbeltMusicScene
    {
        public override string LoadMusic => LostbeltModMusic.LostbeltFatalBattle;
        public override SceneEffectPriority Priority => base.Priority;
        public override bool MusicCondtion(Player player)
        {
            return base.MusicCondtion(player);
        }
    }
}
