using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Projs.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Melee
{
    public class DiamondYoyo : GemYoyoWeapon
    {
        public override Vector2 TargetBuffer => new Vector2(150, 6);
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, Globals.Database.Enums.ShinyRarityType.FateWhite);
        }
        public override void ExSD()
        {
            base.ExSD();
            Item.Lostbelt().drawBuffIconAndDetail = true;
            Item.damage = 15;
            Item.shootSpeed = 13f;
            Item.shoot = ProjectileType<DiamondYoyoProj>();
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.WoodYoyo).
                AddIngredient(ItemID.Diamond, 8).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
