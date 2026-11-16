using LostbeltTraveler.Projs.Pets;
using Terraria;

namespace LostbeltTraveler.Buffs.Pets
{
    public class NoneBuff : PetsBuff
    {
        public override bool IsLightPet => true;
        public override void Update(Player player, ref int buffIndex)
        {
            player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref player.Lostbelt().petNone, ProjectileType<NoneProj>());
        }
    }
}
