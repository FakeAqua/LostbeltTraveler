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
    [AutoloadEquip(EquipType.Legs)]
    public class ReaperLegs : LostbeltArmor
    {
        public float MoveSpeed = .25f;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.ScarletRed);
        }
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MoveSpeed.ToPercent());
        public int Defense = 45;
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
            player.moveSpeed += MoveSpeed;
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
