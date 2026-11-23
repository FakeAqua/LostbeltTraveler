using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Methods.SideTextPanel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Item.Lostbelt().drawFlavorTooltip = true;
            Item.Lostbelt().noFancyFlavorTooltip = true;
            Item.material = true;
            Item.value = Item.sellPrice(gold: 4, silver: 30);
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
