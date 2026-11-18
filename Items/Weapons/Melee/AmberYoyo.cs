using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Projs.Melee;
using LostbeltTraveler.Rarity.RarityDrawHandler;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Melee
{
    public abstract class GemYoyoWeapon : LostbeltWeapon
    {
        public override EnumDamageClass Category => EnumDamageClass.Melee;
        public virtual Vector2 TargetBuffer => new Vector2(Item.damage, 0);
        public override void ExSD()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = Item.useAnimation = 40;
            Item.knockBack = 1.0f;
            Item.SetUpRarityPrice(ItemRarityID.LightRed);
            Item.SetUpNoUseGraphicItem(true);
        }
        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            if (!player.Lostbelt().gemYoyoBag)
                return;
            crit += TargetBuffer.Y;
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (!player.Lostbelt().gemYoyoBag)
                return;
            //必须手动转换，不然会按照int进行加成
            float Buff = (float)((float)(Item.damage + TargetBuffer.X) / (float)Item.damage);
            damage *= Buff;
        }
        public override void PostDrawTooltipLine(DrawableTooltipLine line)
        {
            if (!Main.LocalPlayer.Lostbelt().gemYoyoBag)
                return;
            if (line.Mod != "Terraria")
                return;
            if (line.Name == "CritChance" || line.Name == "Damage")
                RarityDrawHelper.DrawCustomTooltipLine(line, Color.White, Color.Black, Color.White, 1f);
            base.PostDrawTooltipLine(line);
        }
    }
    public class AmberYoyo : GemYoyoWeapon
    {
        public override void ExSD()
        {
            base.ExSD();
            Item.damage = 13;
            Item.shootSpeed = 10f;
            Item.shoot = ProjectileType<AmberYoyoProj>();
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Amber).
                AddIngredient(ItemID.Topaz, 8).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
