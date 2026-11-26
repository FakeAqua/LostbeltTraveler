using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Database.Localization;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Instances.Items
{
    public partial class LostbeltGlobalItems : GlobalItem
    {
        public void UpdateParryShield(Item item, Player player)
        {
            if (!LostbeltList.ParryShieldDictionary.ContainsKey(item.type))
                return;
            player.Lostbelt().parryShieldType = item.type;
        }
        public void ModifyParryShieldTooltip(Item item, List<TooltipLine> tooltips)
        {
            if (!LostbeltList.ParryShieldDictionary.ContainsKey(item.type))
                return;
            string prefix = "Upgrade.ParryShield.";
            string genericText = Mod.GetLocalizationKey(prefix + "GeneralText").ToLangValue();
            int index = tooltips.FindLineIndexLast("Tooltip");
            string combinedValue = ScarletLocalText.GenericText.ModNamePrefix + "\n" + genericText;
            TooltipLine line = new TooltipLine(Mod, "ParryShieldTooltipName", combinedValue) { OverrideColor = Color.LightGoldenrodYellow };
            tooltips.Add(line);
            //tooltips.Insert(index, line);
            if (Main.keyState.PressingAlt())
            {
                if (tooltips.Count < 2)
                    return;
                tooltips.RemoveRange(1, tooltips.Count - 1);
                string types = Mod.GetLocalizationKey(prefix + "Types.");
                string typeName = string.Empty;
                if (item.type < VanillaMaxItem)
                {
                    typeName = ItemID.Search.GetName(item.type);
                }
                else
                {
                    if (item.ModItem is not null)
                    {
                        typeName = item.Name;
                    }
                }
                string combinedValue2 = (types + typeName).ToLangValue();
                TooltipLine line2 = new TooltipLine(Mod, "ParryShieldDetailTooltip", combinedValue2);
                tooltips.Insert(1, line2);

            }
        }
    }
}
