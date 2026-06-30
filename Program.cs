using Telegram.Bot;
using TelegramBot.Commands;
using TelegramBot.Services;
using TelegramBot.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>(optional: true);

builder.Services.AddSingleton<ITelegramBotClient>(sp =>
{
    var token = sp.GetRequiredService<IConfiguration>()["Telegram:Token"]
        ?? throw new InvalidOperationException("Telegram:Token não configurado.");

    return new TelegramBotClient(token);
});

#region Commands

builder.Services.AddTransient<ICommand, TestCommand>();
//builder.Services.AddTransient<ICommand, *Command>(); Add your other commands here, following the same pattern.

#endregion

builder.Services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
builder.Services.AddHostedService<BotHostedService>();

var host = builder.Build();
await host.RunAsync();
