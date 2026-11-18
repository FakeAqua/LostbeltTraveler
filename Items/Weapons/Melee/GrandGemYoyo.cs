using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Melee
{
    public class GrandGemYoyo : LostbeltWeapon
    {
        public override EnumDamageClass Category => EnumDamageClass.Melee;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateWhite);
        }
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.SetUpNoUseGraphicItem(true);
            Item.damage = 554;
            Item.crit = 16;
            Item.shootSpeed = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = Item.useAnimation = 16;
            Item.knockBack = 4;
        }
        public override bool MeleePrefix() => true;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override void HoldItem(Player player)
        {
            base.HoldItem(player);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<OnyxYoyo>().
                AddIngredient<DiamondYoyo>().
                AddIngredient<RubyYoyo>().
                AddIngredient<AmberYoyo>().
                AddIngredient<EmeraldYoyo>().
                AddIngredient<SapphireYoyo>().
                AddIngredient<TopazYoyo>().
                AddIngredient<AmethystYoyo>().
                AddIngredient<EternalBar>(10).
                AddIngredient<LivingBar>(10).
                AddIngredient<CubistBar>(10).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
