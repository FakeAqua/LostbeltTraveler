using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Projs.Ranged;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Ranged
{
    public class Duskfall : LostbeltWeapon
    {
        public override EnumDamageClass Category => EnumDamageClass.Ranged;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Solar);
        }
        public override void ExSD()
        {
            Item.consumable = true;
            Item.SetUpNoUseGraphicItem(false, true);
            Item.damage = 196;
            Item.useTime = Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.maxStack = 9999;
            Item.SetUpRarityPrice(ItemRarityID.LightRed);
            Item.Lostbelt().drawBuffIconAndDetail = true;
            Item.crit = 12;
            Item.UseSound = SoundID.Item45 with { MaxInstances = 0 };
            Item.shootSpeed = 11f;
            Item.shoot = ProjectileType<DuskfallProj>();
            Item.knockBack = .5f;
        }
        public override void AddRecipes()
        {
            CreateRecipe(75).
                AddIngredient<SolarFlareScoria>(5).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
