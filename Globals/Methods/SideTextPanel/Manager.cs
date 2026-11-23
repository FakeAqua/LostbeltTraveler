using Terraria;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Methods.SideTextPanel
{
    public class SideTextPanelManager : ModSystem
    {
        public static float LerpValue = 0;
        public override void PostUpdateTime()
        {
            if (Main.dedServ)
                return;
            Player p = Main.LocalPlayer;
            if (!Main.HoverItem.IsLegal())
            {
                LerpValue = 0;
                return;
            }
            LerpValue = Lerp(LerpValue, 1f, .2f);
            if (LerpValue > .98f)
            {
                LerpValue = 1;
            }
        }
    }
}
