using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Projs.Melee;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Melee
{
    public class SpearofDarknessThrown : ThrownSpearClass
    {
        public override string Texture => LostbeltItemProj.Item_SpearofDarknessThrown.Path;
        public override bool HasLegendary => false;
        public int StrikeTime = 0;
        public override void ExSD()
        {
            Item.damage = 43;
            Item.useTime = Item.useAnimation = 29;
            Item.UseSound = SoundID.Item103;
            Item.knockBack = 2f;
            Item.shootSpeed = 14f;
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ProjectileType<SpearofDarknessThrownProj>();
        }
        public override Color MainTooltipColor => Color.MediumPurple;
    }
}
