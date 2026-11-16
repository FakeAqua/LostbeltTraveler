using LostbeltTraveler.Buffs.Pets;
using LostbeltTraveler.Projs.Pets;

namespace LostbeltTraveler.Items.Pets
{
    public class SquidItem : LostbeltPetItem
    {
        public override int PetProjType => ProjectileType<SquidProj>();
        public override int PetBuffType => BuffType<SquidBuff>();
    }
}
