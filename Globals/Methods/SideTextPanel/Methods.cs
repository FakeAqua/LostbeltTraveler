using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace LostbeltTraveler.Globals.Methods.SideTextPanel
{
    public static partial class SideTextPanelMethods
    {
        /// <summary>
        /// 绘制多个侧边栏，让他们排成一块
        /// <br>这个侧边栏默认带标题，并且可以自定义标题的颜色</br>
        /// </summary>
        /// <param name="lines">原物品的Tooltip</param>
        /// <param name="array">目标侧边栏文本的数组集合</param>
        /// <param name="x">原物品Tooltips的起始X坐标</param>
        /// <param name="y">原物品Tooltips的起始Y坐标</param>
        /// <param name="titleEdgeColor">侧边栏标题文本的描边颜色</param>
        /// <param name="drawBorder">是否给侧边栏画边框</param>
        /// <param name="globalScale">侧边栏的大小</param>
        /// <param name="bgC">侧边栏背景颜色</param>
        /// <param name="edgeC">侧边栏的描边颜色</param>
        /// <param name="titleMainColor">侧边栏标题文本的文本颜色</param>
        /// <param name="extraY">额外的y坐标</param>
        /// <param name="useShinyBorder">是否启用绚丽渐变</param>
        public static void DrawMultipleSideTextPanel(ReadOnlyCollection<TooltipLine> lines, IReadOnlyList<SideTextPanelData>[] array, int x, int y,
            Color titleEdgeColor, bool drawBorder = true, float globalScale = .85f, Color bgC = default, Color edgeC = default, Color titleMainColor = default, int extraY = 0, bool useShinyBorder = false)
        {
            var font = FontAssets.MouseText.Value;
            int widthOffset = 10;
            int heightOffset = 10;
            float lengthX = 0;
            float lerpV = SideTextPanelManager.LerpValue;
            lerpV *= lerpV;
            titleEdgeColor *= lerpV * lerpV;
            float lerpingPosY = (float)Math.Sin(Main.timeForVisualEffects / 60f) * 10;
            y += extraY + (int)lerpingPosY;
            x += (int)Lerp(60, 0, SideTextPanelManager.LerpValue);
            //计算Tooltip框的宽度，用于定位侧边栏相对于Tooltip框的起点X坐标
            foreach (TooltipLine line in lines)
            {
                Vector2 size = ChatManager.GetStringSize(font, line.Text, Vector2.One);
                if (size.X > lengthX)
                    lengthX = size.X;
            }
            //x已经是Tooltip的左起点了。算出最大的长度之后需要再补上30个像素
            x += (int)lengthX + 30;
            //在这里再重新设置字体为我们需要实际绘制的版本
            font = ApplyFont();
            //开始算我们实际的侧边栏文本大小，这里得获取的是这个X的东西。
            lengthX = 0;
            for (int i = 0; i < array.Length; i++)
            {
                float cache = GetSideTextPanelBoxData(array[i], font, globalScale, true).X;
                if (cache > lengthX)
                    lengthX = cache;
            }
            //然后我们再将其依次尝试画出来……
            for (int i = 0; i < array.Length; i++)
            {
                int boxY = (int)GetSideTextPanelBoxData(array[i], font, globalScale, true).Y;
                Rectangle rec = new Rectangle(x, y, (int)lengthX, (int)boxY);
                DrawSideTextPanelBox(rec, drawBorder, bgC, edgeC, widthOffset, heightOffset, useShinyBorder);
                y = DrawSideTextPanelTextWithTitle(array[i], font, x, y, globalScale, lerpV, titleMainColor, titleEdgeColor, lengthX) + 24;
            }
        }
        /// <summary>
        /// 绘制一个侧边栏，这个侧边栏带标题，并且自定义描边，文本也有描边
        ///<br>默认取第一行作为标题文本</br> 
        /// </summary>
        /// <param name="lines">原物品的Tooltip</param>
        /// <param name="x">原物品Tooltips的起始X坐标</param>
        /// <param name="y">原物品Tooltips的起始Y坐标</param>
        /// <param name="titleEdgeColor">侧边栏标题文本的描边颜色</param>
        /// <param name="drawBorder">是否给侧边栏画边框</param>
        /// <param name="globalScale">侧边栏的大小</param>
        /// <param name="bgC">侧边栏背景颜色</param>
        /// <param name="edgeC">侧边栏的描边颜色</param>
        /// <param name="titleMainColor">侧边栏标题文本的文本颜色</param>
        /// <param name="extraY">额外的y坐标</param>
        public static int DrawSideTextEdgePanelWithTitleEdge(ReadOnlyCollection<TooltipLine> lines, IReadOnlyList<SideTextPanelData> list, int x, int y, Color textEdgeColor, Color titleEdgeColor,
            bool drawBorder = true, float globalScale = .85f, Color bgC = default, Color edgeC = default, Color titleMainColor = default, int extraY = 0, bool useShinyBorder = false)
        {
            var font = FontAssets.MouseText.Value;
            int widthOffset = 10;
            int heightOffset = 10;
            float lengthX = 0;
            float lerpV = SideTextPanelManager.LerpValue;
            lerpV *= lerpV;
            titleEdgeColor *= lerpV * lerpV;
            float lerpingPosY = (float)Math.Sin(Main.timeForVisualEffects / 60f) * 10;
            y += extraY + (int)lerpingPosY;
            x += (int)Lerp(60, 0, SideTextPanelManager.LerpValue);
            //计算Tooltip框的宽度，用于定位侧边栏相对于Tooltip框的起点X坐标
            foreach (TooltipLine line in lines)
            {
                Vector2 size = ChatManager.GetStringSize(font, line.Text, Vector2.One);
                if (size.X > lengthX)
                    lengthX = size.X;
            }
            //x已经是Tooltip的左起点了。算出最大的长度之后需要再补上30个像素
            x += (int)lengthX + 30;
            //在这里再重新设置字体为我们需要实际绘制的版本
            font = ApplyFont();
            //开始算我们实际的侧边栏文本的大小
            Vector2 s = GetSideTextPanelBoxData(list, font, globalScale, true);
            lengthX = s.X;
            float lengthY = s.Y;
            DrawSideTextPanelBox(x, y, widthOffset, heightOffset, lengthX, lengthY, drawBorder, bgC * lerpV, edgeC * lerpV, useShinyBorder);
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
                        string lineText = line.Line.Text;
                        Color c = line.Line.OverrideColor ?? Color.White;
                        c *= lerpV * lerpV;
                        //ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, line.Line.Text, new Vector2(x * Lerp(1.025f, 1f, lerpV), y), c, 0, Vector2.Zero, Vector2.One * globalScale * line.Size);
                        //我滴个码
                        for (int j = 0; j < 8; j++)
                            ChatManager.DrawColorCodedString(Main.spriteBatch, font, lineText, new Vector2(x * Lerp(1.025f, 1f, lerpV), y) + (TwoPi / 8f * j).ToRotationVector2() * 1.1f, textEdgeColor, 0, Vector2.Zero, Vector2.One * globalScale * line.Size);
                        ChatManager.DrawColorCodedString(Main.spriteBatch, font, lineText, new Vector2(x * Lerp(1.025f, 1f, lerpV), y), c, 0, Vector2.Zero, Vector2.One * globalScale * line.Size);

                        y += (int)(ChatManager.GetStringSize(font, line.Line.Text, Vector2.One * globalScale * line.Size).Y * globalScale * line.Size);
                    }
                }
            }
            return y - (int)lerpingPosY;
            //y = DrawSideTextPanelTextWithTitle(list, font, x, y, globalScale, lerpV, titleMainColor, titleEdgeColor, lengthX) + 24;
            //return y - (int)lerpingPosY;
        }

        /// <summary>
        /// 绘制一个侧边栏，这个侧边栏带标题，并且自定义描边
        ///<br>默认取第一行作为标题文本</br> 
        /// </summary>
        /// <param name="lines">原物品的Tooltip</param>
        /// <param name="x">原物品Tooltips的起始X坐标</param>
        /// <param name="y">原物品Tooltips的起始Y坐标</param>
        /// <param name="titleEdgeColor">侧边栏标题文本的描边颜色</param>
        /// <param name="drawBorder">是否给侧边栏画边框</param>
        /// <param name="globalScale">侧边栏的大小</param>
        /// <param name="bgC">侧边栏背景颜色</param>
        /// <param name="edgeC">侧边栏的描边颜色</param>
        /// <param name="titleMainColor">侧边栏标题文本的文本颜色</param>
        /// <param name="extraY">额外的y坐标</param>
        public static int DrawSideTextPanelWithTitleEdge(ReadOnlyCollection<TooltipLine> lines, IReadOnlyList<SideTextPanelData> list, int x, int y, Color titleEdgeColor,
            bool drawBorder = true, float globalScale = .85f, Color bgC = default, Color edgeC = default, Color titleMainColor = default, int extraY = 0, bool useShinyBorder = false)
        {
            var font = FontAssets.MouseText.Value;
            int widthOffset = 10;
            int heightOffset = 10;
            float lengthX = 0;
            float lerpV = SideTextPanelManager.LerpValue;
            lerpV *= lerpV;
            titleEdgeColor *= lerpV * lerpV;
            float lerpingPosY = (float)Math.Sin(Main.timeForVisualEffects / 60f) * 10;
            y += extraY + (int)lerpingPosY;
            x += (int)Lerp(60, 0, SideTextPanelManager.LerpValue);
            //计算Tooltip框的宽度，用于定位侧边栏相对于Tooltip框的起点X坐标
            foreach (TooltipLine line in lines)
            {
                Vector2 size = ChatManager.GetStringSize(font, line.Text, Vector2.One);
                if (size.X > lengthX)
                    lengthX = size.X;
            }
            //x已经是Tooltip的左起点了。算出最大的长度之后需要再补上30个像素
            x += (int)lengthX + 30;
            //在这里再重新设置字体为我们需要实际绘制的版本
            font = ApplyFont();
            //开始算我们实际的侧边栏文本的大小
            Vector2 s = GetSideTextPanelBoxData(list, font, globalScale, true);
            lengthX = s.X;
            float lengthY = s.Y;
            DrawSideTextPanelBox(x, y, widthOffset, heightOffset, lengthX, lengthY, drawBorder, bgC * lerpV, edgeC * lerpV, useShinyBorder);
            y = DrawSideTextPanelTextWithTitle(list, font, x, y, globalScale, lerpV, titleMainColor, titleEdgeColor, lengthX) + 24;
            return y - (int)lerpingPosY;
        }
        /// <summary>
        /// 绘制一个侧边栏，这个侧边栏带标题
        ///<br>默认取第一行作为标题文本</br> 
        /// </summary>
        /// <param name="lines">原物品的Tooltip</param>
        /// <param name="list">侧边栏的文本组</param>
        /// <param name="x">原物品Tooltips的起始X坐标</param>
        /// <param name="y">原物品Tooltips的起始Y坐标</param>
        /// <param name="drawBorder">是否给侧边栏画边框</param>
        /// <param name="globalScale">侧边栏的大小</param>
        /// <param name="bgC">侧边栏背景颜色</param>
        /// <param name="edgeC">侧边栏的描边颜色</param>
        /// <param name="extraY">额外的y坐标</param>
        /// <returns>
        /// 返回该侧边栏底部的Y坐标
        /// </returns>
        public static int DrawSideTextPanelWithTitle(ReadOnlyCollection<TooltipLine> lines, IReadOnlyList<SideTextPanelData> list, int x, int y,
            bool drawBorder = true, float globalScale = .85f, Color bgC = default, Color edgeC = default, int extraY = 0, bool useShinyBorder = false)
        {
            var font = FontAssets.MouseText.Value;
            int widthOffset = 10;
            int heightOffset = 10;
            float lengthX = 0;
            float lerpV = SideTextPanelManager.LerpValue;
            lerpV *= lerpV;
            float lerpingPosY = (float)Math.Sin(Main.timeForVisualEffects / 60f) * 10;
            y += extraY + (int)lerpingPosY;
            x += (int)Lerp(60, 0, SideTextPanelManager.LerpValue);
            //计算Tooltip框的宽度，用于定位侧边栏相对于Tooltip框的起点X坐标
            foreach (TooltipLine line in lines)
            {
                Vector2 size = ChatManager.GetStringSize(font, line.Text, Vector2.One);
                if (size.X > lengthX)
                    lengthX = size.X;
            }
            //x已经是Tooltip的左起点了。算出最大的长度之后需要再补上30个像素
            x += (int)lengthX + 30;
            //在这里再重新设置字体为我们需要实际绘制的版本
            font = ApplyFont();
            //开始算我们实际的侧边栏文本的大小
            Vector2 s = GetSideTextPanelBoxData(list, font, globalScale, true);
            lengthX = s.X;
            float lengthY = s.Y;
            //侧边栏是否画背景，跟随原版的系统
            Rectangle rec = new Rectangle(x, y, (int)lengthX, (int)lengthY);
            DrawSideTextPanelBox(rec, drawBorder, bgC, edgeC, widthOffset, heightOffset, useShinyBorder);
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
                        Color c = line.Line.OverrideColor ?? Color.White;
                        string lineText = new("「" + line.Line.Text + "」");
                        ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, lineText, new Vector2(x * Lerp(1.025f, 1f, lerpV), y), c, 0, Vector2.Zero, Vector2.One * globalScale * line.Size * 1.13f);
                        if (line.buffIcon != -1)
                        {
                            Texture2D tex = TextureAssets.Buff[line.buffIcon].Value;
                            Main.spriteBatch.Draw(tex, new Vector2(x * Lerp(1.025f, 1f, lerpV) + lengthX - widthOffset - widthOffset / 2, y + heightOffset + heightOffset / 2), null, Color.White * lerpV, 0, tex.Size() / 2f, globalScale, 0, 0);
                        }

                        y += (int)(ChatManager.GetStringSize(font, lineText, Vector2.One * globalScale * line.Size * 1.13f).Y * globalScale * line.Size * 1.13f * 1.1f);
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
            return y - (int)lerpingPosY + 24;
        }
        /// <summary>
        /// 绘制一个侧边栏
        /// </summary>
        /// <param name="lines">原物品的Tooltip</param>
        /// <param name="list">侧边栏的文本组</param>
        /// <param name="x">原物品Tooltips的起始X坐标</param>
        /// <param name="y">原物品Tooltips的起始Y坐标</param>
        /// <param name="drawBorder">是否给侧边栏画边框</param>
        /// <param name="globalScale">侧边栏的大小</param>
        /// <param name="bgC">侧边栏背景颜色</param>
        /// <param name="edgeC">侧边栏的描边颜色</param>
        /// <param name="extraY">额外的y坐标</param>
        /// <returns>
        /// 返回该侧边栏底部的Y坐标
        /// </returns>

        public static int DrawSideTextPanel(ReadOnlyCollection<TooltipLine> lines, IReadOnlyList<SideTextPanelData> list, int x, int y,
            bool drawBorder = true, float globalScale = .85f, Color bgC = default, Color edgeC = default, int extraY = 0, bool useShinyBorder = false)
        {
            var font = FontAssets.MouseText.Value;
            int widthOffset = 10;
            int heightOffset = 10;
            float lengthX = 0;
            float lerpV = SideTextPanelManager.LerpValue;
            lerpV *= lerpV;
            float lerpingPosY = (float)Math.Sin(Main.timeForVisualEffects / 60f) * 10;
            y += extraY + (int)lerpingPosY;
            x += (int)Lerp(60, 0, SideTextPanelManager.LerpValue);
            //计算Tooltip框的宽度，用于定位侧边栏相对于Tooltip框的起点X坐标
            foreach (TooltipLine line in lines)
            {
                Vector2 size = ChatManager.GetStringSize(font, line.Text, Vector2.One);
                if (size.X > lengthX)
                    lengthX = size.X;
            }
            //x已经是Tooltip的左起点了。算出最大的长度之后需要再补上30个像素
            x += (int)lengthX + 30;
            //在这里再重新设置字体为我们需要实际绘制的版本
            font = ApplyFont();
            //开始算我们实际的侧边栏文本的大小
            Vector2 s = GetSideTextPanelBoxData(list, font, globalScale, true);
            lengthX = s.X;
            float lengthY = s.Y;
            //侧边栏是否画背景，跟随原版的系统
            Rectangle rec = new Rectangle(x, y, (int)lengthX, (int)lengthY);
            DrawSideTextPanelBox(rec, drawBorder, bgC, edgeC, widthOffset, heightOffset, useShinyBorder);
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
                    Color c = line.Line.OverrideColor ?? Color.White;
                    c *= lerpV * lerpV;
                    ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, line.Line.Text, new Vector2(x * Lerp(1.025f, 1f, lerpV), y), c, 0, Vector2.Zero, Vector2.One * globalScale * line.Size);
                    if (i == 0)
                    {
                        if (line.buffIcon != -1)
                        {
                            Texture2D tex = TextureAssets.Buff[line.buffIcon].Value;
                            Main.spriteBatch.Draw(tex, new Vector2(x * Lerp(1.025f, 1f, lerpV) + lengthX - 10 - 10 / 2, y + 10 + 10 / 2), null, Color.White * lerpV, 0, tex.Size() / 2f, globalScale, 0, 0);
                        }
                    }


                    y += (int)(ChatManager.GetStringSize(font, line.Line.Text, Vector2.One * globalScale * line.Size).Y * globalScale * line.Size);
                }
            }
            return y - (int)lerpingPosY + 24;
        }
        /// <summary>
        /// 绘制一个侧边栏
        /// <br>这个会自定义内容的描边颜色</br>
        /// </summary>
        /// <param name="lines">原物品的Tooltip</param>
        /// <param name="list">侧边栏的文本组</param>
        /// <param name="x">原物品Tooltips的起始X坐标</param>
        /// <param name="y">原物品Tooltips的起始Y坐标</param>
        /// <param name="edgeTextColor">侧边栏文本的描边颜色</param>
        /// <param name="drawBorder">是否给侧边栏画边框</param>
        /// <param name="globalScale">侧边栏的大小</param>
        /// <param name="bgC">侧边栏背景颜色</param>
        /// <param name="edgeC">侧边栏的描边颜色</param>
        /// <param name="extraY">额外的y坐标</param>
        /// <returns>
        /// 返回该侧边栏底部的Y坐标
        /// </returns>

        public static int DrawSideTextEdgePanel(ReadOnlyCollection<TooltipLine> lines, IReadOnlyList<SideTextPanelData> list, int x, int y, Color edgeTextColor,
            bool drawBorder = true, float globalScale = .85f, Color bgC = default, Color edgeC = default, int extraY = 0, bool useShinyBorder = false)
        {
            var font = FontAssets.MouseText.Value;
            int widthOffset = 10;
            int heightOffset = 10;
            float lengthX = 0;
            float lerpV = SideTextPanelManager.LerpValue;
            lerpV *= lerpV;
            float lerpingPosY = (float)Math.Sin(Main.timeForVisualEffects / 60f) * 10;
            y += extraY + (int)lerpingPosY;
            x += (int)Lerp(100, 0, SideTextPanelManager.LerpValue);
            //计算Tooltip框的宽度，用于定位侧边栏相对于Tooltip框的起点X坐标
            foreach (TooltipLine line in lines)
            {
                Vector2 size = ChatManager.GetStringSize(font, line.Text, Vector2.One);
                if (size.X > lengthX)
                    lengthX = size.X;
            }
            //x已经是Tooltip的左起点了。算出最大的长度之后需要再补上30个像素
            x += (int)lengthX + 30;
            //在这里再重新设置字体为我们需要实际绘制的版本
            font = ApplyFont();
            //开始算我们实际的侧边栏文本的大小
            Vector2 s = GetSideTextPanelBoxData(list, font, globalScale, true);
            lengthX = s.X;
            float lengthY = s.Y;
            //侧边栏是否画背景，跟随原版的系统
            Rectangle rec = new Rectangle(x, y, (int)lengthX, (int)lengthY);
            DrawSideTextPanelBox(rec, drawBorder, bgC, edgeC, widthOffset, heightOffset, useShinyBorder);
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
                        if (line.buffIcon != -1)
                        {
                            Texture2D tex = TextureAssets.Buff[line.buffIcon].Value;
                            Main.spriteBatch.Draw(tex, new Vector2(x * Lerp(1.025f, 1f, lerpV) + lengthX - 10 - 10 / 2, y + 10 + 10 / 2), null, Color.White * lerpV, 0, tex.Size() / 2f, globalScale, 0, 0);
                        }
                    }
                    Color c = line.Line.OverrideColor ?? Color.White;
                    c *= lerpV * lerpV;
                    string lineText = line.Line.Text;
                    for (int j = 0; j < 8; j++)
                        ChatManager.DrawColorCodedString(Main.spriteBatch, font, lineText, new Vector2(x * Lerp(1.025f, 1f, lerpV), y) + (TwoPi / 8f * j).ToRotationVector2() * 1.1f, edgeTextColor, 0, Vector2.Zero, Vector2.One * globalScale * line.Size);
                    ChatManager.DrawColorCodedString(Main.spriteBatch, font, lineText, new Vector2(x * Lerp(1.025f, 1f, lerpV), y), c, 0, Vector2.Zero, Vector2.One * globalScale * line.Size);
                    y += (int)(ChatManager.GetStringSize(font, lineText, Vector2.One * globalScale * line.Size).Y * line.Size * globalScale);
                }
            }
            return y - (int)lerpingPosY + 24;
        }

        public static void DrawPanelBackground(SpriteBatch sb, Texture2D panelTex, Rectangle rec, Color c = default)
        {
            DrawPanelBackground(sb, panelTex, rec.X, rec.Y, rec.Width, rec.Height, c);
        }
        /// <summary>
        /// 绘制侧边栏的背景
        /// <br>这里的码是固定死的，用的是52x52的图，操作起来也是这样</br>
        /// <br>如果需要其他的侧边栏背景操作，自己根据这里的码去处理</br>
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="panelTex"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="c"></param>
        public static void DrawPanelBackground(SpriteBatch sb, Texture2D panelTex, int x, int y, int w, int h, Color c = default)
        {
            Texture2D value = panelTex;
            if (c == default)
            {
                c = Color.Black * .35f;
            }
            int cutWidth = 10;
            int cutHeight = 10;

            if (w < cutWidth * 2)
                w = cutWidth * 2;

            if (h < cutHeight * 2)
                h = cutHeight * 2;
            //左上的切割
            Rectangle topLeftRec = new Rectangle(x, y, cutWidth, cutHeight);
            Rectangle topLeftSrc = new Rectangle(0, 0, cutWidth, cutHeight);
            sb.Draw(value, topLeftRec, topLeftSrc, c);
            //中上的切割
            Rectangle topMidRec = new Rectangle(x + cutWidth, y, w - cutWidth * 2, cutHeight);
            Rectangle topMidSrc = new Rectangle(cutWidth, 0, value.Width - cutWidth * 2, cutHeight);
            sb.Draw(value, topMidRec, topMidSrc, c);
            //右上的切割
            Rectangle topRightRec = new Rectangle(x + w - cutWidth, y, cutWidth, cutHeight);
            Rectangle topRightSrc = new Rectangle(value.Width - cutWidth, 0, cutWidth, cutHeight);
            sb.Draw(value, topRightRec, topRightSrc, c);
            //中左的切割
            Rectangle midLeftRec = new Rectangle(x, y + cutHeight, cutWidth, h - cutHeight * 2);
            Rectangle midLeftSrc = new Rectangle(0, cutHeight, cutWidth, value.Height - cutHeight * 2);
            sb.Draw(value, midLeftRec, midLeftSrc, c);
            //中间的切割
            Rectangle centerRec = new Rectangle(x + cutWidth, y + cutHeight, w - cutWidth * 2, h - cutHeight * 2);
            Rectangle centerSrc = new Rectangle(cutWidth, cutHeight, value.Width - cutWidth * 2, value.Height - cutHeight * 2);
            sb.Draw(value, centerRec, centerSrc, c);

            //中右的切割
            Rectangle midRightRec = new Rectangle(x + w - cutWidth, y + cutHeight, cutWidth, h - cutHeight * 2);
            Rectangle midRightSrc = new Rectangle(value.Width - cutWidth, cutHeight, cutWidth, value.Height - cutHeight * 2);
            sb.Draw(value, midRightRec, midRightSrc, c);

            //左下的切割
            Rectangle bottomLeftRec = new Rectangle(x, y + h - cutHeight, cutWidth, cutHeight);
            Rectangle bottomLeftSrc = new Rectangle(0, value.Height - cutHeight, cutWidth, cutHeight);
            sb.Draw(value, bottomLeftRec, bottomLeftSrc, c);

            //中下的切割
            Rectangle bottomMidRec = new Rectangle(x + cutWidth, y + h - cutHeight, w - cutWidth * 2, cutHeight);
            Rectangle bottomMidSrc = new Rectangle(cutWidth, value.Height - cutHeight, value.Width - cutWidth * 2, cutHeight);
            sb.Draw(value, bottomMidRec, bottomMidSrc, c);

            //右下的切割
            Rectangle bottomRightRec = new Rectangle(x + w - cutWidth, y + h - cutHeight, cutWidth, cutHeight);
            Rectangle bottomRightSrc = new Rectangle(value.Width - cutWidth, value.Height - cutHeight, cutWidth, cutHeight);
            sb.Draw(value, bottomRightRec, bottomRightSrc, c);
        }
    }
}