using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Tiles;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Useables
{
    public class InstaSupremeMatter : LostbeltItemClass
    {
        public virtual int TileRegister => TileType<AutoSupremeStructureTilesMaterializer>();
        public override string AssetPath => AssetHandler.Useables;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void ExSD()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.useTime = Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.SetUpNoUseGraphicItem(false, false);
            Item.createTile = TileRegister;
        }
        public override void AddRecipes()
        {

        }
    }
    public class InstaSupremeSon : InstaSupremeMatter
    {
        public override int TileRegister => TileType<AutoSupremeStructureTilesSon>();

    }
    public class InstaSupremeDeath : InstaSupremeMatter
    {
        public override int TileRegister => TileType<AutoSupremeStructureTilesWhale>();

    }
    public class InstaSupremeNone : InstaSupremeMatter
    {
        public override int TileRegister => TileType<AutoSupremeStructureTilesSacarb>();

    }
    public class InstaSupremeTime : InstaSupremeMatter
    {
        public override int TileRegister => TileType<AutoSupremeStructureTilesOverwatcher>();

    }
    public class InstaSupremeLiving : InstaSupremeMatter
    {
        public override int TileRegister => TileType<AutoSupremeStructureTilesLifebringer>();
    }

}
