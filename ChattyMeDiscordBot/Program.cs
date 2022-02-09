using ChattyMeDiscordBot;
using ChattyMeDiscordBot.ConfigurationSettings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

class Program
{
    private IHost host;
    
    // Program entry point
    static Task Main(string[] args)
    {
        // Call the Program constructor, followed by the 
        // MainAsync method and wait until it finishes (which should be never).
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


