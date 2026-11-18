using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Globals.Methods.Textbox;
using LostbeltTraveler.Projs.Executor.ColdSteel;
using System;
using System.Collections.Generic;
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
            CacheTooltipList = tooltips;
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
        public override void PostDrawTooltipLine(DrawableTooltipLine line)
        {
            if (!BlockTextboxDetail())
            {
                //记录起始点坐标。
                //通常情况下，物品不可能没有名字，而物品名称通常都在第一行，所以可以用这个来记录第一行的坐标
                if (line.IsItemName())
                {
                    TextboxManager.FirstLineY = line.Y;
                }
                string detailText = this.GetLocalizationKey("ExecutionStrike").ToLangValue();
                if (ZSword || Main.keyState.PressingAlt())
                    detailText = this.GetLocalizationKey("ExecutionStrikeAlt").ToLangValue();
                int requirements = Math.Max(0, ExecutionProgress);
                int curRequirement = Main.LocalPlayer.Lostbelt().ExecutionListStored.TryGetValue(Type, out int value) ? value : 0;
                string numberText = Mod.GetLocalizationKey("ExecutorDamageClass.ExecutionProgressRevampedMode").ToLangValue().ToFormatValue(curRequirement, requirements);
                detailText += "\n" + "\n" + numberText;
                //一堆设置，巴拉巴拉。
                TextboxSettings sets = new TextboxSettings()
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
                    BoxSize = TextBoxSize
                };
                //最后传值。
                TextboxMethods.DrawTextboxTooltipWithBackground(line, CacheTooltipList, ref sets);
            }
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
