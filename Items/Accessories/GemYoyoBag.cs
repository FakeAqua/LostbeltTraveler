using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Accessories
{
    public class GemYoyoBag : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public int StringType = 0;
        public string[] TheString = ["Amethyst", "Topaz", "Sapphire", "Emerald", "Amber", "Ruby", "Diamond", "Onyx"];
        public override void ExSD()
        {
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.yoyoGlove = true;
            player.yoyoString = true;
            player.Lostbelt().gemYoyoStrings = TheString[StringType];
            player.Lostbelt().gemYoyoBag = true;
            player.counterWeight = 1;
        }
        public override bool CanRightClick() => Main.keyState.PressingShift();
        public override void RightClick(Player player)
        {
            StringType++;
            if (StringType >= TheString.Length)
                StringType = 0;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int v = tooltips.FindLastIndex(0, t => t.Name.Contains("Tooltip") && t.Mod == "Terraria");
            CombinationValue(out string v1, out string v2);
            string value = this.GetLocalizedValue("GemTooltip").ToFormatValue(v1, v2);
            tooltips.CreateTooltipDirect(value, Color.LightSkyBlue, Mod, "GemTooltipLine", v);
            base.ModifyTooltips(tooltips);
        }
        public void CombinationValue(out string v1, out string v2)
        {
            string v = TheString[StringType];
            if (v.Equals("Amethyst"))
            {
                v1 = "[i:LostbeltTraveler/YoyoRopeAmethyst]";
                v2 = "[i:Amethyst]";
            }
            else if (v.Equals("Topaz"))
            {
                v1 = "[i:LostbeltTraveler/YoyoRopeTopaz]";
                v2 = "[i:Topaz]";

            }
            else if (v.Equals("Sapphire"))
            {
                v1 = "[i:LostbeltTraveler/YoyoRopeSapphire]";
                v2 = "[i:Sapphire]";

            }
            else if (v.Equals("Emerald"))
            {
                v1 = "[i:LostbeltTraveler/YoyoRopeEmerald]";
                v2 = "[i:Emerald]";

            }
            else if (v.Equals("Ruby"))
            {
                v1 = "[i:LostbeltTraveler/YoyoRopeRuby]";
                v2 = "[i:Ruby]";

            }
            else if (v.Equals("Amber"))
            {
                v1 = "[i:LostbeltTraveler/YoyoRopeAmber]";
                v2 = "[i:Amber]";

            }
            else if (v.Equals("Onyx"))
            {
                v1 = "[i:LostbeltTraveler/YoyoRopeOnyx]";
                v2 = "[i:ContinentOfJourney/Onyx]";

            }
            else
            {
                v1 = "[i:LostbeltTraveler/YoyoRopeDiamond]";
                v2 = "[i:Diamond]";

            }
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
