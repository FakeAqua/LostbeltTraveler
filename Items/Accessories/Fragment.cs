using ContinentOfJourney.NPCs;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class Fragment : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public float DR = .29f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(DR.ToPercent());
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, Globals.Database.Enums.ShinyRarityType.Final);
        }
        public override void ExSD()
        {
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.brainOfConfusionItem = Item;
            player.shinyStone = true;
            player.Lostbelt().royalGel = true;
            player.npcTypeNoAggro[535] = true;
            player.npcTypeNoAggro[NPCID.KingSlime] = true;
            player.npcTypeNoAggro[NPCID.QueenSlimeBoss] = true;
            player.npcTypeNoAggro[NPCID.QueenSlimeMinionBlue] = true;
            player.npcTypeNoAggro[NPCID.QueenSlimeMinionPink] = true;
            player.npcTypeNoAggro[NPCID.QueenSlimeMinionPurple] = true;
            player.npcTypeNoAggro[NPCType<FluorescentSlime>()] = true;
            player.npcTypeNoAggro[NPCType<PrototypeSlime>()] = true;
            player.npcTypeNoAggro[NPCType<ShadowSlime>()] = true;
            if (player.Lostbelt().wormScarf)
                player.endurance += (DR - .17f);
            else
                player.endurance += DR;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.BrainOfConfusion).
                AddIngredient(ItemID.WormScarf).
                AddIngredient(ItemID.SporeSac).
                AddIngredient(ItemID.ShinyStone).
                AddIngredient(ItemID.RoyalGel).
                AddIngredient<CrownofSilveryLight>(15).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
