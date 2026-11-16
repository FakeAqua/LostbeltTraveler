using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Projs.Summon;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Summon
{
    public class MirrorWhip : LostbeltWeapon
    {
        public override EnumDamageClass Category => EnumDamageClass.Summon;
        public override void SetStaticDefaults()
        {
        }
        public override void ExSD()
        {
            Item.SetUpNoUseGraphicItem(true);
            Item.SetUpRarityPrice(ItemRarityID.Orange);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = Item.useAnimation = 24;
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.knockBack = 2;
            Item.UseSound = SoundID.Item152;
            Item.damage = 20;
            Item.shootSpeed = 12f;
            Item.shoot = ProjectileType<MirrorWhipProj>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
    }
}
