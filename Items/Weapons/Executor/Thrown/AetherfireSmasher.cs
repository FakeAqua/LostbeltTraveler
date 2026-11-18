using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Items.Materials;
using LostbeltTraveler.Projs.Executor.Thrown;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Executor.Thrown
{
    public class AetherfireSmasher : ExecutorWeaponClass
    {
        public override int ExecutionProgress => 40;
        public override float ExecutionStrikeDamageMult => 1f;
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.Throw;
        public override void ExSSD()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Solar);
        }
        public override void ExSD()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = LostbeltSoundAssets.Blunt_Swing with { MaxInstances = 1, Pitch = -0.4f, PitchVariance = 0.2f, Volume = 0.5f };
            Item.shoot = ProjectileType<AetherfireSmasherProj>();
            Item.knockBack = 6f;
            Item.DamageType = ExecutorDamageClass.Instance;
            Item.damage = 71;
            Item.useTime = Item.useAnimation = 12;
            Item.shootSpeed = 18f;
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.SetUpNoUseGraphicItem(false);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.PaladinsHammer).
                AddIngredient<DisasterBar>(10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
