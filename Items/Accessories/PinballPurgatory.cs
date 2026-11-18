using LostbeltTraveler.Core.NetCode;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Localization;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Players.Dashes;
using LostbeltTraveler.Globals.Players.InnerCooldown.Methods;
using LostbeltTraveler.Globals.Systems;
using LostbeltTraveler.Projs.General;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Accessories
{
    public class PinballPurgatory : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public static int PinkType = 4;
        public static int OrangeType = 3;
        public static int BlueType = 2;
        public static int WhiteType = 1;
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Pink);
            Item.accessory = true;
            Item.defense = 10;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string value = this.GetLocalizationKey("DetailTooltip");
            if (Main.keyState.PressingAlt())
                tooltips.ReplaceAllTooltip(value, Color.White);
            else
                tooltips.CreateTooltipDirect(ScarletTextSets.GeneralText_BuffShow.ToLangValue(), Color.LightGray, Mod, "DetailLineName");
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.noFallDmg = true;
            player.Lostbelt().mayaPumper = true;
            player.Lostbelt().mayaPumperParty = true;
            if (player.HasInnerCooldown(InnerCooldownType.MayaPumperDash))
            {
                if (player.Lostbelt().mayaPumperDashType != OrangeType && player.Lostbelt().mayaPumperDashType != WhiteType)
                    player.ApplyDash(GetInstance<CelesteDash>().Type);
                if (player.IsOwnerSide() && !player.HasProj<PinballPurgatoryTimer>())
                {
                    Projectile proj = Projectile.NewProjectileDirect(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ProjectileType<PinballPurgatoryTimer>(), 0, 0, player.whoAmI);
                    proj.timeLeft = player.GetInnerCooldownTime(InnerCooldownType.MayaPumperDash);
                }
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MayaBall>().
                AddIngredient(ItemID.HellstoneBar, 10).
                AddIngredient(ItemID.SoulofLight, 10).
                AddIngredient(ItemID.Diamond, 10).
                AddTile(TileID.CrystalBall).
                Register();
        }
    }
}
