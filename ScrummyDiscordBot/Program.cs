using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScrummyDiscordBot.ConfigurationSettings;

namespace ScrummyDiscordBot;

class Program
{
    private readonly IHost host;

    static Task Main(string[] args)
    {
        return new Program().MainAsync();
    }

    private Program()
    {
        host = Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
        {
            var configurationRoot = context.Configuration;
            services.Configure<BotConfiguration>(configurationRoot.GetSection(nameof(BotConfiguration)));
            services.AddSingleton<Bot>();
        }).Build();
    }

    private async Task MainAsync()
    {
        var bot = host.Services.GetRequiredService<Bot>();
        await bot.InitAsync();

        await Task.Delay(-1);
    }
}