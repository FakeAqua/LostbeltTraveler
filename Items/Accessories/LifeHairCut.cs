using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class LifeHairCut:LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public int LifeRegen = 2;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(LifeRegen.ToLifeRegenFormat());
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            LostbeltList.ShinyRarityItemDictionary.Add(Type, Globals.Database.Enums.ShinyRarityType.Life);
        }
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Lime);
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().lifeRegenPool += LifeRegen;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Apple, 5).
                AddIngredient<EssenceofLife>(5).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
