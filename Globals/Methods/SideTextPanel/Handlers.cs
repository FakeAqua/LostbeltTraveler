using LostbeltTraveler.Globals.Configs;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace LostbeltTraveler.Globals.Methods.SideTextPanel
{
    public static partial class SideTextPanelMethods
    {
        public static DynamicSpriteFont ApplyFont()
        {
            if (LostbeltConfigClient.Instance.UseDefaultFont)
                return FontAssets.MouseText.Value;
            if (Language.ActiveCulture == GameCulture.FromCultureName(GameCulture.CultureName.Chinese))
                return LostbeltModAssets.Font_圆体.Value;
            if (Language.ActiveCulture == GameCulture.FromCultureName(GameCulture.CultureName.English))
                return LostbeltModAssets.Font_Cabin.Value;
            return FontAssets.MouseText.Value;
        }
        public static int DrawSideTextPanelTextWithTitle(IReadOnlyList<SideTextPanelData> list, DynamicSpriteFont font, int x, int y, float globalScale = .85f, float lerpV = 1f, Color titleMainColor = default, Color titleEdgeColor = default, float lengthX = 0)
        {
            //最后我们开始画tooltip
            for (int i = 0; i < list.Count; i++)
            {
                SideTextPanelData line = list[i];
                //空行的情况下增加行数，这里的行数会直接读取对应的空行Text的数字
                if (line.Line.Name == "EmptyString" + i)
                {
                    int num = Convert.ToInt32(line.Line.Text);
                    y += num;
                }
                else
                {
                    if (i == 0)
                    {
                        string lineText = new("「" + line.Line.Text + "」");
                        if (titleMainColor == default)
                            titleMainColor = line.Line.OverrideColor ?? Color.White;
                        titleMainColor *= lerpV * lerpV;
                        //我滴个码
                        for (int j = 0; j < 8; j++)
                            ChatManager.DrawColorCodedString(Main.spriteBatch, font, lineText, new Vector2(x * Lerp(1.025f, 1f, lerpV), y) + (TwoPi / 8f * j).ToRotationVector2() * 1.1f, titleEdgeColor, 0, Vector2.Zero, Vector2.One * globalScale * line.Size * 1.13f);
                        ChatManager.DrawColorCodedString(Main.spriteBatch, font, lineText, new Vector2(x * Lerp(1.025f, 1f, lerpV), y), titleMainColor, 0, Vector2.Zero, Vector2.One * globalScale * line.Size * 1.13f);
                        if (line.buffIcon != -1)
                        {
                            Texture2D tex = TextureAssets.Buff[line.buffIcon].Value;
                            Main.spriteBatch.Draw(tex, new Vector2(x * Lerp(1.025f, 1f, lerpV) + lengthX - 10 - 10 / 2, y + 10 + 10 / 2), null, Color.White * lerpV, 0, tex.Size() / 2f, globalScale, 0, 0);
                        }
                        y += (int)(ChatManager.GetStringSize(font, lineText, Vector2.One * globalScale * line.Size * 1.13f).Y * line.Size * globalScale * 1.13f * 1.21f);
                    }
                    else
                    {
                        Color c = line.Line.OverrideColor ?? Color.White;
                        c *= lerpV * lerpV;
                        ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, line.Line.Text, new Vector2(x * Lerp(1.025f, 1f, lerpV), y), c, 0, Vector2.Zero, Vector2.One * globalScale * line.Size);
                        y += (int)(ChatManager.GetStringSize(font, line.Line.Text, Vector2.One * globalScale * line.Size).Y * globalScale * line.Size);
                    }
                }
            }
            return y;
        }
        public static void DrawSideTextPanelBox(Rectangle rec, bool drawBorder = true, Color bgC = default, Color edgeC = default, int widthOffset = 10, int heightOffset = 10,bool useShinyBorder = false)
        {
            DrawSideTextPanelBox(rec.X, rec.Y, widthOffset, heightOffset, rec.Width, rec.Height, drawBorder, bgC, edgeC,useShinyBorder);
        }
        public static void DrawSideTextPanelBox(int x, int y, int widthOffset, int heightOffset, float lengthX,
            float lengthY, bool drawBorder = true, Color bgC = default, Color edgeC = default, bool useShinyBorder = false)
        {
            //侧边栏是否画背景，跟随原版的系统
            bool useBackground = Main.SettingsEnabled_OpaqueBoxBehindTooltips;
            if (!useBackground)
                return;
            //原版居然有一个直接画背景的方法吗？那我累死累活搞的一个伪背景是为了什么？
            Rectangle rec = new Rectangle(
                x - widthOffset,
                y - heightOffset,
                (int)lengthX + widthOffset * 2,
                (int)lengthY + heightOffset + heightOffset / 2);

            if (bgC == default)
                bgC = Color.Black * .5f;
            if (useShinyBorder)
            {
                Main.spriteBatch.EnterHudArea(BlendState.AlphaBlend);
                Color c = bgC;
                bgC = Color.White;
                LostbeltMethods.ApplyAlphaCut(new Vector4(0, 0, 0, 1.5f), Vector2.Zero, new Vector2(1), c);
                DrawPanelBackground(Main.spriteBatch, LostbeltModAssets.Hud_GlowNoEdge.Value, rec, bgC);
                LostbeltMethods.ApplyAlphaCut(new Vector4(0, 0, .8f, 0f), Vector2.Zero, new Vector2(1), Color.Black);
                DrawPanelBackground(Main.spriteBatch, LostbeltModAssets.Hud_GlowNoEdge.Value, rec, bgC * .35f);
                Main.spriteBatch.EndHudArea();

                if (drawBorder)
                {
                    if (edgeC == default)
                        edgeC = Color.White;
                    DrawPanelBackground(Main.spriteBatch, LostbeltModAssets.Hud_GlowEdge.Value, rec, edgeC);
                }
            }
            else
            {
                if (drawBorder)
                {
                    if (edgeC == default)
                        edgeC = Color.White;
                    DrawPanelBackground(Main.spriteBatch, LostbeltModAssets.Hud_GlowEdgeAlt.Value, rec, edgeC);
                }
                DrawPanelBackground(Main.spriteBatch, LostbeltModAssets.Hud_GlowNoEdgeAlt.Value, rec, bgC);
            }
        }
        /// <summary>
        /// 获取这个侧边栏的背景长度与宽度
        /// </summary>
        /// <param name="list"></param>
        /// <param name="font"></param>
        /// <param name="globalScale"></param>
        /// <returns>
        /// 返回一个<see cref="Vector2"/>，<see cref="Vector2.X"/>存储了侧边栏的宽，<see cref="Vector2.Y"/>存储了侧边栏的高
        /// </returns>
        public static Vector2 GetSideTextPanelBoxData(IReadOnlyList<SideTextPanelData> list, DynamicSpriteFont font, float globalScale = .85f, bool hasTitle = false)
        {
            float lengthY = 0;
            float lengthX = 0;
            for (int i = 0; i < list.Count; i++)
            {
                SideTextPanelData line = list[i];
                //空行的情况下增加行数，这里的行数会直接读取对应的空行Text的数字
                if (line.Line.Name == "EmptyString" + i)
                {
                    int num = Convert.ToInt32(line.Line.Text);
                    lengthY += num;
                }
                else
                {
                    if (i == 0)
                    {
                        if (hasTitle)
                        {
                            string lineText = new("「" + line.Line.Text + "」");
                            var size = ChatManager.GetStringSize(font, lineText, Vector2.One * globalScale * line.Size * 1.13f);
                            if (size.X > lengthX)
                                lengthX = size.X;
                            lengthY += size.Y * line.Size * 1.13f * globalScale * 1.21f;
                        }
                    }
                    else
                    {
                        string lineText = line.Line.Text;
                        var size = ChatManager.GetStringSize(font, lineText, Vector2.One * globalScale * line.Size);
                        if (size.X > lengthX)
                            lengthX = size.X;
                        lengthY += size.Y * line.Size * globalScale;
                    }
                }
            }
            return new Vector2(lengthX, lengthY);
        }
    }
}
