using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Instances.Items;
using LostbeltTraveler.Globals.Methods;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Accessories
{
    public class MayaBall : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Pink);
            Item.accessory = true;
            Item.defense = 1;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().mayaPumper = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(LostbeltRecipeGroup.AnyEvilBar, 10).
                AddRecipeGroup(LostbeltRecipeGroup.AnyEvilScale, 10).
                AddTile(TileID.DemonAltar).
                Register();
        }
    }
}
