using LostbeltTraveler.Core.NetCode;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.IDSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Summon
{
    public class ShardSpitter : LostbeltWeapon
    {
        public bool RangerMode = false;
        public override EnumDamageClass Category => EnumDamageClass.Summon;
        public override void SetStaticDefaults()
        {
            ScarletItemIDSets.IsHeldProjItem[Type] = true;
            ExSSD();
        }
        public virtual void ExSSD() { }
        public override void ExSD()
        {
            Item.damage = 146;
            Item.knockBack = 3f;
            Item.shootSpeed = 12f;
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.SetUpRarityPrice(ItemRarityID.Red);
            Item.SetUpNoUseGraphicItem(true);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = Item.useAnimation = 12;
        }
        public override bool CanShoot(Player player)
        {
            return false;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }
        public override void HoldItem(Player player)
        {
            if (!player.IsOwnerSide())
                return;

            if (player.HasProj(Item.shoot))
                return;
            int projDamage = (int)player.GetTotalDamage(Item.DamageType).ApplyTo(Item.damage);
            Projectile proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, Item.shoot, projDamage, Item.knockBack, player.whoAmI);
            proj.originalDamage = projDamage;
            proj.netUpdate = true;
            //ScarletSound(LostbeltSoundAssets.Misc_KnifeExpired, player.Center, 1, 0, -.2f);
        }
    }
}
