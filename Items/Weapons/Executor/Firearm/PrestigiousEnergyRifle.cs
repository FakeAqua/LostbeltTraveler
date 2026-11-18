using LostbeltTraveler.Core.NetCode;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Projs.Executor.Firearm;
using Terraria;
using Terraria.ID;


namespace LostbeltTraveler.Items.Weapons.Executor.Firearm
{
    public class PrestigiousEnergyRifle : ExecutorWeaponClass
    {
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.Firearm;
        public override int ExecutionProgress => 15;
        public override void ExSSD()
        {
            ScarletItemIDSets.ForceToTacticalExecute[Type] = true;
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Gunwitch);
        }
        public override void ExSD()
        {
            Item.damage = 324;
            Item.shootSpeed = 13;
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.SetUpNoUseGraphicItem(true);
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = null;
            Item.knockBack = 1.5f;
            Item.useTime = Item.useAnimation = 16;
            Item.shoot = ProjectileType<PrestigiousEnergyRifleHeldProj>();
        }
        public override bool CanShoot(Player player)
        {
            return false;
        }
        public override void HoldItem(Player player)
        {
            if (player.HasProj(Item.shoot) || player.HasProj(ProjectileType<PrestigiousEnergyRifleBurstHeldProj>()) || !player.IsOwnerSide())
                return;
            int projDamage = (int)player.GetTotalDamage<ExecutorDamageClass>().ApplyTo(Item.damage);
            Projectile proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, Item.shoot, 0,
                Item.knockBack, player.whoAmI);
            proj.originalDamage = projDamage;
            proj.Lostbelt().HasExecutionMechanic = true;
            proj.netUpdate = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<OutlawSniper>().
                AddIngredient(ItemID.BrokenHeroSword, 1).
                AddIngredient(ItemID.DefenderMedal, 100).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
