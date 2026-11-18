using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Core.ScreenEffect;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Methods;
using LostbeltTraveler.Projs.Executor.ColdSteel;
using LostbeltTraveler.Projs.Executor.Firearm;
using LostbeltTraveler.Projs.General;
using LostbeltTraveler.Projs.Magic;
using LostbeltTraveler.Projs.Melee;
using System.Collections.Generic;
using System.Diagnostics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using UtfUnknown.Core.Models.SingleByte.Russian;

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
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity.ToSafeNormalize()*16f, 
                ProjectileType<OnyxYoyoOnyx>(), damage, 1);
            Stopwatch.StartNew();
            Stopwatch sw = Stopwatch.StartNew();
            sw.Stop();
            // 输出经过的时间（毫秒）
            //Main.NewText($"执行耗时: {sw.ElapsedMilliseconds} ms");
            //// 更高精度输出
            //Main.NewText($"精确耗时: {sw.Elapsed.TotalMilliseconds:F4} ms");
            return false;
            //Vector2 ownerMW = player.LocalMouseWorld();
            //添加需要的攻击单位
        }
    }

}

namespace LostbeltTraveler.Items.Weapons.Requirement
{
    // 根据你实际需要调整类名、成员和访问级别
    public class WeaponRequirement
    {
        // 占位构造／方法
        public WeaponRequirement() { }
    }
}
