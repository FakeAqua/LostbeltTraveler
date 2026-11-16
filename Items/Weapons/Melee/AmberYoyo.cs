using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Projs.Melee;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Melee
{
    public abstract class GemYoyoWeapon : LostbeltWeapon
    {
        public override EnumDamageClass Category => EnumDamageClass.Melee;
        public override void ExSD()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = Item.useAnimation = 40;
            Item.knockBack = 1.0f;
            Item.SetUpRarityPrice(ItemRarityID.LightRed);
            Item.SetUpNoUseGraphicItem(true);
        }
    }
    public class AmberYoyo : GemYoyoWeapon
    {
        public override void ExSD()
        {
            base.ExSD();
            Item.damage = 13;
            Item.shootSpeed = 10f;
            Item.shoot = ProjectileType<AmberYoyoProj>();
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Amber).
                AddIngredient(ItemID.Topaz, 8).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
