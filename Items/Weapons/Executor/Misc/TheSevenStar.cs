using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Globals.Keybinds;
using LostbeltTraveler.Projs.Executor.Misc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Executor.Misc
{
    public class TheSevenStar : ExecutorWeaponClass
    {
        public override int ExecutionProgress => 7;
        public static int Damage = 34;
        public override void ExSSD()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Donator);
            ScarletItemIDSets.ForceToTacticalExecute[Type] = true;
            ScarletItemIDSets.NoGeneralExecutionProgressDraw[Type] = true;
            ScarletItemIDSets.IsHeldProjItem[Type] = true;
        }
        public override void ExSD()
        {
            Item.damage = 32;
            Item.knockBack = 4.5f;
            Item.useTime = Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 16f;
            Item.shoot = ProjectileType<TheSevenStarHeldProj>();
            Item.SetUpRarityPrice(ItemRarityID.Orange);
            Item.SetUpNoUseGraphicItem(true);
            Item.Lostbelt().OwnerName = "冰川咲";
            Item.Lostbelt().ItemBelongTo = EnumItemOwner.Donator;
            Item.master = true;

        }
        protected override ColorStructe ShinyColorStruct => new(true, Color.Lerp(Color.Purple,Color.LightPink,.75f), Color.HotPink, Color.Pink);
        public override void ApplyExecutionTooltips(ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y, params object[] args)
        {
            SetStrikeTooltips(lines, ref x, ref y, LostbeltKeybinds.GeneralActionKeybind.GetHotKeyString());
        }
        public override void ExModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ExModifyTooltips(tooltips);
        }
        public override bool CanShoot(Player player)
        {
            return !player.HasProj(Item.shoot);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            bool exe = player.GetExecutionSrike();
            Vector2 dir = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            proj.Lostbelt().HasExecutionMechanic = true;
            proj.Lostbelt().ExecutionStrike = exe;
            ((TheSevenStarHeldProj)proj.ModProjectile).BeginTargetRotation = dir.ToRotation();
            ((TheSevenStarHeldProj)proj.ModProjectile).Flip = Main.rand.NextBool();

            return false;
        }
        public override void HoldItem(Player player)
        {
            bool noSevenStar = player.HasProj<TheSevenStarDipper>() && player.HasProj<TheSevenStarDipperStar>();
            if (noSevenStar || player.whoAmI != Main.myPlayer)
                return;
            Projectile proj = Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.MountedCenter, Vector2.Zero, ProjectileType<TheSevenStarDipper>(), 0, 0, player.whoAmI);
            proj.Center = player.MountedCenter - player.direction * 80f * Vector2.UnitX;
        }
    }
}
