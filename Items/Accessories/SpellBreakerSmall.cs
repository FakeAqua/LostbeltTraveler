using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Methods;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class SpellBreakerSmall : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public static float DamageMult = .05f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(DamageMult.ToPercent(), SpellBreaker.Countdown);
        public override void ExSD()
        {
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Orange);
            Item.Lostbelt().drawBuffIconAndDetail = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().spellBreakerLevel = 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.FlaskofFire).
                AddIngredient(ItemID.FlaskofPoison).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
