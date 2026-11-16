using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Methods;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Armor.Shinobi
{

    [AutoloadEquip(EquipType.Legs)]
    public class ShinobiLegs : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Armors;
        public override void ExSD()
        {
            Item.width = Item.height = 40;
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.defense = 20;
        }
        public int ArmorPenetration = 20;
        public float MoveSpeed = .30f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(ArmorPenetration, MoveSpeed.ToPercent());
        public override void UpdateEquip(Player player)
        {
            player.GetArmorPenetration<ExecutorDamageClass>() += ArmorPenetration;
            player.moveSpeed += MoveSpeed;
        }

    }
}
