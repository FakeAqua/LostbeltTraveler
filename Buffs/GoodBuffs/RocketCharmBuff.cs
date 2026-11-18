using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Items.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Buffs.GoodBuffs
{
    public class RocketCharmBuff : ModBuff
    {
        public override LocalizedText Description => base.Description.WithFormatArgs(RocketCharm.ExtraAP);
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            ScarletBuffIDSets.IsStatueBuff[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetArmorPenetration<ExecutorDamageClass>() += player.Lostbelt().blackKeyReduceDefense;
        }
    }
}
