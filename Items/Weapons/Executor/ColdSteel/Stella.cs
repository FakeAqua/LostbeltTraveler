using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Items.Materials;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Executor.ColdSteel
{
    public class Stella : ExecutorWeaponClass
    {

        public override int ExecutionProgress => 50;
        public override void ExSSD()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Solar);
        }
        public override void ExSD()
        {
            Item.damage = 1426;
            Item.useTime = Item.useAnimation = 36;
            Item.knockBack = 2;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.Lostbelt().NotFinished = true;
            Item.SetUpRarityPrice(ItemRarityID.Blue);
            Item.shootSpeed = 9f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.MoltenFury).
                AddIngredient<DisasterBar>(5).
                AddIngredient<UniversalCube>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
