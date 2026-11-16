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
    public class ChargeFeatherSpooky: ChargeFeatherBroken
    {
        public override float RestoredCharge => .25f;
        public override int MatchWing => ItemID.SpookyWings;
        public static float RunSpeed = .25f;
        public static float MinionSlot = 2;
        public static float MinionDamage = .05f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs($"[i:{ItemID.Search.GetName(MatchWing)}]",RunSpeed.ToPercent(),MinionSlot,MinionDamage.ToPercent());
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
