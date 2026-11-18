using System;

namespace LostbeltTraveler.Globals.Database.Enums
{
    [Flags]
    public enum ScarletDrawLayer
    {
        BeforeTiles,
        BeforeNPCs,
        BeforeProjectiles,
        BeforePlayer,
        BeforeDusts,
        AfterDusts,
        AfterProjectiles,
        EndCapture
    }
}
