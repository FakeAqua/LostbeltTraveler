using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Buffs.GoodBuffs
{
    public class CycleMadnessBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
