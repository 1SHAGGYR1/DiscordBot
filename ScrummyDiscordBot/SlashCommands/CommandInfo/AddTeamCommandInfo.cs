namespace ScrummyDiscordBot.SlashCommands.CommandInfo;

public class AddTeamCommandInfo: BaseCommandInfo
{
    public override string Name => "team";
    public override string Description => "Add new team role";
    public override List<ParameterInfo> Parameters => new(){new ParameterInfo("team-name", "Name of the team you want to create.")};
    public override List<BaseCommandInfo>? SubCommands => null;
}