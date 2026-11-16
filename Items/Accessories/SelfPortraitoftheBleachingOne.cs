using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Methods;
using LostbeltTraveler.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class SelfPortraitoftheBleachingOne : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public static float DotDamageMult = 5f;
        public static float ExtraDamageMult = .5f;
        public static int DoTMulter = 4;
        public static int MaxDoTCounts = 16;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(DotDamageMult.ToPercent(), ExtraDamageMult.ToPercent(), MaxDoTCounts);
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateWhite);
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
                AddIngredient<SelfPortraitoftheSunflower>().
                AddIngredient<CrownofSilveryLight>(15).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
