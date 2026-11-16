using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Projs.Pets;

namespace LostbeltTraveler.Buffs.Pets

{
    public class DiverBuff : PetsBuff
    {
        public override int PetProjType => ProjectileType<DiverProj>();
    }
}
