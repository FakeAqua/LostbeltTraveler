using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Globals.Methods;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Armor.ExecutorVanillaHead
{
    [AutoloadEquip(EquipType.Head)]
    public class MythrilHeadExecutor : LostbeltArmor
    {
        public override int[] ArmorSlots => [Type, ItemID.MythrilChainmail, ItemID.MythrilGreaves];
        public override bool SetUpArmorSet => true;
        public int Crit = 10;
        public int ArmorSetCrit = 10;
        public float CritDamage = 0.10f;
        public override void ExSD()
        {
            Item.defense = 14;
            Item.SetUpRarityPrice(ItemRarityID.LightRed);
        }
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Crit + "%", CritDamage.ToPercent());
        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<ExecutorDamageClass>() += Crit;
            player.Lostbelt().critDamageExecutor += CritDamage;
        }
        public override void UpdateArmorSetBetter(Player player, string setBonusPath)
        {
            player.setBonus += "\n" + setBonusPath.ToLangValue().ToFormatValue(ArmorSetCrit + "%");
            player.GetCritChance<ExecutorDamageClass>() += ArmorSetCrit;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.MythrilBar, 10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }

    }
}
