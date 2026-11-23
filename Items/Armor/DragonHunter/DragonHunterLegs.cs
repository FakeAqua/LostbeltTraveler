using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Items.Armor.RedDragonKnight;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Armor.DragonHunter
{
    [AutoloadEquip(EquipType.Legs)]
    public class DragonHunterLegs : LostbeltArmor
    {
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.ScarletRed);
        }
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Purple);
            Item.defense = 50;
        }
        public float MoveSpeed = .30f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MoveSpeed.ToPercent());
        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += MoveSpeed;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<RedDragonKnightLegs>().
                AddIngredient<SunlightGel>(2).
                AddIngredient<EssenceofTime>(2).
                AddIngredient<EssenceofLife>(2).
                AddIngredient<EssenceofMatter>(2).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
