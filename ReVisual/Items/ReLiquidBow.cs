using ContinentOfJourney.Items;
using LostbeltTraveler.ReVisual.Class;
using Terraria;
using Terraria.DataStructures;

namespace LostbeltTraveler.ReVisual.Items
{
    public class ReLiquidBow : ReVisualItemClass
    {
        public override int ApplyItem => ItemType<LiquidBow>();
        public override void ExHoldItem(Item item, Player player, ReVisualPlayer vp)
        {
            vp.reLiquidBow = !vp.reLiquidBow;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (player.GetModPlayer<ReVisualPlayer>().reLiquidBow)
            {
                item.noUseGraphic = true;
            }
            base.UpdateInventory(item, player);
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.GetModPlayer<ReVisualPlayer>().reLiquidBow)
            {
                if (!player.HasProj<ReVisualRecoilProj>())
                {
                    Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, ProjectileType<ReVisualRecoilProj>(), 0, 0, player.whoAmI);
                    if (proj.ModProjectile is ReVisualRecoilProj holdout)
                    {
                        holdout.SetUpHoldoutData(ApplyItem, 0f, item.useAnimation, new Vector2(20, -0f));
                    }
                }
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
