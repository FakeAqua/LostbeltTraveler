using ContinentOfJourney;
using ContinentOfJourney.Items.Accessories;
using ContinentOfJourney.Tiles;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class Desterrennacht : LostbeltItemClass
    {
        public static int LifeThreshold = 100;
        public static float ChanceToInflictHorror = .6f;
        public static float DR = .05f;
        public static float CritDamage = .20f;
        public override string AssetPath => AssetHandler.Equips;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(LifeThreshold,
            ChanceToInflictHorror.ToPercent(), DR.ToPercent(), CritDamage.ToPercent());
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, Globals.Database.Enums.ShinyRarityType.StardustFancy);
        }

        public override void ExSD()
        {
            Item.defense = 16;
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Purple);
            Item.Lostbelt().drawBuffIconAndDetail = true;

        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().desterrennacht = true;
            player.GetModPlayer<TemplatePlayer>().TimeTurner = true;
            player.longInvince = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SouloftheTidalMark>().
                AddIngredient<TimeTurner>().
                AddIngredient<CrownofSilveryLight>(15).
                AddTile<FinalAnvil>().
                Register();
        }
    }
}
