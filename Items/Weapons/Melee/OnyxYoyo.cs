using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Projs.Melee;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Melee
{
    public class OnyxYoyo : GemYoyoWeapon
    {
        public override void ExSD()
        {
            base.ExSD();
            Item.damage = 167;
            Item.shootSpeed = 10f;
            Item.shoot = ProjectileType<OnyxYoyoProj>();
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.WoodYoyo).
                AddIngredient<Onyx>(8).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
