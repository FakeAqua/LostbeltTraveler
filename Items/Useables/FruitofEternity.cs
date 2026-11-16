using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Buffs.GoodBuffs;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Methods.Textbox;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

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
            Item.Lostbelt().drawBuffIconAndDetail = true;
            Item.value = Item.buyPrice(gold: 5, silver: 30);
            Item.DefaultToFood(32, 32, BuffType<FruitofEternityBuff>(), GetSeconds(60) * 5);
        }
        public IReadOnlyList<TooltipLine> CacheTooltipList = null;
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            CacheTooltipList = tooltips;
        }

        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.IsItemName())
            {
                TextboxManager.FirstLineY = line.Y;
            }
            string text = this.GetLocalizationKey("DetailTooltip").ToLangValue();
            TextboxSettings sets = new TextboxSettings
                (
                backgroundColor: Color.White * .24f,
                textColor: Color.White,
                textEdgeColor: Color.Lerp(Color.HotPink, Color.Black, .74f),
                mainText: text,
                hasTitle: false
                );
            //TextboxMethods.DrawTextboxTooltipWithBackground(line, CacheTooltipList, ref sets);
            return true;
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
