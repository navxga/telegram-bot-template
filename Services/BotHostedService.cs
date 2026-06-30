using TelegramBot.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBot.Services;

public class BotHostedService(
    ITelegramBotClient botClient,
    ICommandDispatcher dispatcher,
    ILogger<BotHostedService> logger) : IHostedService
{
    private CancellationTokenSource? _cts;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        var receiverOptions = new ReceiverOptions { AllowedUpdates = { } };

        botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            _cts.Token);

        var me = await botClient.GetMe(cancellationToken);
        logger.LogInformation("Bot iniciado: @{Username}", me.Username);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cts?.Cancel();
        return Task.CompletedTask;
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
    {
        if (update.Type != UpdateType.Message || update.Message?.Text == null)
            return;

        var chatId = update.Message.Chat.Id;
        var messageText = update.Message.Text;

        try
        {
            var resposta = await dispatcher.DispatchAsync(messageText);

            await botClient.SendMessage(chatId, resposta, cancellationToken: ct);
        }
        catch (NotSupportedException)
        {
            await botClient.SendMessage(
                chatId,
                "❌ Comando não identificado!\n\nPara saber quais os comandos disponíveis, digite /help.",
                cancellationToken: ct
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao processar mensagem de {ChatId}", chatId);
            await botClient.SendMessage(chatId, $"Erro: {ex.Message}", cancellationToken: ct);
        }
    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken ct)
    {
        var errorMessage = exception switch
        {
            ApiRequestException api => $"Erro na API do Telegram:\n{api.ErrorCode}\n{api.Message}",
            _ => exception.ToString()
        };

        logger.LogError(errorMessage);
        return Task.CompletedTask;
    }
}