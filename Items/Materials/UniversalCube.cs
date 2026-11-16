using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Methods.Textbox;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Materials
{
    public class UniversalCube : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Materials;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateWhite);
            ItemID.Sets.ItemNoGravity[Type] = true;
        }
        public override void ExSD()
        {
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Blue;
            Item.material = true;
            Item.value = Item.sellPrice(gold: 4, silver: 30);
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
            string text = this.GetLocalizationKey("FlavorTooltip").ToLangValue();
            TextboxSettings sets = new TextboxSettings
            (
                hasTitle: false,
                backgroundColor: Color.White * .24f,
                backgroundEdgeColor: Color.White,
                textColor: Color.White,
                textEdgeColor: Color.Black,
                mainText: text
            );
            TextboxMethods.DrawTextboxTooltipWithBackground(line, CacheTooltipList, ref sets);
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.FragmentSolar).
                AddIngredient(ItemID.FragmentVortex).
                AddIngredient(ItemID.FragmentNebula).
                AddIngredient(ItemID.FragmentStardust).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
