using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MagicBot;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(services => {
    services.AddSingleton<DiscordSocketClient>()
        .AddSingleton<CommandService>()
        .AddSingleton<CommandHandler>();
});

var host = builder.Build();

using (var scope = host.Services.CreateAsyncScope())
{
    var commands = scope.ServiceProvider.GetRequiredService<CommandService>();
    await commands.AddModulesAsync(Assembly.GetEntryAssembly(), host.Services);
}

await host.StartAsync();