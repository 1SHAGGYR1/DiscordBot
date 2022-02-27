using Discord;
using ScrummyDiscordBot.SlashCommands.CommandInfo;

namespace ScrummyDiscordBot.SlashCommands;

public static class CommandsCreator
{
    public static List<ApplicationCommandProperties> GetNewCommandsDescriptions()
    {
        return new List<ApplicationCommandProperties> {CreateAddCommand()};
    }

    private static ApplicationCommandProperties CreateAddCommand()
    {
        if (CommandsHierarchy.CommandsInfo.FirstOrDefault(command => command is AddCommandInfo) is not AddCommandInfo addCommandInfo)
        {
            //TODO: specify exception
            throw new Exception();
        }

        var addTeamCommandInfo = CommandsHierarchy.CommandsInfo.First(command => command is AddTeamCommandInfo) as AddTeamCommandInfo;
        var addTeamCommandParameterInfo = addTeamCommandInfo!.Parameters.First();

        var addCommand = new SlashCommandBuilder()
            .WithName(addCommandInfo.Name)
            .AddOption(new SlashCommandOptionBuilder()
                .WithName(addTeamCommandInfo.Name)
                .WithDescription(addTeamCommandInfo.Description)
                .AddOption(addTeamCommandParameterInfo.Name, ApplicationCommandOptionType.String, addTeamCommandParameterInfo.Description, isRequired: true));
        return addCommand.Build();
    }
    
}