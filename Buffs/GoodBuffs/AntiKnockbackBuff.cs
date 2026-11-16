using Terraria;
using Terraria.ModLoader;

namespace LostbeltTraveler.Buffs.GoodBuffs
{
    public class AntiKnockbackBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.noKnockback = true;
        }
    }
}
