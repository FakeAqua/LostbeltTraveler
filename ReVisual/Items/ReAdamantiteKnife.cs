using ContinentOfJourney.Items;
using LostbeltTraveler.ReVisual.Class;
using Terraria;

namespace LostbeltTraveler.ReVisual.Items
{
    public class ReAdamantiteKnife : ReVisualItemClass
    {
        public override int ApplyItem => ItemType<AdamantiteKnife>();
        public override void ExHoldItem(Item item, Player player, ReVisualPlayer vp)
        {
            vp.reAdamantiteKnife = !vp.reAdamantiteKnife;
        }
    }
}
