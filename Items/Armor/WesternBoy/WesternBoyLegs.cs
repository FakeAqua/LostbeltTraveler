using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Instances.Items;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Armor.WesternBoy
{
    [AutoloadEquip(EquipType.Legs)]
    public class WesternBoyLegs : LostbeltArmor
    {
        public float MoveSpeed = .15f;
        public override void ExSD()
        {
            Item.defense = 3;
            Item.SetUpRarityPrice(ItemRarityID.Orange);
        }
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MoveSpeed.ToPercent());
        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += MoveSpeed;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Silk, 5).
                AddRecipeGroup(LostbeltRecipeGroup.AnyEvilScale, 5).
                AddTile(TileID.Hellforge).
                Register();
        }
    }
}
