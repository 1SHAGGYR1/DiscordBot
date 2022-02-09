using ChattyMeDiscordBot;
using ChattyMeDiscordBot.ConfigurationSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

class Program
{
    private readonly IHost host;

    static Task Main(string[] args)
    {
        return new Program().MainAsync();
    }

    private Program()
    {
        host = Host.CreateDefaultBuilder().ConfigureAppConfiguration((_, configuration) =>
            {
                configuration.Sources.Clear();
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                configuration
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env}.json", true, true);
            })
            .ConfigureServices((context, services) =>
            {
                var configurationRoot = context.Configuration;
                services.Configure<BotConfiguration>(configurationRoot.GetSection(nameof(BotConfiguration)));
                services.AddSingleton<Bot>();
            })
            .Build();
    }

    private async Task MainAsync()
    {
        var bot = host.Services.GetRequiredService<Bot>();
        await bot.InitAsync();

        await Task.Delay(-1);
    }
}