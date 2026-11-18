using LostbeltTraveler.Core.NetCode;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Items.Materials;
using LostbeltTraveler.Projs.Executor.Firearm;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Executor.Firearm
{
    public class Monocle : ExecutorWeaponClass
    {
        public static int ExecutionPenetrate = 15;
        public static float ExecutionDamageMult = 1.5f;
        public override int ExecutionProgress => 12;
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.Firearm;
        public override void ExSSD()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateGolden);
            ScarletItemIDSets.IsHeldProjItem[Type] = true;
            ScarletItemIDSets.GrantsBoosterAfterSon[Type] = true;
        }
        public override void ExSD()
        {
            Item.damage = 2415;
            Item.shootSpeed = 19;
            Item.SetUpRarityPrice(ItemRarityID.Red);
            Item.SetUpNoUseGraphicItem(true);
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = null;
            Item.knockBack = 7f;
            Item.useTime = Item.useAnimation = 42;
            Item.crit = 46;
            Item.shoot = ProjectileType<MonocleHeldProj>();
            Item.Lostbelt().borderlandWeapon = true;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D tex = TextureAssets.Projectile[ProjectileType<MonocleHeldProj>()].Value;
            Rectangle frame = tex.Frame();
            Vector2 ori = frame.Size() / 2;
            Vector2 drawPos = Item.Bottom - Main.screenPosition - new Vector2(0, ori.Y);
            spriteBatch.Draw(tex, drawPos, frame, lightColor, rotation, ori, scale, 0, 0);
            return false;
        }
        public override bool CanShoot(Player player)
        {
            return !player.HasProj(Item.shoot);
        }
        public override void HoldItem(Player player)
        {
            if (!player.IsOwnerSide())
                return;
            if (player.HasProj(Item.shoot) || Main.dedServ)
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
                AddIngredient<Headsplosion>().
                AddIngredient(ItemID.ShadowbeamStaff).
                AddIngredient<CrownofSilveryLight>(15).
                AddIngredient(ItemID.IllegalGunParts, 15).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
