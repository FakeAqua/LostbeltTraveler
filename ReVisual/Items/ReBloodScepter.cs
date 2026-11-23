using ContinentOfJourney.Items;
using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.ReVisual.Class;
using Terraria;
using Terraria.DataStructures;

namespace LostbeltTraveler.ReVisual.Items
{
    public class ReBloodScepter : ReVisualItemClass
    {
        public override int ApplyItem => ItemType<BloodScepter>();
        public override void ExHoldItem(Item item, Player player, ReVisualPlayer vp)
        {
            vp.reBloodScepter = !vp.reBloodScepter;
            item.noUseGraphic = vp.reBloodScepter;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (player.GetModPlayer<ReVisualPlayer>().reBloodScepter)
            {
                item.noUseGraphic = true;
            }
            base.UpdateInventory(item, player);
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.GetModPlayer<ReVisualPlayer>().reBloodScepter)
            {
                for (int i = 0; i < 16; i++)
                {
                    ECSParticle.ShinyCrossStarECS(position.ToRandCirclePos(6), velocity.ToRandVelocity(ToRadians(10), 1, 10), RandLerpColor(Color.Crimson, Color.Red), 40,
                        1, Main.rand.NextFloat(.9f, 1.1f) * .4f, .2f);
                    ECSParticle.ShrinkParticle(position.ToRandCirclePosEdge(8), velocity.ToRandVelocity(ToRadians(10), 1, 10), RandLerpColor(Color.Red, Color.Crimson), 40, 1,
                        RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .12f, 1);

                }
                Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, ProjectileType<ReVisualRecoilProj>(), 0, 0, player.whoAmI);
                if (proj.ModProjectile is ReVisualRecoilProj holdout)
                {
                    holdout.SetUpHoldoutData(ApplyItem, 0f, item.useAnimation, new Vector2(20, -0f));
                }
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
