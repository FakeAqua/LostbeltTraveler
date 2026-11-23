using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Projs.Executor.Thrown;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Executor.Thrown
{
    public class DeathTolls : ExecutorWeaponClass
    {
        public override float ExecutionStrikeDamageMult => 1.0f;
        public override int ExecutionProgress => 25;
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.Throw;
        public override void ExSSD()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.ForeverNight);
        }
        public override void ExSD()
        {
            Item.SetUpNoUseGraphicItem();
            Item.SetUpRarityPrice(ItemRarityID.Purple);
            Item.Lostbelt().drawFlavorTooltip = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ProjectileType<DeathTollsProj>();
            Item.useTime = Item.useAnimation = 21;
            Item.knockBack = 8f;
            Item.damage = 162;
            //这里的UseTime是有意改的很慢的
            Item.shootSpeed = 19f;
            //这里不会给音效，因为要考虑一些射弹的联动
            //实际音效会在射弹初始化的时候提供
            Item.UseSound = null;

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //初始化。
            int projID = type;
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, projID, damage, knockback, player.whoAmI);
            proj.Lostbelt().HasExecutionMechanic = true;
            if (player.GetExecutionSrike() && player.HasProj<DeathTollsMinion>())
                proj.Lostbelt().ExecutionStrike = true;
            return false;
        }
        public override void HoldItem(Player player)
        {
            if (player.HasProj<DeathTollsMinion>())
                return;
            if (!player.GetExecutionSrike())
                return;
            int damage = (int)player.GetTotalDamage<ExecutorDamageClass>().ApplyTo(Item.damage);
            Projectile proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, ProjectileType<DeathTollsMinion>(), damage, Item.knockBack, player.whoAmI);
            player.RemoveExecutionProgress();
            player.Lostbelt().tacticalExecutionInputCache = 0;
        }
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            return base.PreDrawTooltipLine(line, ref yOffset);
        }

        public override void ExModifyTooltips(List<TooltipLine> tooltips)
        {
        }
    }
}
