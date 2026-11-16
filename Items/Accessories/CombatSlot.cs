using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Methods;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Accessories
{
    public class CombatSlot : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Orange);
            Item.accessory = true;
            Item.value = Item.buyPrice(3, 0, 0, 0);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().combatSlot = true;
        }
    }
}
