using ContinentOfJourney.Items.Accessories;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Accessories
{
    public class Extinction : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public float Damage = .15f;
        public int Crit = 20;
        public float CritDamage = .10f;
        public static float ExtraDamage = .85f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Damage.ToPercent(), Crit + "%", CritDamage.ToPercent(), ExtraDamage.ToPercent());
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
            player.GetDamage<GenericDamageClass>() += Damage;
            player.GetCritChance<GenericDamageClass>() += Crit;
            player.Lostbelt().critDamageAll += CritDamage;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.DestroyerEmblem).
                AddIngredient<SwordmasterBadge>().
                AddIngredient<BullseyeBadge>().
                AddIngredient<ArchmageBadge>().
                AddIngredient<CounsellorBadge>().
                AddIngredient<ExecutorBadge>().
                AddIngredient<CrownofSilveryLight>(15).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
