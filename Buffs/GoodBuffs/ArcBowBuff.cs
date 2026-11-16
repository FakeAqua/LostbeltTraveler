using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Methods;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Buffs.GoodBuffs
{
    public class ArcBowBuff : ModBuff
    {
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            if (Main.LocalPlayer.Lostbelt().arcBowLevel > 1)
            {
                tip = tip.Replace("25", "50");
            }
        }
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            ScarletBuffIDSets.IsStatueBuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
        }
    }
}
