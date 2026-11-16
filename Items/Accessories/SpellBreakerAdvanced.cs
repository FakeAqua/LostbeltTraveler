using ContinentOfJourney.Items;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Methods;
using LostbeltTraveler.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Accessories
{
    public class SpellBreakerAdvanced : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public static float Damage = .10f;
        public static int Crit = 5;
        public static float DamageMult = .1f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Damage.ToPercent(), Crit + "%", DamageMult.ToPercent(), SpellBreaker.Countdown);
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.Lostbelt().drawBuffIconAndDetail = true;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<MeleeDamageClass>() += Damage;
            player.GetCritChance<MeleeDamageClass>() += Crit;
            player.Lostbelt().spellBreakerLevel = 3;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SpellBreaker>().
                AddIngredient<DivineFireFlask>().
                AddIngredient<PlagueFlask>().
                AddIngredient<SteelFlask>().
                AddIngredient<CrownofSilveryLight>(15).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
