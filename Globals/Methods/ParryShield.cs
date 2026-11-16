using LostbeltTraveler.Globals.Database.IDSets;
using Terraria;

namespace LostbeltTraveler.Globals.Methods
{
    public static partial class LostbeltMethods
    {
        public static bool HasParryShieldProj(this Player player)
        {
            foreach (var p in Main.ActiveProjectiles)
            {
                if (p.owner != player.whoAmI)
                    continue;
                if (ScarletProjIDSets.IsParryShield[p.type])
                {
                    return true;
                }
            }
            return false;
        }
    }
}
