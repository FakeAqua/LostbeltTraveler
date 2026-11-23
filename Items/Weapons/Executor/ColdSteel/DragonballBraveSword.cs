using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Globals.Methods.SideTextPanel;
using LostbeltTraveler.Projs.Executor.ColdSteel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Executor.ColdSteel
{
    public class DragonBallSword : ExecutorWeaponClass
    {
        public bool ZSword = false;
        public override int ExecutionProgress => 30;
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.ColdSteel;
        public override void ExSSD()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Donator);
            ScarletItemIDSets.ForceToCustomExecute[Type] = true;
            ScarletItemIDSets.IsHeldProjItem[Type] = true;
        }
        public override void ExSD()
        {
            Item.damage = 546;
            Item.useTime = Item.useAnimation = 40;
            Item.knockBack = 4;
            Item.shootSpeed = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.SetUpRarityPrice(ItemRarityID.Lime);
            Item.SetUpNoUseGraphicItem(true);
            Item.Lostbelt().OwnerName = "林为简";
            Item.Lostbelt().ItemBelongTo = EnumItemOwner.Donator;
            Item.Lostbelt().ExecutionProj = ProjectileType<DragonBallSwordHeldProjX>();
            Item.shoot = ProjectileType<DragonBallSwordHeldProj>();
            Item.shootSpeed = 16;
        }
        public override bool CanShoot(Player player)
        {
            return !player.HasProj(Item.shoot) && !player.HasProj(Item.Lostbelt().ExecutionProj);
        }
        public override void UpdateInventory(Player player)
        {
            bool swordTex = ZSword || Main.keyState.PressingAlt();
            TextureAssets.Item[Type] = swordTex ? Request<Texture2D>(Texture + "Z") : Request<Texture2D>(Texture);
            string v = swordTex ? this.GetLocalizedValue("DisplayNameAlt") : this.GetLocalizedValue("DisplayName");
            Item.SetNameOverride(v);

        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (!BlockTextboxDetail())
            {
                int executionProgressIndex = tooltips.FindIndex(line => line.Name == "Tooltip0" && line.Mod == "Terraria");
                string categoryText = Mod.GetLocalizationKey($"ExecutorDamageClass.WeaponType.{ExecutorWeaponType}").ToLangValue();
                int executionLineIndex = tooltips.FindIndex(line => line.Name == "ExecutionTooltipName" && line.Mod == "LostbeltTraveler") - 1;
                executionLineIndex = executionProgressIndex - 1;
                var categoryLine = new TooltipLine(Mod, "ExecutorWeaponTypeName", $"-{categoryText}-")
                {
                    OverrideColor = Color.LightGoldenrodYellow
                };
                tooltips.Insert(executionLineIndex + 1, categoryLine);
                if (ZSword || Main.keyState.PressingAlt())
                {
                    string zSwordText = this.GetLocalizationKey("TooltipAlt");
                    tooltips.ReplaceAllTooltip(zSwordText, Color.White);
                }
            }
            tooltips.CreateHoldAltTooltip();
            ExModifyTooltips(tooltips);
        }
        public override void ExModifyTooltips(List<TooltipLine> tooltips)
        {
            int flavorTooltipIndex2 = tooltips.FindIndex(line => line.Name == "ItemName" && line.Mod == "Terraria");
            string value = this.GetLocalizedValue("FlavorTooltip").ToLangValue();
            if (ZSword || Main.keyState.PressingAlt())
                value = this.GetLocalizedValue("FlavorTooltipAlt").ToLangValue();
            //实例化toolti并注册名字
            TooltipLine flavorTooltips = new(Mod, "FlavorTooltipsName", "「" + value + "」")

            {
                OverrideColor = Color.Lerp(Color.MediumPurple, Color.LightPink, 0.3f)
            };
            //植入Tooltip
            tooltips.Insert(flavorTooltipIndex2 + 1, flavorTooltips);
        }
        public override bool PreDrawTooltip(ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y)
        {
            string detailText = this.GetLocalizationKey("ExecutionStrike").ToLangValue();
            if (ZSword || Main.keyState.PressingAlt())
                detailText = this.GetLocalizationKey("ExecutionStrikeAlt").ToLangValue();
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
            list.Add(new SideTextPanelData(new TooltipLine(Mod, "ExecutionNumberName", "\n" + numberText)));
            int y2 = SideTextPanelMethods.DrawSideTextPanelWithTitleEdge(lines, list, x, y, Color.DarkRed);
            return base.PreDrawTooltip(lines, ref x, ref y);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!ZSword)
            {
                bool exe = player.GetExecutionSrike();
                Vector2 dir = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);
                type = Item.Lostbelt().ExecutionProj;
                Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
                proj.Lostbelt().HasExecutionMechanic = true;
                proj.Lostbelt().ExecutionStrike = exe;
                ((DragonBallSwordHeldProjX)proj.ModProjectile).BeginTargetRotation = dir.ToRotation();
                ((DragonBallSwordHeldProjX)proj.ModProjectile).Flip = Main.rand.NextBool();

            }
            else
            {
                bool exe = player.GetExecutionSrike();
                Vector2 dir = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);
                Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
                proj.Lostbelt().HasExecutionMechanic = true;
                proj.Lostbelt().ExecutionStrike = exe;
                ((DragonBallSwordHeldProj)proj.ModProjectile).BeginTargetRotation = dir.ToRotation();
                ((DragonBallSwordHeldProj)proj.ModProjectile).Flip = Main.rand.NextBool();
            }
            return false;
        }
        public override void HoldItem(Player player)
        {
            player.Lostbelt().tacticalExecution = !player.Lostbelt().theDragonBallSwordMode;
        }
    }
}
