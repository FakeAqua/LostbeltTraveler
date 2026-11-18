using ContinentOfJourney.Items;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Projs.Melee;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Melee
{
    public class EvolutionThrown : ThrownSpearClass
    {
        public override string Texture => GetInstance<Evolution>().Texture;
        public override bool HasLegendary => true;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Life);
        }
        public override void ExSD()
        {
            Item.damage = 345;
            Item.useTime = Item.useAnimation = 28;
            Item.UseSound = LostbeltSoundAssets.Evolution_Thrown with { MaxInstances = 0 };
            Item.shootSpeed = 16;
            Item.shoot = ProjectileType<EvolutionThrownProj>();
            Item.SetUpRarityPrice(ItemRarityID.Purple);
        }
        public override Color MainTooltipColor => Color.LightGreen;
    }
}
