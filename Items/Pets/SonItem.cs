using LostbeltTraveler.Buffs.Pets;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Projs.Pets;

namespace LostbeltTraveler.Items.Pets
{
    public class SonItem : LostbeltPetItem
    {
        public override int PetProjType => ProjectileType<SonProj>();
        public override int PetBuffType => BuffType<SonBuff>();
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateWhite);
        }

    }
}
