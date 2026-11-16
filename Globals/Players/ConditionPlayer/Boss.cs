using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Players.ConditionPlayer
{
    public partial class ConditionModPlayer : ModPlayer
    {
        /// <summary>
        /// 是否位于FTW
        /// </summary>
        public static bool ForTheWorthy;
        public void UpdateBossCondition()
        {
            ForTheWorthy = Main.getGoodWorld;
        }

    }
}
