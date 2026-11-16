using Terraria;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Players.AccessoriesSlot
{
    public class LightofHorizonAccessorySlot : ModAccessorySlot
    {
        public override string Name => "LightofHorizon";
        public override bool IsEnabled()
        {
            bool isEnable = Main.LocalPlayer.TryGetModPlayer<LostbeltPlayer>(out var value);
            if (isEnable)
                isEnable = value.LightofHorizon;
            return isEnable;
        }
    }
    public class CombatSlotAcceesorySlot : ModAccessorySlot
    {
        public override string Name => "CombatSlot";
        public override bool IsEnabled()
        {
            bool isEnable = Main.LocalPlayer.TryGetModPlayer<LostbeltPlayer>(out var value);
            if (isEnable)
                isEnable = value.combatSlot;
            return isEnable;
        }
    }
    public class CombatSlotAcceesorySlot2 : ModAccessorySlot
    {
        public override string Name => "CombatSlot2";
        public override bool IsEnabled()
        {
            bool isEnable = Main.LocalPlayer.TryGetModPlayer<LostbeltPlayer>(out var value);
            if (isEnable)
                isEnable = value.combatSlot2;
            return isEnable;
        }
    }
}

