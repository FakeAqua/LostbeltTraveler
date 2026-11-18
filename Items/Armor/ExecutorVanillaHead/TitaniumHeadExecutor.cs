using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Executor;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Armor.ExecutorVanillaHead
{
    [AutoloadEquip(EquipType.Head)]
    public class TitaniumHeadExecutor : LostbeltArmor
    {
        public override int[] ArmorSlots => [Type, ItemID.TitaniumBreastplate, ItemID.TitaniumLeggings];
        public float Damage = 0.12f;
        public int Crit = 12;
        public float CritDamage = 0.12f;
        public static int ShardDamage = 25;
        public override bool SetUpArmorSet => true;
        public override void ExSD()
        {
            Item.defense = 17;
            Item.SetUpRarityPrice(ItemRarityID.Pink);
        }
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Damage.ToPercent(), Crit + "%", CritDamage.ToPercent());
        public override void UpdateEquip(Player player)
        {
            player.GetDamage<ExecutorDamageClass>() += Damage;
            player.GetCritChance<ExecutorDamageClass>() += Crit;
            player.Lostbelt().critDamageExecutor += CritDamage;
        }
        public override void UpdateArmorSetBetter(Player player, string setBonusPath)
        {
            player.setBonus += "\n" + setBonusPath.ToLangValue().ToFormatValue(ShardDamage);
            player.armorEffectDrawShadow = true;
            player.Lostbelt().titaniumHeadExecutor = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.TitaniumBar, 13).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
