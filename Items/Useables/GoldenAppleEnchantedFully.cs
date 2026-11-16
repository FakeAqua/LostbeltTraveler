using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Buffs.GoodBuffs;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Useables
{
    public class GoldenAppleEnchantedFully : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Useables;
        public override string Texture => GetInstance<GoldenAppleEnchanted>().Texture;
        public override void ExSD()
        {
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Blue);
        }
        public override void UpdateInventory(Player player)
        {
            player.buffImmune[BuffType<GoldenAppleBuff>()] = true;
            player.buffImmune[BuffType<GoldenAppleBuffEnchanted>()] = true;
            player.Lostbelt().goldenAppleDamageAbsorb = 200;
            player.Lostbelt().goldenAppleEnchantedFully = true;
            player.lavaImmune = true;
            player.noKnockback = true;
            player.Lostbelt().lifeRegenPool += 4;
            player.statDefense += 50;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<GoldenAppleEnchanted>(50).
                AddIngredient<EssenceofBright>(10).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
