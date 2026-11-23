using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Useables
{
    public class CrystallizedLore : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Useables;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateGolden);
        }
        public override void ExSD()
        {
            Item.Lostbelt().drawFlavorTooltip = true;
            Item.Lostbelt().noFancyFlavorTooltip = true;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.buyPrice(0, 1, 0, 0);
        }
        public override bool CanRightClick() => Main.keyState.PressingShift();
        public override void RightClick(Player player)
        {
            player.Lostbelt().crystallizeLoreReforgeIndex++;
            if (player.Lostbelt().crystallizeLoreReforgeIndex > 4)
                player.Lostbelt().crystallizeLoreReforgeIndex = 0;
        }
        public override bool ConsumeItem(Player player)
        {
            return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int index = tooltips.FindLastIndex(0, t => t.Name.Contains("Tooltip") && t.Mod == "Terraria");
            string prefixID = "Mencing";
            prefixID = Main.LocalPlayer.Lostbelt().crystallizeLoreReforgeIndex switch
            {
                0 => "Mencing",
                1 => "Lucky",
                2 => "Quick",
                3 => "Violent",
                _ => "Warding",
            };
            string text = Mod.GetLocalizationKey("Database.PrefixData." + prefixID).ToLangValue();
            tooltips.CreateTooltip(this.GetLocalizationKey("PrefixLine"), Color.SkyBlue, Mod, "CrystallizedLorePrefixName", index, text);
        }
    }
}
