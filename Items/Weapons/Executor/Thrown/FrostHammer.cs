using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Globals.Instances.Items;
using LostbeltTraveler.Projs.Executor.Thrown;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Executor.Thrown
{
    public class FrostHammer : ExecutorWeaponClass
    {
        public override int ExecutionProgress => 30;
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.Throw;
        public override void ExSSD()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Frost);
        }
        public override void ExSD()
        {
            Item.damage = 42;
            Item.UseSound = LostbeltSoundAssets.HymnFireball_Release with { MaxInstances = 1, Pitch = -0.20f, PitchVariance = 0.15f, Volume = 0.8f };
            Item.SetUpNoUseGraphicItem();
            Item.SetUpRarityPrice(ItemRarityID.LightRed);
            Item.SetUpItemShoot<FrostHammerProj>(18);
            Item.SetUpItemUseTime(ItemUseStyleID.Swing, 30);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(LostbeltRecipeGroup.AnyTitaniumBar, 15).
                AddIngredient(ItemID.FrostCore, 3).
                AddIngredient(ItemID.SoulofLight, 15).
                AddIngredient(ItemID.SoulofNight, 15).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
