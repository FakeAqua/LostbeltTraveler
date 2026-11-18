using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Instances.Items;
using Terraria.ID;

namespace LostbeltTraveler.Items.Useables
{
    public class FreehandFrame : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Useables;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateGolden);
        }
        public override void ExSD()
        {
            Item.maxStack = 9999;
            Item.SetUpRarityPrice(ItemRarityID.Green);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(LostbeltRecipeGroup.AnyGoldBar, 5).
                AddIngredient(ItemID.Silk, 5).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}
