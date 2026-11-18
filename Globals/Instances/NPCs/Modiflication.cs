using LostbeltTraveler.Buffs.Debuffs;
using Terraria;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Instances.NPCs
{
    public partial class LostbeltGlobalNPCs : GlobalNPC
    {
        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            if (theBleachingBuffEnemy)
            {
                modifiers.FinalDamage *= TheBleachingBuff.HitDamageMultEnemy;
            }
        }
        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (inHorror)
            {
                modifiers.SourceDamage *= (1 + DeathTimer.ExtraDamage);
            }
            if (absoluteZeroBuffEnemy)
            {
                modifiers.SourceDamage *= (1 + AbsoluteZeroBuff.BadDamageMult);
            }
            if (theBleachingBuffEnemy)
            {
                modifiers.DefenseEffectiveness *= 0;
            }
        }

    }
}
