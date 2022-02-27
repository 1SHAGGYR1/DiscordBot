using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using ScrummyDiscordBot.SlashCommands.CommandHandlers;
using ScrummyDiscordBot.SlashCommands.CommandInfo;

namespace ScrummyDiscordBot.SlashCommands.Factories;

public class TopLevelCommandHandlerFactory
{
    private readonly IServiceProvider _serviceProvider;
    
    public TopLevelCommandHandlerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ICommandHandler GetCommandHandler(SocketSlashCommand command)
    {
        var appropriateCommand = CommandsHierarchy.CommandsInfo.FirstOrDefault(commandInfo => commandInfo.Name == command.Data.Name);
        if (appropriateCommand is null)
        {
            //TODO: specify exception
            throw new Exception();
        }
        
        return appropriateCommand switch
        {
            AddCommandInfo => _serviceProvider.GetRequiredService<AddCommandHandler>(),
            _ => throw new ArgumentOutOfRangeException(nameof(appropriateCommand))
        };
    }
}