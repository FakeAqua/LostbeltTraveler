using Terraria;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Executor
{
    public class ExecutorDamageClass : DamageClass, ILocalizedModType
    {
        public override string LocalizationCategory => "Database";
        internal static ExecutorDamageClass Instance;
        public override void Load()
        {
            Instance = this;
        }
        public override void Unload()
        {
            Instance = null;
        }
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == Generic)
                return StatInheritanceData.Full;
            return StatInheritanceData.None;
        }
        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            return false;
        }
        public override bool UseStandardCritCalcs => true;
        public override bool ShowStatTooltipLine(Player player, string lineName)
        {
            return true;
        }
    }
}
