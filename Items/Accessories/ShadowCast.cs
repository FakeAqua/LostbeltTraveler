using LostbeltTraveler.Core;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Players.Dashes;
using LostbeltTraveler.Globals.Systems;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Accessories
{
    public class ShadowCast : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public override void ExSD()
        {
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Purple);
            Item.master = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.ApplyDash(ScarletContent.DashType<ShadowCastDash>());
        }
    }
}
