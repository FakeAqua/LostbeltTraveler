using ContinentOfJourney.Items;
using LostbeltTraveler.ReVisual.Class;
using Terraria;

namespace LostbeltTraveler.ReVisual.Items
{
    public class ReLonginus : ReVisualItemClass
    {
        public override int ApplyItem => ItemType<Longinus>();
        public override void ExHoldItem(Item item, Player player, ReVisualPlayer vp)
        {
            vp.reLonginus = !vp.reLonginus;
        }
    }
}
