using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
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
    public class SelfPortraitoftheJourney : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public static float DotDamageMult = 10f;
        public static float ExtraDamageMult = 1f;
        public static int MaxDoTCounts = 20;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(DotDamageMult.ToPercent(), ExtraDamageMult.ToPercent(), MaxDoTCounts);
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateWhite);
        }
        public override void ExSD()
        {
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.Lostbelt().drawBuffIconAndDetail = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.pStone = true;
            player.Lostbelt().lifeRegenPool += 2;
            player.Lostbelt().badLifeRegenMult *= .25f;
            player.Lostbelt().dotMult += DotDamageMult;
            player.Lostbelt().dotMultExtra += ExtraDamageMult;
            player.Lostbelt().dotMultCount = MaxDoTCounts;
            player.Lostbelt().selfPortraitType = Type;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SelfPortraitoftheBleachingOne>().
                AddIngredient<SelfPortraitoftheHoloFace>().
                AddIngredient(ItemID.CharmofMyths).
                AddIngredient<EssenceofDeath>(15).
                AddIngredient<EssenceofNothingness>(15).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
