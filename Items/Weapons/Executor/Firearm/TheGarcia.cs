using LostbeltTraveler.Core.NetCode;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Globals.Instances.Items;
using LostbeltTraveler.Projs.Executor.Firearm;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Executor.Firearm
{
    public class TheGarcia : ExecutorWeaponClass
    {
        public override int ExecutionProgress => 3;
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.Firearm;
        public override void ExSSD()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateCopper);
            ScarletItemIDSets.ForceToAutomaticExecute[Type] = true;
        }
        public override void ExSD()
        {
            Item.damage = 20;
            Item.shootSpeed = 19;
            Item.SetUpRarityPrice(ItemRarityID.Orange);
            Item.SetUpNoUseGraphicItem(true);
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = null;
            Item.knockBack = 7f;
            Item.useTime = Item.useAnimation = 45;
            Item.shoot = ProjectileType<TheGarciaHeldProj>();
            Item.Lostbelt().borderlandWeapon = true;
        }
        public override bool CanShoot(Player player)
        {
            return false;
        }
        public override void HoldItem(Player player)
        {
            if (!player.IsOwnerSide())
                return;
            if (player.HasProj(Item.shoot))
                return;
            int projDamage = (int)player.GetTotalDamage<ExecutorDamageClass>().ApplyTo(Item.damage);
            Projectile proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, Item.shoot, 0, Item.knockBack, player.whoAmI);
            proj.originalDamage = projDamage;
            proj.Lostbelt().HasExecutionMechanic = true;
            proj.netUpdate = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Boomstick).
                AddRecipeGroup(LostbeltRecipeGroup.AnyEvilScale, 5).
                AddIngredient(ItemID.IllegalGunParts, 5).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
