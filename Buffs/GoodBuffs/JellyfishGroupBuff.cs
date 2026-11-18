using LostbeltTraveler.Globals.Players;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Buffs.GoodBuffs
{
    public class JellyfishGroupBuff : ModBuff
    {
        public static int JellyfishGroupBadLifeRegenEnemy = 12;
        public float BlueJellyfishDamage = .10f;
        public int BlueJellyfishCrit = 10;
        public int BlueJellyfishAP = 5;
        public int PinkJellyfishDefense = 4;
        public float PinkJellyfishDR = .04f;
        public int PinkJellyfishLifeRegenSpeed = 2;
        public override LocalizedText Description => base.Description.WithFormatArgs(
            BlueJellyfishDamage.ToPercent(),
            BlueJellyfishCrit + "%",
            "+" + BlueJellyfishAP,
            "+" + PinkJellyfishDefense,
            PinkJellyfishDR.ToPercent(),
            PinkJellyfishLifeRegenSpeed.ToLifeRegenFormat());
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            switch (player.Lostbelt().jellyfishGroupIndex)
            {
                case NPCID.BlueJellyfish:
                    player.GetDamage<GenericDamageClass>() += BlueJellyfishDamage;
                    player.GetCritChance<GenericDamageClass>() += BlueJellyfishCrit;
                    player.GetArmorPenetration<GenericDamageClass>() += BlueJellyfishAP;
                    break;
                case NPCID.GreenJellyfish:
                    player.Lostbelt().infiniteFlightTime = true;
                    player.runAcceleration *= 1.1f;
                    break;
                case NPCID.PinkJellyfish:
                    player.statDefense += PinkJellyfishDefense;
                    player.endurance += PinkJellyfishDR;
                    player.Lostbelt().lifeRegenPool += PinkJellyfishLifeRegenSpeed;
                    break;
            }
        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            Player localPlayer = Main.LocalPlayer;
            LostbeltPlayer modPlayer = localPlayer.Lostbelt();
            int type = modPlayer.jellyfishGroupIndex;
            string value;
            switch (type)
            {
                case NPCID.BlueJellyfish:
                    value = this.GetLocalizedValue("Blue");
                    buffName = buffName + "-" + value;
                    rare = ItemRarityID.Cyan;
                    break;
                case NPCID.GreenJellyfish:
                    value = this.GetLocalizedValue("Green");
                    buffName = buffName + "-" + value;
                    rare = ItemRarityID.Lime;
                    break;
                case NPCID.PinkJellyfish:
                    value = this.GetLocalizedValue("Pink");
                    buffName = buffName + "-" + value;
                    rare = ItemRarityID.Pink;
                    break;
            }
            base.ModifyBuffText(ref buffName, ref tip, ref rare);
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.Lostbelt().badLifeRegenPool += JellyfishGroupBadLifeRegenEnemy * 2;
        }

    }
}
