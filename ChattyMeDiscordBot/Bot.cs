using System.Reflection;
using ChattyMeDiscordBot.ConfigurationSettings;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;

namespace ChattyMeDiscordBot;

public class Bot : IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly BotConfiguration _configuration;

    private readonly DiscordSocketClient _client;
    private readonly CommandService _commandService;

    public Bot(IOptions<BotConfiguration> configuration, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
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
        await InitCommands();

        await _client.LoginAsync(TokenType.Bot, _configuration.Token);
        await _client.StartAsync();
    }

    private async Task InitCommands()
    {
        _client.MessageReceived += HandleCommandAsync;
        await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
    }

    private async Task HandleCommandAsync(SocketMessage arg)
    {
        //Ignore system messages.
        if (arg is not SocketUserMessage message)
        {
            return;
        }

        // Ignore bots
        if (message.Author.Id == _client.CurrentUser.Id || message.Author.IsBot)
        {
            return;
        }

        var commandStartPosition = 0;
        if (message.HasCharPrefix('~', ref commandStartPosition) || message.HasMentionPrefix(_client.CurrentUser, ref commandStartPosition))
        {
            var context = new SocketCommandContext(_client, message);

            var result = await _commandService.ExecuteAsync(context, commandStartPosition, _serviceProvider);

            if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
            {
                await message.Channel.SendMessageAsync(result.ErrorReason);
            }
        }
    }

    public void Dispose()
    {
        //TODO: implement IDisposable normally
        _client.Log -= Log;
        _commandService.Log -= Log;
        _client.MessageReceived -= HandleCommandAsync;
        _client.Dispose();
    }
}