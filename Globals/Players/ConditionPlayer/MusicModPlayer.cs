using Terraria;
using Terraria.GameContent.Events;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Players.ConditionPlayer
{
    public partial class ConditionModPlayer : ModPlayer
    {

        public static bool LanternNightEvent;
        public static bool Town;
        public override void PreUpdate()
        {
            Player p = Main.LocalPlayer;
            LanternNightEvent = LanternNight.LanternsUp && (Ocean ? !(Graveyard) : Surface && !(Graveyard));
            bool isOverThreeNPCs = p.townNPCs > 2;
            Town = isOverThreeNPCs && Surface;
        }

    }
}
