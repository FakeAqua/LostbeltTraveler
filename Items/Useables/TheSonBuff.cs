using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Useables
{
    public class TheSonBuff : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Useables;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateGolden);
        }
        public override void ExSD()
        {
            Item.master = true;
            Item.SetUpRarityPrice(ItemRarityID.Blue);
        }
        public override bool CanRightClick() => true;
        public override void RightClick(Player player)
        {
            player.Lostbelt().weaponUpgradePostSon = !player.Lostbelt().weaponUpgradePostSon;
        }
        public override void HoldItem(Player player)
        {
            player.Lostbelt().drawUseableItemIcon = Type;
            for (int i = 0; i < player.inventory.Length; i++)
            {
                Item item = player.inventory[i];
                if (!item.IsLegal())
                    continue;
                if (ScarletItemIDSets.GrantsBoosterAfterSon[item.type])
                    item.Lostbelt().setTintIcon = true;
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player p = Main.LocalPlayer;
            Color c = p.Lostbelt().weaponUpgradePostSon ? Color.LightGreen : Color.Coral;
            int index = tooltips.FindLineIndex("Tooltip0");
            string text = this.GetLocalizationKey("EnableTooltips").ToLangValue().ToFormatValue(p.Lostbelt().weaponUpgradePostSon.ToString());
            var executionLine = new TooltipLine(Mod, "EnableTooltipsName", text)
            {
                OverrideColor = c
            };
            tooltips.Insert(index, executionLine);
        }

        public override bool ConsumeItem(Player player) => false;
    }
}
