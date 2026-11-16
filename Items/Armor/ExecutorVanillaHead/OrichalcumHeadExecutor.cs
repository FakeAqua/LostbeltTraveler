using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Methods;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Armor.ExecutorVanillaHead
{

    [AutoloadEquip(EquipType.Head)]
    public class OrichalcumHeadExecutor : LostbeltArmor
    {
        public override int[] ArmorSlots => [Type, ItemID.OrichalcumBreastplate, ItemID.OrichalcumLeggings];
        public override bool SetUpArmorSet => true;
        public float CritDamage = .20f;
        public float MoveSpeed = .20f;
        public override void ExSD()
        {
            Item.defense = 15;
            Item.SetUpRarityPrice(ItemRarityID.LightRed);
        }
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(CritDamage.ToPercent(), MoveSpeed.ToPercent());
        public override void UpdateEquip(Player player)
        {
            player.Lostbelt().critDamageExecutor += CritDamage;
            player.moveSpeed += MoveSpeed;
        }
        public override void UpdateArmorSetBetter(Player player, string setBonusPath)
        {
            player.setBonus += "\n" + setBonusPath.ToLangValue();
            player.onHitPetal = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.OrichalcumBar, 10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }

    }
}
