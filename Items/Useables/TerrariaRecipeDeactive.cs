using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Useables
{
    public class TerrariaRecipeDeactive : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Useables;
        public override void ExSD()
        {
            Item.width = 50;
            Item.height = 50;
            Item.SetUpRarityPrice(ItemRarityID.Blue);
            Item.stack = 1;
            Item.useTime = Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }
        public override bool ConsumeItem(Player player)
        {
            return true;
        }
        public override bool CanRightClick()
        {
            return true;
        }
        public override void RightClick(Player player)
        {
            player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemType<TerrariaRecipe>());
            //player.Lostbelt().terraRecipe = false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string value = this.GetLocalizedValue("Active");
            tooltips.CreateTooltip(value, Color.GreenYellow);
        }
    }
}
