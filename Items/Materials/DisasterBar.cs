using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Materials
{
    public class DisasterBar : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Materials;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Solar);
        }
        public override void ExSD()
        {
            Item.width = 34;
            Item.height = 34;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Red;
            Item.material = true;
            Item.value = Item.sellPrice(gold: 4, silver: 30);
        }
        public override void AddRecipes()
        {
            CreateRecipe(5).
                AddIngredient(ItemID.HellstoneBar, 5).
                AddIngredient<DisasterEssence>().
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
