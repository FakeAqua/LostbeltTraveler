using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ModLoader;

namespace LostbeltTraveler.Core.Cooldowns
{
    public class ScarletCooldownPlayer:ModPlayer
    {
        public List<ScarletCooldown> ActiveCooldown= [];
        public int CurCooldownID;
        /// <summary>
        /// 这个钩子保留，主要是用来绘制CD的出现与消失
        /// <br>目前还没有想好如何呈现给玩家看，因此暂时不做处理</br>
        /// </summary>
        public override void UpdateAutopause()
        {
            if (ActiveCooldown.Count == 0)
                return;
        }
        public override void PostUpdateMiscEffects()
        {
            if (ActiveCooldown.Count == 0)
                return;
            for(int i = 0; i < ActiveCooldown.Count;i++)
            {
                ActiveCooldown[i].Update(Player);
                if (ActiveCooldown[i].Time<=0)
                {
                    ActiveCooldown[i].OnKill(Player);
                    if (ActiveCooldown[i].DeactiveSound is not null)
                        SoundEngine.PlaySound(ActiveCooldown[i].DeactiveSound,Player.Center);
                }
                if (ActiveCooldown[i].Time > 0 && ActiveCooldown[i].CanTickDown())
                    ActiveCooldown[i].Time--;
            }
            ActiveCooldown?.RemoveAll(i =>
            {
                if (i.Time <= 0)
                    return true;
                return false;
            });
        }
        public override void UpdateDead()
        {
            ActiveCooldown?.RemoveAll(i =>
            {
                return i.CleanOnDeath();
            });
        }
    }
}
