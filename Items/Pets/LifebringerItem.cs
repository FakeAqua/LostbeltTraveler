using LostbeltTraveler.Buffs.Pets;
using LostbeltTraveler.Projs.Pets;

namespace LostbeltTraveler.Items.Pets
{
    internal class LifeWormItem : LostbeltPetItem
    {
        public override int PetProjType => ProjectileType<LifeWormProj>();
        public override int PetBuffType => BuffType<LifeWormBuff>();
    }
}
