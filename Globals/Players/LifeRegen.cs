using LostbeltTraveler.Buffs.Debuffs;
using LostbeltTraveler.Items.Accessories;
using LostbeltTraveler.Items.Useables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Players
{
    public partial class LostbeltPlayer : ModPlayer
    {
        public bool absoluteZeroBuff = false;
        public bool theBleachingBuff = false;
        public int lifeRegenPool = 0;
        public float lifeRegenMult = 1;
        public int badLifeRegenPool = 0;
        public float badLifeRegenMult = 1;
        public override void UpdateLifeRegen()
        {
            int totalRegen = 0;
            if (wingsType == ItemID.AngelWings && wingsChargeType == ItemType<ChargeFeatherAngel>())
            {
                totalRegen += ChargeFeatherAngel.LifeRegen;
            }
            if (wingsType == ItemID.FestiveWings && wingsChargeType == ItemType<ChargeFeatherFestival>())
            {
                totalRegen += ChargeFeatherFestival.LifeRegen;
            }
            lifeRegenPool += totalRegen;
            if (lifeRegenPool > 0)
                Player.lifeRegen += lifeRegenPool;
            if (selfPortraitType == -2)
            {
                dotMult += Player.lifeRegen * SelfPortraitoftheHoloFace.DotDamageMult;
                Player.lifeRegen = 0;
            }
            lifeRegenPool = 0;
        }
        public override void UpdateBadLifeRegen()
        {
            if (saintChurch && saintChurchLastStanding > 0)
                ApplyDoT(0);
            if (badLifeRegenPool > 0)
                Player.lifeRegen -= badLifeRegenPool;
            badLifeRegenPool = 0;
            if (badLifeRegenMult != 1)
            {
                if (badLifeRegenMult < 0)
                    badLifeRegenMult = 0;
                Player.lifeRegen = (int)(Player.lifeRegen * badLifeRegenMult);
            }
            badLifeRegenMult = 1;
        }
        public void ApplyDoT(int badLifeRegen)
        {
            if (Player.lifeRegen > 0)
                Player.lifeRegen = 0;
            Player.lifeRegenTime = 0;
            Player.lifeRegen -= badLifeRegen * 2;
        }

    }
}
