using LostbeltTraveler.Buffs.Pets;
using LostbeltTraveler.Projs.Pets;

namespace LostbeltTraveler.Items.Pets
{
    public class ShadowItem : LostbeltPetItem
    {
        public override int PetProjType => ProjectileType<ShadowProj>();
        public override int PetBuffType => BuffType<ShadowBuff>();
    }
}
