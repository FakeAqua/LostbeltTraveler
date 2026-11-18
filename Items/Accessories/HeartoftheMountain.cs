using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Accessories
{
    public class HeartoftheMountain : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, Globals.Database.Enums.ShinyRarityType.Nebula);
        }
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Purple);
            Item.defense = 5;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().heartoftheCrystal = true;
            player.GetDamage<MagicDamageClass>() *= 10.0f;
            player.GetCritChance<MagicDamageClass>() += 50;
            player.GetAttackSpeed<MagicDamageClass>() += .50f;
            player.statManaMax2 += 300;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<HeartoftheCrystal>().
                AddIngredient<EssenceofBright>(10).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
