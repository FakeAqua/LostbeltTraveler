using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.NPCs.Enemy.CelestialPillar
{
    public class ExecutorCelestialTower:ModNPC
    {
        public override string Texture => LostbeltNPCAssets.ApplyNPC(GetType().Name);
        public enum AttackStyle
        {
            None,
            ChasingSlasher
        }
        public ref float Timer => ref NPC.ai[0];
        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailingMode[Type] = 3;
            NPCID.Sets.TrailCacheLength[Type] = 4;

        }
        public override void SetDefaults()
        {
            base.SetDefaults();
        }


    }
}
