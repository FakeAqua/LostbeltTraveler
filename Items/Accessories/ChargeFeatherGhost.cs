using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Accessories
{
    public class ChargeFeatherGhost : ChargeFeatherBroken
    {
        public override int MatchWing => ItemID.GhostWings;
        public override float RestoredCharge => 0.25f;
        public int WingTime = 4;
        public float WingPower = 1.0f;
        public float ManaCost = .1f;
        public float Damage = .05f;
        public int Crit = 5;
        public int MaxMana = 25;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(WingTime,
            ((float)(int)(WingPower * 100 / 5)) + "%",
            Damage.ToPercent(), ManaCost.ToPercent(), Crit + "%", MaxMana, $"[i:{ItemID.Search.GetName(MatchWing)}]");
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
        }
        public override void UpdateGlobal(Player player)
        {
            player.wingTimeMax += GetSeconds(WingTime);
            player.jumpSpeedBoost += WingPower;
        }
        public override void UpdateMatchWings(Player player)
        {
            player.GetDamage<MagicDamageClass>() += Damage;
            player.GetCritChance<MagicDamageClass>() += Crit;
            player.manaCost -= ManaCost;
            player.statManaMax2 += MaxMana;
        }
    }
}
