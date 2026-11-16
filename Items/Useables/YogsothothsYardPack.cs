using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Items.Armor.Reaper;
using LostbeltTraveler.Items.Vanity;
using LostbeltTraveler.Items.Vanity.Yards;
using LostbeltTraveler.Items.Weapons.Executor.ColdSteel;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Useables
{
    public class YogsothothsYardPack : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Useables;
        public override void SetStaticDefaults()
        {
            ItemID.Sets.OpenableBag[Type] = true;
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateWhite);
        }
        public override void ExSD()
        {
            Item.rare = RarityType<VanityEffectClass>();
            Item.consumable = true;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.AddCommon(ItemType<CrimsonScythe>());
            itemLoot.AddCommon(ItemType<ReaperHead>());
            itemLoot.AddCommon(ItemType<ReaperBody>());
            itemLoot.AddCommon(ItemType<ReaperLegs>());
            itemLoot.AddCommon(ItemType<LeafMaidItem>());
            itemLoot.AddCommon(ItemType<RedDragonItem>());
            itemLoot.AddCommon(ItemType<CantoneseGirlItem>());
        }
    }
}
