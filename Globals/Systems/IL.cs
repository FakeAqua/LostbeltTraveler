using ContinentOfJourney.Rarity;
using LostbeltTraveler.Globals.Configs;
using LostbeltTraveler.Items.Armor.Monk;
using LostbeltTraveler.Items.Armor.Shinobi;
using MonoMod.Cil;
using ReLogic.Graphics;
using System;
using System.Linq.Expressions;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace LostbeltTraveler.Globals.Systems
{
    public class LostbeltILSystem : ModSystem
    {
        public override void Load()
        {
            IL_Main.GUIHotbarDrawInner += DrawRarity;
        }
        public override void Unload()
        {
            IL_Main.GUIHotbarDrawInner -= DrawRarity;
        }
        public override void OnModLoad()
        {
            On_ShimmerTransforms.IsItemTransformLocked += On_ShimmerTransforms_IsItemTransformLocked;
        }
        public override void OnModUnload()
        {
            On_ShimmerTransforms.IsItemTransformLocked -= On_ShimmerTransforms_IsItemTransformLocked;
        }
        private static void DrawRarity(ILContext il)
        {
            LostbeltConfigClient config = LostbeltConfigClient.Instance;
            if ((!config.DrawItemNameRarity || ModLoader.HasMod("FixHotbarRarity")))
                return;
            ILCursor cursor = new(il);

            // ───── 第一段：定位位置计算完成点 ─────
            if (!cursor.TryGotoNext(
                MoveType.After,
                i => i.MatchCall<Vector2>("op_Division"),
                i => i.MatchLdfld<Vector2>("X"),
                i => i.MatchSub(),
                i => i.MatchLdcR4(0f),
                i => i.MatchCall<Vector2>(".ctor")))
            {
                LostbeltTraveler.Instance.Logger.Warn("[LostbeltTraveler] 未找到热键栏物品名位置计算点，IL 注入失败。");
                return;
            }

            // 插入跳转，跳过原版 DrawString 段
            ILLabel skipLabel = cursor.DefineLabel();
            cursor.EmitBr(skipLabel);

            // ───── 第二段：定位原版 DrawString 调用点 ─────
            if (!cursor.TryGotoNext(
                MoveType.After,
                i => i.MatchLdloc(4),
                i => i.MatchLdcR4(1f),
                i => i.MatchLdcI4(0),
                i => i.MatchLdcR4(0f),
                i => i.MatchCall(typeof(DynamicSpriteFontExtensionMethods), "DrawString")))
            {
                LostbeltTraveler.Instance.Logger.Warn("[LostbeltTraveler] 未找到原版 DrawString 调用点，IL 注入失败。");
                return;
            }

            // 标记跳转目标：执行到这里时原版白色绘制已被跳过
            cursor.MarkLabel(skipLabel);

            // ───── 第三段：插入自定义绘制 ─────
            // 原版在 GUIHotbarDrawInner 里的局部变量布局：ldloc.0 = text, ldloc.3 = position
            // 这两个局部变量在原版整个方法内有效，跳转过来后仍然可以读取
            cursor.EmitLdloc(0);
            cursor.EmitLdloc(3);
            cursor.EmitDelegate<Action<string, Vector2>>(DrawColoredHotbarName);
        }
        public static void DrawColoredHotbarName(string text, Vector2 position)
        {
            Player localPlayer = Main.LocalPlayer;
            Item item = localPlayer.inventory[localPlayer.selectedItem];
            DynamicSpriteFont value = FontAssets.MouseText.Value;
            float fading = (float)Main.mouseTextColor / 255f;
            if (item.IsAir || item.rare == ItemRarityID.White)
            {
                fading = 1f;
            }
            Color color = ItemRarity.GetColor(item.IsAir ? 0 : item.rare);
            if (item.expert || item.rare == ItemRarityID.Expert)
            {
                color = Main.DiscoColor;
            }
            if (item.master || item.rare == ItemRarityID.Master)
            {
                color = new Color(255, (byte)(Main.masterColor * 200f), 0);
            }
            TooltipLine line = LostbeltMethods.CreateVanillaLine("ItemName", text);
            DrawableTooltipLine drawableTooltipLine = new(line, 0, (int)position.X, (int)position.Y, color);
            int yOffset = 0;
            if (ItemLoader.PreDrawTooltipLine(item, drawableTooltipLine, ref yOffset))
            {
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, value, text, position, color * fading, 0, Vector2.Zero, Vector2.One, -1f, 2f);
            }
            ItemLoader.PostDrawTooltipLine(item, drawableTooltipLine);
        }
        private bool On_ShimmerTransforms_IsItemTransformLocked(On_ShimmerTransforms.orig_IsItemTransformLocked orig, int type)
        {
            if (type == ItemType<MonkHead>() || type == ItemType<ShinobiHead>())
            {
                return !Condition.DownedGolem.IsMet();
            }
            if (type == ItemType<MonkBody>() || type == ItemType<ShinobiBody>())
            {
                return !Condition.DownedGolem.IsMet();

            }
            if (type == ItemType<MonkLegs>() || type == ItemType<ShinobiLegs>())
            {
                return !Condition.DownedGolem.IsMet();
            }
            if (type == ItemID.PrincessWeapon)
            {
                return !Condition.DownedPlantera.IsMet();
            }
            return orig(type);

        }
    }
}
