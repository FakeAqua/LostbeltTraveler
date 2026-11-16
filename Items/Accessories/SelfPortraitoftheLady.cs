using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class SelfPortraitoftheLady : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public static float DotDamageMult = 2f;
        public static float ExtraDamageMult = .2f;
        public static int MaxDoTCounts = 8;
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
                AddIngredient<SelfPortraitoftheNameless>().
                AddIngredient(ItemID.ChlorophyteBar, 10).
                AddIngredient(ItemID.HallowedBar, 10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
