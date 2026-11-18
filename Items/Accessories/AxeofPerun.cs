using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Instances.Items;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class AxeofPerun : LostbeltItemClass
    {
        public int Crit = 5;
        public float CritDamage = .10f;

        public override string AssetPath => AssetHandler.Equips;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Crit + "%", CritDamage.ToPercent());
        public override void ExSD()
        {
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Orange);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().blackKeyExecutorCriticalChanceAdd = Crit;
            player.Lostbelt().critDamageExecutor += CritDamage;


        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(LostbeltRecipeGroup.AnyCopperBar, 15).
                AddRecipeGroup(LostbeltRecipeGroup.AnyEvilBar, 10).
                AddRecipeGroup(LostbeltRecipeGroup.AnyEvilScale, 5).
                AddIngredient(ItemID.Bone, 5).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
