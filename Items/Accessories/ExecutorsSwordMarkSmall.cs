using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Instances.Items;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class ExecutorsSwordMarkSmall : LostbeltItemClass
    {
        public static float CritDamage = .05f;
        public static int ExecutionProgressRegen = 1;
        public static int CasterExecutionProgressRegen = 8;
        public override string AssetPath => AssetHandler.Equips;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(ExecutionProgressRegen, CasterExecutionProgressRegen, CritDamage.ToPercent());
        public override void ExSD()
        {
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Orange);

        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().executorSwordMarkLevel = 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(LostbeltRecipeGroup.AnyGoldSword).
                AddRecipeGroup(LostbeltRecipeGroup.AnyEvilBar, 5).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
