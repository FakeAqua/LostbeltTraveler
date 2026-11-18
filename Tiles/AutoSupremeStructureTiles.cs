using LostbeltTraveler.Projs.General;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Tiles
{
    /// <summary>
    /// Credit Fargo Mutant
    /// </summary>
    public class AutoSupremeStructureTilesOverwatcher : ModTile
    {
        public virtual AutoSupremeStructureProj.SupremeStructureType RegisterType => AutoSupremeStructureProj.SupremeStructureType.Overwatcher;
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int p = Player.FindClosest(new Vector2(i * 16 + 8, j * 16 + 8), 0, 0);
                if (p != -1)
                {
                    int proj = Projectile.NewProjectile(new EntitySource_TileBreak(i, j), i * 16 + 8, (j + 2) * 16, 0f, 0f, ProjectileType<AutoSupremeStructureProj>(), 0, 0, p);
                    ((AutoSupremeStructureProj)Main.projectile[proj].ModProjectile).UseStructure = RegisterType;
                }
            }

            noItem = true;
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            WorldGen.KillTile(i, j);
            if (Main.netMode != NetmodeID.SinglePlayer)
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j);
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            return false;
        }

    }
    public class AutoSupremeStructureTilesLifebringer : AutoSupremeStructureTilesOverwatcher
    {
        public override AutoSupremeStructureProj.SupremeStructureType RegisterType => AutoSupremeStructureProj.SupremeStructureType.Lifebringer;
    }
    public class AutoSupremeStructureTilesMaterializer : AutoSupremeStructureTilesOverwatcher
    {
        public override AutoSupremeStructureProj.SupremeStructureType RegisterType => AutoSupremeStructureProj.SupremeStructureType.Materializer;
    }
    public class AutoSupremeStructureTilesSacarb : AutoSupremeStructureTilesOverwatcher
    {
        public override AutoSupremeStructureProj.SupremeStructureType RegisterType => AutoSupremeStructureProj.SupremeStructureType.Scarcb;
    }
    public class AutoSupremeStructureTilesWhale : AutoSupremeStructureTilesOverwatcher
    {
        public override AutoSupremeStructureProj.SupremeStructureType RegisterType => AutoSupremeStructureProj.SupremeStructureType.Whale;
    }
    public class AutoSupremeStructureTilesSon : AutoSupremeStructureTilesOverwatcher
    {
        public override AutoSupremeStructureProj.SupremeStructureType RegisterType => AutoSupremeStructureProj.SupremeStructureType.Son;
    }
}
