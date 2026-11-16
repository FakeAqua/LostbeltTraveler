using ContinentOfJourney.Items;
using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Globals.Configs;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Items.Materials;
using LostbeltTraveler.Projs.Melee;
using LostbeltTraveler.Rarity.RarityShiny;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Melee
{
    public class EclipseWraith : ThrownSpearClass
    {
        public override bool NotHomewardJourneySpear => true;
        public override bool HasLegendary => true;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.ScarletRed);
        }
        public override void ExSD()
        {
            Item.width = Item.height = 50;
            Item.damage = 75;
            Item.useTime = Item.useAnimation = 26;
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.shootSpeed = 16f;
            Item.shoot = ProjectileType<EclipseWraithProj>();
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = LostbeltSoundAssets.SodomsDisaster_Toss with { MaxInstances = 0, Pitch = 0.21f, Volume = 0.74f };
            Item.Lostbelt().EnableCritDamage = true;
            Item.Lostbelt().CritsDamageBonus = 0.30f;
        }
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 26f;
        public override void ExModifyTooltips(List<TooltipLine> tooltips, string localAddress)
        {
            int flavorTooltipIndex = tooltips.FindIndex(line => line.Name == "ItemName" && line.Mod == "Terraria");
            //通过本地化路径搜索需要的特殊文本
            string value = this.GetLocalizedValue("FlavorTooltips").ToLangValue();
            //实例化toolti并注册名字
            TooltipLine flavorTooltips = new TooltipLine(Mod, "FlavorTooltipsName", value)
            {
                OverrideColor = Color.Lerp(Color.Red, Color.Crimson, 0.65f)
            };

            //植入Tooltip
            tooltips.Insert(flavorTooltipIndex + 1, flavorTooltips);
        }
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (!LostbeltConfigClient.Instance.SpecialRarity)
                return base.PreDrawTooltipLine(line, ref yOffset);

            if (line.Name == "FlavorTooltipsName" && line.Mod == Mod.Name)
            {
                DisasterRarity.DrawFlavorRarity2(line);
                return false;
            }
            return base.PreDrawTooltipLine(line, ref yOffset);
        }
        public override Color MainTooltipColor => Color.Lerp(Color.Red, Color.Crimson, 0.65f)
;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<FierySpear>().
                AddIngredient<LightBiteThrown>().
                AddIngredient<DisasterBar>(10).
                AddTile(TileID.MythrilAnvil).
                Register();
            CreateRecipe().
                AddIngredient<FierySpear>().
                AddIngredient<LightBite>().
                AddIngredient<DisasterBar>(10).
                DisableDecraft().
                AddTile(TileID.MythrilAnvil).
                Register();

        }
    }
}
