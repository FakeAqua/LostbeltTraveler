using LostbeltTraveler.Globals.Players.InnerCooldown.Methods;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Players.InnerCooldown.Players
{
    public partial class InnerCooldownPlayer : ModPlayer
    {
        public Dictionary<InnerCooldownType, int> ActiveInnerCD = new Dictionary<InnerCooldownType, int>();
        public readonly List<InnerCooldownType> keysToRemove = new List<InnerCooldownType>();
        public override void PostUpdateMiscEffects()
        {
            if (ActiveInnerCD.Count == 0)
            {
                keysToRemove.Clear();
                return;
            }
            foreach (var p in ActiveInnerCD)
            {
                var key = p.Key;
                int time = p.Value;
                if (time > 0)
                {
                    time--;
                    ActiveInnerCD[key] = time;
                }
                if (time == 0)
                {
                    keysToRemove.Add(key);
                }
            }
            foreach (var p in keysToRemove)
            {
                ActiveInnerCD.Remove(p);
            }
        }
    }
}
