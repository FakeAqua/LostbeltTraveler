using LostbeltTraveler.Core;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Players.Dashes;
using LostbeltTraveler.Globals.Systems;
using LostbeltTraveler.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Accessories
{
    public class Tomorrow : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public float Damage = .05f;
        public float Crit = 5f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Damage.ToPercent(), Crit + "%");
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
            player.blackBelt = true;
            player.spikedBoots = 2;
            player.autoJump = true;
            player.ApplyDash(ScarletContent.DashType<TomorrowDash>());
            player.accRunSpeed = 7.25f;
            player.jumpSpeedBoost += 1.6f;
            player.noFallDmg = true;
            player.GetDamage<GenericDamageClass>() += Damage;
            player.GetCritChance<GenericDamageClass>() += Crit;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.MasterNinjaGear).
                AddIngredient(ItemID.AmphibianBoots).
                AddIngredient(ItemID.BoneGlove).
                AddIngredient(ItemID.PutridScent).
                AddIngredient<CrownofSilveryLight>(15).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
