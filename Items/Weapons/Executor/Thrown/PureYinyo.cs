using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Projs.Executor.Thrown;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Executor.Thrown
{
    public class PureYinyo : ExecutorWeaponClass
    {
        public int AlterType = -1;
        public override int ExecutionProgress => 12;
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.Throw;
        public override void ExSSD()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateWhite);
        }
        public override void ExSD()
        {
            Item.damage = 50;
            Item.SetUpRarityPrice(ItemRarityID.LightPurple);
            Item.SetUpNoUseGraphicItem();
            Item.Lostbelt().drawBuffIconAndDetail = true;
            Item.knockBack = 2f;
            Item.useTime = Item.useAnimation = 31;
            Item.shootSpeed = 19f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shoot = ProjectileType<PureYinyoBlack>();
            Item.Lostbelt().ExecutionProj = ProjectileType<PureYinyoExecution>();
            Item.UseSound = LostbeltSoundAssets.Blunt_Swing;

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //初始化。
            bool useExecution = player.GetExecutionSrike();
            int projID = Item.Lostbelt().ExecutionProj != -1 && useExecution ? Item.Lostbelt().ExecutionProj : AlterType != 0 ? ProjectileType<PureYinyoWhite>() : type;
            if (useExecution)
            {
                Projectile proj = Projectile.NewProjectileDirect(source, position, -Vector2.UnitY * 32f, projID, damage, knockback, player.whoAmI);
                proj.Lostbelt().ExecutionStrike = true;
                player.RemoveExecutionProgress(Type);
                projID = AlterType != 0 ? ProjectileType<PureYinyoWhite>() : type;
            }
            {
                Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, projID, damage, knockback, player.whoAmI);
                proj.Lostbelt().HasExecutionMechanic = true;
                if (AlterType != 0)
                    AlterType = 0;
                else
                    AlterType = -1;
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.LightShard, 2).
                AddIngredient(ItemID.DarkShard, 2).
                AddIngredient(ItemID.SoulofLight, 5).
                AddIngredient(ItemID.SoulofNight, 5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
