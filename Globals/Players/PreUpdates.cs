using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Players
{
    public partial class LostbeltPlayer : ModPlayer
    {
        public override void PreUpdate()
        {
            if (infiniteFlightTime)
            {
                Player.wingTime = Player.wingTimeMax;
                Player.rocketTime = Player.rocketTimeMax;
            }
        }
    }
}
