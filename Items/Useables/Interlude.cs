using ContinentOfJourney.Items;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Keybinds;
using LostbeltTraveler.Items.Materials;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Useables
{
    public class Interlude:LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Useables;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Final);
        }
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = Item.useTime = 16;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
        }
        public override void UpdateVanity(Player player)
        {
            base.UpdateVanity(player);
        }
        public override void UpdateInfoAccessory(Player player)
        {
            base.UpdateInfoAccessory(player);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.FindReplaceWithHotkeyString(LostbeltKeybinds.GeneralActionKeybind);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.RodOfHarmony).
                AddIngredient(ItemID.Shellphone).
                AddIngredient(ItemID.LavaproofTackleBag).
                AddIngredient<HolyReflexion>().
                AddIngredient<CrownofSilveryLight>(15).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
