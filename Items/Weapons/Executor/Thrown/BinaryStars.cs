using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Items.Materials;
using LostbeltTraveler.Projs.Executor.Thrown;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Executor.Thrown
{
    public class BinaryStars : ExecutorWeaponClass
    {
        public override float ExecutionStrikeDamageMult => 1.0f;
        public override int ExecutionProgress => 32;
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.Throw;
        public override void ExSSD()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Nebula);
            ScarletItemIDSets.GrantsBoosterAfterSon[Type] = true;
        }
        protected override ColorStructe ShinyColorStruct => new(true, Color.Lerp(Color.Purple,Color.Black,.575f), Color.Lerp(Color.MediumPurple, Color.HotPink, 0.2f), Color.Lerp(Color.Purple,Color.White,.5f));
        public override void ExSD()
        {
            Item.DamageType = ExecutorDamageClass.Instance;
            Item.damage = 380;
            Item.useTime = Item.useAnimation = 10;
            Item.shootSpeed = 20f;
            Item.SetUpRarityPrice(ItemRarityID.Red);
            Item.SetUpNoUseGraphicItem();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ProjectileType<BinaryStarsProj>();
            Item.knockBack = 12f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AetherfireSmasher>().
                AddIngredient<DeathTolls>().
                AddIngredient<CrownofSilveryLight>(15).
                AddTile<ContinentOfJourney.Tiles.FinalAnvil>().
                Register();

        }
    }
}