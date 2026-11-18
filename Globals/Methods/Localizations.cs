using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Database.Localization;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Terraria.Localization;
using Terraria.ModLoader;
namespace LostbeltTraveler.Globals.Methods
{
    public static partial class LostbeltMethods
    {
        public static void ReplaceAllTooltip(this List<TooltipLine> tooltips, string replacedTextPath, Color? textColor = null)
        {
            tooltips.RemoveAll((line) => line.Mod == "Terraria" && line.Name != "Tooltip0" && line.Name.StartsWith("Tooltip"));
            TooltipLine getTooltip = tooltips.FirstOrDefault((x) => x.Name == "Tooltip0" && x.Mod == "Terraria");
            string formateText = replacedTextPath.ToLangValue();
            Color overrideColor = textColor ?? Color.White;
            if (getTooltip is not null)
            {
                getTooltip.Text = formateText;
                getTooltip.OverrideColor = overrideColor;
            }
        }
        /// <summary>
        /// 干翻所有Tooltip，并借助本地化完全重写一次，重载染色，附带键入值
        /// </summary>
        /// <param name="tooltips"></param>
        /// <param name="replacedTextPath"></param>
        /// <param name="args"></param>
        public static void ReplaceAllTooltip(this List<TooltipLine> tooltips, string replacedTextPath, Color? textColor = null, params object[] args)
        {
            tooltips.RemoveAll((line) => line.Mod == "Terraria" && line.Name != "Tooltip0" && line.Name.StartsWith("Tooltip"));
            TooltipLine getTooltip = tooltips.FirstOrDefault((x) => x.Name == "Tooltip0" && x.Mod == "Terraria");
            string formateText = replacedTextPath.ToLangValue().ToFormatValue(args);
            Color overrideColor = textColor ?? Color.White;
            if (getTooltip is not null)
            {
                getTooltip.Text = formateText;
                getTooltip.OverrideColor = textColor;
            }
        }
        public static void CreateTooltip(this List<TooltipLine> tooltips, string textPath, Color? color = null, Mod mod = null, string LineName = "Lostbelt", int index = -1)
        {
            string text = textPath.ToLangValue();
            Mod tooltipMod = mod ?? LostbeltTraveler.Instance;
            Color overrideColor = color ?? Color.White;
            var newLine = new TooltipLine(tooltipMod, LineName, text)
            {
                OverrideColor = overrideColor
            };
            int count = tooltips.Count;
            if (count is 0)
                tooltips.Add(newLine);
            else
            {
                if (index != -1)
                    count = index;
                tooltips.Insert(count, newLine);
            }
        }
        public static void CreateTooltip(this List<TooltipLine> tooltips, string textPath, Color? color = null, Mod mod = null, string LineName = "Lostbelt", int index = -1, params object[] args)
        {
            string text = textPath.ToLangValue().ToFormatValue(args);
            Mod tooltipMod = mod ?? LostbeltTraveler.Instance;
            Color overrideColor = color ?? Color.White;
            var newLine = new TooltipLine(tooltipMod, LineName, text)
            {
                OverrideColor = overrideColor
            };
            int count = tooltips.Count;
            if (count is 0)
                tooltips.Add(newLine);
            else
            {
                if (index != -1)
                    count = index;
                tooltips.Insert(count, newLine);
            }
        }
        public static void CreateTooltipDirect(this List<TooltipLine> tooltips, string textPath, Color? color = null, Mod mod = null, string LineName = "Lostbelt", int index = -1)
        {
            string text = textPath;
            Mod tooltipMod = mod ?? LostbeltTraveler.Instance;
            Color overrideColor = color ?? Color.White;
            var newLine = new TooltipLine(tooltipMod, LineName, text)
            {
                OverrideColor = overrideColor
            };
            int count = tooltips.Count;
            if (count is 0)
                tooltips.Add(newLine);
            else
            {
                if (index != -1)
                    count = index;
                tooltips.Insert(count, newLine);
            }
        }

        public static void CreateTooltipDirect(this List<TooltipLine> tooltips, string textValue, Color? color = null, Mod mod = null, string LineName = "Lostbelt", int index = -1, params object[] args)
        {
            string text = textValue.ToFormatValue(args);
            Mod tooltipMod = mod ?? LostbeltTraveler.Instance;
            Color overrideColor = color ?? Color.White;
            var newLine = new TooltipLine(tooltipMod, LineName, text)
            {
                OverrideColor = overrideColor
            };
            int count = tooltips.Count;
            if (count is 0)
                tooltips.Add(newLine);
            else
            {
                if (index != -1)
                    count = index;
                tooltips.Insert(count, newLine);
            }
        }
        public static void CreateHoldShiftRightClickTooltip(this List<TooltipLine> tooltips)
        {
            tooltips.CreateTooltip(ScarletTextSets.GeneralText_HoldShiftRightClick, ScarletTextSets.GeneralText_BuffShowColor, LostbeltTraveler.Instance, "HoldShiftAndRightClickTooltipLine");
        }
        public static void CreateHoldAltTooltip(this List<TooltipLine> tooltips)
        {
            tooltips.CreateTooltip(ScarletTextSets.GeneralText_BuffShow, ScarletTextSets.GeneralText_BuffShowColor, LostbeltTraveler.Instance, "HoldAltTooltipLine");

        }
        public static int FindLineIndex(this List<TooltipLine> tooltips, string lineName, string lineMod = "Terraria") => tooltips.FindIndex(t => t.Name == lineName && t.Mod == lineMod);
        public static int FindLineIndexLast(this List<TooltipLine> tooltips, string lineName, string lineMod = "Terraria") =>
            tooltips.FindLastIndex(t => t.Name.Contains(lineName) && t.Mod == lineMod);
        public static void AddSwapTooltipValueBossCondition(this List<TooltipLine> tooltips, int downedNPCID, Color? color = null)
        {
            string listPath = "Mods.LostbeltTraveler.Database.DownedConditionList";
            string downedConditionPath = ScarletTextSets.GenericText.SwitchCondition;
            string bossValue = string.Empty;
            if (LostbeltList.DownedBossConditionList.TryGetValue(downedNPCID, out string keyValue))
                bossValue = keyValue;
            Color color1 = color ?? Color.Lerp(Color.LawnGreen, Color.LightGreen, 0.5f);
            string downedValue = (listPath + bossValue).ToLangValue();
            string listValue = downedConditionPath.ToFormatValue(downedValue);
            tooltips.CreateTooltipDirect(listValue, color1, LostbeltTraveler.Instance, "SwapConditionName");

        }
        public static string ToLangValue(this string textPath) => Language.GetTextValue(textPath);

        public static string ToFormatValue(this string baseTextValue, params object[] args)
        {
            try
            {
                return string.Format(baseTextValue, args);
            }
            catch
            {
                return baseTextValue + "格式化出错";
            }
        }
        public static bool PressingAlt(this KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.LeftAlt) || keyboardState.IsKeyDown(Keys.RightAlt);
        /// <summary>
        /// 反射缓存的原版构造函数。
        /// <br>编译为委托，所以不需要太过于在乎反射的开销问题</br>
        /// </summary>
        private static readonly Func<string, string, string, TooltipLine> CreateLineFunc = BuildFactory();
        private static Func<string, string, string, TooltipLine> BuildFactory()
        {
            ConstructorInfo ctor = typeof(TooltipLine).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance,
                null, [typeof(string), typeof(string), typeof(string)], null);

            //todo：我不确定未来这里会不会改为public，现在是默认其为internal
            ParameterExpression a = Expression.Parameter(typeof(string), "mod");
            ParameterExpression b = Expression.Parameter(typeof(string), "name");
            ParameterExpression c = Expression.Parameter(typeof(string), "text");
            //返回我们需要的反射构造
            return Expression.Lambda<Func<string, string, string, TooltipLine>>(Expression.New(ctor, a, b, c), a, b, c).Compile();
        }

        /// <summary>
        /// 创建来自于原版内容的工具方法
        /// 这个才是我们会公开的内容
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static TooltipLine CreateVanillaLine(string name, string text) => CreateLineFunc("Terraria", name, text);

    }
}
