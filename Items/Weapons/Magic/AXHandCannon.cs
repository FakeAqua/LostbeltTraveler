using ContinentOfJourney.Items;
using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Core.NetCode;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Projs.Magic;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Magic
{
    public class AXCannon : LostbeltWeapon
    {
        public override EnumDamageClass Category => EnumDamageClass.Magic;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void ExSD()
        {
            Item.damage = 234;
            Item.SetUpNoUseGraphicItem(true);
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 3;
            Item.shoot = ProjectileType<AXCannonHeldProj>();
            Item.useTime = Item.useAnimation = 12;
            Item.mana = 12;
            Item.shootSpeed = 16;
        }
        public override bool CanShoot(Player player)
        {
            return false;
        }
        public override void HoldItem(Player player)
        {
            if (!player.IsOwnerSide())
                return;
            if (player.HasProj<AXCannonHeldProj>(out int projID))
                return;
            int projDamage = (int)player.GetTotalDamage(Item.DamageType).ApplyTo(Item.damage);
            Projectile proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, projID, 0, Item.knockBack, player.whoAmI);
            proj.originalDamage = projDamage;
            proj.netUpdate = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MegaCanon>().
                AddIngredient(ItemID.ChargedBlasterCannon).
                AddIngredient<EssenceofMatter>(5).
                AddTile(FinalAnvilTile).
                Register();

        }
    }
}
