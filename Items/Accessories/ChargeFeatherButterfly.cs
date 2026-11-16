using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Accessories
{
    public class ChargeFeatherButterfly: ChargeFeatherBroken
    {
        public override int MatchWing => ItemID.ButterflyWings;
        public override float RestoredCharge => .05f;
        public static int WingTime = 3;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs($"[i:{ItemID.Search.GetName(MatchWing)}]",WingTime);
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
        }
        public override void UpdateGlobal(Player player)
        {
        }
        public override void UpdateMatchWings(Player player)
        {
        }
    }
}
