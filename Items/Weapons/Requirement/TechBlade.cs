using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Requirement
{
    public class TechBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 32;
            Item.damage = 1234;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.buyPrice(1);
            Item.useTime = Item.useAnimation = 30;
            Item.knockBack = 5;
            Item.DamageType = DamageClass.Melee;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shoot = ProjectileType<TechBladeHeldProj>();
            Item.shootSpeed = 16;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
    }
}
