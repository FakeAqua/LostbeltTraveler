using ContinentOfJourney.Tiles;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Magic
{
    public class ShatterSongs : LostbeltWeapon
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public override EnumDamageClass Category => EnumDamageClass.Magic;
        public override void ExSD()
        {
            Item.height = Item.width = 50;
            Item.mana = 20;
            Item.useAnimation = Item.useTime = 20;
            Item.rare = ItemRarityID.Purple;
            Item.useStyle = ItemUseStyleID.Guitar;
            Item.shootSpeed = 17f;
            Item.noMelee = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.CarbonGuitar).
                AddIngredient<DisasterEssence>(10).
                AddIngredient(ItemID.LunarBar, 10).
                AddTile<FinalAnvil>().
                Register();
        }
    }
}
