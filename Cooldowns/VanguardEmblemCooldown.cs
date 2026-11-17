using LostbeltTraveler.Core.Cooldowns;
using LostbeltTraveler.Core.ParticleECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace LostbeltTraveler.Cooldowns
{
    public class VanguardEmblemCooldown : ScarletCooldown
    {
        public override void Update(Player player)
        {
            base.Update(player);
        }
        public override SoundStyle? DeactiveSound => SoundID.DD2_DarkMageAttack;
        public override void OnKill(Player player)
        {
            for (int i = 0; i < 16; i++)
                ECSParticle.SmokeParticle(player.ToRandRec(), -Vector2.UnitY * Main.rand.NextFloat(12), Color.White, 36, RandRotTwoPi, 1, .4f, blendstate: BlendState.AlphaBlend);
        }
    }
}
