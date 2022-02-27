using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ScrummyDiscordBot.ConfigurationSettings;
using ScrummyDiscordBot.SlashCommands;
using ScrummyDiscordBot.SlashCommands.Factories;

namespace ScrummyDiscordBot;

public class Bot : IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly BotConfiguration _configuration;
    private readonly TopLevelCommandHandlerFactory _commandHandlerFactory;

    private readonly DiscordSocketClient _client;
    private readonly CommandService _commandService;

    public Bot(IOptions<BotConfiguration> configuration, IServiceProvider serviceProvider, TopLevelCommandHandlerFactory commandHandlerFactory)
    {
        _serviceProvider = serviceProvider;
        _commandHandlerFactory = commandHandlerFactory;
        _configuration = configuration.Value;
        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Info,
        });
        _commandService = new CommandService(new CommandServiceConfig
        {
            LogLevel = LogSeverity.Info,
            CaseSensitiveCommands = false,
        });

        _client.Log += Log;
        _commandService.Log += Log;
    }

    private Task Log(LogMessage arg)
    {
        Console.WriteLine(arg.Message);
        return Task.CompletedTask;
    }

    public async Task InitAsync()
    {
        _client.Ready += OnReady;
        _client.SlashCommandExecuted += command => _commandHandlerFactory.GetCommandHandler(command).HandleCommand(command);
        
        await _client.LoginAsync(TokenType.Bot, _configuration.Token);
        await _client.StartAsync();
    }

    private async Task OnReady()
    {
        await RegisterNewCommands();
        await InformConnectedGuilds();
    }

    private async Task RegisterNewCommands()
    {
        try
        {
            await _client.BulkOverwriteGlobalApplicationCommandsAsync(CommandsCreator.GetNewCommandsDescriptions().ToArray()).ConfigureAwait(false);
        }
        catch (HttpException exception)
        {
            var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
            Console.WriteLine(json);
        }
    }

    private Task InformConnectedGuilds()
    {
        var tasks = new List<Task>();
        foreach (var guild in _client.Guilds)
        {
            if (guild.Channels.FirstOrDefault(channel => channel.Name == "bots-logger") is ITextChannel textChannel)
            {
                var embed = new EmbedBuilder
                {
                    Title = "Slash commands created."
                };
                embed
                    .WithAuthor(new EmbedAuthorBuilder()
                    .WithName("Scrummy"))
                    .WithColor(Color.Magenta)
                    .WithCurrentTimestamp();
                tasks.Add(textChannel.SendMessageAsync("I started working!", embed: embed.Build()));
            }
        }
        return Task.WhenAll(tasks);
    }
    
    public void Dispose()
    {
        //TODO: implement IDisposable normally
        _client.Log -= Log;
        _commandService.Log -= Log;
        _client.Dispose();
    }
}