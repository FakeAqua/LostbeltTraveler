using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Accessories
{
    public class HeartoftheCrystal : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, Globals.Database.Enums.ShinyRarityType.Nebula);
        }

        public override void ExSD()
        {
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Red);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().heartoftheCrystal = true;
            player.GetCritChance<MagicDamageClass>() += 15;
            player.GetDamage<MagicDamageClass>() *= 0.82f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.FragmentNebula, 10).
                AddIngredient(ItemID.ManaFlower).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
