using Discord.WebSocket;

namespace ScrummyDiscordBot.SlashCommands.CommandHandlers;

public class AddTeamCommandHandler: ICommandHandler
{
    public Task HandleCommand(SocketSlashCommand command)
    {
        var teamName = command.Data.Options!.First().Options.First().ToString();
        //TODO: refactor everything using interactions framework. 
        throw new NotImplementedException();
    }
}