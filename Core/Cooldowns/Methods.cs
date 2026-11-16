using LostbeltTraveler.Core.NetCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;

namespace LostbeltTraveler.Core.Cooldowns
{
    public static partial class CooldownMethods
    {
        public static bool HasCooldown<T>(this Player player) where T:ScarletCooldown
        {
            int type = ScarletContent.CooldownType<T>();
            return player.CDPlayer().ActiveCooldown.Contains(ScarletCooldownManager.CooldownCollection[type]);
        }
        public static bool HasCooldown(this Player player, int type)
        {
            return player.CDPlayer().ActiveCooldown.Contains(ScarletCooldownManager.CooldownCollection[type]);
        }

        public static ScarletCooldownPlayer CDPlayer(this Player player) => player.GetModPlayer<ScarletCooldownPlayer>();
        public static ScarletCooldown AddCooldown(this Player player, int type, int timeLeft, bool syned = true)
        {
            ScarletCooldown cd = ScarletCooldownManager.CooldownCollection[type];
            cd.OnSpawn(player);
            if (cd.ActiveSound is not null)
                SoundEngine.PlaySound(cd.ActiveSound, player.Center);
            var cdp = player.CDPlayer();
            for (int i = 0; i < cdp.ActiveCooldown.Count; i++)
            {
                if (type == cdp.ActiveCooldown[i].Type)
                {
                    cdp.ActiveCooldown[i].MaxTime = timeLeft;
                    cdp.ActiveCooldown[i].Time = timeLeft;
                    if (syned)
                        player.SyncedCooldown(type, timeLeft);
                    return cd;
                }
            }
            return cd;
        }
        /// <summary>
        /// 直接摧毁这个CD，不会触发完成事件(OnKill)
        /// </summary>
        /// <param name="player"></param>
        /// <param name="type"></param>
        /// <param name="syned"></param>
        /// <param name="complete"></param>
        public static void DestroyCooldown(this Player player, int type, bool syned = true, bool complete = true)
        {
            if (!player.HasCooldown(type))
                return;
            for(int i =0;i<player.CDPlayer().ActiveCooldown.Count;i++)
            {
                ScarletCooldown sc = player.CDPlayer().ActiveCooldown[i];
                if(sc.Type==type)
                {
                    sc.Time = 0;
                    sc.OnDestroy(player);
                    break;
                }
            }
            if (syned)
                player.SyncedKillCooldown(type, complete);
        }
    }
}
