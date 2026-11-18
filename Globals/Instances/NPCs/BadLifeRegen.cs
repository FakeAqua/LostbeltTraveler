using LostbeltTraveler.Core.ParticleECS;
using Terraria;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Instances.NPCs
{
    public partial class LostbeltGlobalNPCs : GlobalNPC
    {
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            int badLife = 0;
            if (isBeingShadowCast > 0)
            {
                badLife += 100 * 2;
                Vector2 pos = npc.ToRandRec();
                ECSParticle.ShrinkParticle(pos, npc.velocity / 4f, Color.Black, 45, 1, 0, 0.4f, 1, blendstate: BlendState.NonPremultiplied);
            }
            if (badLifeRegenPool > 0)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;
                npc.lifeRegen -= badLifeRegenPool;
                damage = badLifeRegenPool / 5;

            }
            badLifeRegenPool = 0;
            if (npc.lifeRegen < 0)
            {
                if (isPotraitTimer > 0)
                {
                    npc.lifeRegen = (int)(npc.lifeRegen * potraityDoT);
                    damage = (int)(damage * 5);
                }
            }
        }

    }
}
