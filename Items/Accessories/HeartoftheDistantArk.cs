using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Core;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Instances.Items;
using LostbeltTraveler.Globals.Players.Dashes;
using LostbeltTraveler.Globals.Systems;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class HeartoftheDistantArk : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public int LifeMax2 = 20;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateCopper);
        }
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.accessory = true;
            Item.expert = true;
            Item.defense = 3;
        }
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(LifeMax2);
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.noKnockback = true;
            player.statLifeMax2 += LifeMax2;
            player.ApplyDash(ScarletContent.DashType<HeartoftheDistantArkDash>());
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.EoCShield).
                AddIngredient(ItemID.CobaltShield).
                AddRecipeGroup(LostbeltRecipeGroup.AnyCopperBar, 10).
                AddIngredient<DeepBar>(10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
