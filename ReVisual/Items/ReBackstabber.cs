using ContinentOfJourney.Items;
using LostbeltTraveler.ReVisual.Class;
using Terraria;

namespace LostbeltTraveler.ReVisual.Items
{
    public class ReBackstabber : ReVisualItemClass
    {
        public override int ApplyItem => ItemType<Backstabber>();
        public override void ExHoldItem(Item item, Player player, ReVisualPlayer vp)
        {
            vp.reVisualBackStabber = !vp.reVisualBackStabber;
        }
    }
}
