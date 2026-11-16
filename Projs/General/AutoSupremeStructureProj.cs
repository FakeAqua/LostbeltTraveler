using ContinentOfJourney.Tiles;
using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Projs.NPCs.Enemy;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Projs.General
{
    /// <summary>
    /// Credit from Fargo Mutant，感谢参考
    /// </summary>
    public class AutoSupremeStructureProj : LostbeltProjs
    {
        public enum SupremeStructureType
        {
            Overwatcher,
            Lifebringer,
            Materializer,
            Whale,
            Scarcb,
            Son
        }
        public SupremeStructureType UseStructure
        {
            get => (SupremeStructureType)Projectile.ai[0];
            set => Projectile.ai[0] = (float)value;
        }
        public override EnumDamageClass Category => EnumDamageClass.Typeless;
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public override void ExSD()
        {
            Projectile.timeLeft = 2;
        }
        public override void OnKill(int timeLeft)
        {
            Vector2 pos = Projectile.Center;
            ScarletSound(SoundID.Item44, pos);
            if(UseStructure == SupremeStructureType.Materializer)
            {
                //Owner.Hurt(PlayerDeathReason.ByNPC(NPCID.AncientCultistSquidhead), 1, 1);
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Owner.Center, Vector2.Zero, ProjectileType<SuicideKnifeInvisProj>(), 1, 1,Owner.whoAmI, ai2:1);
                ScarletSound(SoundID.Item153, Owner.Center);
                for(int i =0;i<16;i++)
                {
                    ECSParticle.ManaStarDust(Owner.Center, RandVelTwoPi(6f, 8f), Color.LightSkyBlue, 40, 1, 0, 1);
                }

            }
            int dir = ((Owner.Center.X - pos.X) > 0).ToDirectionInt();
            if (UseStructure == SupremeStructureType.Overwatcher)
            {
                for (int i = -1; i < 2; i += 1)
                    PlacingOverwatcherStructure(i, 1, pos, dir, Owner);
                //第三步，先放底部的木头
                PlacingOverwatcherStructure(-2, 2, pos, dir, Owner);
                //然后我们再放需要的沙子
                PlacingOverwatcherStructure(-2, 1, pos, dir, Owner);
                //祭坛
                PlacingOverwatcherStructure(0, 0, pos, dir, Owner);
            }
            if (UseStructure == SupremeStructureType.Lifebringer)
            {
                for (int i = -1; i < 2; i += 1)
                    PlacingOverwatcherStructure(i, 1, pos, dir, Owner);
                //第三步，先放底部的木头
                PlacingOverwatcherStructure(0, 0, pos, dir, Owner);
                CombatText.NewText(Owner.Hitbox, Color.White, "你认真的？");
            }
            if (UseStructure == SupremeStructureType.Materializer)
            {
                //祭坛，物块
                for (int i = -1; i < 2; i += 1)
                    PlacingOverwatcherStructure(i, 1, pos, dir, Owner);
                //第三步，先放底部的木头
                PlacingOverwatcherStructure(0, 0, pos, dir, Owner);
                //我们开始放木头
                for (int i = -1; i > -4; i--)
                {
                    for (int j = 1; j <= 3; j++)
                    {
                        //skip掉第一个j，跳过
                        if (j == 1 && i == -1)
                            continue;
                        PlacingMaterializerStructure(i, j, pos, dir, Owner, false);
                    }
                }
                //最后放上液体
                PlacingMaterializerStructure(-2, 2, pos, dir, Owner, true);
                PlacingMaterializerStructure(-2, 1, pos, dir, Owner, true);
            }
            if (UseStructure == SupremeStructureType.Scarcb)
            {
                for (int i = -1; i < 2; i += 1)
                    PlacingScarabStructure(i, 1, pos, dir, Owner,false);
                //第三步，先放底部的木头
                PlacingScarabStructure(0, 0, pos, dir, Owner,false);
            }
            if(UseStructure == SupremeStructureType.Whale)
            {

            }
            //圣子
            if(UseStructure == SupremeStructureType.Son)
            {

            }
        }
        public void DestroyThisTile(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            bool hadLiquid = tile.LiquidAmount != 0;
            WorldGen.KillTile(x, y, noItem: true);
            tile.ClearEverything();

            if (Main.netMode == NetmodeID.Server)
            {
                if (hadLiquid)
                    NetMessage.sendWater(x, y);
                NetMessage.SendTileSquare(-1, x, y, 1);
            }
        }
        public void PlacingSonStructure(int x, int y, Vector2 pos, int side, Player player)
        {

        }
        public void PlacingScarabStructure(int x, int y, Vector2 pos, int side, Player player, bool placeLiquid)
        {
            int xPos = (int)((side * -1) + x + pos.X / 16f);
            int yPos = (int)((y - 2 + pos.Y / 16f));
            DestroyThisTile(xPos, yPos);
            //及时钟表祭坛
            if (y == 0)
            {
                WorldGen.PlaceTile(xPos, yPos, TileType<HallowedAltar>(), forced: true);
            }
            if (y > 0)
            {
                WorldGen.PlaceTile(xPos, yPos, TileID.EchoBlock, forced: true);
            }
        }
        public void PlacingMaterializerStructure(int x, int y, Vector2 pos, int side, Player player, bool placeLiquid)
        {
            int xPos = (int)((side * -1) + x + pos.X / 16f);
            int yPos = (int)((y - 2 + pos.Y / 16f));
            DestroyThisTile(xPos, yPos);
            //祭坛
            if (y == 0)
            {
                WorldGen.PlaceTile(xPos, yPos, TileType<HallowedAltar>(), forced: true);
            }
            //物块
            if (y > 0)
            {
                //放置木头
                if (x >= -1)
                    WorldGen.PlaceTile(xPos, yPos, TileID.WoodBlock, forced: true);
                else
                {
                    //前面两个格子中间需要空心
                    if (y <= 2)
                    {
                        if (x == -2)
                        {
                            if (!placeLiquid)
                            {
                                if (y == 1)
                                    WorldGen.PlaceTile(xPos, yPos, TileID.GoldCoinPile, forced: true);
                            }
                            else
                            {
                                Tile tile = Main.tile[xPos, yPos];
                                if (y == 2)
                                {
                                    tile.LiquidType = LiquidID.Water;
                                    tile.LiquidAmount = 255;
                                }
                                if (y == 1)
                                {
                                    tile.LiquidType = LiquidID.Lava;
                                    tile.LiquidAmount = 255;
                                }
                            }
                        }
                        else
                        {
                            WorldGen.PlaceTile(xPos, yPos, TileID.WoodBlock, forced: true);
                        }
                    }
                    else
                        WorldGen.PlaceTile(xPos, yPos, TileID.WoodBlock, forced: true);
                }
            }
        }
        public void PlacingLifebringerStructure(int x, int y, Vector2 pos, int side, Player player)
        {
            int xPos = (int)((side * -1) + x + pos.X / 16f);
            int yPos = (int)((y - 2 + pos.Y / 16f));
            DestroyThisTile(xPos, yPos);
            //及时钟表祭坛
            if (y == 0)
            {
                WorldGen.PlaceTile(xPos, yPos, TileType<HallowedAltar>(), forced: true);
            }
            if (y > 0)
            {
                WorldGen.PlaceTile(xPos, yPos, TileID.WoodBlock, forced: true);
            }
        }
        public void PlacingOverwatcherStructure(int x, int y, Vector2 pos, int side, Player player)
        {
            int xPos = (int)((side * -1) + x + pos.X / 16f);
            int yPos = (int)((y - 2 + pos.Y / 16f));
            DestroyThisTile(xPos, yPos);
            //及时钟表祭坛
            if (y == 0)
            {
                WorldGen.PlaceTile(xPos, yPos, TileType<HallowedAltar>(), forced: true);
            }
            if (y > 0)
            {
                if (x > -2)
                    WorldGen.PlaceTile(xPos, yPos, TileID.WoodBlock, forced: true);
                else
                {
                    if (y == 1)
                    {
                        WorldGen.PlaceTile(xPos, yPos, TileID.Sand, forced: true);
                    }
                    if (y == 2)
                    {
                        WorldGen.PlaceTile(xPos, yPos, TileID.WoodBlock, forced: true);
                        WorldGen.PoundTile(xPos, yPos);
                    }

                }
            }
        }
        public void PlacingStructure(int x, int y, Vector2 pos, int side, Player player)
        {
        }
    }
}
