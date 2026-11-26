using LostbeltTraveler.Core.PixelatedRender;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace LostbeltTraveler.Projs.Magic
{
    public class CrystalPikeProj:LostbeltProjs,IPixelatedRenderer
    {
        public override EnumDamageClass Category => EnumDamageClass.Magic;
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(22);
        }
        public override void ExSD()
        {
            base.ExSD();
        }
        public override void ProjAI()
        {
            base.ProjAI();
        }
        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
        public BlendState BlendState => BlendState.Additive;
        public ScarletDrawLayer LayerToRenderTo => ScarletDrawLayer.BeforeDusts;
        public void RenderPixelated(SpriteBatch spriteBatch)
        {

        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}
