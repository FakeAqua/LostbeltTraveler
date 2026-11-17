using ContinentOfJourney.Items.Flamethrowers;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Players.InnerCooldown.Methods;
using LostbeltTraveler.Projs.Ranged;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Ranged
{
    public class TerraFlamethrower : LostbeltWeapon
    {
        public override EnumDamageClass Category => EnumDamageClass.Ranged;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Life);
        }
        public override void ExSD()
        {
            Item.damage = 60;
            Item.useTime = Item.useAnimation = 6;
            Item.knockBack = 5f;
            Item.SetUpRarityPrice(ItemRarityID.LightRed);
            Item.SetUpNoUseGraphicItem(true, false);
            Item.Lostbelt().drawBuffIconAndDetail = true;
            Item.useAmmo = AmmoID.Gel;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ProjectileType<TerraFlamethrowerHeldProj>();
            Item.shootSpeed = 12f;
        }
        public override bool CanUseItem(Player player)
        {
            return !player.HasProj(Item.shoot) && !player.HasInnerCooldown(InnerCooldownType.TerraFlamethrowerReuse);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI, ai0: 9);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<RadiantBlazeDawn>().
                AddIngredient<FT7SporeGun>().
                AddIngredient(ItemID.BrokenHeroSword).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
