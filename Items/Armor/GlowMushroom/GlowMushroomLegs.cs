using LostbeltTraveler.Globals.Classes;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Armor.GlowMushroom
{
    [AutoloadEquip(EquipType.Legs)]
    public class GlowMushroomLegs : LostbeltArmor
    {
        public float MoveSpeed = .05f;
        public override void ExSD()
        {
            Item.defense = 3;
            Item.SetUpRarityPrice(ItemRarityID.Green);
        }
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MoveSpeed.ToPercent());
        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += MoveSpeed;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.GlowingMushroom, 10).
                AddTile(TileID.WorkBenches).
                Register();
        }

    }
}
