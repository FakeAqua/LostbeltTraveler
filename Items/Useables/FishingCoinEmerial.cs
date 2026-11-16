using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Useables
{
    public class FishingCoinEmerial : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Useables;
        public override void ExSD()
        {
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.buyPrice(0, 0, 50, 0);
            Item.maxStack = Item.CommonMaxStack;
        }
    }
}
