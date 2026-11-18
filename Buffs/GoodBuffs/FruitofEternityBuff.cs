using LostbeltTraveler.Items.Useables;
using Terraria;
using Terraria.ModLoader;

namespace LostbeltTraveler.Buffs.GoodBuffs
{
    public class FruitofEternityBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.Lostbelt().lifeRegenPool += FruitofEternity.LifeRegenSpeed;
            player.Lostbelt().fruitofEthernity = true;
        }
    }
}
