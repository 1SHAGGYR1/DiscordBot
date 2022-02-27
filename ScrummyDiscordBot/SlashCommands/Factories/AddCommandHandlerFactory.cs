using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using ScrummyDiscordBot.SlashCommands.CommandHandlers;
using ScrummyDiscordBot.SlashCommands.CommandInfo;

namespace ScrummyDiscordBot.SlashCommands.Factories;

public class AddCommandHandlerFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly AddCommandInfo _addCommandInfo;


    public AddCommandHandlerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        var addCommandInfo = CommandsHierarchy.CommandsInfo.FirstOrDefault(commandInfo => commandInfo is AddCommandInfo);
        //TODO: specify exception
        _addCommandInfo = addCommandInfo as AddCommandInfo ?? throw new Exception();
    }
    
    public ICommandHandler GetCommandHandler(SocketSlashCommand command)
    {
        var appropriateCommand = _addCommandInfo.SubCommands.FirstOrDefault(commandInfo => commandInfo.Name == command.Data.Name);
        if (appropriateCommand is null)
        {
            //TODO: specify exception
            throw new Exception();
        }
        
        return appropriateCommand switch
        {
            AddTeamCommandInfo => _serviceProvider.GetRequiredService<AddTeamCommandHandler>(),
            _ => throw new ArgumentOutOfRangeException(nameof(appropriateCommand))
        };
    }
}