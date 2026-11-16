using ContinentOfJourney.Items;
using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Projs.Melee;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Melee
{
    public class DeepToneThrown : ThrownSpearClass
    {
        public override string Texture => GetInstance<DeepTone>().Texture;
        public override void ExSD()
        {
            Item.damage = 71;
            Item.useTime = Item.useAnimation = 35;
            Item.UseSound = LostbeltSoundAssets.Misc_KnifeToss with {MaxInstances = 0, Pitch = 0.5f,PitchVariance=.1f, Variants = [1] };
            Item.knockBack = 3f;
            Item.shootSpeed = 14f;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ProjectileType<DeepToneThrownProj>();
        }
        public override Color MainTooltipColor => Color.DarkOliveGreen;
    }
}
