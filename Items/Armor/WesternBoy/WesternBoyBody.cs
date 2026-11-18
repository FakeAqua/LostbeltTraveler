using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Instances.Items;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Armor.WesternBoy
{
    [AutoloadEquip(EquipType.Body)]
    public class WesternBoyBody : LostbeltArmor
    {
        public float Crit = 5;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Crit + "%");
        public override void ExSD()
        {
            Item.defense = 7;
            Item.SetUpRarityPrice(ItemRarityID.Orange);
        }
        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<GenericDamageClass>() += Crit;

        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Silk, 15).
                AddRecipeGroup(LostbeltRecipeGroup.AnyEvilScale, 10).
                AddTile(TileID.Hellforge).
                Register();
        }

    }
}
