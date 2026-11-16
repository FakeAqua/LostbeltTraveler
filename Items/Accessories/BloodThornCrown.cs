using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Methods;
using LostbeltTraveler.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Accessories
{
    [AutoloadEquip(EquipType.Head)]
    public class BloodThornCrown : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public static float DR = .25f;
        public static float DamageReduce = .25f;
        public static int Crit = 25;
        public static float DamageMult = .5f;
        public static int MaxHitCounter = 3;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(DR.ToPercent(), DamageReduce.ToPercent(), Crit + "%", DamageMult.ToPercent(), MaxHitCounter);
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.ScarletRed);
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
            ArmorIDs.Head.Sets.IsTallHat[Item.headSlot] = true;
        }
        public override void ExSD()
        {
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Blue);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().bloodThronCrown = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CrownofSilveryLight>(15).
                AddIngredient<TankOfThePastHallow>(15).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
