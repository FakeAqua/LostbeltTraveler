using LostbeltTraveler.Core.NetCode;
using LostbeltTraveler.Globals.Database.IDSets;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace LostbeltTraveler.Globals.Classes
{
    /// <summary>
    /// 这个基类仅用于使用射弹进行武器外观显示的用途
    /// </summary>
    public abstract class LostbeltWeaponoutItemClass : LostbeltWeapon
    {
        public override void SetStaticDefaults()
        {
            ScarletItemIDSets.IsHeldProjItem[Type] = true;
            ExSSD();
        }
        public virtual void ExSSD() { }
        public override void SetDefaults()
        {
            Item.width = Item.height = 16;
            Item.DamageType = GetDamageClass;
            Item.Lostbelt().CanDrawIcon = true;
            Item.SetUpNoUseGraphicItem(true);
            Item.useStyle = ItemUseStyleID.Shoot;
            ExSD();
        }
        public override bool CanShoot(Player player)
        {
            return false;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }
        public override void HoldItem(Player player)
        {
            if (!player.IsOwnerSide())
                return;

            if (player.HasProj(Item.shoot))
                return;
            int projDamage = (int)player.GetTotalDamage(Item.DamageType).ApplyTo(Item.damage);
            Projectile proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, Item.shoot, projDamage, Item.knockBack, player.whoAmI);
            proj.originalDamage = projDamage;
            proj.netUpdate = true;
            ScarletSound(LostbeltSoundAssets.Misc_KnifeExpired, player.Center, 1, 0, -.2f);
        }
    }
}
