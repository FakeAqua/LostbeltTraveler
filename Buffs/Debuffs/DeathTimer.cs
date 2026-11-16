using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Buffs.Debuffs
{
    public class DeathTimer : ModBuff
    {
        public static float ExtraDamage = .20f;
        public static int StopTime = 3;
        public override LocalizedText Description => base.Description.WithFormatArgs(ExtraDamage.ToPercent(), StopTime);
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.Lostbelt().inHorror = true;
            if(Main.rand.NextBool(600))
            {
                npc.DelBuff(buffIndex);
            }
        }
        public override void Update(Player player, ref int buffIndex)
        {
            base.Update(player, ref buffIndex);
        }
    }
}
