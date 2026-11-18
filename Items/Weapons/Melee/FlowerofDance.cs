using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Projs.Melee;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Melee
{
    public class FlowerofDance : LostbeltWeapon
    {
        public override EnumDamageClass Category => EnumDamageClass.Melee;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Donator);
        }
        public override void ExSD()
        {
            Item.damage = 145;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3;
            Item.shootSpeed = 16;
            Item.UseSound = LostbeltSoundAssets.Misc_KnifeToss with { Pitch = 0.5f, Variants = [2] };
            Item.shoot = ProjectileType<FlowerofDanceProj>();
            Item.SetUpNoUseGraphicItem();
            Item.Lostbelt().ItemBelongTo = EnumItemOwner.Donator;
            Item.Lostbelt().OwnerName = "曦";
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = -2; i < 2; i++)
            {

                float baseRad = ToRadians(15f * i);
                Vector2 这个刀就不能再小点吗 = velocity.RotatedBy(Main.rand.NextFloat(ToRadians(-1f), ToRadians(1f)) + baseRad).ToSafeNormalize();
                Vector2 vel = 这个刀就不能再小点吗 * Main.rand.NextFloat(19f, 22f);
                Projectile proj = Projectile.NewProjectileDirect(source, position, vel, type, damage, knockback, player.whoAmI);
                if (i == 0)
                    proj.ai[2] = 1;
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SolarFlareScoria>(12).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
