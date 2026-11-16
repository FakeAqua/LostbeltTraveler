using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Methods;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class ExecutorsSwordMark : LostbeltItemClass
    {
        public static float CritDamage = .10f;
        public static int ExecutionProgressRegen = 2;
        public static int CasterExecutionProgressRegen = 6;
        public override string AssetPath => AssetHandler.Equips;
        public override void ExSD()
        {
            Item.width = Item.height = 60;
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.LightPurple);

        }
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(ExecutionProgressRegen, CasterExecutionProgressRegen, CritDamage.ToPercent());
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().executorSwordMarkLevel = 2;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ExecutorsSwordMarkSmall>().
                AddIngredient(ItemID.SoulofLight, 5).
                AddIngredient(ItemID.SoulofNight, 5).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
