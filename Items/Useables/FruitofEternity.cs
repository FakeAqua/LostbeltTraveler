using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Buffs.GoodBuffs;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Useables
{
    public class FruitofEternity : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Useables;
        public static float DamageReduceMultiplier = 0.5f;
        public static float DefenseMultipler = 1.5f;
        public static int TeleportChance = 5;
        public static int LifeRegenSpeed = 4;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateGolden);
        }
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs((DefenseMultipler - 1).ToPercent(), LifeRegenSpeed / 2, DamageReduceMultiplier.ToPercent(), TeleportChance);
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Red);
            Item.Lostbelt().drawFlavorTooltip = true;
            Item.Lostbelt().drawBuffIcon = true;
            Item.Lostbelt().noFancyFlavorTooltip = true;
            Item.value = Item.buyPrice(gold: 5, silver: 30);
            Item.DefaultToFood(32, 32, BuffType<FruitofEternityBuff>(), GetSeconds(60) * 5);
        }
        public override void AddRecipes()
        {
            CreateRecipe(2).
                AddIngredient<EternalBar>(10).
                AddIngredient<LivingBar>(10).
                AddIngredient<CubistBar>(10).
                AddRecipeGroup(RecipeGroupID.Fruit).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
