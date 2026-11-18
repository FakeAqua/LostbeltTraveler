using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class RocketCharm : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public int AP = 30;
        public static int ExtraAP = 20;
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Lime);
            Item.Lostbelt().drawBuffIconAndDetail = true;
            Item.accessory = true;
        }
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(AP, ConceptualBlackKey.BuffTime);
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetArmorPenetration<ExecutorDamageClass>() += AP;
            player.Lostbelt().rocketCharmLevel = 1;
            player.Lostbelt().blackKeyReduceDefense = ExtraAP;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<DeepBar>(10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
