using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Projs.Magic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Magic
{
    public class CrystalPike : LostbeltWeapon
    {
        public override EnumDamageClass Category => EnumDamageClass.Magic;
        public override void ExSD()
        {
            Item.SetUpNoUseGraphicItem(true);
            Item.SetUpRarityPrice(ItemRarityID.Orange);
            Item.damage = 55;
            Item.useTime = Item.useAnimation = 20;
            Item.mana = 15;
            Item.shootSpeed = 16;
            Item.shoot = ProjectileType<CrystalPikeProj>();
            Item.knockBack = 4;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.SapphireStaff).
                AddIngredient<SwollenStar>(15).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
