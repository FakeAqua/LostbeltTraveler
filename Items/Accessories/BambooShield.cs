using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Accessories
{
    public class BambooShield : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public override void SetStaticDefaults()
        {
        }
        public override void ExSD()
        {
            Item.accessory = true;
            Item.defense = 2;
            Item.SetUpRarityPrice(ItemRarityID.Green);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.buffImmune[BuffID.Poisoned] = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.BambooBlock, 15).
                AddIngredient(ItemID.JungleSpores, 5).
                AddIngredient(ItemID.Vine, 5).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
