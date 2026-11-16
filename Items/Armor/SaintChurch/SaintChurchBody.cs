using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Armor.SaintChurch
{
    [AutoloadEquip(EquipType.Body)]
    public class SaintChurchBody : LostbeltArmor
    {
        public float CritDamage = 0.10f;
        public int Crit = 5;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateGolden);
        }
        public override void ExSD()
        {
            Item.defense = 12;
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.Lostbelt().drawBuffIconAndDetail = true;
        }
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(CritDamage.ToPercent(), Crit + "%");
        public override void UpdateEquip(Player player)
        {
            player.Lostbelt().critDamageExecutor += CritDamage;
            player.GetCritChance<ExecutorDamageClass>() += Crit;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<DeepBar>(10).
                AddIngredient(ItemID.Silk, 10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }

    }
}
