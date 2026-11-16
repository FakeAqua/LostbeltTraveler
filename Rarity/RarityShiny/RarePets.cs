using LostbeltTraveler.Rarity.RarityDrawHandler;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace LostbeltTraveler.Rarity.RarityShiny
{
    public class RarePets : ModRarity
    {
        public override Color RarityColor => Color.SkyBlue;
        internal static List<RaritySparkle> RarePetsParticle = [];
    }
}
