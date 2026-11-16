using ContinentOfJourney.Items.Material;
using ContinentOfJourney.Tiles;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Executor;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Armor.Diver
{

    [AutoloadEquip(EquipType.Head)]
    public class DiverHead : LostbeltArmor
    {
        public override int[] ArmorSlots => [Type, ItemType<DiverBody>(), ItemType<DiverLegs>()];
        public override bool SetUpArmorSet => true;
        public float DamageAdd = .05f;
        public override void ExSD()
        {
            Item.defense = 6;
            Item.SetUpRarityPrice(ItemRarityID.Orange);
            Item.Lostbelt().drawBuffIconAndDetail = true;
        }
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(DamageAdd.ToPercent());
        public override void UpdateArmorSetBetter(Player player, string setBonusPath)
        {
            player.setBonus += "\n" + setBonusPath.ToLangValue();
            player.Lostbelt().diverArmor = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage<ExecutorDamageClass>() += DamageAdd;
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
