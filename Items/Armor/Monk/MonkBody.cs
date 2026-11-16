using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Globals.Methods;
using LostbeltTraveler.Items.Armor.Shinobi;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Armor.Monk
{
    [AutoloadEquip(EquipType.Body)]
    public class MonkBody : LostbeltArmor
    {
        public override void SetStaticDefaults()
        {
            Type.ShimmerEach<ShinobiBody>();
        }
        public override void ExSD()
        {
            Item.width = Item.height = 40;
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.defense = 22;
        }
        public int CritChance = 15;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(CritChance + "%");

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<ExecutorDamageClass>() += CritChance;
        }
    }
}
