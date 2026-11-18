using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Projs.Melee;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Melee
{
    public class ShroomiteFork : LostbeltWeapon
    {
        public static int MaxChargeTime = 2;
        public float ChargeProgress = 0;
        public float Timer = 0;
        public int BaseDamage = 145;
        public override void ExSD()
        {
            Item.damage = BaseDamage;
            Item.SetUpNoUseGraphicItem(false);
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = Item.useTime = 30;
            Item.UseSound = LostbeltSoundAssets.Misc_KnifeToss with { Variants = [2] };
            Item.shootSpeed = 16f;
            Item.shoot = ProjectileType<ShroomiteForkProj>();
        }
        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            crit += (int)(Lerp(0, 30, ChargeProgress));
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            // 必须手动转换，不然会按照int进行加成
            float Buff = (float)((float)(BaseDamage * Lerp(1f, 3f, ChargeProgress)) / (float)BaseDamage);
            damage *= Buff;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Timer = 0;
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            int pene = (int)(Lerp(1, 8, ChargeProgress));
            int slashTime = (int)(Lerp(1, 4, ChargeProgress));
            proj.ai[1] = slashTime;
            proj.penetrate = pene;

            return false;
        }
        public override void HoldItem(Player player)
        {
            ChargeProgress = Utils.GetLerpValue(0, GetSeconds(MaxChargeTime), Timer, true);
            Timer++;
            if (Main.rand.NextFloat() > ChargeProgress)
                return;
            //ECSParticle.ShinyCrossStarECS(player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, 0), Vector2.UnitY, Color.LightSkyBlue, 40, 1, 1, .2f);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.ShroomiteBar, 16).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
