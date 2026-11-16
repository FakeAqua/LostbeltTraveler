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
    public class ChargeFeatherVortex : ChargeFeatherBroken
    {
        public override int MatchWing => ItemID.WingsVortex;
        public override float RestoredCharge => 0.5f;
        public static float Crit = 10;
        public static float Damage = .10f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs($"[i:{ItemID.Search.GetName(MatchWing)}]",Damage.ToPercent(),Crit+"%");
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
