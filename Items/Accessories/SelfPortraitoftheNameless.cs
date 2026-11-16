using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Instances.Items;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class SelfPortraitoftheNameless : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public static float DotDamageMult = 1f;
        public static float ExtraDamageMult = .1f;
        public static int MaxDoTCounts = 4;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(DotDamageMult.ToPercent(), ExtraDamageMult.ToPercent(), MaxDoTCounts);
        public override void SetStaticDefaults()
        {
        }
        public override void ExSD()
        {
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.Lostbelt().drawBuffIconAndDetail = true;
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
                AddIngredient(ItemID.Paintbrush).
                AddIngredient(ItemID.PaintScraper).
                AddIngredient(ItemID.PaintRoller).
                AddIngredient(ItemID.WhitePaint, 10).
                AddIngredient(ItemID.BlackPaint, 10).
                AddRecipeGroup(LostbeltRecipeGroup.AnyEvilScale, 10).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
