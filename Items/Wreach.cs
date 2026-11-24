using LostbeltTraveler.Core.ScreenEffect;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Methods.SideTextPanel;
using LostbeltTraveler.Projs.Magic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items
{
    public class Wreach : LostbeltWeapon
    {
        public override EnumDamageClass Category => EnumDamageClass.Melee;
        public override string Texture => LostbeltItemProj.Wreach.Path;
        public override void ExSD()
        {
            Item.width = Item.height = 50;
            Item.damage = 20;
            Item.rare = ItemRarityID.Red;
            Item.shoot = ProjectileType<CoronaFireball>();
            Item.shootSpeed = 17f;
            Item.useTime = Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noUseGraphic = true;
            Item.Lostbelt().drawBuffIconAndDetail = true;
        }
        private IReadOnlyList<TooltipLine> AlterTooltip = null;
        private float LineY = 0;
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            AlterTooltip = tooltips;
            base.ModifyTooltips(tooltips);

        }
        public override bool PreDrawTooltip(ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y)
        {
            List<SideTextPanelData> list =
            [
                new (new TooltipLine(Mod, "Test0","标题"),1),
                new (new TooltipLine(Mod, "Test1", "To Test Something"),1),
                new (new TooltipLine(Mod, "Test2", "然后这是插补字符串：[i:Diamond]"),1),
                new (new TooltipLine(Mod, "Test3", "这是[c/8815A0:颜色代码]"),1),
            ];

            int y2 = SideTextPanelMethods.DrawSideTextPanelWithTitleEdge(lines, list, x, y, Color.DarkRed);
            int y3 = SideTextPanelMethods.DrawSideTextPanelWithTitleEdge(lines, list, x, y, Color.DarkRed, extraY: y2 - y);
            return base.PreDrawTooltip(lines, ref x, ref y);
        }
        public override void PostDrawTooltip(ReadOnlyCollection<DrawableTooltipLine> lines)
        {
            base.PostDrawTooltip(lines);
        }
        public override void HoldItem(Player player)
        {
            if (Main.mouseLeft)
            {
                ScreenZoomSystem.UseTexture = true;
                LineY = Lerp(LineY, 1f, 0.1f);
            }
            else
            {
                LineY = Lerp(LineY, 0, .1f);
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity.ToSafeNormalize() * 8f,
                ProjectileType<AXCannonFullCharge>(), damage, 1);
            Stopwatch.StartNew();
            Stopwatch sw = Stopwatch.StartNew();
            sw.Stop();
            // 输出经过的时间（毫秒）
            //Main.NewText($"执行耗时: {sw.ElapsedMilliseconds} ms");
            //// 更高精度输出
            //Main.NewText($"精确耗时: {sw.Elapsed.TotalMilliseconds:F4} ms");
            return false;
        }
    }
}
