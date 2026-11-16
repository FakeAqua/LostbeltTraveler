using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Useables
{
    public class SunWorm : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Useables;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Solar);
        }
        public override void ExSD()
        {
            Item.maxStack = Item.CommonMaxStack;
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.bait = 100;
            Item.consumable = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(5).
                AddIngredient(ItemID.EnchantedNightcrawler, 5).
                AddIngredient<SolarFlareScoria>().
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
