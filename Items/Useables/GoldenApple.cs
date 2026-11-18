using LostbeltTraveler.Buffs.GoodBuffs;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Useables
{
    public class GoldenApple : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Useables;
        public override void SetStaticDefaults()
        {
            LostbeltList.LegalFoodList.Add(Type);
        }
        public override void ExSD()
        {
            Item.DefaultToFood(16, 16, BuffType<GoldenAppleBuff>(), GetSeconds(60) * 10);
            Item.SetUpRarityPrice(ItemRarityID.LightPurple);
            Item.value = Item.sellPrice(gold: 3);
            Item.Lostbelt().drawBuffIconAndDetail = true;
        }
        public override bool? UseItem(Player player)
        {
            player.AddBuff(BuffID.WellFed3, GetSeconds(60) * 10);
            return base.UseItem(player);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Apple).
                AddIngredient(ItemID.GoldDust, 100).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}
