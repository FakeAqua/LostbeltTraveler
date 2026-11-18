using ContinentOfJourney.Items;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Projs.Melee;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Melee
{
    public class TerraSpear : ThrownSpearClass
    {
        public override bool NotHomewardJourneySpear => true;
        public override bool HasLegendary => false;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Life);
        }
        public override void ExSD()
        {
            Item.width = Item.height = 92;
            Item.damage = 60;
            Item.knockBack = 6f;
            Item.useTime = Item.useAnimation = 55;
            Item.shootSpeed = 18f;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = LostbeltSoundAssets.Hammer_Shoot with { Variants = [1], Pitch = -0.5f, MaxInstances = 0, Volume = 0.8f };
            Item.shoot = ProjectileType<TerraSpearProj>();

        }
        public override Color MainTooltipColor => Color.GreenYellow;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CryoblazeHymn>().
                AddIngredient<SpearofDarknessThrown>().
                AddIngredient(ItemID.BrokenHeroSword).
                AddTile(TileID.MythrilAnvil).
                Register();
            CreateRecipe().
                AddIngredient<CryoblazeHymn>().
                AddIngredient<SpearOfDarkness>().
                AddIngredient(ItemID.BrokenHeroSword).
                DisableDecraft().
                AddTile(TileID.MythrilAnvil).
                Register();
        }

    }
}
