using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;

namespace OsVSNOs.extensions;

public static class PlayerExtensions
{
    public static bool IsOTag(this CCSPlayerController player)
    {
        if (AdminManager.PlayerInGroup(player, "#ego/ego"))
            return true;
        return false;
    }

    public static bool IsReal(this CCSPlayerController player)
    {
        if (!player.IsValid)
            return false;

        if (player.Connected != PlayerConnectedState.PlayerConnected)
            return false;

        if (player.IsBot || player.IsHLTV)
            return false;

        return true;
    }

}
