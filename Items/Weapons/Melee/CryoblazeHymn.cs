using LostbeltTraveler.Globals.Instances.Items;
using LostbeltTraveler.Projs.Melee;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Melee
{
    public class CryoblazeHymn : ThrownSpearClass
    {
        public override bool NotHomewardJourneySpear => true;
        public override void ExSD()
        {
            Item.damage = 69;
            Item.useTime = Item.useAnimation = 30;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item45 with { MaxInstances = 0 };
            Item.Lostbelt().drawBuffIconAndDetail = true;
            Item.shootSpeed = 16f;
            Item.shoot = ProjectileType<CryoblazeHymnProj>();
            Item.rare = ItemRarityID.Orange;
        }
        public override Color MainTooltipColor => Color.AliceBlue;
        public override void AddRecipes()
        {
            if (!ModLoader.HasMod("CalamityMod"))
            {
                CreateRecipe().
                    AddIngredient<FierySpear>().
                    AddIngredient<AzureFrostmark>().
                    AddIngredient(ItemID.SoulofFright, 15).
                    AddIngredient(ItemID.SoulofSight, 15).
                    AddIngredient(ItemID.SoulofMight, 15).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddIngredient<FierySpear>().
                    AddIngredient<AzureFrostmark>().
                    AddRecipeGroup(LostbeltRecipeGroup.AnyMechBossSoul).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
        }
    }
}
