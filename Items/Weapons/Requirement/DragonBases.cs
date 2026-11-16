using Terraria;
using Terraria.GameContent;

namespace LostbeltTraveler.Items.Weapons.Requirement
{
    /// <summary>
    /// Scarlet:
    /// 下面这些都是这些项目用到的一些封装方法
    /// 主要是为了方便开发才用上的
    /// 看你们程序员的情况看看要不要保留还是说融合进你们自己的那些封装方法里面
    /// 都给了注释，不过是AI跑的（
    /// </summary>
    public static class DragonMethods
    {
        /// <summary>
        /// 计算玩家当前武器攻速加成下的实际攻击间隔（帧）。
        /// </summary>
        /// <param name="player">持有武器的玩家。</param>
        /// <param name="item">对应的武器物品，用于获取攻速加成。</param>
        /// <param name="baseInterval">基础攻击间隔（帧）</param>
        /// <param name="minInterval">攻击间隔的下限（帧）。</param>
        /// <returns>应用攻速加成后的实际攻击间隔，至少为 <paramref name="minInterval"/>。</returns>
        public static int GetModifiedAttackInterval(this Player player, Item item, int baseInterval, int minInterval)
        {
            float attackSpeed = player.GetWeaponAttackSpeed(item);
            float mult = 1f / attackSpeed;
            int realAttackSpeed = (int)(baseInterval * mult);
            return realAttackSpeed < minInterval ? minInterval : realAttackSpeed;
        }
        /// <summary>
        /// 计算玩家当前手持武器在攻速加成下的实际攻击间隔（帧）。
        /// 重载使用 <see cref="Player.HeldItem"/> 和其 <see cref="Item.useTime"/> 作为基础间隔。
        /// </summary>
        /// <param name="player">持有武器的玩家。</param>
        /// <param name="minInterval">攻击间隔的下限（帧）</param>
        /// <returns>应用攻速加成后的实际攻击间隔，至少为 <paramref name="minInterval"/>。</returns>

        public static int GetModifiedAttackInterval(this Player player, int minInterval)
        {
            Item item = player.HeldItem;
            float attackSpeed = player.GetWeaponAttackSpeed(item);
            float mult = 1f / attackSpeed;
            int realAttackSpeed = (int)(item.useTime * mult);
            return realAttackSpeed < minInterval ? minInterval : realAttackSpeed;
        }
        /// <summary>
        /// 控制玩家手臂的旋转角度，支持单独控制前臂、后臂或同时控制双臂。
        /// </summary>
        /// <param name="player">目标玩家。</param>
        /// <param name="armRotation">手臂的目标旋转角度（世界坐标系，弧度）。</param>
        /// <param name="armFlag">
        /// 指定要控制的手臂：<br/>
        /// - <c>0</c>（默认）：同时控制前臂和后臂。<br/>
        /// - <c>&gt;0</c>：仅控制前臂（<see cref="Player.SetCompositeArmFront"/>）。<br/>
        /// - <c>&lt;0</c>：仅控制后臂（<see cref="Player.SetCompositeArmBack"/>）。
        /// </param>
        /// <param name="baseOffset">
        /// 手臂的基础偏移角度（弧度），用于修改默认朝向。<br/>
        /// 最终应用角度为 <paramref name="armRotation"/> - <paramref name="baseOffset"/>。<br/>
        /// 默认值为 <see cref="PiOver2"/>（90°）。
        /// </param>
        public static void SetPlayerArmRotation(this Player player, float armRotation, int armFlag = 0, float baseOffset = PiOver2)
        {
            float finalAngle = armRotation - baseOffset;

            if (armFlag > 0)
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, finalAngle);
            else if (armFlag < 0)
                player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, finalAngle);
            else
            {
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, finalAngle);
                player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, finalAngle);
            }
        }
        /// <summary>
        /// 设置默认手持射弹属性
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="eu"></param>
        public static void SetDefaultsHeldProj(this Projectile proj, int eu = 0)
        {
            proj.ignoreWater = true;
            proj.tileCollide = false;
            proj.timeLeft = 2000;
            proj.penetrate = -1;
            proj.width = proj.height = 2;
            proj.noEnchantmentVisuals = true;
            proj.extraUpdates = eu;
        }
        public static bool IsSafe(this NPC target)
        {
            return target is not null && target.CanBeChasedBy();
        }
        public static Texture2D GetProjTexture(this Projectile proj)
        {
            return TextureAssets.Projectile[proj.type].Value;
        }
        public static string GetItemTexturePath(int itemID)
        {
            return $"Terraria/Images/Item_{itemID}";
        }
        public static Texture2D GetProjTexture(int projID)
        {
            return TextureAssets.Projectile[projID].Value;
        }
    }
}
