using LostbeltTraveler.Buffs.Debuffs;
using LostbeltTraveler.Core.NetCode;
using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Globals.Graphics.Particles;
using LostbeltTraveler.Globals.Keybinds;
using LostbeltTraveler.Items.Accessories;
using LostbeltTraveler.Items.Weapons.Executor.ColdSteel;
using LostbeltTraveler.Projs.General;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Players
{
    public partial class LostbeltPlayer : ModPlayer
    {
        public bool CanSwitchWeaponType = false;
        public bool CanRevisual = false;
        public bool CanExecution = false;
        public bool CanWeaponSpecialAbility = false;
        public bool CanArmorAbility = false;
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (LostbeltNetInput.ParryCurrent)
            {
                ParryShieldHelper();
            }
            if (LostbeltNetInput.SkillJustPressed)
            {
                if (defenderEmblemCD == 0 && emblemVanguard)
                {
                    Player.GetImmnue(ImmunityCooldownID.General, GetSeconds(EmblemVanguard.InvinceTime), false);
                    ScarletSound(LostbeltSoundAssets.GrabCharge, Player.Center, instances: 0);
                    defenderEmblemCD = GetSeconds(EmblemVanguard.Cooldown + EmblemVanguard.InvinceTime);
                    for (int i = 0; i < 30; i++)
                        ECSParticle.TurbulenceShinyOrb(Player.Center.ToRandCirclePos(15), 2.4f, Color.White, 120, 1, 0.3f, RandRotTwoPi);
                    new GeneralMountedPlayerParticle(LostbeltModAssets.Particle_RingShiny.Value, Color.White, 0, .24f, .8f, GetSeconds(3), Player.whoAmI, BlendState.Additive).Spawn();
                }
                if (desterrennacht)
                {
                    int c = 0;
                    foreach (var tar in Main.ActiveNPCs)
                    {
                        if (!tar.CanBeChasedBy())
                            continue;
                        if ((Player.Center - tar.Center).LengthSquared() > 600f * 600f)
                            continue;
                        if (!tar.HasBuff<CursedHeavy>())
                            continue;
                        int index = tar.FindBuffIndex(BuffType<CursedHeavy>());
                        c++;
                        tar.DelBuff(index);
                        Projectile.NewProjectileDirect(Player.GetSource_FromThis(), tar.Center, RandVelTwoPi(10, 13), ProjectileType<DesterrannachtCursedBullet>(), 0, 0, Player.whoAmI);
                    }
                    if (c > 0)
                    {
                        ScarletSound(LostbeltSoundAssets.Gaia_Charge, Player.Center);
                        Player.AddBuff(BuffType<CursedHeavy>(), GetSeconds(60));
                        desterrannachtStack += c;
                        if (desterrannachtStack> 10)
                            desterrannachtStack = 10;
                        CombatText.NewText(Player.Hitbox, Color.Azure, $"(-{0.25 * desterrannachtStack}/帧)", false, true);
                    }
                }
            }
            if (LostbeltKeybinds.GeneralActionKeybind.JustPressed)
            {
                bool tier1 = PiorityTier1();
                if (tier1)
                {
                    return;
                }
                bool tier2 = PiorityTier2();
                if (tier2)
                    return;
            }
        }
        /// <summary>
        /// 第一批优先度梯队：物品切换，代行者手动触发的处刑攻击
        /// </summary>
        /// <returns></returns>
        private bool PiorityTier1()
        {
            bool anyPiorityTier1Active = false;
            //按情况提供硬编码
            //第一批顺序：查看武器转化
            //获取这个列表的第一个值，对比玩家手持的情况，符合则打一个标记出去
            int heldItem = Player.HeldItem.type;
            bool hasValue = WeaponSwapMaps.ContainsKey(heldItem) || ArmorMaps.Contains(heldItem) || WeaponSwapMaps.ContainsValue(heldItem);
            if (!CanSwitchWeaponType && hasValue)
            {
                //打一个标记出去
                CanSwitchWeaponType = true;
                anyPiorityTier1Active = true;
            }
            if (tacticalExecutionInputCache == 0 && tacticalExecution)
            {
                //12帧的输入缓冲
                tacticalExecutionInputCache = 60;
                anyPiorityTier1Active = true;
            }
            if (!CanWeaponSpecialAbility && (heldItem == ItemID.MonkStaffT1 || heldItem == ItemID.MonkStaffT3 || Player.IsHolding<CrimsonScythe>()))
            {
                CanWeaponSpecialAbility = true;
                anyPiorityTier1Active = true;
            }
            return anyPiorityTier1Active;
        }
        /// <summary>
        /// 第二批优先梯队：盔甲技能，部分武器技能
        /// </summary>
        /// <returns></returns>
        private bool PiorityTier2()
        {
            bool anyPiorityTier2Active = false;
            if (!CanArmorAbility)
            {
                CanArmorAbility = true;
                anyPiorityTier2Active = true;
            }
            return anyPiorityTier2Active;
        }
    }
}
