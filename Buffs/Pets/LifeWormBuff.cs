using LostbeltTraveler.Projs.Pets;

namespace LostbeltTraveler.Buffs.Pets
{
    public class LifeWormBuff : PetsBuff
    {
        public override int PetProjType => ProjectileType<LifeWormProj>();
    }
}
