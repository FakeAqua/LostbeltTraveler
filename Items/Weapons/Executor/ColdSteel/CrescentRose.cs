using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Items.Materials;
using LostbeltTraveler.Projs.Executor.ColdSteel;
using LostbeltTraveler.Rarity.RarityDrawHandler;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Executor.ColdSteel
{
    public class CrescentRose : ExecutorWeaponClass
    {
        public override int ExecutionProgress => 40;
        public static int DefensePerAdd = 2;
        public static int MaxSoulStone = 20;
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.ColdSteel;
        public override void ExSSD()
        {
            ScarletItemIDSets.GrantsBoosterAfterSon[Type] = true;
            ScarletItemIDSets.IsHeldProjItem[Type] = true;
        }
        public override bool AllowPrefix(int pre) => false;
        public override void ExSD()
        {
            Item.damage = 1413;
            Item.useTime = Item.useAnimation = 31;
            Item.SetUpNoUseGraphicItem(true);
            Item.SetUpRarityPrice(ItemRarityID.Red);
            Item.Lostbelt().drawBuffIconAndDetail = true;
            Item.Lostbelt().drawFlavorTooltip = true;
            Item.shootSpeed = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5;
            Item.shoot = ProjectileType<CrescentRoseHeldProj>();
        }
        public override bool CanShoot(Player player)
        {
            return !player.HasProj<CrescentRoseHeldProj>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            bool exe = player.GetExecutionSrike();
            Vector2 dir = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, ProjectileType<CrescentRoseHeldProj>(), damage, knockback, player.whoAmI);
            proj.Lostbelt().HasExecutionMechanic = true;
            proj.Lostbelt().ExecutionStrike = exe;
            ((CrescentRoseHeldProj)proj.ModProjectile).BeginTargetRotation = dir.ToRotation();
            ((CrescentRoseHeldProj)proj.ModProjectile).Flip = Main.rand.NextBool();
            return false;
        }
        public override void ExModifyTooltips(List<TooltipLine> tooltips)
        {
        }
        public override void PostDrawTooltipLine(DrawableTooltipLine line)
        {
            if (line.IsItemName())
                DrawItemName(line);
            if (line.Mod == Mod.Name && line.Name == "FlavorTooltipsName")
            {
                DrawFlavorName(line);
            }

            base.PostDrawTooltipLine(line);
        }
        public void DrawFlavorName(DrawableTooltipLine line)
        {
            //先更新需要的背景图
            line.QuickSetupRarityBackground(Color.Red);
            //最后画出渐变需要的效果
            DynamicSpriteFont font = line.Font;
            float xa = 0;
            Color namecolor = Color.Black;
            Color strokeColor = Color.Crimson * .75f;
            Color targetStrokeColor = Color.Crimson * .75f;
            Color targetColor = Color.Lerp(Color.DarkRed, Color.Black, .5f);
            int totalLength = line.Text.Length;
            for (int i = 0; i < totalLength; i++)
            {
                float pro = (i / (float)totalLength);
                string text = line.Text[i].ToString();
                Vector2 size = line.Font.MeasureString(text);
                int cj = (int)(Math.Cos(Main.timeForVisualEffects * 1f / 14 - i * 1) * 15);
                Color color = Color.Lerp(new(namecolor.R + cj, namecolor.G + cj, namecolor.B + cj, namecolor.A),
                    new Color(targetColor.R, targetColor.G + cj, targetColor.B + cj, targetColor.A), EaseInOutQuad(i / (float)totalLength));
                color.R = (byte)(int)Clamp(color.R, 0, 255);
                color.G = (byte)(int)Clamp(color.G, 0, 255);
                color.B = (byte)(int)Clamp(color.B, 0, 255);
                for (int j = 0; j < 8; j++)
                {
                    Color stroke = Color.Lerp(strokeColor, targetStrokeColor, pro);
                    Vector2 textPos = new Vector2(line.X + xa, line.Y) + (TwoPi / 8f * j).ToRotationVector2() * 1.1f;
                    Main.spriteBatch.DrawString(font, text, textPos,
                        stroke, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                }
                Main.spriteBatch.DrawString(font, text, new Vector2(line.X + xa, line.Y), color);
                xa += size.X;
            }
        }

        public void DrawItemName(DrawableTooltipLine line)
        {
            //先更新需要的背景图
            line.QuickSetupRarityBackground(Color.Red);
            //最后画出渐变需要的效果
            DynamicSpriteFont font = line.Font;
            float xa = 0;
            Color namecolor = Color.Crimson;
            Color strokeColor = Color.Black * .75f;
            Color targetStrokeColor = Color.DarkRed * .75f;
            Color targetColor = Color.Red;
            int totalLength = line.Text.Length;
            for (int i = 0; i < totalLength; i++)
            {
                float pro = (i / (float)totalLength);
                string text = line.Text[i].ToString();
                Vector2 size = line.Font.MeasureString(text);
                int cj = (int)(Math.Cos(Main.timeForVisualEffects * 1f / 14 - i * 1) * 15);
                Color color = Color.Lerp(new(namecolor.R + cj, namecolor.G + cj, namecolor.B + cj, namecolor.A),
                    new Color(targetColor.R, targetColor.G + cj, targetColor.B + cj, targetColor.A), EaseInOutQuad(i / (float)totalLength));
                color.R = (byte)(int)Clamp(color.R, 0, 255);
                color.G = (byte)(int)Clamp(color.G, 0, 255);
                color.B = (byte)(int)Clamp(color.B, 0, 255);
                for (int j = 0; j < 8; j++)
                {
                    Color stroke = Color.Lerp(strokeColor, targetStrokeColor, pro);
                    Vector2 textPos = new Vector2(line.X + xa, line.Y) + (TwoPi / 8f * j).ToRotationVector2() * 1.1f;
                    Main.spriteBatch.DrawString(font, text, textPos,
                        stroke, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                }
                Main.spriteBatch.DrawString(font, text, new Vector2(line.X + xa, line.Y), color);
                xa += size.X;
            }
        }
        public override void HoldItem(Player player)
        {
            base.HoldItem(player);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CrimsonScythe>().
                AddIngredient<UniversalCube>(15).
                AddIngredient<CrownofSilveryLight>(15).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
