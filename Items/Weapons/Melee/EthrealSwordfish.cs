using ContinentOfJourney.Tiles;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Items.Useables;
using LostbeltTraveler.Projs.Melee;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Melee
{
    public class EthrealSwordfish : LostbeltWeapon
    {
        public override EnumDamageClass Category => EnumDamageClass.Melee;
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SkipsInitialUseSound[Type] = true;
            ItemID.Sets.Spears[Type] = true;
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Eternity);
        }
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.damage = 154;
            Item.SetUpNoUseGraphicItem(true);
            Item.UseSound = SoundID.Item1;
            Item.knockBack = .5f;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 2.5f;
            Item.shoot = ProjectileType<EthrealSwordfishHeldProj>();
        }
        public override bool CanUseItem(Player player)
        {
            return !player.HasProj(Item.shoot);
        }
        public override bool? UseItem(Player player)
        {
            // Because we're skipping sound playback on use animation start, we have to play it ourselves whenever the item is actually used.
            if (!Main.dedServ && Item.UseSound.HasValue)
            {
                SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
            }
            return null;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<FishingCoinEmerial>(30).
                AddTile(TileType<FountainofTime>()).
                Register();

            CreateRecipe().
                AddIngredient(ItemID.GoldenFishingRod).
                AddIngredient<SunWorm>(300).
                DisableDecraft().
                AddTile(TileType<FountainofTime>()).
                Register();
        }

    }
}
