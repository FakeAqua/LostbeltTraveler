using LostbeltTraveler.Buffs.Pets;
using LostbeltTraveler.Projs.Pets;

namespace LostbeltTraveler.Items.Pets
{
    public class WatcherItem : LostbeltPetItem
    {
        public override int PetProjType => ProjectileType<WatcherProj>();
        public override int PetBuffType => BuffType<WatcherBuff>();
    }
}
