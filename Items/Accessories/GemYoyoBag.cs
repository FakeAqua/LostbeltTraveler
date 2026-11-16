using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Accessories
{
    public class GemYoyoBag:LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void ExSD()
        {
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
        }
        public override bool CanRightClick() => Main.keyState.PressingShift();
        public override void RightClick(Player player)
        {
            base.RightClick(player);
        }
        public override bool ConsumeItem(Player player) => false;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<YoyoRopeOnyx>().
                AddIngredient<YoyoRopeDiamond>().
                AddIngredient<YoyoRopeAmber>().
                AddIngredient<YoyoRopeRuby>().
                AddIngredient<YoyoRopeEmerald>().
                AddIngredient<YoyoRopeSapphire>().
                AddIngredient<YoyoRopeTopaz>().
                AddIngredient<YoyoRopeAmethyst>().
                AddIngredient(ItemID.YoyoBag).
                AddTile(TileID.TinkerersWorkbench).
                Register();
        }
    }
}
