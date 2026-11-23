using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Methods.SideTextPanel;
using LostbeltTraveler.Projs.Melee;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Melee
{
    public class SupremeGem : LostbeltWeapon
    {
        public override EnumDamageClass Category => EnumDamageClass.Melee;
        public bool EnableCoinMode = false;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateWhite);
        }
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.SetUpNoUseGraphicItem(true);
            Item.Lostbelt().drawBuffIcon = true;
            Item.damage = 554;
            Item.crit = 16;
            Item.shootSpeed = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = Item.useAnimation = 16;
            Item.shoot = ProjectileType<SupremeGemYoyoProj>();
            Item.shootSpeed = 16;
            Item.knockBack = 4;
        }
        public override bool CanRightClick() => Main.keyState.PressingShift();
        public override void RightClick(Player player)
        {
            EnableCoinMode = !EnableCoinMode;
        }
        public override bool ConsumeItem(Player player) => false;
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Main.keyState.PressingAlt())
            {
                tooltips.ReplaceAllTooltip(this.GetLocalizationKey("GemTooltip"));
            }
            else
                tooltips.CreateHoldAltTooltip();
        }
        public override bool PreDrawTooltip(ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y)
        {
            return base.PreDrawTooltip(lines, ref x, ref y);
        }
        public override bool MeleePrefix() => true;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override void HoldItem(Player player)
        {
            player.noKnockback = true;
            if (player.HasProj<SupremeGemYoyoProjAmber>())
            {
                int count = 0;
                foreach (var p in Main.ActiveProjectiles)
                {
                    if (p.owner != player.whoAmI)
                        continue;
                    if (p.type != ProjectileType<SupremeGemYoyoProjAmber>())
                        continue;
                    count++;
                }
                if (count > 0)
                    player.endurance += count * .02f;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<OnyxYoyo>().
                AddIngredient<DiamondYoyo>().
                AddIngredient<RubyYoyo>().
                AddIngredient<AmberYoyo>().
                AddIngredient<EmeraldYoyo>().
                AddIngredient<SapphireYoyo>().
                AddIngredient<TopazYoyo>().
                AddIngredient<AmethystYoyo>().
                AddIngredient<EternalBar>(10).
                AddIngredient<LivingBar>(10).
                AddIngredient<CubistBar>(10).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
