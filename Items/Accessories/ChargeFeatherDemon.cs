using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class ChargeFeatherDemon: ChargeFeatherBroken
    {
        public override int MatchWing => ItemID.DemonWings;
        public override float RestoredCharge => .1f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs($"[i:{ItemID.Search.GetName(MatchWing)}]",RunSpeedAcce.ToPercent(),Damage.ToPercent(),Crit+"%");
        public static float Damage = .05f;
        public static float Crit = 5;
        public static float RunSpeedAcce = .05f;
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
