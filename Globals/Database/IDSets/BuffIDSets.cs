using LostbeltTraveler.Buffs.GoodBuffs;
using LostbeltTraveler.Buffs.Status;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Database.IDSets
{
    [ReinitializeDuringResizeArrays]
    public static class ScarletBuffIDSets
    {
        /// <summary>
        /// 如果为<see langword="true"/>，该效果标记为<see langword="状态类Buff"/>
        /// <br>标记为<see langword="状态类"/>时，会自动不被模组（异闻旅者）视作debuff</br>
        /// </summary>
        public static bool[] IsStatueBuff = BuffID.Sets.Factory.CreateBoolSet(BuffType<JellyfishGroupBuff>(), BuffType<SaintChurchBuff>());

    }
}
