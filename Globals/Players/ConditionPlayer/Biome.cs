using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Players.ConditionPlayer
{
    /// <summary>
    /// 代办：实际上按理来说不需要存放至玩家类
    /// </summary>
    public partial class ConditionModPlayer : ModPlayer
    {
        /// <summary>
        /// 是否为海洋环境
        /// </summary>
        public static bool Ocean;
        /// <summary>
        /// 是否为神圣地环境
        /// </summary>
        public static bool Hallow;
        /// <summary>
        /// 是否为森林
        /// </summary>
        public static bool Forest;
        /// <summary>
        /// 是否为地表
        /// </summary>
        public static bool Surface;
        /// <summary>
        /// 是否为地下
        /// </summary>
        public static bool Underground;
        /// <summary>
        /// 是否为沙漠
        /// </summary>
        public static bool Desert;
        /// <summary>
        /// 是否为雪原
        /// </summary>
        public static bool Snow;
        /// <summary>
        /// 是否为丛林
        /// </summary>
        public static bool Jungle;
        /// <summary>
        /// 是否为墓地
        /// </summary>
        public static bool Graveyard;
        /// <summary>
        /// 是否为血腥地
        /// </summary>
        public static bool Crimson;
        /// <summary>
        /// 是否为腐化地
        /// </summary>
        public static bool Corruption;
        /// <summary>
        /// 是否为地狱
        /// </summary>
        public static bool Underworld;
        /// <summary>
        /// 是否为太空
        /// </summary>
        public static bool Space;
        /// <summary>
        /// 是否为地牢
        /// </summary>
        public static bool Dungeon;
        /// <summary>
        /// 是否为丛林神庙
        /// </summary>
        public static bool JungleTemple;
        /// <summary>
        /// 是否为微光湖
        /// </summary>
        public static bool Aether;
        public void UpdateBiomeCondition()
        {
            Player p = Main.LocalPlayer;
            Hallow = p.ZoneHallow;
            Dungeon = p.ZoneDungeon;
            Space = p.ZoneNormalSpace;
            Jungle = p.ZoneJungle;
            JungleTemple = p.ZoneLihzhardTemple;
            Underworld = p.ZoneUnderworldHeight;
            Crimson = p.ZoneCrimson;
            Graveyard = p.ZoneGraveyard;
            Ocean = p.ZoneBeach && !Hallow && !Crimson && !Corruption;

            Forest = p.ZoneForest;
            Desert = p.ZoneDesert;
            Snow = p.ZoneSnow;
            Corruption = p.ZoneCorrupt;
            Aether = p.ZoneShimmer;
            Surface = p.ZoneOverworldHeight;
            Underground = p.ZoneDirtLayerHeight || p.ZoneRockLayerHeight;
        }
    }
}
