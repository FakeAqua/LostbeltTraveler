using Microsoft.Xna.Framework.Audio;
using rail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace LostbeltTraveler.Core.Cooldowns
{
    /// <summary>
    /// 冷却系统
    /// <br>这里的冷却系统一般用于“有意”外显给予给玩家查阅，也就是一些状态效果</br>
    /// <br>对于部分小型内容，比如说攻击冷却这种内置CD，不推荐在这里无脑创建</br>
    /// <br>更推荐使用<see cref="LostbeltMethods.AddGeneralTimer(Terraria.Player, int, int)"/>方法</br>
    /// </summary>
    public class ScarletCooldown : ModType
    {
        public int Type;
        public int Time;
        public int MaxTime;
        public bool Buff = true;
        public bool Debuff = false;
        public bool StatusBuff = false;
        public virtual void OnRegister()
        {

        }
        public virtual bool CanTickDown()
        {
            return true;

        }
        public virtual SoundStyle? ActiveSound => null;
        public virtual SoundStyle? DeactiveSound => null;
        /// <summary>
        /// CD刚创建时执行的行为
        /// </summary>
        /// <param name="player"></param>
        public virtual void OnSpawn(Player player) { }
        /// <summary>
        /// CD更新期间执行什么行为
        /// </summary>
        /// <param name="player"></param>
        public virtual void Update(Player player) { }
        /// <summary>
        /// 如果直接被摧毁的方法，这个方法不会经过消亡
        /// </summary>
        /// <param name="player"></param>
        public virtual void OnDestroy(Player player) { }
        /// <summary>
        /// 自然消亡时执行的方法
        /// </summary>
        /// <param name="player"></param>

        public virtual void OnKill(Player player) { }
        public virtual bool CleanOnDeath()
        {
            return true;
        }
        protected sealed override void Register()
        {
            Type = ScarletCooldownManager.CooldownCollection.Count;
            OnRegister();
            if (!ScarletCooldownManager.CooldownCollection.Contains(this))
                ScarletCooldownManager.CooldownCollection.Add(this);
        }
    }
}
