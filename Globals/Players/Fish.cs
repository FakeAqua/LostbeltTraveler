using ContinentOfJourney;
using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Graphics.Particles;
using LostbeltTraveler.Globals.Systems.Conditions;
using LostbeltTraveler.Items.Accessories;
using LostbeltTraveler.Items.Armor.Diver;
using LostbeltTraveler.Items.Useables;
using LostbeltTraveler.Items.Weapons.Melee;
using LostbeltTraveler.Projs.General;
using rail;
using System.Collections.Generic;
using System.Xml.Schema;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Players
{
    public partial class LostbeltPlayer : ModPlayer
    {
        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            int bait = attempt.playerFishingConditions.BaitItemType;
            int power = attempt.playerFishingConditions.BaitPower + attempt.playerFishingConditions.PolePower;
            int questFish = attempt.questFish;
            int poolSize = attempt.waterTilesCount;
            bool water = !attempt.inHoney && !attempt.inLava;
            if(wingsType == ItemID.FinWings&&wingsChargeType==ItemType<ChargeFeatherFish>()&&Main.rand.NextBool(5))
            {
                int food = -1;
                float chance = Main.rand.NextFloat();
                if(chance<.6f)
                {
                    int c = Main.rand.Next(0, LostbeltList.WellFed1HashSet.Count);
                    food = LostbeltList.WellFed1HashSet[c];
                }
                else if (chance<.9f)
                {
                    int c = Main.rand.Next(0, LostbeltList.WellFed2HashSet.Count);
                    food = LostbeltList.WellFed2HashSet[c];
                }
                else
                {
                    int c = Main.rand.Next(0, LostbeltList.WellFed3HashSet.Count);
                    food = LostbeltList.WellFed3HashSet[c];
                }
                //if (Player.HasProj<FishingFoodProj>())
                {
                    Vector2 center = Player.Center - Vector2.UnitY * 100;
                    for (int i = 0; i < 8; i++)
                    {
                        float progress = Lerp(.7f, 1f, i / 8f);
                        new NoiseShockRing(center, Vector2.Zero, Color.White, 60, 1, progress * .5f, -1, Vector2.Zero, false).Spawn();
                    }
                    for (int i = 0; i < 20; i++)
                    {
                        ECSParticle.ShinyCrossStarECS(center, RandVelTwoPi(0.52f, 6.4f), Color.White, 40, 1, 0.4f, .2f);
                    }
                    ECSParticle.KiraStar(center, Color.White, 30, 1, 1);
                    ScarletSound(LostbeltSoundAssets.Moonlight_Ding, center, pitch: -.4f, volume: .45f);
                    LostbeltMethods.ScarletSpawnItem(Player.GetSource_FromThis(), center, Player.Hitbox, food);
                }
                //else
                //{
                //    Projectile proj = Projectile.NewProjectileDirect(Player.GetSource_FromThis(), sonarPosition, Vector2.Zero, ProjectileType<FishingFoodProj>(), 0, 0, Player.whoAmI);
                //    proj.ai[1] = food;
                //}
            }
            if (!bitingClaw)
                return;

            if (water)
            {
                int poolSizeAmt = poolSize / 10;
                if (poolSizeAmt > 100)
                    poolSizeAmt = 100;
                if (Player.ZoneBeach && ScarletDownedBoss.DownedEvilBosses)
                {
                    HandleDiverArmor(poolSizeAmt, power, ref itemDrop, ref sonar);
                    return;
                }
                if (Player.ZoneDesert && DownedBossSystem.downedTimeGod)
                    FastPoolHandler(1000, ItemType<EthrealSwordfish>(), poolSizeAmt, power, ref itemDrop, ref sonar);
                if (Player.ZoneSnow && DownedBossSystem.downedMatterGod)
                    FastPoolHandler(1000, ItemType<MatterSawtoothShark>(), poolSizeAmt, power, ref itemDrop, ref sonar);
                if (Player.ZoneJungle && DownedBossSystem.downedLifeGod)
                    FastPoolHandler(1000, ItemType<LivingReaverShark>(), poolSizeAmt, power, ref itemDrop, ref sonar);
                if (Player.ZoneBeach && Main.hardMode)
                    FastPoolHandler(1000, ItemType<EnchantedSwordfish>(), poolSizeAmt, power, ref itemDrop, ref sonar);
                if (DownedBossSystem.downedBarrier && !Main.dayTime)
                    FastPoolHandler(1000, ItemType<DarkenRockFish>(), poolSizeAmt, power, ref itemDrop, ref sonar);
            }
        }
        public void FastPoolHandler(int chanceAmt, int targetItem, int poolSizeAmt, int power, ref int itemDrop, ref AdvancedPopupRequest sonar)
        {
            int fishPowerDiv = power + poolSizeAmt;
            int chanceToCatch = chanceAmt / fishPowerDiv;
            if (Main.rand.NextBool(chanceToCatch))
            {
                itemDrop = targetItem;
            }
        }
        public void HandleDiverArmor(int poolSizeAmt, int power, ref int itemDrop, ref AdvancedPopupRequest sonar)
        {
            int fishPowerDiv = power + poolSizeAmt;
            int chanceToCatchDiverArmor = 1000 / fishPowerDiv;
            List<int> list = [ItemType<DiverHead>(), ItemType<DiverBody>(), ItemType<DiverLegs>()];
            int increaseChanceTime = 1;
            for (int i = 0; i < Player.inventory.Length; i++)
            {
                if (Player.inventory[i].IsAir || Player.inventory[i] == null)
                    continue;
                if (Player.inventory[i].type == ItemType<DiverHead>() && list.Contains(ItemType<DiverHead>()))
                {
                    increaseChanceTime += 1;
                    list.Remove(ItemType<DiverHead>());
                }
                if (Player.inventory[i].type == ItemType<DiverBody>() && list.Contains(ItemType<DiverBody>()))
                {
                    increaseChanceTime += 1;
                    list.Remove(ItemType<DiverBody>());
                }
                if (Player.inventory[i].type == ItemType<DiverLegs>() && list.Contains(ItemType<DiverLegs>()))
                {
                    increaseChanceTime += 1;
                    list.Remove(ItemType<DiverLegs>());
                }
            }
            chanceToCatchDiverArmor /= increaseChanceTime;
            if (Main.rand.NextBool(chanceToCatchDiverArmor))
            {
                if (list.Count != 0)
                {
                    itemDrop = list[Main.rand.Next(list.Count)];
                    sonar.Color = Color.Red;
                }
            }
        }
    }
}
