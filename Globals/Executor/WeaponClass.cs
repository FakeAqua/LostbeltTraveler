using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Methods.SideTextPanel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Executor
{
    public abstract class ExecutorWeaponClass : ModItem, ILocalizedModType
    {
        public override string Texture => $"LostbeltTraveler/Assets/Texture/Items/Weapons/{GetType().Name}";
        /// <summary>
        /// 该武器处决攻击时的射弹
        /// 实际上只有一个标记，而且用于默认Shoot的直接作用
        /// 连我自己大部分情况下都不会直接用这个字段
        /// </summary>
        public virtual int ExecutionProj => -1;
        public new string LocalizationCategory => $"Weapons.Executor";
        /// <summary>
        /// 代行者的武器类型
        /// 划分为五种：投掷品、不脱手冷兵器、热武器、魔术载体和仆从
        /// </summary>
        public virtual ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.Misc;
        public object[] ExecutionDetail = [];
        public override bool WeaponPrefix() => true;
        public override bool RangedPrefix() => false;
        public virtual int ExecutionProgress => 10;
        public virtual float ExecutionStrikeDamageMult => 1.0f;
        public virtual float TextBoxSize => 1f;
        public virtual int SupportWeaponBuff => -1;
        public virtual bool BlockTextboxDetail()
        {
            return false;
        }
        public override void SetStaticDefaults()
        {
            ExSSD();
            LostbeltList.ExecuteRequests.Add(Type, ExecutionProgress);
            LostbeltList.ExecutorTypes.Add(Type, ExecutorWeaponType);
            if (ExecutorWeaponType == ExecutorWeaponType.Caster)
                ScarletItemIDSets.ForceToTacticalExecute[Type] = true;
            if (ExecutorWeaponType == ExecutorWeaponType.Assistance)
                ScarletItemIDSets.ForceToAutomaticExecute[Type] = true;

        }
        public virtual void ExSSD()
        {
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 16;
            Item.DamageType = ExecutorDamageClass.Instance;
            Item.Lostbelt().CanDrawIcon = true;
            ExSD();
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {

            if (Item.GetGlobalItem<ExecutorGlobalItem>().ExecutionDamageMult != 1 && damage > 0 && player.GetExecutionSrike())
            {
                damage = (int)(damage * ExecutionStrikeDamageMult * (Item.GetGlobalItem<ExecutorGlobalItem>().ExecutionDamageMult));
            }
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //初始化。
            bool useExecution = player.GetExecutionSrike();
            int projID = Item.Lostbelt().ExecutionProj != -1 && useExecution ? Item.Lostbelt().ExecutionProj : type;
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, projID, damage, knockback, player.whoAmI);
            proj.Lostbelt().HasExecutionMechanic = true;
            if (useExecution)
            {
                proj.Lostbelt().ExecutionStrike = true;
                player.RemoveExecutionProgress(Type);
                player.Lostbelt().tacticalExecutionInputCache = 0;
            }
            return false;
        }
        public virtual bool PrePreDrawTooltip(ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y)
        {
            return true;
        }
        public override bool PreDrawTooltip(ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y)
        {
            if (!PrePreDrawTooltip(lines, ref x, ref y))
                return base.PreDrawTooltip(lines, ref x, ref y);
            if (!BlockTextboxDetail())
            {
                string detailText = this.GetLocalizationKey("ExecutionStrike").ToLangValue();
                int requirements = Math.Max(0, ExecutionProgress);
                int curRequirement = Main.LocalPlayer.Lostbelt().ExecutionListStored.TryGetValue(Type, out int value) ? value : 0;
                string numberText = Mod.GetLocalizationKey("ExecutorDamageClass.ExecutionProgressRevampedMode").ToLangValue().ToFormatValue(curRequirement, requirements);
                string titleText = Mod.GetLocalizationKey("ExecutorDamageClass.ExecutionDescriptionName").ToLangValue();
                Color c = Color.DarkRed;
                if (SupportWeaponBuff != -1 && ExecutorWeaponType == ExecutorWeaponType.Assistance)
                {
                    titleText = Mod.GetLocalizationKey("Buffs." + Name + "Buff.DisplayName").ToLangValue();
                    c = Color.DarkSlateBlue;
                }
                TooltipLine titleLine = new TooltipLine(Mod, "ExecutionTitleName", titleText);
                List<SideTextPanelData> list =
                    [
                    new SideTextPanelData(titleLine,1,SupportWeaponBuff),
                    ];
                string[] textBundle = detailText.Split('\n');
                for (int i = 0; i < textBundle.Length; i++)
                {
                    list.Add(new(new(Mod, "ExecutionStrikeTooltip" + i, textBundle[i]), 1));
                }
                list.Add(new SideTextPanelData(new TooltipLine(Mod, "ExecutionNumberName", "\n" + numberText)));
                SideTextPanelMethods.DrawSideTextPanelWithTitleEdge(lines, list, x, y, c);
            }
            return base.PreDrawTooltip(lines, ref x, ref y);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (!BlockTextboxDetail())
            {
                int executionProgressIndex = tooltips.FindIndex(line => line.Name == "ItemName" && line.Mod == "Terraria");

                string categoryText = Mod.GetLocalizationKey($"ExecutorDamageClass.WeaponType.{ExecutorWeaponType}").ToLangValue();
                int executionLineIndex = tooltips.FindIndex(line => line.Name == "ExecutionTooltipName" && line.Mod == "LostbeltTraveler") - 1;
                executionLineIndex = executionProgressIndex;
                var categoryLine = new TooltipLine(Mod, "ExecutorWeaponTypeName", ">" + categoryText)
                {
                    OverrideColor = Color.LightGoldenrodYellow
                };
                tooltips.Insert(executionLineIndex + 1, categoryLine);
            }
            ExModifyTooltips(tooltips);
        }
        public virtual void ExModifyTooltips(List<TooltipLine> tooltips)
        {

        }
        public virtual void ExSD() { }
    }
}
