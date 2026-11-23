using Terraria;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Instances.NPCs
{
    public partial class LostbeltGlobalNPCs : GlobalNPC
    {
        public bool isBeingParry = false;
        public bool parryNoPassingWall = false;
        public float parryTime = 0;
        public float parryLength = 0;
        public float parryPrevSpeed = -1;

        public void ParryingTimerReset(NPC npc)
        {
            if (parryTime > 0)
                parryTime--;
            //没被格挡的时候，每时每刻都得查看这个nopassingwall的情况
            if (!isBeingParry)
            {
                parryNoPassingWall = npc.noTileCollide;
                if (ParryProgress >= 1 && parryLength == 0)
                {
                    isBeingParry = true;
                    parryLength = GetSeconds(5);
                }
            }
        }
        public bool UpdateParrying(NPC npc)
        {
            if (!npc.CanBeParry())
                return true;
            if (isBeingParry && parryLength > 0)
            {
                parryLength--;
                if (parryLength < 60)
                    dizzedStarIconLerp = Lerp(dizzedStarIconLerp, 0f, parryLength / 30f);
                else
                    dizzedStarIconLerp = Lerp(dizzedStarIconLerp, 1.01f, .12f);
                if (parryLength < 1)
                {
                    isBeingParry = false;
                    ParryProgress = 0;
                    parryLength = 0;
                    npc.noTileCollide = parryNoPassingWall;
                    return true;
                }
                npc.velocity.X *= .96f;
                npc.velocity.Y += 0.85f;
                if (npc.velocity.Y > 32f)
                    npc.velocity.Y = 32f;
                npc.noTileCollide = false;
                return false;
            }
            return true;
        }
    }
}
