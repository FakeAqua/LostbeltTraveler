using ContinentOfJourney.Items;
using LostbeltTraveler.ReVisual.Class;
using Terraria;

namespace LostbeltTraveler.ReVisual.Items
{
    public class ReOrichalcumKnife : ReVisualItemClass
    {
        public override int ApplyItem => ItemType<OrichalcumKnife>();
        public override void ExHoldItem(Item item, Player player, ReVisualPlayer vp)
        {
            vp.reOrihalcumKnife = !vp.reOrihalcumKnife;
        }
    }
}
