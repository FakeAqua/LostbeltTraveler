using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class ChargeFeatherFish: ChargeFeatherBroken
    {
        public override int MatchWing => ItemID.FinWings;
        public override float RestoredCharge => base.RestoredCharge;
        public static int FishingPower = 50;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs($"[i:{ItemID.Search.GetName(MatchWing)}]",FishingPower+"%");
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.Lostbelt().drawBuffIcon = true;
        }
        public override void UpdateGlobal(Player player)
        {

        }
        public override void UpdateMatchWings(Player player)
        {
        }
    }
}
