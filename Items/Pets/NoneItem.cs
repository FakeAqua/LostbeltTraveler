using LostbeltTraveler.Buffs.Pets;
using LostbeltTraveler.Projs.Pets;

namespace LostbeltTraveler.Items.Pets
{
    public class NoneItem : LostbeltPetItem
    {
        public override int PetProjType => ProjectileType<NoneProj>();
        public override int PetBuffType => BuffType<NoneBuff>();
    }
}
