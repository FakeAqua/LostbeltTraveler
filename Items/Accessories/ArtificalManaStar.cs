using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Methods;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Accessories
{
    public class ArtificalManaStar : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Blue);
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.manaFlower = true;
            player.Lostbelt().artificalManaStar = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.StoneBlock, 20).
                AddRecipeGroup(RecipeGroupID.IronBar, 5).
                AddIngredient(ItemID.Sapphire).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
