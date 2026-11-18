using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Armor.GunWitch
{
    [AutoloadEquip(EquipType.Body)]
    public class GunWitchBody : LostbeltArmor
    {
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, Globals.Database.Enums.ShinyRarityType.Gunwitch);
            base.SetStaticDefaults();
        }
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.defense = 22;
        }
        public int Crit = 25;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Crit + "%");
        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<ExecutorDamageClass>() += Crit;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.DefenderMedal, 15).
                AddTile(TileID.WorkBenches).
                DisableDecraft().
                Register();
        }
    }
}
