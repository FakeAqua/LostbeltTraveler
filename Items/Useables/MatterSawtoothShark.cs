using ContinentOfJourney.Tiles;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Projs.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Useables
{
    public class MatterSawtoothShark : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Useables;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Matter);
            ItemID.Sets.IsChainsaw[Type] = true;
            ScarletItemIDSets.IsHeldProjItem[Type] = true;
        }
        public override void ExSD()
        {
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.SetUpNoUseGraphicItem(true);
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.damage = 888;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = Item.useAnimation = 15;
            Item.shootSpeed = 16f;
            Item.shoot = ProjectileType<MatterSawtoothSharkHeldProj>();
            Item.tileBoost = 10;
            Item.axe = 55;
            Item.knockBack = .5f;
            Item.UseSound = SoundID.Item23;
        }
        public override bool CanUseItem(Player player)
        {
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<FishingCoinEmerial>(30).
                AddTile(TileType<FountainofMatter>()).
                Register();

        }
    }
}
