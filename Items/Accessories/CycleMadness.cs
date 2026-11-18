using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class CycleMadness : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public static int CritsAdd = 5;
        public static int CritsPerSecond = 3;
        public static int MaxCrits = 250;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(CritsAdd + "%", CritsPerSecond, MaxCrits + "%");
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateWhite);
        }
        public override void ExSD()
        {
            Item.width = Item.height = 60;
            Item.rare = ItemRarityID.Purple;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().cycleMadnessLevel = 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.SoulofLight, 10).
                AddIngredient(ItemID.SoulofNight, 10).
                AddIngredient(ItemID.LightShard, 1).
                AddIngredient(ItemID.DarkShard, 1).
                AddTile(TileID.CrystalBall).
                Register();
        }
    }
}
