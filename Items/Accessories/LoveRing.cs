using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Instances.Items;
using LostbeltTraveler.Globals.Methods;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Accessories
{
    public class LoveRing : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public override void ExSD()
        {
            Item.accessory = true;
            Item.defense = 4;
            Item.SetUpRarityPrice(ItemRarityID.Cyan);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().loveRing = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.LargeDiamond).
                AddRecipeGroup(LostbeltRecipeGroup.AnyLifeCrystal, 12).
                AddIngredient(ItemID.LovePotion, 5).
                AddIngredient(ItemID.GoldCoin, 69).
                AddIngredient(ItemID.SoulofLight, 5).
                AddTile(TileID.Beds).
                Register();
        }
    }
}
