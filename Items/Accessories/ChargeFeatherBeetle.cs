using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class ChargeFeatherBeetle : ChargeFeatherBroken
    {
        public override int MatchWing => ItemID.BeetleWings;
        public override float RestoredCharge => .25f;
        public static float AttackSpeed = .20f;
        public static float DR = .10f;

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs($"[i:{ItemID.Search.GetName(MatchWing)}]", AttackSpeed.ToPercent(), DR.ToPercent());
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
