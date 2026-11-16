using ContinentOfJourney.Items;
using LostbeltTraveler.ReVisual.Class;
using Terraria;

namespace LostbeltTraveler.ReVisual.Items
{
    public class ReMythrilKnife : ReVisualItemClass
    {
        public override int ApplyItem => ItemType<MythrilKnife>();
        public override void ExHoldItem(Item item, Player player, ReVisualPlayer vp)
        {
            vp.reVisualMythrilKnife = !vp.reVisualMythrilKnife;
        }
    }
}
