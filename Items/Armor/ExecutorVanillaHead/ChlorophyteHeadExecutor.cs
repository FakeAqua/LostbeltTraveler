using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Executor;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Armor.ExecutorVanillaHead
{
    [AutoloadEquip(EquipType.Head)]
    public class ChlorophyteHeadExecutor : LostbeltArmor
    {
        public override int[] ArmorSlots => [Type, ItemID.ChlorophytePlateMail, ItemID.ChlorophyteGreaves];
        public override bool SetUpArmorSet => true;
        public float Damage = 0.10f;
        public float CritDamage = 0.10f;
        public int Crit = 5;
        public static int BoltDamage = 75;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Damage.ToPercent(), Crit + "%", CritDamage.ToPercent());
        public override void ExSD()
        {
            Item.defense = 17;
            Item.SetUpRarityPrice(ItemRarityID.Lime);
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage<ExecutorDamageClass>() += Damage;
            player.GetCritChance<ExecutorDamageClass>() += Crit;
            player.Lostbelt().critDamageExecutor += CritDamage;
        }
        public override void UpdateArmorSetBetter(Player player, string setBonusPath)
        {
            player.setBonus += "\n" + setBonusPath.ToLangValue().ToFormatValue(BoltDamage);
            player.Lostbelt().chlorophyteHeadExecutor = true;
            player.armorEffectDrawShadow = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.ChlorophyteBar, 12).
                AddTile(TileID.MythrilAnvil).
                Register();
        }

    }
}
