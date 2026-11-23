using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Methods.SideTextPanel
{
    public struct SideTextPanelData(TooltipLine line, float scale = 1f, int buffIcon = -1)
    {
        public TooltipLine Line = line;
        public float Size = scale;
        public int buffIcon = buffIcon;
    }
}
