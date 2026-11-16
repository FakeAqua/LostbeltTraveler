using LostbeltTraveler.Projs.Pets;
using Terraria;
using Terraria.ModLoader;

namespace LostbeltTraveler.Buffs.Pets
{
    public class WhaleBuff : PetsBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref player.Lostbelt().petWhale, ProjectileType<WhaleProj>());
        }
    }
    public abstract class PetsBuff : ModBuff
    {
        internal static string BuffPath = "LostbeltTraveler/Assets/Texture/Pets/Pet_";
        public override string Texture => BuffPath + GetType().Name;
        public virtual bool IsLightPet => false;
        public virtual int PetProjType => 0;
        public override void Update(Player player, ref int buffIndex)
        {
            bool unused = false;
            player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref unused, PetProjType);
        }
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            if (IsLightPet)
                Main.lightPet[Type] = true;
            else
                Main.vanityPet[Type] = true;
        }
    }
}
