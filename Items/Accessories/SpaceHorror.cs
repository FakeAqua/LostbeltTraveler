using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Methods;
using LostbeltTraveler.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class SpaceHorror : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs((CycleMadness.CritsAdd * 2) + "%", (CycleMadness.CritsPerSecond * 2), (CycleMadness.MaxCrits * 2) + "%");
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateWhite);
        }
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Purple);
            Item.accessory = true;
            Item.Lostbelt().drawBuffIconAndDetail = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().cycleMadnessLevel = 2;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CycleMadness>().
                AddIngredient<UniversalCube>(5).
                AddIngredient(ItemID.LunarBar, 5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }

    }
}
