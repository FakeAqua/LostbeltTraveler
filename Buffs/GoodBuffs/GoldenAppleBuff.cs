using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Buffs.GoodBuffs
{
    public class GoldenAppleBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.Lostbelt().goldenAppleDamageAbsorb = 20;
            player.Lostbelt().lifeRegenPool += 2;
            player.statDefense += 8;
            if (player.miscCounter % 3 == 0)
            {
                Dust d = Dust.NewDustPerfect(player.ToRandRec() + Vector2.UnitY * 10, DustID.GoldCoin);
                d.velocity = -Vector2.UnitY * 0.5f;
                d.noGravity = true;
            }
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
