namespace ScrummyDiscordBot.SlashCommands.CommandInfo;

public abstract class BaseCommandInfo
{
    public abstract string Name { get; }
    
    public abstract string? Description { get; }

    public abstract List<ParameterInfo>? Parameters { get; }

    public abstract List<BaseCommandInfo>? SubCommands { get; }
}