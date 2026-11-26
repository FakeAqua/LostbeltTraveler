using LostbeltTraveler.Globals.Configs;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace LostbeltTraveler.Core.ScreenEffect
{
    public class ScreenDarknessInfo(float darkStrength, int inTime, int holdTime, int outTime, int whoAmI = -1, Func<float, float> fadeInFunc = null, Func<float, float> fadeOutFunc = null)
    {
        public int WhoAmI = whoAmI;
        /// <summary>
        /// 屏幕暗化强度
        /// </summary>
        public float DarkStrength = darkStrength;
        /// <summary>
        /// 屏幕暗化渐入时间
        /// </summary>
        public int InTime = inTime;
        /// <summary>
        /// 屏幕暗化淡出时间
        /// </summary>
        public int OutTime = outTime;
        /// <summary>
        /// 屏幕暗化持续时间
        /// </summary>
        public int HoldTime = holdTime;
        /// <summary>
        /// 屏幕暗化总时间
        /// </summary>
        public float TotalDarkTime = inTime + outTime + holdTime;
        public float DarkTimer = 0;
        /// <summary>
        /// 屏幕暗化的渐入曲线
        /// <br>默认<see cref="EaseOutCubic(float)"/></br>
        /// </summary>
        public Func<float, float> FadeInFunc = fadeInFunc ?? EaseOutCubic;
        /// <summary>
        /// 屏幕暗化的淡出曲线
        /// <br>默认<see cref="EaseOutCubic(float)"/></br>
        /// </summary>
        public Func<float, float> FadeOutFunc = fadeOutFunc ?? EaseOutCubic;
        /// <summary>
        /// 维持暗化的条件，返回 <see langword="true"/> 时暗化会停留在保持阶段，不会淡出。
        /// <br>为 <see langword="null"/> 时表示没有自动维持条件。</br>
        /// </summary>
        public Func<bool> HoldCondition = null;

        /// <summary>
        /// 强制维持暗化，优先于 <see cref="HoldCondition"/>。
        /// <br>可由外部随时切换，用于手动控制维持状态。</br>
        /// </summary>
        public bool ForceHold = false;

        /// <summary>
        /// 当前是否处于维持状态。
        /// </summary>
        public bool IsHolding => ForceHold || (HoldCondition?.Invoke() ?? false);
        public bool IsDone => DarkTimer >= TotalDarkTime;
        public float Update()
        {
            if (IsHolding)
            {
                //渐入阶段仍然正常推进
                if (DarkTimer < InTime + HoldTime)
                    DarkTimer++;

                //如果之前已经进入淡出阶段，把计时器拉回保持阶段末尾
                if (DarkTimer > InTime + HoldTime)
                    DarkTimer = InTime + HoldTime;

                if (InTime > 0 && DarkTimer <= InTime)
                    return FadeInFunc((float)DarkTimer / InTime);
                return 1f;
            }
            DarkTimer++;
            if (DarkTimer <= InTime)
            {
                float t = (float)DarkTimer / InTime;
                return FadeInFunc(t);
            }
            else if (DarkTimer <= InTime + HoldTime)
            {
                return 1f;
            }
            else if (DarkTimer <= TotalDarkTime)
            {
                float t = (float)(DarkTimer - InTime - HoldTime) / OutTime;
                return (1f - FadeOutFunc(t));
            }
            return 0f;
        }
    }
    public class ScreenDarknessSystem : ModSystem
    {
        public static readonly List<ScreenDarknessInfo> ActiveDarkness = [];
        public static int HoldoutDarknessIndex = -1;
        public static float CurDarkRatios = 0;
        public override void OnWorldUnload()
        {
            ClearWorld();
        }
        public override void OnWorldLoad()
        {
            ClearWorld();
        }
        public override void PostUpdatePlayers()
        {
            if (Main.dedServ)
                return;


            if (ActiveDarkness.Count < 1)
                return;
            if (LostbeltConfigClient.Instance.ScreenDarkStrength == 0)
                return;
            float darkRatio = 0f;
            for (int i = ActiveDarkness.Count - 1; i >= 0; i--)
            {
                ScreenDarknessInfo info = ActiveDarkness[i];
                if (info.WhoAmI != Main.myPlayer)
                    continue;
                float ratio = info.Update() * info.DarkStrength;
                //多个暗化效果取当前的最大值
                if (ratio > darkRatio)
                    darkRatio = ratio;
                if (info.IsDone)
                    ActiveDarkness.RemoveAt(i);
            }
            CurDarkRatios = darkRatio;
        }
        public static void DrawScreenDarkness(On_Main.orig_DrawBackground orig, Main self)
        {
            orig(self);
            if (Main.dedServ)
                return;

            if (LostbeltConfigClient.Instance.ScreenDarkStrength == 0)
                return;
            if (ActiveDarkness.Count < 1)
                return;
            float darkRatio = CurDarkRatios;
            //将工作交给gpu
            Main.tileBatch.End();
            Main.tileBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            LostbeltMethods.ApplyAlphaCut(new Vector4(0) * (1-darkRatio), Vector2.Zero, Vector2.One, Color.Black);
            //最后画出来
            Vector2 pos = new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f);
            Rectangle rec = Utils.CenteredRectangle(pos, new Vector2(Main.screenWidth, Main.screenHeight));
            Texture2D tex = LostbeltModAssets.Texture_WhiteCube.Value;
            darkRatio *= LostbeltConfigClient.Instance.ScreenDarkStrength;
            Main.spriteBatch.Draw(tex, pos, rec, Color.Lerp(Color.DarkGray, Color.Black, 0.95f) with { A = 250 } * darkRatio, 0, tex.Size() / 2, new Vector2(Main.screenWidth/2, Main.screenHeight/2), 0, 0);
            Main.tileBatch.End();
            Main.tileBatch.Begin();

        }
        /// <summary>
        /// 强制设置某个暗化效果的维持状态。
        /// </summary>
        public static void HoldScreenDarkness(ScreenDarknessInfo info, bool hold)
        {
            if (info != null)
                info.ForceHold = hold;
        }

        /// <summary>
        /// 清除全部暗化效果
        /// </summary>
        public override void ClearWorld()
        {
            ActiveDarkness.Clear();
        }
        /// <summary>
        /// 添加一个屏幕暗化效果，返回其信息对象以便外部后续控制
        /// </summary>
        public static ScreenDarknessInfo AddScreenDarkness(float maxStrength, int inTime, int holdTime, int outTime,Player player, Func<float, float> easeIn = null, Func<float, float> easeOut = null, Func<bool> holdCondition = null)
        {
            var info = new ScreenDarknessInfo(maxStrength, inTime, holdTime, outTime, player.whoAmI, easeIn, easeOut)
            {
                HoldCondition = holdCondition
            };
            ActiveDarkness.Add(info);
            return info;
        }
        public static ScreenDarknessInfo AddScreenDarkness(float maxStrength, int holdTime, Func<float, float> easeIn = null, Func<float, float> easeOut = null, Func<bool> holdCondition = null)
        {
            int inTime = (int)(holdTime * .1f);
            int outTime = (int)(holdTime * .9f);
            var info = new ScreenDarknessInfo(maxStrength, inTime, holdTime, outTime, ActiveDarkness.Count, easeIn, easeOut)
            {
                HoldCondition = holdCondition
            };

            ActiveDarkness.Add(info);
            return info;
        }
    }
}
