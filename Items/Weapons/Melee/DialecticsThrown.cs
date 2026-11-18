using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Projs.Melee;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using static LostbeltTraveler.Projs.Melee.DialecticsThrownProj;

namespace LostbeltTraveler.Items.Weapons.Melee
{
    public class DialecticsThrown : ThrownSpearClass
    {
        public override string Texture => LostbeltItemProj.Item_DialecticsThrown.Path;
        public override bool HasLegendary => true;
        public int UsePhase = 0;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Matter);
            ItemID.Sets.BonusAttackSpeedMultiplier[Type] = 0.33f;
        }
        public override void ExSD()
        {
            Item.damage = 140;
            Item.SetUpRarityPrice(ItemRarityID.Purple);
            //不需要声音，在shoot里手动控制
            Item.UseSound = null;
            Item.shootSpeed = 17f;
            Item.knockBack = 8f;
            Item.autoReuse = true;
            Item.useTime = Item.useAnimation = 30;
            Item.shoot = ProjectileType<DialecticsThrownProj>();
        }
        public override Color MainTooltipColor => Color.Lerp(Color.RoyalBlue, Color.AliceBlue, 0.5f);
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var curStyle = UsePhase switch
            {
                0 => WaveStyle.Sin,
                1 => WaveStyle.Square,
                2 => WaveStyle.ParaSquare,
                _ => WaveStyle.Paraline,
            };
            if (curStyle != WaveStyle.Paraline)
                ScarletSound(LostbeltSoundAssets.Dialectics_Throw, player.Center, .7f, 0, .5f);
            else
                ScarletSound(LostbeltSoundAssets.Atom_Strike, player.Center, .7f, 1, variantType: 1);
            int realDamage = curStyle == WaveStyle.Square ? damage * (3) : damage;
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, realDamage, knockback, player.whoAmI);
            ((DialecticsThrownProj)proj.ModProjectile).CurWaveStyle = curStyle;
            UsePhase++;
            if (UsePhase > 3)
                UsePhase = 0;
            return false;
        }
    }
}
