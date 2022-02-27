using Discord.WebSocket;

namespace ScrummyDiscordBot.SlashCommands.CommandHandlers;

public interface ICommandHandler
{
    public Task HandleCommand(SocketSlashCommand command);
}