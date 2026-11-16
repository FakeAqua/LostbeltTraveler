using LostbeltTraveler.Globals.Database.List;
using Terraria;
using Terraria.ModLoader;

namespace LostbeltTraveler.Buffs.GoodBuffs
{
    public abstract class ExecutorWhipBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            LostbeltList.ExecutorWhip.Add(Type);
        }
    }

    public class StarofHopeBuff : ExecutorWhipBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            base.Update(player, ref buffIndex);
        }
    }
}
