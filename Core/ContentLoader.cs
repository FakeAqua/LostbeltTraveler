using LostbeltTraveler.Core.Cooldowns;
using LostbeltTraveler.Globals.Systems;

namespace LostbeltTraveler.Core
{
    public static class ScarletContent
    {
        public static int DashType<T>() where T : PlayerDashClass => GetInstance<T>()?.Type ?? 0;
        public static int CooldownType<T>() where T : ScarletCooldown => GetInstance<T>()?.Type ?? 0;
    }
}
