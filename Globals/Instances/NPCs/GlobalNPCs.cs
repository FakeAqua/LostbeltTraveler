using LostbeltTraveler.Buffs.Debuffs;
using LostbeltTraveler.Globals.Graphics.Metaballs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Instances.NPCs
{
    public partial class LostbeltGlobalNPCs : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool Dialectics_Mark = false;
        public bool terraFlamethrowerDebuff = false;
        public int isBeingStabByLavaFlowExecution = 0;
        public int isBeingStabByContainedStick = 0;
        public int isPotraitTimer = 0;
        public int isBeingShadowCast = 0;
        public bool refluxChain = false;
        public float potraityDoT = 0;
        public float miscCounter = 0;
        public int StopNpcTime = 0;
        public Vector2 PostSpeed = Vector2.Zero;
        public bool absoluteZeroBuffEnemy = false;
        public bool theBleachingBuffEnemy = false;
        public int isUnderPreciousTargetCross = 2;
        public bool foreverNightBuff = false;
        public int badLifeRegenPool = 0;
        public int cursedHeavyStack = 0;
        public bool inHorror = false;
        public float horrorLerp = 0;
        public float cursedLerp = 0;
        public bool inCursedHeavy = false;
        public float ParryProgress = 0;
        public override void ResetEffects(NPC npc)
        {
            if (cursedHeavyStack != 0 && !inHorror)
                cursedHeavyStack = 0;
            if (cursedLerp != 0 && !inCursedHeavy)
                cursedLerp = 0;
            ParryingTimerReset(npc);
            terraFlamethrowerDebuff = false;
            absoluteZeroBuffEnemy = false;
            theBleachingBuffEnemy = false;
            foreverNightBuff = false;
            inHorror = false;
            inCursedHeavy = false;
            refluxChain = false;
            Dialectics_Mark = false;

            if (isBeingShadowCast > 0)
                isBeingShadowCast--;
            if (isUnderPreciousTargetCross > 0)
                isUnderPreciousTargetCross--;
            if (isBeingStabByContainedStick > 0)
                isBeingStabByContainedStick--;
            if (isBeingStabByLavaFlowExecution > 0)
                isBeingStabByLavaFlowExecution = 0;
            if (StopNpcTime > 0)
                StopNpcTime--;
            if (isPotraitTimer > 0)
                isPotraitTimer--;

            if (isPotraitTimer == 0)
            {
                potraityDoT = 0;
            }
            miscCounter++;
            if (miscCounter > 300)
                miscCounter = 0;
        }
        public float dizzedStarIconLerp = 0;
        public void SmoothLerpingSelf(bool condition, ref float lerping)
        {
            if (condition)
            {
                lerping = Lerp(lerping, 1, .2f);
                if (lerping > .98f)
                    lerping = 1;
            }
            else
            {
                lerping = Lerp(lerping, 0, .2f);
                if (lerping < .2f)
                    lerping = 0;
            }
        }
        public override bool PreAI(NPC npc)
        {
            SmoothLerpingSelf(inHorror, ref horrorLerp);
            SmoothLerpingSelf(inCursedHeavy, ref cursedLerp);

            //if (parryTime > 0 && !npc.CountAsBoss())
            //{
            //    parryTime--;
            //    if (parryTime < 60)
            //    {
            //        dizzedStarIconLerp = Lerp(dizzedStarIconLerp, 0f, parryTime / 30f);

            //    }
            //    else
            //    {
            //        dizzedStarIconLerp = Lerp(dizzedStarIconLerp, 1.01f, .12f);
            //    }
            //    if (parryTime < 1)
            //    {
            //        isBeingParry = false;
            //        npc.noTileCollide = parryNoPassingWall;
            //        return true;
            //    }
            //    isBeingParry = true;
            //    npc.velocity.X *= .96f;
            //    npc.velocity.Y += 0.85f;
            //    if (npc.velocity.Y > 32f)
            //        npc.velocity.Y = 32f;
            //    npc.noTileCollide = false;
            //    return true;
            //}
            bool activeParry = UpdateParrying(npc);
            if (!activeParry)
                return false;
            return base.PreAI(npc);
        }
        public override void PostAI(NPC npc)
        {
            if (StopNpcTime > 0)
                npc.velocity *= 0.1f;
            if (StopNpcTime == 0 && PostSpeed != Vector2.Zero)
            {
                npc.velocity = PostSpeed;
                PostSpeed = Vector2.Zero;
            }
            if (isBeingShadowCast > 0)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC spread = Main.npc[i];

                    if (i != npc.whoAmI && spread != null && spread.active && !spread.townNPC && !spread.friendly && spread.lifeMax > 5 && Vector2.Distance(npc.Center, spread.Center) < 500 && spread.Lostbelt().isBeingShadowCast < 1)
                    {
                        spread.Lostbelt().isBeingShadowCast = isBeingShadowCast;
                    }
                }
            }
        }
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {

            if (absoluteZeroBuffEnemy)
            {
                drawColor = Color.Lerp(drawColor, Color.SkyBlue, .5f) with { A = 255 };
                Vector2 randRec = npc.ToRandRec();
            }
            if (theBleachingBuffEnemy)
            {
            }
            if (foreverNightBuff)
            {
                if (Main.rand.NextBool(4))
                    ShadowNebulaAlt.SpawnSharpTearClean(npc.ToRandRec(), -Vector2.UnitY, Main.rand.NextFloat(.9f, 1.1f) * .31f, 60);
            }
            if (isBeingShadowCast > 0)
            {
                drawColor = Color.Lerp(drawColor, Color.Black, .40f);
            }
            base.DrawEffects(npc, ref drawColor);
        }
        public bool IsOnShader = false;
        public readonly record struct BuffIconVisual(int buffType, float opacity, float floatAmp, float backgroundShake, float frontShake, float frontAlpha = .72f);
        public void DrawBuffIconList(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor, ReadOnlySpan<BuffIconVisual> icons)
        {
            int count = icons.Length;
            int maxWidth = 5;
            for (int i = 0; i < count; i++)
            {
                var icon = icons[i];
                int row = i / maxWidth;
                int col = i % maxWidth;
                int itemsInRow = Math.Min(maxWidth, count - row * maxWidth);
                //整体居中设置
                float rowWitdh = (itemsInRow - 1) * 34f;
                float xOffset = -rowWitdh / 2f + col * 34f;
                float yOffset = npc.height + 5.5f + row * 40;
                float lerping = Clamp(Lerp(1, 0, icon.opacity), 0, 1);
                yOffset += lerping * icon.floatAmp;
                //最后绘制出来
                Texture2D tex = TextureAssets.Buff[icon.buffType].Value;
                Vector2 ori = tex.Size() / 2f;
                Vector2 anchor = npc.Center + new Vector2(xOffset, 0);
                Vector2 pos1 = anchor.ToRandCirclePos(icon.backgroundShake * icon.opacity) - screenPos - Vector2.UnitY * yOffset;
                spriteBatch.Draw(tex, pos1, null, Color.White * icon.opacity, 0, ori, 1, 0, 0);
                Vector2 pos2 = anchor.ToRandCirclePos(icon.frontShake * icon.opacity * icon.opacity) - screenPos - Vector2.UnitY * yOffset;
                spriteBatch.Draw(tex, pos2, null, Color.White * icon.frontAlpha * icon.opacity, 0f, ori, 1, 0, 0);
            }

        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (IsOnShader)
            {
                spriteBatch.EndShaderArea();
                IsOnShader = false;
            }
            if (terraFlamethrowerDebuff)
            {
                spriteBatch.EnterShaderArea();
                Texture2D ring = LostbeltModAssets.Particle_RingShiny.Value;
                spriteBatch.Draw(ring, npc.Center - screenPos, null, Color.LimeGreen.ToAddColor(), 0, ring.ToOrigin(), 0.5f, 0, 0);
                spriteBatch.EndShaderArea();
            }
            //这里目前需求量不够并且为局部方法，直接栈内存丢出去
            //考虑到和fargo的适配，我们不会考虑过度的使用临时的buff调用。
            //事实上，模组内应该只考虑8个需要用到显示buff图标的方案
            Span<BuffIconVisual> icons = stackalloc BuffIconVisual[8];
            int n = 0;

            if (parryLength > 0)
            {
                icons[n++] = new BuffIconVisual(BuffType<ParrySpin>(), dizzedStarIconLerp, 50, 1, 5);

            }
            if (horrorLerp >= .02f)
            {
                icons[n++] = new BuffIconVisual(BuffType<DeathTimer>(), horrorLerp, 50, 1, 3);
            }
            if (cursedLerp >= .02f)
                icons[n++] = new BuffIconVisual(BuffType<CursedHeavy>(), horrorLerp, 50, 1, 3);
            DrawBuffIconList(npc, spriteBatch, screenPos, drawColor, icons[..n]);
        }
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            base.EditSpawnRate(player, ref spawnRate, ref maxSpawns);
        }
        public override void EditSpawnRange(Player player, ref int spawnRangeX, ref int spawnRangeY, ref int safeRangeX, ref int safeRangeY)
        {
            base.EditSpawnRange(player, ref spawnRangeX, ref spawnRangeY, ref safeRangeX, ref safeRangeY);
        }
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {

            base.EditSpawnPool(pool, spawnInfo);
        }
    }
}
