using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Projs.Executor.Assistance;
using LostbeltTraveler.Projs.Magic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Magic
{
    public class GhostKnife : LostbeltWeapon
    {
        public override EnumDamageClass Category => EnumDamageClass.Magic;
        public static int MarkGhostKnifeAttackSpeed = 30;
        public static int MarkGhostKnifeAttackDamage = 28;
        public override void ExSD()
        {
            Item.shootSpeed = 16;
            Item.damage = MarkGhostKnifeAttackDamage * 2;
            Item.useTime = Item.useAnimation = MarkGhostKnifeAttackSpeed / 2;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shoot = ProjectileType<GhostKnifeProj>();
            Item.mana = 10;
            Item.crit = 16;
            Item.knockBack = 2;
            Item.SetUpRarityPrice(ItemRarityID.LightPurple);
            Item.SetUpNoUseGraphicItem();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            proj.Lostbelt().HasExecutionMechanic = true;
            //QuickSpawnMark(Type, Item.Lostbelt().ExecutionProj, player, source, position);
            return false;
        }
        public static void QuickSpawnMark(int weaponID, int markID, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position)
        {
            bool exe = player.GetExecutionSrike();
            if (!exe)
                return;
            //场上如果存在其余处死标记，立刻删除
            foreach (var p in Main.ActiveProjectiles)
            {
                if (p.active && p.owner == player.whoAmI && p.ModProjectile is KnifeMarkClass && p.type != markID)
                {
                    p.Kill();
                }
            }
            if (!player.HasProj(markID))
            {
                Vector2 targetVector = player.Center.GetNormalVector2(Main.MouseWorld);
                Projectile mark = Projectile.NewProjectileDirect(source, position - targetVector * 80f, Vector2.Zero, markID, 0, 0, player.whoAmI);
                player.RemoveExecutionProgress(weaponID);
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.SpectreBar, 12).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
