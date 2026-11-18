using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Projs.Melee;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Melee
{
    public class AzureFrostmark : ThrownSpearClass
    {
        public override bool NotHomewardJourneySpear => true;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Frost);
        }

        public override void ExSD()
        {
            Item.damage = 20;
            Item.rare = ItemRarityID.Green;
            Item.useTime = Item.useAnimation = 40;
            Item.knockBack = 3f;
            Item.shootSpeed = 11f;
            Item.shoot = ProjectileType<AzureFrostmarkProj>();
            Item.UseSound = LostbeltSoundAssets.HymnFireball_Release with { MaxInstances = 1, Pitch = -0.20f, PitchVariance = 0.15f, Volume = 0.8f };
        }
        public override Color MainTooltipColor => Color.RoyalBlue;
    }
}
