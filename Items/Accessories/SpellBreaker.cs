using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Accessories
{
    public class SpellBreaker : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public static float Damage = .05f;
        public static float DamageMult = .05f;
        public static int Countdown = 60;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Damage.ToPercent(), DamageMult.ToPercent(), Countdown);
        public override void ExSD()
        {
            Item.accessory = true;
            Item.Lostbelt().drawBuffIconAndDetail = true;
            Item.SetUpRarityPrice(ItemRarityID.LightPurple);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<MeleeDamageClass>() += Damage;
            player.Lostbelt().spellBreakerLevel = 2;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SpellBreakerSmall>().
                AddIngredient(ItemID.FlaskofCursedFlames).
                AddIngredient(ItemID.FlaskofVenom).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
