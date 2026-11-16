using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Accessories
{
    public class SkyofHorizon : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.StardustFancy);
        }
        public override void ExSD()
        {
            Item.accessory = true;
            Item.expert = true;
            Item.SetUpRarityPrice(ItemRarityID.Red);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!player.empressBrooch)
            {
                player.moveSpeed += .075f;
                player.jumpSpeedBoost += 1.8f;
                player.runAcceleration *= 1.75f;
            }
            player.Lostbelt().infiniteFlightTime = true;
            player.noKnockback = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.EmpressFlightBooster).
                AddIngredient<SolarFlareScoria>(50).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
