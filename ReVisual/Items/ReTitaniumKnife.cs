using ContinentOfJourney.Items;
using LostbeltTraveler.ReVisual.Class;
using Terraria;

namespace LostbeltTraveler.ReVisual.Items
{
    public class ReTitaniumKnife : ReVisualItemClass
    {
        public override int ApplyItem => ItemType<TitaniumKnife>();
        public override void ExHoldItem(Item item, Player player, ReVisualPlayer vp)
        {
            vp.reTitaniumKnife = !vp.reTitaniumKnife;
        }
    }
}
