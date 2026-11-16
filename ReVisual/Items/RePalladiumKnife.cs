using ContinentOfJourney.Items;
using LostbeltTraveler.ReVisual.Class;
using Terraria;

namespace LostbeltTraveler.ReVisual.Items
{
    public class RePalladiumKnife : ReVisualItemClass
    {
        public override int ApplyItem => ItemType<PalladiumKnife>();
        public override void ExHoldItem(Item item, Player player, ReVisualPlayer vp)
        {
            vp.reVisualPalladiumKnife = !vp.reVisualPalladiumKnife;
        }
    }
}
