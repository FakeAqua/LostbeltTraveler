using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Methods;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class SelfPortraitoftheSunflower : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public static float DotDamageMult = 3f;
        public static float ExtraDamageMult = .25f;
        public static int MaxDoTCounts = 12;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(DotDamageMult.ToPercent(), ExtraDamageMult.ToPercent(), MaxDoTCounts);
        public override void SetStaticDefaults()
        {
        }
        public override void ExSD()
        {
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Purple);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().dotMult += DotDamageMult;
            player.Lostbelt().dotMultExtra += ExtraDamageMult;
            player.Lostbelt().dotMultCount = MaxDoTCounts;
            player.Lostbelt().selfPortraitType = Type;

        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Sunflower, 1).
                AddIngredient(ItemID.LunarBar, 15).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
