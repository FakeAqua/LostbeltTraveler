using LostbeltTraveler.Core.Cooldowns;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Core.NetCode.Content
{
    public class ReadSyncCooldown: BaseLostbeltHandlePack
    {
        public override void Read(BinaryReader reader, int whoAmI)
        {
            int p = reader.ReadInt32();
            int bType = reader.ReadInt32();
            int bTime = reader.ReadInt32();
            if(Main.netMode==NetmodeID.Server)
            {
                ModPacket modPacket = LostbeltTraveler.Instance.GetPacket();
                modPacket.Write(Type);
                modPacket.Write(p);
                modPacket.Write(bType);
                modPacket.Write(bTime);
                modPacket.Send(-1, whoAmI);
            }
            else if(Main.netMode==NetmodeID.MultiplayerClient)
            {
                Player p2 = Main.player[p];
                p2.AddCooldown(bType, bTime, false);
            }
        }
    }
}
