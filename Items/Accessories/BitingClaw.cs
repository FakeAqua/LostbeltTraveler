using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class BitingClaw : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs("10%");
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Blue);
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().bitingClaw = true;
            player.fishingSkill += 10;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(RecipeGroupID.IronBar, 5).
                AddIngredient(ItemID.Seashell, 5).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
