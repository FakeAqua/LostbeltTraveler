using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Buffs.GoodBuffs;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Useables
{
    public class GoldenAppleEnchanted : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Useables;
        public override void SetStaticDefaults()
        {
            LostbeltList.LegalFoodList.Add(Type);
        }
        public override void ExSD()
        {
            Item.DefaultToFood(16, 16, BuffType<GoldenAppleBuffEnchanted>(), GetSeconds(60) * 60);
            Item.SetUpRarityPrice(ItemRarityID.Red);
            Item.value = Item.sellPrice(platinum: 3);
            Item.Lostbelt().drawBuffIconAndDetail = true;
        }
        public override bool? UseItem(Player player)
        {
            player.AddBuff(BuffID.WellFed3, GetSeconds(60) * 60);
            return base.UseItem(player);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<GoldenApple>().
                AddIngredient<EternalBar>(50).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
