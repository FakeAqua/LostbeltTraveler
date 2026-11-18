using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class ChargeFeatherFestival : ChargeFeatherBroken
    {
        public override int MatchWing => ItemID.FestiveWings;
        public override float RestoredCharge => .25f;
        public static int Defense = 5;
        public static int LifeRegen = 1;
        public static int MaxLife = 40;

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs($"[i:{ItemID.Search.GetName(MatchWing)}]", LifeRegen.ToLifeRegenFormat(), Defense, MaxLife);
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
