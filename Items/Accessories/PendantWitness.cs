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
    public class PendantWitness : LostbeltItemClass
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }

        public override string AssetPath => AssetHandler.Equips;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateCopper);
        }
        public override void ExSD()
        {
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Orange);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().pendantLevel = 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(LostbeltRecipeGroup.AnyGoldBar, 10).
                AddIngredient(ItemID.Lens, 5).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
