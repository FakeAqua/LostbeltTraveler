using LostbeltTraveler.Globals.Players.InnerCooldown.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace LostbeltTraveler.Globals.Players.InnerCooldown.Methods
{
    public static partial class InnerCooldownMethod
    {
        public static InnerCooldownPlayer InnerCooldown(this Player player) => player.GetModPlayer<InnerCooldownPlayer>();
        public static void AddInnerCooldown(this Player player, InnerCooldownType type, int time)
        {
            if (player.InnerCooldown().ActiveInnerCD.ContainsKey(type))
                player.InnerCooldown().ActiveInnerCD[type] = time;
            else
                player.InnerCooldown().ActiveInnerCD.TryAdd(type, time);
        }
        public static int GetInnerCooldownTime(this Player player, InnerCooldownType type)
        {
            int count = -1;
            if (player.InnerCooldown().ActiveInnerCD.TryGetValue(type, out count))
                return count;
            return count;

        }
        public static bool HasInnerCooldown(this Player player, InnerCooldownType type)
        {
            return player.InnerCooldown().ActiveInnerCD.ContainsKey(type);
        }
    }
}
