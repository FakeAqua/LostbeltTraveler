using LostbeltTraveler.Cooldowns;
using LostbeltTraveler.Core;
using LostbeltTraveler.Core.Cooldowns;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Globals.Methods
{
    public static partial class LostbeltMethods
    {
        public static bool IsLegalTarget(this NPC npc) => !(npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active);
        /// <summary>
        /// 给你的目标来一拳
        /// <br>用于强制移动你的目标，你可以用这个去强制击退</br>
        /// </summary>
        public static void PunchTarget(this NPC target, Vector2 punchDirection, float punchStrength)
        {
            if (!target.IsLegal())
                return;
            Vector2 punchVel = punchDirection.ToSafeNormalize() * punchStrength;
            target.velocity = punchVel;
            //代办：给一拳的击退是需要做多人同步的
            if (Main.netMode != NetmodeID.MultiplayerClient)
                return;
        }
        public static void AddParry(this NPC target, Player player, int parryTime, float parryRatios = .20f)
        {
            if (!player.HasCooldown<ParryShieldCooldown>())
            {
                player.GetImmnue(ImmunityCooldownID.General, 60, true);
                player.AddCooldown(ScarletContent.CooldownType<ParryShieldCooldown>(), GetSeconds(3));
            }
            if (!target.CanBeParry())
                return;
            if (target.Lostbelt().isBeingParry)
                return;
            if (target.Lostbelt().parryTime > 0)
                return;
            target.Lostbelt().parryNoPassingWall = target.noTileCollide;
            target.Lostbelt().parryTime = parryTime;
            target.Lostbelt().ParryProgress += parryRatios;
        }
    }
}
