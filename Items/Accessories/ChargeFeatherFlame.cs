using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class ChargeFeatherFlame : ChargeFeatherBroken
    {
        public int WingTime = 3;
        public float Damage = .05f;
        public float CritChance = 5;
        public override string AssetPath => AssetHandler.Equips;
        public override int MatchWing => ItemID.FlameWings;
        public override float RestoredCharge => .15f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs($"[i:{ItemID.Search.GetName(MatchWing)}]", WingTime, Damage.ToPercent(), CritChance + "%");
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Pink);
            Item.accessory = true;
        }
        public override void UpdateGlobal(Player player)
        {
        }
        public override void UpdateMatchWings(Player player)
        {
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.FireFeather).
                AddIngredient(ItemID.SoulofFlight, 10).
                AddTile(TileID.TinkerersWorkbench).
                Register();
        }
    }
}
