using ContinentOfJourney.Items.Accessories;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Items.Materials;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Accessories
{
    internal class Adjudication : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, Globals.Database.Enums.ShinyRarityType.Final);
        }
        public override void ExSD()
        {
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<DeadRinger>().
                AddIngredient<FungusDeluxe>().
                AddIngredient<JellyMushroom>().
                AddIngredient<MapleMushroom>().
                AddIngredient<HoneyMushroom>().
                AddIngredient<CrownofSilveryLight>(15).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
