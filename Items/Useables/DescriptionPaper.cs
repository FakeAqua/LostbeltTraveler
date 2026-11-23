using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Useables
{
    public class DescriptionPaper : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Useables;
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Blue);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftAlt))
                tooltips.ReplaceAllTooltip(this.GetLocalizationKey("ThanksList"));
        }
    }
}
