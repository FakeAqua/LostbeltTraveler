using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Armor.Shinobi
{
    [AutoloadEquip(EquipType.Body)]
    public class ShinobiBody : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Armors;
        public override void ExSD()
        {
            Item.width = Item.height = 40;
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.defense = 30;
        }
        public override void UpdateArmorSet(Player player)
        {
            base.UpdateArmorSet(player);
        }
        public float CritChance = 30;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(CritChance + "%");

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<ExecutorDamageClass>() += CritChance;
        }
    }
}
