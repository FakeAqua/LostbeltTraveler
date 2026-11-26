using ContinentOfJourney.Items;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Globals.Methods.SideTextPanel;
using LostbeltTraveler.Items.Materials;
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
    public class FrostoftheStorm : ExecutorWeaponClass
    {
        public override int ExecutionProgress => 12;
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.ColdSteel;
        public override void ExSSD()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Frost);
            ScarletItemIDSets.GrantsBoosterAfterSon[Type] = true;
            ScarletItemIDSets.IsHeldProjItem[Type] = true;
        }
        public override void ExSD()
        {
            Item.damage = 1276;
            Item.crit = 20;
            Item.SetUpRarityPrice(ItemRarityID.Red);
            Item.SetUpNoUseGraphicItem(true, true);
            Item.Lostbelt().drawBuffIconAndDetail = true;
            Item.useTime = Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = null;
            Item.knockBack = 2f;
            Item.shoot = ProjectileType<FrostoftheStormHeldProj>();
            Item.shootSpeed = 16;
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
        }
        public override bool CanUseItem(Player player)
        {
            return !player.HasProj(Item.shoot) && !player.HasProj<FrostoftheStormExecution>() && !player.HasProj<FrostoftheStormChargeProj>();
        }
        public override void ApplyExecutionTooltips(ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y, params object[] args)
        {
            string detailText = this.GetLocalizationKey("ExecutionStrike").ToLangValue();
            if (args.Length > 0)
            {
                detailText = detailText.ToFormatValue(args);
            }
            int requirements = Math.Max(0, ExecutionProgress);
            int curRequirement = Main.LocalPlayer.Lostbelt().ExecutionListStored.TryGetValue(Type, out int value) ? value : 0;
            string progressText = ExecutorDamageClass.Instance.GetLocalizationKey("Progress").ToLangValue().ToFormatValue(curRequirement, requirements);
            string titleText = ExecutorDamageClass.Instance.GetLocalizationKey("StrikeTitle").ToLangValue();
            Color c = Color.DarkRed;
            TooltipLine titleLine = new TooltipLine(Mod, "StrikeTitle", titleText);
            List<SideTextPanelData> list =
                [
                new SideTextPanelData(titleLine,1,SupportWeaponBuff),
                    ];
            string[] textBundle = detailText.Split('\n');
            for (int i = 0; i < textBundle.Length; i++)
            {
                list.Add(new(new(Mod, "StrikeTooltip" + i, textBundle[i]), 1));
            }
            list.Add(new SideTextPanelData(new TooltipLine(Mod, "ExecutionNumberName", "\n" + progressText)));
            if (Item.IsBestPrefix())
                SideTextPanelMethods.DrawSideTextPanelWithTitleEdge(lines, list, x, y, Color.CornflowerBlue, bgC: Color.DodgerBlue, edgeC: Color.LightBlue, useShinyBorder: true);
            else
                SideTextPanelMethods.DrawSideTextPanelWithTitleEdge(lines, list, x, y, c);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 dir = (player.LocalMouseWorld() - player.Center).SafeNormalize(Vector2.UnitX);
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            proj.Lostbelt().HasExecutionMechanic = true;
            ((FrostoftheStormHeldProj)proj.ModProjectile).BeginTargetRotation = dir.ToRotation();
            ((FrostoftheStormHeldProj)proj.ModProjectile).Flip = true;
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Frostbrand).
                AddIngredient<Frostgrief>().
                AddIngredient<CrownofSilveryLight>(15).
                AddTile(FinalAnvilTile).
                Register();

        }
    }
}
