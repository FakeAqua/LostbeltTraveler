using ContinentOfJourney.Items;
using LostbeltTraveler.ReVisual.Class;
using Terraria;

namespace LostbeltTraveler.ReVisual.Items
{
    public class ReCasta : ReVisualItemClass
    {
        public override int ApplyItem => ItemType<Casta>();
        public override void ExHoldItem(Item item, Player player, ReVisualPlayer vp)
        {
            vp.reCasta = !vp.reCasta;
        }
    }
}
