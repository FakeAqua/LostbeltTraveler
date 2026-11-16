using LostbeltTraveler.Projs.Pets;
using Terraria;

namespace LostbeltTraveler.Buffs.Pets
{
    public class SonBuff : PetsBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref player.Lostbelt().petSon, ProjectileType<SonProj>());
        }

    }
}
