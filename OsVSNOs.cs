using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using OsVSNOs.extensions;

namespace OsVSNOs;

public class OsVSNOs : BasePlugin
{
    public override string ModuleName => "OsVSNOs";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "ShookEagle";
    private CsTeam OTeam = CsTeam.None;
    private CsTeam NOTeam = CsTeam.None;

    public override void Load(bool hotReload)
    {
        AddCommandListener("jointeam", Command_Jointeam);
        OTeam = CsTeam.Terrorist;
        NOTeam = CsTeam.CounterTerrorist;
        Server.ExecuteCommand($"mp_teamname_1 NO's");
        Server.ExecuteCommand($"mp_teamname_2 O's");
    }

    public override void Unload(bool hotReload)
    {
        Server.ExecuteCommand("mp_teamname_1 COUNTER-TERRORISTS");
        Server.ExecuteCommand("mp_teamname_2 TERRORISTS");
    }

    public HookResult Command_Jointeam(CCSPlayerController? player, CommandInfo info)
    {
        if (player == null || !player.IsReal())
            return HookResult.Continue;

        var team = (player.IsOTag()) ? OTeam : NOTeam;
        player.SwitchTeam(team);
        return HookResult.Stop;
    }

    public void SwapTeams()
    {
        CsTeam temp = OTeam;
        OTeam = NOTeam;
        NOTeam = temp;

        Server.ExecuteCommand($"mp_teamname_1 {(OTeam == CsTeam.CounterTerrorist ? "O's" : "NO's")}");
        Server.ExecuteCommand($"mp_teamname_2 {(OTeam == CsTeam.Terrorist ? "O's" : "NO's")}");

        var OTags = Utilities.GetPlayers().Where(p => p.IsOTag());
        var NOTags = Utilities.GetPlayers().Where(p => !p.IsOTag());

        foreach (var player in OTags)
        {
            player.SwitchTeam(OTeam);
        }
        foreach (var player in NOTags)
        {
            player.SwitchTeam(NOTeam);
        }
    }

    [GameEventHandler(HookMode.Post)]
    public HookResult OnHalfTimeStart(EventStartHalftime @event, GameEventInfo info)
    {
        SwapTeams();
        return HookResult.Continue;
    }

    [ConsoleCommand("css_swapsides", "Swap the sides in O's vs NO's")]
    public void OnSwapSides(CCSPlayerController player, CommandInfo info)
    {
        SwapTeams();
        return;
    }
}
