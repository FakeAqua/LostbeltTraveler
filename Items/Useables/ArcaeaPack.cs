using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Items.Vanity;
using LostbeltTraveler.Items.Vanity.Arceca;
using System.Collections.ObjectModel;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Useables
{
    public class ArcaeaPack : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Useables;
        public override void SetStaticDefaults()
        {
            ItemID.Sets.OpenableBag[Type] = true;
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateGolden);
        }
        public override void ExSD()
        {
            Item.rare = RarityType<VanityEffectClass>();
            Item.consumable = true;
        }
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Mod == "Terraria")
            {
                if (line.Name == "Tooltip3")
                {
                    VanityData vanityData = new VanityData(
                        Color.Lerp(Color.White, Color.DarkRed, 0.35f),
                        Color.Lerp(Color.White, Color.IndianRed, 0.95f),
                        Color.White);
                    VanityEffectClass.DrawMisc(line, vanityData, Color.White, Color.IndianRed);

                    return false;
                }

                if (line.Name == "Tooltip4")
                {
                    VanityData vanityData =
                         new VanityData(Color.RoyalBlue, Color.Lerp(Color.White, Color.DeepSkyBlue, 0.65f), Color.Black);

                    VanityEffectClass.DrawMisc(line, vanityData, Color.DeepSkyBlue, Color.Black);
                    return false;

                }
                if (line.Name == "Tooltip5")
                {
                    VanityData vanityData = new VanityData(
                        Color.Gold, Color.Lerp(Color.Gold, Color.White, .5f), Color.Black);
                    VanityEffectClass.DrawMisc(line, vanityData, Color.Gold, Color.Black);
                    return false;
                }
            }
            return true;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.AddLoot<TairitsuItem>();
            itemLoot.AddLoot<HikariItem>();
        }
    }
}
