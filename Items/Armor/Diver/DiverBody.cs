using ContinentOfJourney.Items.Material;
using ContinentOfJourney.Tiles;
using LostbeltTraveler.Globals.Classes;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Armor.Diver
{
    [AutoloadEquip(EquipType.Body)]
    public class DiverBody : LostbeltArmor
    {
        public float CritDamage = 0.05f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(CritDamage.ToPercent());
        public override void ExSD()
        {
            Item.defense = 8;
            Item.SetUpRarityPrice(ItemRarityID.Orange);
            Item.Lostbelt().drawBuffIconAndDetail = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.Lostbelt().critDamageExecutor += CritDamage;

        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemType<AnglerCoin>(), 36).
                AddTile(TileType<FishmenFreeMarketTradingSystem>()).
                Register();
        }

    }
}
