using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class ChargeFeatherEmpress : ChargeFeatherBroken
    {
        public override int MatchWing => ItemID.RainbowWings;
        public override float RestoredCharge => 0;
        public static float Damage = .20f;
        public static int StatDefense = 10;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs($"[i:{ItemID.Search.GetName(MatchWing)}]", Damage.ToPercent(), StatDefense);
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
