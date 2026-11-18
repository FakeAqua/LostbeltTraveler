using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class ChargeFeatherFairy : ChargeFeatherBroken
    {
        public override int MatchWing => ItemID.FairyWings;
        public override float RestoredCharge => .30f;
        public static float Luck = 5;
        public static float RareRate = .5f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs($"[i:{ItemID.Search.GetName(MatchWing)}]", Luck, RareRate.ToPercent());
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
