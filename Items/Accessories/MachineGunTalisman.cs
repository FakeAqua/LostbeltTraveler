using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Methods;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class MachineGunTalisman : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public int HealAmt = 16;
        public float DamageReduce = .5f;
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Cyan);
            Item.Lostbelt().drawBuffIconAndDetail = true;
            Item.accessory = true;
        }
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(HealAmt, ConceptualBlackKey.BuffTime);
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().blackKeyHeal = HealAmt;
            player.Lostbelt().blackKeyDefenseBuff = DamageReduce;
            player.Lostbelt().arcBowLevel = 2;

        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ArcBow>().
                AddIngredient(ItemID.ShroomiteBar, 10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
