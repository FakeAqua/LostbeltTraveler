using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class ChargeFeatherLeafs : ChargeFeatherBroken
    {
        public override int MatchWing => ItemID.LeafWings;
        public override float RestoredCharge => .35f;
        public static float RunSpeedAcc = .1f;
        public static float WhipRange = 1f;

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(
            $"[i:{ItemID.Search.GetName(MatchWing)}]", RunSpeedAcc.ToPercent(), WhipRange.ToPercent());
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
