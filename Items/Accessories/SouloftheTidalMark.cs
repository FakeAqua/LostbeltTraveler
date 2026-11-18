using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class SouloftheTidalMark : LostbeltItemClass
    {
        public static float CritDamage = .10f;
        public static float DR = .05f;
        public static int LifeThreshold = 50;
        public int MinionSlots = 2;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(LifeThreshold, DR.ToPercent(), CritDamage.ToPercent());
        public override string AssetPath => AssetHandler.Equips;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, Globals.Database.Enums.ShinyRarityType.StardustFancy);
        }
        public override void ExSD()
        {
            Item.defense = 8;
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Red);
            Item.Lostbelt().drawBuffIconAndDetail = true;

        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().souloftheTidalMark = true;
            player.longInvince = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.CrossNecklace).
                AddIngredient(ItemID.FragmentStardust, 20).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
