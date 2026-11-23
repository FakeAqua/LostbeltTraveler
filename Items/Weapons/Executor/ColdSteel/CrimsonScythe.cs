using ContinentOfJourney;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Globals.Methods.SideTextPanel;
using LostbeltTraveler.Globals.Systems.Conditions;
using LostbeltTraveler.Items.Armor.Reaper;
using LostbeltTraveler.Projs.Executor.ColdSteel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Item.Lostbelt().drawFlavorTooltip = true;
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
            ExModifyTooltips(tooltips);
        }
        public override void ExModifyTooltips(List<TooltipLine> tooltips)
        {
            //int flavorTooltipIndex2 = tooltips.FindIndex(line => line.Name == "ItemName" && line.Mod == "Terraria");
            //string value = this.GetLocalizedValue("FlavorTooltips").ToLangValue();
            ////实例化toolti并注册名字
            //TooltipLine flavorTooltips = new TooltipLine(Mod, "FlavorTooltipsName", value);
            ////植入Tooltip
            //tooltips.Insert(flavorTooltipIndex2 + 1, flavorTooltips);
            if (!DownedBossSystem.downedSunGod)
            {
                string mechanic = Mod.GetLocalizationKey("Weapons.Executor.CrimsonScythe.ConditionMechanic");
                tooltips.ReplaceAllTooltip("");
                tooltips.CreateTooltip(mechanic, Color.White, null, "TlipocaScytheCondition");
            }
        }
        public override bool PrePreDrawTooltip(ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y)
        {
            if (!DownedBossSystem.downedSunGod)
                return false;

            string detailText = this.GetLocalizationKey("ExecutionStrike").ToLangValue();
            int requirements = Math.Max(0, ExecutionProgress);
            int curRequirement = Main.LocalPlayer.Lostbelt().ExecutionListStored.TryGetValue(Type, out int value) ? value : 0;
            string numberText = Mod.GetLocalizationKey("ExecutorDamageClass.ExecutionProgressRevampedMode").ToLangValue().ToFormatValue(curRequirement, requirements);
            string titleText = Mod.GetLocalizationKey("ExecutorDamageClass.ExecutionDescriptionName").ToLangValue();
            List<SideTextPanelData> list = [];
            list.Add(new SideTextPanelData(new TooltipLine(Mod, "ExecutionTitleName", titleText), 1));
            string[] textBundle = detailText.Split('\n');
            for (int i = 0; i < textBundle.Length; i++)
            {
                list.Add(new(new(Mod, "ExecutionStrikeTooltip" + i, textBundle[i]), 1));
            }
            list.Add(new SideTextPanelData(new TooltipLine(Mod, "ExecutionNumberName", numberText)));
            titleText = Mod.GetLocalizationKey("ExecutorDamageClass.CompanionWeapon").ToLangValue();
            detailText = this.GetLocalizationKey("ExtraMechanic").ToLangValue();
            List<SideTextPanelData> list2 = [];
            list2.Add(new SideTextPanelData(new TooltipLine(Mod, "CompanionTitle", titleText)));
            string[] textBundle2 = detailText.Split('\n');
            for (int i = 0; i < textBundle2.Length; i++)
            {
                list2.Add(new(new(Mod, "CompanionTooltip" + i, textBundle2[i]), 1));
            }
            IReadOnlyList<SideTextPanelData>[] array = [list, list2];
            if(Item.IsBestPrefix())
            SideTextPanelMethods.DrawMultipleSideTextPanel(lines, array, x, y, Color.DarkRed,bgC:Color.Lerp( Color.DarkRed,Color.Black,.6f),edgeC:Color.Lerp(Color.DarkRed,Color.Crimson,5f),useShinyBorder:true);
            else
            SideTextPanelMethods.DrawMultipleSideTextPanel(lines, array, x, y, Color.DarkRed,edgeC:Color.DarkRed,useShinyBorder:false);
            return false;
        }
        public override void PostDrawTooltipLine(DrawableTooltipLine line)
        {
            if (!DownedBossSystem.downedSunGod)
            {
                ReaperHead.ModifyTooltipLine(line);
                return;
            }
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
