using ContinentOfJourney.Items.Flamethrowers;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Projs.Ranged;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Ranged
{
    public class RadiantBlazeDawn : LostbeltWeapon
    {
        public override EnumDamageClass Category => EnumDamageClass.Ranged;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.ForeverNight);
        }
        public override void ExSD()
        {
            Item.damage = 34;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item51;
            Item.SetUpRarityPrice(ItemRarityID.Orange);
            Item.Lostbelt().drawBuffIconAndDetail = true;
            Item.useAmmo = AmmoID.Gel;
            Item.scale = .85f;
            Item.useTime = 5;
            Item.useAnimation = 25;
            Item.shootSpeed = 9f;
            Item.shoot = ProjectileType<RadiantBlazeDawnFlame>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item45 with { MaxInstances = 1 };
            Item.knockBack = 3f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            //Item.HoldoutProjUpdateAim(proj, 0, new Vector2(20, 0));
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-25, 0);
        }
        public override void UseItemFrame(Player player)
        {
            player.NoHeldProjUpdateAim();
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<FT1Sparkthrower>().
                AddIngredient<FT2Wildfire>().
                AddIngredient<FT3Waterthrower>().
                AddIngredient<FT4DragonsFury>().
                AddTile(TileID.DemonAltar).
                Register();
        }
    }
}
