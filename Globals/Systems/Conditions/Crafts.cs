using ContinentOfJourney;
using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Globals.Methods;
using Terraria;

namespace LostbeltTraveler.Globals.Systems.Conditions
{
    public static class LostbeltCraftingConditions
    {
        public static string ConditionString => "Mods.LostbeltTraveler.Conditions.Crafting";
        public static Condition FirstTimeGaiaStriker = new Condition("Mods.LostbeltTraveler.Weapons.Executor.GaiaStriker.Conditions.FirstTime",
            () => !Main.LocalPlayer.Lostbelt().firstTimeCraftGaia && !Main.dedServ);
        public static Condition AnyAfterCrafting = new Condition("Mods.LostbeltTraveler.Weapons.Executor.GaiaStriker.Conditions.SecondTime",
            () => Main.LocalPlayer.Lostbelt().firstTimeCraftGaia || Main.dedServ);
        public static Condition IsDownSlimeGodAndInEclipse = new Condition($"{ConditionString}.{nameof(IsDownSlimeGodAndInEclipse)}",
            () => DownedBossSystem.downedSunGod && Main.eclipse);

        public static Condition HasFuckingCalamityMod = new Condition($"{ConditionString}.{nameof(HasFuckingCalamityMod)}", () => LostbeltTraveler.CrossMod_Calamity is not null);
        public static Condition NoFuckingCalamityMod = new Condition($"{ConditionString}.{nameof(NoFuckingCalamityMod)}", () => LostbeltTraveler.CrossMod_Calamity is null);
        public static Condition InMultiplayer = new Condition($"{ConditionString}.{nameof(InMultiplayer)}", () => Main.dedServ);
        public static Condition InSingleplayer = new Condition($"{ConditionString}.{nameof(InSingleplayer)}", () => !Main.dedServ);
    }

}
