using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using System;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Accessories
{
    public class ManaSavingsJar : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public int GiveMana = 0;
        public override void ExSD()
        {
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Green);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            GiveMana = (int)Clamp(Math.Min(150 - player.statManaMax, 150), 0, 150);
            player.Lostbelt().manaSavingsJar = GiveMana;
            player.statManaMax2 += GiveMana;
        }
    }
}
