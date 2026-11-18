using LostbeltTraveler.Projs.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Melee
{
    public class TheMars : ThrownSpearClass
    {
        public override bool NotHomewardJourneySpear => true;
        public override void ExSD()
        {
            Item.damage = 164;
            Item.useTime = Item.useAnimation = 35;
            Item.knockBack = .5f;
            Item.UseSound = LostbeltSoundAssets.TheMars_Toss with { MaxInstances = 0, Volume = 0.85f };
            //投射出去之后会被减速
            Item.shootSpeed = 24f;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.shoot = ProjectileType<TheMarsProj>();
            Item.rare = ItemRarityID.Red;
            Item.Lostbelt().drawBuffIconAndDetail = true;
        }
        public override Color MainTooltipColor => Color.LightGray;
    }
}
