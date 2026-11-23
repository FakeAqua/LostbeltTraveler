using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Useables
{
    public class UnregisteredSpiritOrigin : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Useables;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateWhite);
        }
        public override void ExSD()
        {
            Item.rare = ItemRarityID.Orange;
            Item.Lostbelt().drawFlavorTooltip = true;
            Item.maxStack = 9999;
            Item.master = true;
        }
        public override void HoldItem(Player player)
        {
            player.Lostbelt().drawUseableItemIcon = Type;
            for (int i = 0; i < player.inventory.Length; i++)
            {
                Item item = player.inventory[i];
                if (item.IsAir || item is null)
                    continue;
                bool isWeapon = item.damage > 0 && item.pick == 0 && item.axe == 0 && item.hammer == 0 && !item.IsACoin && item.ammo == AmmoID.None;
                bool isAccessory = (item.accessory || item.defense > 0) && item.pick == 0 && item.axe == 0 && item.hammer == 0 && !item.IsACoin && item.ammo == AmmoID.None && !item.vanity;
                bool isTreasureBag = ItemID.Sets.BossBag[item.type];
                if (isWeapon || isAccessory || isTreasureBag)
                {
                    item.Lostbelt().setTintIcon = true;
                }
            }
        }
    }
}
