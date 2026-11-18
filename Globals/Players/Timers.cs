using LostbeltTraveler.Buffs.Debuffs;
using LostbeltTraveler.Buffs.GoodBuffs;
using LostbeltTraveler.Buffs.Status;
using LostbeltTraveler.Globals.Graphics.Particles;
using LostbeltTraveler.Globals.Players.InnerCooldown.Methods;
using LostbeltTraveler.Items.Weapons.Executor.ColdSteel;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Players
{
    public partial class LostbeltPlayer : ModPlayer
    {
        public int swapTimer = 0;
        public int ruShiWoWenBanTimer = 0;
        public int globalSoundDelay = 0;
        public int bookcaseBuffTime = 0;
        public int emblemFirearmTimer = 0;
        public int jellyfishGroupBuffTime = 0;
        public int selfPortraitHealTiming = -1;
        public void ResetTimer()
        {
            outlawSniperStack = 0;
            mayaPumperDashType = -1;
            emblemFirearmTimer = 0;
            climaticHawstringLaserCounter = 0;
            tacticalExecutionInputCache = 0;
            antiKnockbackTime = 0;
            crimsonCharmReduceTime = 0;
            containedBlastBoomCount = 0;
            powerLilyTimer = 0;
            conferenceCallBuffTime = 0;
            tearEyeBuff = 0;
            jellyfishGroupBuffTime = 0;
        }
        private readonly List<int> keysToRemoveCache = new();
        public void UpdateTimer()
        {
            if (ExecutionBuffTimeStored.Keys.Count == 0)
            {
                keysToRemoveCache.Clear();
            }
            else
            {

                foreach (var pair in ExecutionBuffTimeStored)
                {
                    int key = pair.Key;
                    int time = pair.Value;
                    if (time > 0)
                    {
                        time--;
                        ExecutionBuffTimeStored[key] = time;
                    }
                    if (time == 0)
                    {
                        keysToRemoveCache.Add(key);
                    }
                }
                foreach (int key in keysToRemoveCache)
                {
                    ExecutionBuffTimeStored.Remove(key);
                }
            }
            if (!Player.HasInnerCooldown(InnerCooldownType.MayaPumperDash))
            {
                mayaPumperDashType = -1;
            }
            if (!bloodThronCrown)
            {
                if (bloodThornCrownHit > 0)
                    bloodThornCrownHit--;
            }
            if (!Player.HasInnerCooldown(InnerCooldownType.OutlawSniperMulter))
                outlawSniperStack = 0;
            if (!Player.HasBuff<CursedHeavy>() && desterrannachtStack > 0)
                desterrannachtStack = 0;
            if (!Player.HasBuff<JellyfishGroupBuff>())
                jellyfishGroupIndex = -1;
            if (!emblemFirearm)
                emblemFirearmTimer = 0;
            if (tearEyeBuff > 0)
                tearEyeBuff--;
            if (conferenceCallBuffTime > 0)
                conferenceCallBuffTime--;
            if (powerLilyTimer > 1)
            {
                powerLilyTimer--;
                powerLilyCacheTimer = powerLilyTimer;
            }
            if (!powerLily && powerLilyTimer > 0)
                powerLilyTimer = 0;
            if (tacticalExecutionInputCache > 0)
                tacticalExecutionInputCache--;

            if (flybackhandBuffTime > 0)
                flybackhandBuffTime--;

            if (flybackhandBuffTime == 0)
                flybackhandBuffTimeCurrent = 0;

            if (galvanizedHandDashCD > 0)
            {
                if (galvanizedHandDashCD == 1)
                {
                    SoundEngine.PlaySound(SoundID.Item35, Player.Center);
                    for (int i = 0; i < 25; i++)
                    {
                        new TurbulenceShinyOrb(Player.Center.ToRandCirclePosEdge(10), 2f, RandLerpColor(Color.SkyBlue, Color.White), 120, 0.4f, RandRotTwoPi).Spawn();
                    }
                }
                galvanizedHandDashCD--;
            }

            if (NoSlowFall > 0)
                NoSlowFall--;
            protectorPlantID = Player.HasBuff<HerbBagBuff>() ? protectorPlantID : -1;
            for (int i = 0; i < protectorHerbTimerList.Length; i++)
            {
                if (protectorHerbTimerList[i] != 0)
                    protectorHerbTimerList[i]--;
            }

            if (!Player.HasBuff<BlackKeyExecutionBuff>())
                blackKeyDefenseTrigger = false;
            if (!Player.HasBuff<CrimsonCharmBuff>())
            {
                if (Player.miscCounter % 10 == 0 && crimsonCharmReduceTime > 0)
                    crimsonCharmReduceTime--;
            }
            if (!Player.HasInnerCooldown(InnerCooldownType.ContainedBlastBuff))
                containedBlastBoomCount = 0;
            if (antiKnockbackTime > 0)
            {
                Player.noKnockback = true;
                antiKnockbackTime--;
            }
            if (Player.HasInnerCooldown(InnerCooldownType.PureDaggerDefense))
                Player.statDefense += PureDagger.DefenseAdd;
            if (Player.HeldItem.type != lastHeldItemIndex)
            {
                lastHeldItemIndex = Player.HeldItem.type;
                hasSendExecutionTint = false;
                executorSwordMarkPing = false;
                emblemFirearmTimer = 0;
            }
            if (crimsonScytheDefense > 0 && crimsonScytheAttackCounter < 1)
                crimsonScytheDefense -= 1;
        }
    }
}
