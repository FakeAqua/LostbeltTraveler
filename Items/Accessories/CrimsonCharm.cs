using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class GreedyMask : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public static int OverSatuTime = 45;
        public static int MininumHeal = 1;
        public static float MinusRatios = .25f;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MinusRatios.ToPercent(), OverSatuTime, MininumHeal);
        public override void ExSD()
        {
            Item.defense = 5;
            Item.accessory = true;
            Item.Lostbelt().drawBuffIcon = true;
            Item.SetUpRarityPrice(ItemRarityID.Red);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().lifeRegenPool += 4;
            player.crimsonRegen = true;
            player.PotionDelayModifier *= 0f;
            player.Lostbelt().crimsonCharm = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CrimsonRune>().
                AddIngredient<CrownofSilveryLight>(15).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
