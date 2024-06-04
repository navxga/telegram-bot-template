using BotTelegram.Commands;
using BotTelegram.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

class Program
{
    private static TelegramBotClient _botClient;
    private static string _token = "7016136059:AAEPn56AjIKWLD58jii-B7Q26AEfQxsZ1A8";
    public static string _caminhoAutenticacaoP3 = @"\\rpasc01app02\ged_djur_robo_cetelem\P3\AutenticacaoP3.json";
    public static string _caminhoAutenticacaoAutorizador = @"\\sevrj01fs03\ged_djur_robo_cetelem\Autorizador\AutenticacaoAutorizador.json";
    public static string _caminhoAutenticacaoFrontEnd = @"\\rpasc01app02\ged_djur_robo_cetelem\FrontEnd\AutenticacaoFrontEnd.json";

    static async Task Main(string[] args)
    {
        const string mutexName = "BotTelegramMutex";

        using (var mutex = new Mutex(false, mutexName, out bool createdNew))
        {
            if (!createdNew)
            {
                Console.WriteLine("Uma instância do bot já está em execução.");
                return;
            }

            _botClient = new TelegramBotClient(_token);

            var cancellationToken = new CancellationTokenSource().Token;
            var receiverOptions = new ReceiverOptions { AllowedUpdates = { } };

            _botClient.StartReceiving(ControladorAsync, ErroAsync, receiverOptions, cancellationToken);

            var me = await _botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
        }
    }

    private static async Task ControladorAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message && update.Message.Text != null)
        {
            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;

            try
            {
                string comandoDigitado = messageText.ToUpper().Split(' ').FirstOrDefault();

                ICommand command;

                switch (comandoDigitado)
                {
                    case "/HELP":
                        command = new HelpCommand();
                        break;

                    case "/ATUALIZARSENHA":
                        command = new AtualizarSenhaCommand();
                        break;

                    case "/OBTERLOGINS":
                        command = new ObterLoginsCommand();
                        break;

                    default:
                        {
                            string mensagemRetornoErro = "❌ Comando não identificado!\n\n" +
                                                        $"Para saber quais os comandos disponíveis, digite /Help.";

                            await botClient.SendTextMessageAsync(chatId, mensagemRetornoErro, cancellationToken: cancellationToken);

                            return;
                        }
                }

                string mensagemRetorno = command.Execute(messageText);

                await botClient.SendTextMessageAsync(chatId, mensagemRetorno, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Erro: {ex.Message}", cancellationToken: cancellationToken);
            }
        }
    }

    private static Task ErroAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Erro na API do Telegram:\n{apiRequestException.ErrorCode}\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }

}
