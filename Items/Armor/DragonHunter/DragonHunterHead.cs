using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Items.Armor.RedDragonKnight;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Armor.DragonHunter
{
    [AutoloadEquip(EquipType.Head)]
    public class DragonHunterHead : LostbeltArmor
    {
        public override int[] ArmorSlots => [Type, ItemType<DragonHunterBody>(), ItemType<DragonHunterLegs>()];
        public override bool SetUpArmorSet => true;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.ScarletRed);
        }
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Purple);
            Item.defense = 55;
        }
        public float Damage = .20f;
        public float AttackSpeed = .35f;
        public static int RangedCrit = 50;
        public int MagicMana = 150;
        public float MagicManaCost = .45f;
        public int SummonerMinionSlot = 1;
        public int SummonerSentrySlot = 1;
        public static float FixedDamage = .30f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Damage.ToPercent());
        public override void UpdateEquip(Player player)
        {
            player.GetDamage<ExecutorDamageClass>() += Damage;
        }
        public override void UpdateArmorSetBetter(Player player, string setBonusPath)
        {
            player.GetAttackSpeed<MeleeDamageClass>() += AttackSpeed;
            player.GetCritChance<RangedDamageClass>() += RangedCrit;
            player.statManaMax2 += MagicMana;
            player.manaCost *= MagicManaCost;
            player.maxMinions = SummonerMinionSlot;
            player.maxTurrets = SummonerSentrySlot;
            player.Lostbelt().dragonHunter = true;
            player.Lostbelt().critDamageExecutor += FixedDamage;
            string value = setBonusPath.ToLangValue().ToFormatValue(AttackSpeed.ToPercent(), RangedCrit + "%", MagicMana, MagicManaCost.ToPercent(), SummonerMinionSlot, FixedDamage.ToPercent());
            player.setBonus += "\n" + value;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<RedDragonKnightHead>().
                AddIngredient<SunlightGel>(4).
                AddIngredient<EssenceofTime>(4).
                AddIngredient<EssenceofLife>(4).
                AddIngredient<EssenceofMatter>(4).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
