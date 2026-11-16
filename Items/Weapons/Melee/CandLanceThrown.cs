using ContinentOfJourney.Items;
using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Projs.Melee;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Melee
{
    public class CandLanceThrown : ThrownSpearClass
    {
        public override string Texture => GetInstance<Candlance>().Texture;
        public override void ExSD()
        {
            Item.damage = 32;
            Item.shoot = ProjectileType<CandLanceThrownProj>();
            Item.shootSpeed = 12f;
            Item.rare = ItemRarityID.Orange;
            Item.knockBack = 2f;
            Item.UseSound = LostbeltSoundAssets.Misc_MagicStaffFire with { MaxInstances = 0, Pitch = 0.1f, Volume = 0.18f, PitchVariance = 0.2f };
            Item.useTime = Item.useAnimation = 38;
            Item.Lostbelt().drawBuffIconAndDetail = true;
        }
        public override Color MainTooltipColor => Color.SkyBlue;
    }
}
