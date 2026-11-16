using LostbeltTraveler.Rarity.RarityDrawHandler;
using LostbeltTraveler.Rarity.RarityParticles;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace LostbeltTraveler.Rarity.RarityShinyMethod
{
    /// <summary>
    /// 物质重构
    /// </summary>
    public static class StardustFancyText
    {
        public static void DrawItemName(DrawableTooltipLine line)
        {
            //先更新需要的背景图
            line.QuickSetupRarityBackground(Color.RoyalBlue);
            //最后画出渐变需要的效果
            DynamicSpriteFont font = line.Font;
            float xa = 0;
            Color namecolor = Color.DodgerBlue;
            Color strokeColor = Color.SkyBlue* .75f;
            Color targetStrokeColor = Color.White* .75f;
            Color targetColor = Color.RoyalBlue;
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
                        stroke.ToAddColor(), 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                }
                Main.spriteBatch.DrawString(font, text, new Vector2(line.X + xa, line.Y), color);
                xa += size.X;
            }
        }
        public static void UpdateItemNameParticle(DrawableTooltipLine line, ref List<RaritySparkle> raritySparkles)
        {
            //在这里手动创建新的粒子，然后我们再将其添加进需要的表单内
            Vector2 textSize = line.Font.MeasureString(line.Text);
            if (Main.rand.NextBool(14))
            {
                float scale = Main.rand.NextFloat(0.30f * 0.5f, 0.30f) * 1.2f;
                int lifetime = 160;
                Vector2 position = Main.rand.NextVector2FromRectangle(new(-(int)(textSize.X * 0.5f), -(int)(textSize.Y * 0.5f), (int)textSize.X, (int)(textSize.Y * 0.35f)));
                Vector2 velocity = Vector2.UnitX * Main.rand.NextFloat(0.25f, 0.35f) * .5f;
                RarityShinyOrb rarityShinyOrb = new RarityShinyOrb(position, velocity, RandLerpColor(Color.RoyalBlue, Color.White).ToAddColor(), lifetime, scale);
                RarityShinyOrb rarityShinyOrb2 = new RarityShinyOrb(position, velocity, Color.White.ToAddColor(), lifetime, scale * 0.35f);
                raritySparkles.Add(rarityShinyOrb2);
                raritySparkles.Add(rarityShinyOrb);
            }
        }
    }
}
