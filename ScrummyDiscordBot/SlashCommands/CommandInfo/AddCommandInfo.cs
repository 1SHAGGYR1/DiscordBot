namespace ScrummyDiscordBot.SlashCommands.CommandInfo;

public class AddCommandInfo: BaseCommandInfo
{
    public override string Name => "add";
    public override string? Description => null;
    public override List<ParameterInfo>? Parameters => null;
    public override List<BaseCommandInfo> SubCommands => new() {new AddTeamCommandInfo()};
}