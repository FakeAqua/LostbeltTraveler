using ContinentOfJourney.Items;
using LostbeltTraveler.ReVisual.Class;
using Terraria;

namespace LostbeltTraveler.ReVisual.Items
{
    public class ReAccelerationism : ReVisualItemClass
    {
        public override int ApplyItem => ItemType<Accelerationism>();
        public override void ExHoldItem(Item item, Player player, ReVisualPlayer vp)
        {
            vp.reAccelerationism = !vp.reAccelerationism;
        }
    }
}
