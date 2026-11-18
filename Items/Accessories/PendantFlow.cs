using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Instances.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Accessories
{
    public class PendantFlow : LostbeltItemClass
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }

        public override string AssetPath => AssetHandler.Equips;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateWhite);
        }
        public override void ExSD()
        {
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Lime);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().pendantLevel = 2;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PendantWitness>().
                AddRecipeGroup(LostbeltRecipeGroup.AnyTitaniumBar, 10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
