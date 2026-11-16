using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Useables
{
    public class AxeCharm : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Useables;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Green);
            Item.accessory = true;
            Item.maxStack = 1;
        }
        public override bool CanRightClick() => true;
        public override void RightClick(Player player)
        {
            player.Lostbelt().tacticalExecutionManual = !player.Lostbelt().tacticalExecutionManual;
        }
        public override bool ConsumeItem(Player player) => false;
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player p = Main.LocalPlayer;
            Color c = p.Lostbelt().tacticalExecutionManual ? Color.LightGreen : Color.Coral;
            int executionProgressIndex = tooltips.FindIndex(line => line.Name == "Tooltip0" && line.Mod == "Terraria");
            string text = this.GetLocalizationKey("EnableTooltips").ToLangValue().ToFormatValue(p.Lostbelt().tacticalExecutionManual.ToString());
            var executionLine = new TooltipLine(Mod, "EnableTooltipsName", text)
            {
                OverrideColor = c
            };
            tooltips.Insert(executionProgressIndex, executionLine);
            int finalIndex = tooltips.FindLastIndex(line => line.Name.Contains("Tooltip") && line.Mod == "Terraria");
            string critDamage = this.GetLocalizationKey("CritDamageMult");
            tooltips.CreateTooltip(critDamage, Color.SkyBlue, Mod, "CritDamageLine", finalIndex + 1, p.Lostbelt().critDamageExecutor.ToPercent());
        }
    }
}
