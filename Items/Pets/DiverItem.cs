using LostbeltTraveler.Buffs.Pets;
using LostbeltTraveler.Projs.Pets;

namespace LostbeltTraveler.Items.Pets
{
    public class DiverItem : LostbeltPetItem
    {
        public override int PetProjType => ProjectileType<DiverProj>();
        public override int PetBuffType => BuffType<DiverBuff>();
    }
}
