using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Projs.Executor.ColdSteel;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Executor.ColdSteel
{
    public class BambooBow : ExecutorWeaponClass
    {
        public override int ExecutionProgress => 12;
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.ColdSteel;
        public override void ExSD()
        {
            Item.damage = 26;
            Item.useTime = Item.useAnimation = 36;
            Item.knockBack = 2;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.SetUpRarityPrice(ItemRarityID.Blue);
            Item.shoot = ProjectileType<BambooBowArrow>();
            Item.shootSpeed = 9f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Lostbelt().GeneralWeaponBuffTimer > 0)
            {
                player.Lostbelt().GeneralWeaponBuffTimer--;
                ScarletSound(SoundID.Item14, player.Center);
            }
            else
            {
                ScarletSound(SoundID.Item11, player.Center);
            }

            bool exe = player.GetExecutionSrike();
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            proj.Lostbelt().HasExecutionMechanic = player.Lostbelt().GeneralWeaponBuffTimer == 0;
            proj.extraUpdates += player.Lostbelt().GeneralWeaponBuffTimer == 0 ? 0 : 1;
            if (exe)
            {
                ScarletSound(LostbeltSoundAssets.Air_HeavyFlow, player.Center);
                for (int i = 0; i < 16; i++)
                {
                    Vector2 pos = player.ToRandRec();
                    ECSParticle.ShinyCrossStarECS(pos, -Vector2.UnitY * 2f, RandLerpColor(Color.LimeGreen, Color.ForestGreen), 40, 1, 0.4f);
                }
                player.Lostbelt().GeneralWeaponBuffTimer = 12;
                player.RemoveExecutionProgress(Type);
            }
            return false;
        }
        public override float UseTimeMultiplier(Player player)
        {
            float mult = 1f;
            if (player.Lostbelt().GeneralWeaponBuffTimer > 0)
                mult = .5f;
            return mult;

        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1f, 0);
        }

        public override void UseItemFrame(Player player)
        {
            player.NoHeldProjUpdateAim();
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.BambooBlock, 30).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}
