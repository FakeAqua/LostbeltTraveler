using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Methods;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class ArcBow : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public int HealAmt = 8;
        public static float DamageReduce = .25f;
        public override void ExSD()
        {
            Item.width = Item.height = 32;
            Item.SetUpRarityPrice(ItemRarityID.Lime);
            Item.Lostbelt().drawBuffIconAndDetail = true;
            Item.accessory = true;
        }
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(HealAmt, ConceptualBlackKey.BuffTime);
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().blackKeyHeal = HealAmt;
            player.Lostbelt().blackKeyDefenseBuff = DamageReduce;
            player.Lostbelt().arcBowLevel = 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.HallowedBar, 10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
