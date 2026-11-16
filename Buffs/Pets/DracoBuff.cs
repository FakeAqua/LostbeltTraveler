using LostbeltTraveler.Projs.Pets;
using Terraria;

namespace LostbeltTraveler.Buffs.Pets
{
    public class DracoBuff : PetsBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref player.Lostbelt().petDraco, ProjectileType<DracoProj>());
        }

    }
}
