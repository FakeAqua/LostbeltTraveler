using ContinentOfJourney.Items;
using LostbeltTraveler.ReVisual.Class;
using Terraria;

namespace LostbeltTraveler.ReVisual.Items
{
    public class ReBloodthirst : ReVisualItemClass
    {
        public override int ApplyItem => ItemType<Bloodthirst>();
        public override void ExHoldItem(Item item, Player player, ReVisualPlayer vp)
        {
            vp.reVisualBloodthirst = !vp.reVisualBloodthirst;
        }
    }
}
