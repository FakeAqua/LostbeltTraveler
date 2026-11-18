using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class MiniatureBayonet : LostbeltItemClass
    {
        public int Crit = 10;
        public float CritDamage = .15f;

        public override string AssetPath => AssetHandler.Equips;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Crit + "%", CritDamage.ToPercent());
        public override void ExSD()
        {
            Item.width = Item.height = 33;
            Item.SetUpRarityPrice(ItemRarityID.Cyan);
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().blackKeyExecutorCriticalChanceAdd = Crit;
            player.Lostbelt().critDamageExecutor += CritDamage;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AxeofPerun>().
                AddIngredient(ItemID.ChlorophyteBar, 10).
                AddTile(TileID.TinkerersWorkbench).
                Register();
        }

    }

}
