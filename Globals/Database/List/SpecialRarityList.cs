using LostbeltTraveler.Globals.Database.Enums;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Database.List
{
    public partial class LostbeltList : ModSystem
    {
        /// <summary>
        /// 主要用于存储并引用特殊稀有度绘制
        /// </summary>
        public static Dictionary<int, ShinyRarityType> ShinyRarityItemDictionary = [];
    }
}
