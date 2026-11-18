using LostbeltTraveler.Items.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Buffs.Debuffs
{
    public class CursedHeavyUpgrade : ModBuff
    {
        public static int BadLifeRegenEnemy = 100;
        public static int BadLifeRegenPlayer = 10;
        public static int MaxStack = 10;
        public override LocalizedText Description => base.Description.WithFormatArgs(MaxStack);
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            base.ModifyBuffText(ref buffName, ref tip, ref rare);
        }
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.Lostbelt().badLifeRegenPool += BadLifeRegenEnemy * 2 * npc.Lostbelt().cursedHeavyStack;
            npc.Lostbelt().inCursedHeavy = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            int stack = player.Lostbelt().desterrannachtStack;
            //咒厄的烧血需要独立写出来，不然一直大跳字有点难绷了
            if (player.miscCounter % 8 == 0)
            {
                if (player.statLife > 0)
                    player.statLife -= 2 * stack;
            }
            player.Lostbelt().critDamageAll += stack * Desterrennacht.CritDamage;
            player.endurance += stack * Desterrennacht.DR;
            base.Update(player, ref buffIndex);
        }

    }
}
