using LostbeltTraveler.Projs.Pets;
using Terraria;

namespace LostbeltTraveler.Buffs.Pets
{
    public class ShadowBuff : PetsBuff
    {
        public override bool IsLightPet => true;
        public override void Update(Player player, ref int buffIndex)
        {
            player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref player.Lostbelt().petShadow, ProjectileType<ShadowProj>());
        }

    }
}
