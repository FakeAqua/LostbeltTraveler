using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Core.NetCode.Content
{
    // 同步“武器技能键按下”那一帧，落地到 JustPressedWeaponSKill。
    // 发送端 LostbeltNetUtils.SyncedWeaponSkill 已经写好，目前还没人调，要用时在按键那帧发一次。
    public class ReadWeaponSkill : BaseLostbeltHandlePack
    {
        public override void Read(BinaryReader reader, int whoAmI)
        {
            byte playerIndex = reader.ReadByte();
            bool weaponSkill = reader.ReadBoolean();
            if (playerIndex < Main.maxPlayers && Main.player[playerIndex].active)
                Main.player[playerIndex].Lostbelt().JustPressedWeaponSKill = weaponSkill;
            if (Main.netMode == NetmodeID.Server)
            {
                ModPacket packet = LostbeltTraveler.Instance.GetPacket();
                packet.Write(Type);
                packet.Write(playerIndex);
                packet.Write(weaponSkill);
                packet.Send(-1, whoAmI);
            }
        }
    }
}
