using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Buffs.GoodBuffs
{
    public class GoldenAppleBuffEnchanted : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.buffImmune[BuffType<GoldenAppleBuff>()] = true;
            player.Lostbelt().goldenAppleDamageAbsorb = 50;
            player.Lostbelt().lifeRegenPool += 4;
            player.lavaImmune = true;
            player.statDefense += 10;
            if (player.miscCounter % 3 == 0)
            {
                Dust d = Dust.NewDustPerfect(player.ToRandRec() + Vector2.UnitY * 10, DustID.GoldCoin);
                d.velocity = -Vector2.UnitY * 0.5f;
                d.noGravity = true;
                d = Dust.NewDustPerfect(player.ToRandRec() + Vector2.UnitY * 10, DustID.HallowedTorch);
                d.velocity = -Vector2.UnitY * 0.5f;
                d.noGravity = true;
            }

        }
        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
