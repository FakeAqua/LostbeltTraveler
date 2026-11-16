using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Methods;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class ExecutorsSwordMarkPlus : LostbeltItemClass
    {
        public static float CritDamage = .20f;
        public static int ExecutionProgressRegen = 3;
        public static int CasterExecutionProgressRegen = 4;
        public override string AssetPath => AssetHandler.Equips;
        public override void ExSD()
        {
            Item.width = Item.height = 60;
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Red);

        }
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(ExecutionProgressRegen, CasterExecutionProgressRegen, CritDamage.ToPercent());
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().executorSwordMarkLevel = 3;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ExecutorsSwordMark>().
                AddIngredient<EssenceofTime>(5).
                AddIngredient<EssenceofMatter>(5).
                AddIngredient<EssenceofLife>(5).
                AddTile(FinalAnvilTile).
                Register();

        }
    }
}
