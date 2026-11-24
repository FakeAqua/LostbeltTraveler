using LostbeltTraveler.Globals.Database.IDSets;
using Terraria;

namespace LostbeltTraveler.Globals.Classes
{
    public abstract class LostbeltHeldProj : LostbeltProjs
    {
        /// <summary>
        /// 这个手持射弹的原始物品
        /// <br>必须得提供这个东西，因为需要同时计算攻击速度</br>
        /// </summary>
        public virtual int OriginalItemID => -1;
        public virtual int AttackSpeed => Owner.ApplyWeaponAttackSpeed(Owner.HeldItem, Owner.HeldItem.useTime * Projectile.MaxUpdates, 5 * Projectile.MaxUpdates);
        public virtual int ExtraUpdates => 0;
        public override void SetStaticDefaults()
        {
            ScarletProjIDSets.IsHeldProj[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = SetDamageClass;
            Projectile.SetUpHeldProj(ExtraUpdates);
            ExSD();
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}
