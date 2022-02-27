namespace ScrummyDiscordBot.SlashCommands.CommandInfo;

public class ParameterInfo
{
    public ParameterInfo(string name, string? description = null)
    {
        Name = name;
        Description = description;
    }

    public string? Description { get; }
    
    public string Name { get; }
}