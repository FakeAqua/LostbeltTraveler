using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Projs.Executor.Firearm;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Executor.Firearm
{
    public class TheCompanion : ExecutorWeaponClass
    {
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.Firearm;
        public override void ExSSD()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateCopper);
        }
        public override void ExSD()
        {
            Item.damage = 42;
            Item.shootSpeed = 13;
            Item.SetUpRarityPrice(ItemRarityID.Green);
            Item.SetUpNoUseGraphicItem(true);
            Item.Lostbelt().drawBuffIconAndDetail = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = null;
            Item.knockBack = 1.5f;
            Item.useTime = Item.useAnimation = 32;
            Item.shoot = ProjectileType<TheCompanionHeldProj>();
            Item.Lostbelt().borderlandWeapon = true;
        }
        public override bool CanShoot(Player player)
        {
            return false;
        }
        public override void HoldItem(Player player)
        {
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
                AddIngredient(ItemID.FlintlockPistol).
                AddRecipeGroup(RecipeGroupID.IronBar, 10).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
