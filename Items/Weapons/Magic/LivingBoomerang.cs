using ContinentOfJourney.Items;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Items.Materials;
using LostbeltTraveler.Projs.Magic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Magic
{
    public class LivingBoomerangMagic : LostbeltWeapon
    {
        public override string Texture => GetInstance<LivingBoomerang>().Texture;
        public override EnumDamageClass Category => EnumDamageClass.Magic;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Life);
            Item.staff[Type] = true;
        }
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Red);
            Item.Lostbelt().drawBuffIconAndDetail = true;
            Item.damage = 1012;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.mana = 15;
            Item.useTime = Item.useAnimation = 40;
            Item.shoot = ProjectileType<LivingBoomerangMagicHeldProj>();
            Item.shootSpeed = 16f;
            Item.SetUpNoUseGraphicItem(true);
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }

        public override bool CanUseItem(Player player)
        {
            return !player.HasProj(Item.shoot);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<LivingBoomerang>().
                AddIngredient<CrownofSilveryLight>(15).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
