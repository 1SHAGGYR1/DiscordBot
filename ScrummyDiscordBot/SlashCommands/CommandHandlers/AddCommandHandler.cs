using Discord.WebSocket;
using ScrummyDiscordBot.SlashCommands.Factories;

namespace ScrummyDiscordBot.SlashCommands.CommandHandlers;

public class AddCommandHandler: ICommandHandler
{
    private readonly AddCommandHandlerFactory _addCommandHandlerFactory;

    public AddCommandHandler(AddCommandHandlerFactory addCommandHandlerFactory)
    {
        _addCommandHandlerFactory = addCommandHandlerFactory;
    }

    public Task HandleCommand(SocketSlashCommand command)
    {
        var factory = _addCommandHandlerFactory.GetCommandHandler(command);
        return factory.HandleCommand(command);
    }
}