using ContinentOfJourney;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Items.Useables;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Armor.Reaper
{
    [AutoloadEquip(EquipType.Body)]
    public class ReaperBody : LostbeltArmor
    {
        public float CritDamage = 0.15f;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.ScarletRed);
        }
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(CritDamage.ToPercent());
        public int Defense = 50;
        public override void ExSD()
        {
            Item.defense = Defense;
            Item.Lostbelt().CanDrawGhost = true;
            Item.Lostbelt().CanDrawIcon = false;
            Item.SetUpRarityPrice(ItemRarityID.Cyan);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            ReaperHead.ModifyTooltipsAdd(tooltips, Mod);
        }
        public override void PostDrawTooltipLine(DrawableTooltipLine line)
        {
            ReaperHead.ModifyTooltipLine(line);
        }

        public override void UpdateEquip(Player player)
        {
            if (!DownedBossSystem.downedSunGod)
            {
                player.statDefense -= Defense;
                return;
            }
            player.Lostbelt().critDamageExecutor += CritDamage;
            player.aggro += 500;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<FreehandFrame>().
                DisableDecraft().
                Register();
        }
    }
}
