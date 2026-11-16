using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Requirement
{
    public class TechSniper : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = Item.height = 32;
            Item.damage = 1234;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.buyPrice(1);
            Item.useTime = Item.useAnimation = 64;
            Item.knockBack = 5;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ProjectileType<TechSniperHeldProj>();
            Item.shootSpeed = 16;
        }
        public override void HoldItem(Player player)
        {
            if (player.HasProj(Item.shoot))
                return;
            int projDamage = (int)player.GetTotalDamage<MagicDamageClass>().ApplyTo(Item.damage);
            Projectile proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, Item.shoot, 0, Item.knockBack, player.whoAmI);
            proj.originalDamage = projDamage;
            proj.netUpdate = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }
    }
}
