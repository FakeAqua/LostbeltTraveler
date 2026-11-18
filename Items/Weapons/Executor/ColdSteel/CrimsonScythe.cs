using ContinentOfJourney;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Globals.Methods.Textbox;
using LostbeltTraveler.Globals.Systems.Conditions;
using LostbeltTraveler.Items.Armor.Reaper;
using LostbeltTraveler.Projs.Executor.ColdSteel;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Executor.ColdSteel
{
    public class CrimsonScythe : ExecutorWeaponClass
    {
        public override int ExecutionProgress => 40;
        public static int DefensePerAdd = 2;
        public static int MaxSoulStone = 20;
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.ColdSteel;
        public override void ExSSD()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.ScarletRed);
            ScarletItemIDSets.GrantsBoosterAfterSon[Type] = true;
            ScarletItemIDSets.IsHeldProjItem[Type] = true;
        }
        public override void ExSD()
        {
            Item.damage = 813;
            Item.useTime = Item.useAnimation = 30;
            Item.SetUpNoUseGraphicItem(true);
            Item.SetUpRarityPrice(ItemRarityID.Red);
            Item.Lostbelt().CanDrawIcon = false;
            Item.Lostbelt().CanDrawGhost = true;
            Item.Lostbelt().drawBuffIconAndDetail = true;
            Item.shootSpeed = 10;
            Item.shoot = ProjectileType<CrimsonScytheHeldProj>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5;
        }
        public override bool CanShoot(Player player)
        {
            return !player.HasProj(Item.shoot) && !player.HasProj<CrimsonScytheSkillProj>();
        }
        public override bool CanRightClick() => true;
        public override void RightClick(Player player)
        {
            player.Lostbelt().crimsonScytheSlayNPCType += 1;
            if (player.Lostbelt().crimsonScytheSlayNPCType > 2)
                player.Lostbelt().crimsonScytheSlayNPCType = 0;
        }
        public override bool ConsumeItem(Player player) => false;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!DownedBossSystem.downedSunGod)
                damage = 1;
            bool exe = player.GetExecutionSrike();
            Vector2 dir = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            proj.Lostbelt().HasExecutionMechanic = true;
            proj.Lostbelt().ExecutionStrike = exe;
            ((CrimsonScytheHeldProj)proj.ModProjectile).BeginTargetRotation = dir.ToRotation();
            ((CrimsonScytheHeldProj)proj.ModProjectile).Flip = Main.rand.NextBool();
            return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (DownedBossSystem.downedSunGod)
            {
                bool traditionalMode = false;
                bool isPressingLeftAlt = Main.keyState.PressingAlt();
                int requirements = Math.Max(0, ExecutionProgress);
                string progressText = Mod.GetLocalizationKey("ExecutorDamageClass.ExecutionProgress").ToLangValue().ToFormatValue(requirements);
                string executionText = traditionalMode && isPressingLeftAlt ? Mod.GetLocalizationKey("ExecutorDamageClass.ExecutionDescriptionName").ToLangValue() : progressText;
                Color executionColor = traditionalMode && isPressingLeftAlt ? Color.Lerp(Color.Red, Color.White, .4f) : Color.GreenYellow;

                int executionProgressIndex = tooltips.FindIndex(line => line.Name == "Tooltip0" && line.Mod == "Terraria");
                if (traditionalMode)
                {
                    var executionLine = new TooltipLine(Mod, "ExecutionTooltipName", executionText)
                    {
                        OverrideColor = executionColor
                    };
                    tooltips.Insert(executionProgressIndex, executionLine);
                    if (traditionalMode && isPressingLeftAlt)
                        tooltips.ReplaceAllTooltip(this.GetLocalizationKey("ExecutionStrike"));
                }

                string categoryText = Mod.GetLocalizationKey($"ExecutorDamageClass.WeaponType.{ExecutorWeaponType}").ToLangValue();
                int executionLineIndex = tooltips.FindIndex(line => line.Name == "ExecutionTooltipName" && line.Mod == "LostbeltTraveler") - 1;
                if (!traditionalMode)
                    executionLineIndex = executionProgressIndex - 1;
                var categoryLine = new TooltipLine(Mod, "ExecutorWeaponTypeName", "-" + categoryText + "-")
                {
                    OverrideColor = Color.LightGoldenrodYellow
                };
                tooltips.Insert(executionLineIndex + 1, categoryLine);
            }
            int killerType = tooltips.FindLastIndex(t => t.Name.Contains("Tooltip") && t.Mod == "Terraria");
            int killerIndex = Main.LocalPlayer.Lostbelt().crimsonScytheSlayNPCType;
            string killName = null;
            killName = killerIndex switch
            {
                1 => "OnlyCoins",
                2 => "AllStuff",
                _ => "NoKilling",
            };
            string killerText = this.GetLocalizationKey("KillerType." + killName);
            tooltips.CreateTooltip(killerText, Color.SkyBlue, Mod, "KillerTypeName", killerType + 1);
            CacheTooltipList = tooltips;
            ExModifyTooltips(tooltips);
        }

        public override void ExModifyTooltips(List<TooltipLine> tooltips)
        {
            int flavorTooltipIndex2 = tooltips.FindIndex(line => line.Name == "ItemName" && line.Mod == "Terraria");
            string value = this.GetLocalizedValue("FlavorTooltips").ToLangValue();
            //实例化toolti并注册名字
            TooltipLine flavorTooltips = new TooltipLine(Mod, "FlavorTooltipsName", value);
            //植入Tooltip
            tooltips.Insert(flavorTooltipIndex2 + 1, flavorTooltips);
            if (!DownedBossSystem.downedSunGod)
            {
                string mechanic = Mod.GetLocalizationKey("Weapons.Executor.CrimsonScythe.ConditionMechanic");
                tooltips.ReplaceAllTooltip("");
                tooltips.CreateTooltip(mechanic, Color.White, null, "TlipocaScytheCondition");
            }
        }
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            return true;
        }
        public override void PostDrawTooltipLine(DrawableTooltipLine line)
        {
            if (!DownedBossSystem.downedSunGod)
            {
                ReaperHead.ModifyTooltipLine(line);
                return;
            }
            //记录起始点坐标。
            //通常情况下，物品不可能没有名字，而物品名称通常都在第一行，所以可以用这个来记录第一行的坐标
            if (line.IsItemName())
            {
                TextboxManager.FirstLineY = line.Y;
            }
            var settingList = new List<TextboxSettings>();
            string detailText = this.GetLocalizationKey("ExecutionStrike").ToLangValue();
            int requirements = Math.Max(0, ExecutionProgress);
            int curRequirement = Main.LocalPlayer.Lostbelt().ExecutionListStored.TryGetValue(Type, out int value) ? value : 0;
            string numberText = Mod.GetLocalizationKey("ExecutorDamageClass.ExecutionProgressRevampedMode").ToLangValue().ToFormatValue(curRequirement, requirements);
            detailText += "\n" + "\n" + numberText;
            //一堆设置，巴拉巴拉。
            TextboxSettings sets = new()
            {
                TitleText = Mod.GetLocalizationKey("ExecutorDamageClass.ExecutionDescriptionName").ToLangValue(),
                TitleTextColor = Color.Lerp(Color.Crimson, Color.WhiteSmoke, 1f) with { A = 255 },
                TitleEdgeColor = Color.DarkRed,
                HasTitle = true,
                BackgroundColor = Color.Lerp(Color.WhiteSmoke, Color.Black, .9f) * .60f,
                BackgroundEdgeColor = Color.Lerp(Color.White, Color.Red, 0.0f) * .78f,
                MainText = detailText,
                TextColor = Color.White,
                TextEdgeColor = Color.Black,
                TitleTextSize = 1.15f,
                BoxSize = 1
            };
            settingList.Add(sets);
            //最后传值。
            detailText = this.GetLocalizationKey("ExtraMechanic").ToLangValue();
            sets = new TextboxSettings()
            {
                TitleText = Mod.GetLocalizationKey("ExecutorDamageClass.CompanionWeapon").ToLangValue(),
                TitleTextColor = Color.Lerp(Color.Crimson, Color.WhiteSmoke, 1f) with { A = 255 },
                TitleEdgeColor = Color.DarkRed,
                HasTitle = true,
                BackgroundColor = Color.Lerp(Color.WhiteSmoke, Color.Black, .9f) * .60f,
                BackgroundEdgeColor = Color.Lerp(Color.White, Color.Red, 0.0f) * .78f,
                MainText = detailText,
                TextColor = Color.White,
                TextEdgeColor = Color.Black,
                TitleTextSize = 1.15f,
                BoxSize = 1
            };
            settingList.Add(sets);
            TextboxMethods.DrawMultipleTextboxes(line, CacheTooltipList, settingList, 30);
        }
        public override void HoldItem(Player player)
        {
            player.ApplyNoKnockbackBuff(30);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.DeathSickle).
                AddCondition(LostbeltCraftingConditions.IsDownSlimeGodAndInEclipse).
                AddTile(FinalAnvilTile).
                DisableDecraft().
                Register();
        }
    }
}
