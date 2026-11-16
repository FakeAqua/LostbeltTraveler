using LostbeltTraveler.Globals.Database.IDSets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Buffs.Debuffs
{
    public class ParrySpin : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            ScarletBuffIDSets.IsStatueBuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            base.Update(player, ref buffIndex);
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.Lostbelt().parryTime = GetSeconds(5);
        }
    }
}
