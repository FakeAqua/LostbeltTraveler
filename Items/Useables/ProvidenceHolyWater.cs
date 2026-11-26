using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Keybinds;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Useables
{
    public class ProvidenceHolyWater : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Useables;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateWhite);
        }
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Cyan);
            Item.Lostbelt().drawFlavorTooltip = true;
            Item.healMana = 10;
            Item.consumable = false;
        }
        public override void GetHealMana(Player player, bool quickHeal, ref int healValue)
        {
            healValue = player.Lostbelt().providenceHolyWaterHealMana + 10;
        }
        public override void HoldItem(Player player)
        {
            player.Lostbelt().drawUseableItemIcon = Type;
            for (int i = 0; i < player.inventory.Length; i++)
            {
                Item item = player.inventory[i];
                if (item.IsAir || item is null)
                    continue;
                bool isWeapon = item.damage < 1 && item.pick == 0 && item.axe == 0 && item.hammer == 0 && item.healMana > 0;
                if (isWeapon && item.type != Type)
                {
                    item.Lostbelt().setTintIcon = true;
                }
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.FindReplaceWithHotkeyString(LostbeltKeybinds.GeneralActionKeybind);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.GreaterManaPotion, 30).
                AddIngredient(ItemID.Star, 30).
                AddTile(TileID.CrystalBall).
                Register();
        }
    }
}
