using ContinentOfJourney.Items;
using LostbeltTraveler.ReVisual.Class;
using Terraria;

namespace LostbeltTraveler.ReVisual.Items
{
    public class ReHellfire : ReVisualItemClass
    {
        public override int ApplyItem => ItemType<Hellfire>();
        public override void ExHoldItem(Item item, Player player, ReVisualPlayer vp)
        {
            vp.reVisualHellfire = !vp.reVisualHellfire;
        }
    }
}
