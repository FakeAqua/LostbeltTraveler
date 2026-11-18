using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Armor.RedDragonKnight
{
    [AutoloadEquip(EquipType.Legs), LegacyName("DragonSlayerLegs")]
    public class RedDragonKnightLegs : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Armors;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.ScarletRed);
        }
        public override void ExSD()
        {
            Item.defense = 30;
            Item.SetUpRarityPrice(ItemRarityID.Purple);
        }
        public float MoveSpeed = .20f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MoveSpeed.ToPercent());
        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += MoveSpeed;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<UniversalCube>(4).
                AddIngredient(ItemID.LunarBar, 4).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }

    }
}
