using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Globals.Database.List;
using Terraria;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Players
{
    public partial class LostbeltPlayer : ModPlayer
    {
        public void ParryShieldHelper()
        {
            if (parryShieldType < 0)
                return;
            if (LostbeltList.ParryShieldDictionary.TryGetValue(parryShieldType, out int projID))
            {
                if (!Player.HasProj(projID))
                {
                    Projectile proj = Projectile.NewProjectileDirect(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, projID, 5, 1, Player.whoAmI);
                    ScarletSound(LostbeltSoundAssets.Blunt_Swing, Player.Center, variantType: 2);
                }
            }
        }
    }
}
