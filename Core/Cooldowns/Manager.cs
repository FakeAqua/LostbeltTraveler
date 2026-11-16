using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace LostbeltTraveler.Core.Cooldowns
{
    public class ScarletCooldownManager : ModSystem
    {
        /// <summary>
        /// 这个合集只是用来存已经有的CD列表
        /// <br>并且也会用于在卸载时的更新</br>
        /// </summary>
        public static List<ScarletCooldown> CooldownCollection = [];
        public override void Unload()
        {
            CooldownCollection.Clear();
        }
    }
}
