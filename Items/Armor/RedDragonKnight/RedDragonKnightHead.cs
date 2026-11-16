using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Methods;
using LostbeltTraveler.Items.Materials;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Armor.RedDragonKnight
{
    [AutoloadEquip(EquipType.Head), LegacyName("DragonSlayerHead")]
    public class RedDragonKnightHead : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Armors;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.ScarletRed);
        }
        public override void ExSD()
        {
            Item.defense = 25;
            Item.SetUpRarityPrice(ItemRarityID.Purple);

        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<RedDragonKnightBody>() && legs.type == ItemType<RedDragonKnightLegs>();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
        }

        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            return true;
        }
        public float Damage = .15f;
        public int Crit = 10;
        public float CritDamage = .30f;
        public static int ProgressRegenTime = 5;
        public static float ProgressRegenCount = .05f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Damage.ToPercent(), Crit + "%");
        public override void UpdateEquip(Player player)
        {
            player.GetDamage<ExecutorDamageClass>() += Damage;
            player.GetCritChance<ExecutorDamageClass>() += Crit;
        }
        public override void UpdateArmorSet(Player player)
        {
            string value = Mod.GetLocalizationKey($"{LocalizationCategory}.{GetType().Name}.SetBouns").
                ToLangValue().ToFormatValue(CritDamage.ToPercent(), ProgressRegenTime, ProgressRegenCount.ToPercent());
            player.setBonus += "\n" + value;
            player.Lostbelt().redDragonKnight = true;
            player.Lostbelt().critDamageExecutor += CritDamage;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<UniversalCube>(6).
                AddIngredient(ItemID.LunarBar, 6).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
