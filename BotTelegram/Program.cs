using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

class Program
{
    private static TelegramBotClient _botClient;
    private static string _token = "7016136059:AAEPn56AjIKWLD58jii-B7Q26AEfQxsZ1A8";
    private static string _caminhoAutenticacaoP3 = @"\\rpasc01app02\ged_djur_robo_cetelem\P3\AutenticacaoP3.json";
    private static string _caminhoAutenticacaoAutorizador = @"\\sevrj01fs03\ged_djur_robo_cetelem\Autorizador\AutenticacaoAutorizador.json";
    private static string _caminhoAutenticacaoFrontEnd = @"\\rpasc01app02\ged_djur_robo_cetelem\FrontEnd\AutenticacaoFrontEnd.json";
    private static string _nomeRobo = string.Empty;
    private static string _usuario = string.Empty;
    private static string _senha = string.Empty;

    public enum RoboEnum
    {
        P3,
        Autorizador,
        FrontEnd
    }

    static async Task Main(string[] args)
    {
        _botClient = new TelegramBotClient(_token);

        var cancellationToken = new CancellationTokenSource().Token;
        var receiverOptions = new ReceiverOptions { AllowedUpdates = { } };

        _botClient.StartReceiving(AtualizarAsync, ErroAsync, receiverOptions, cancellationToken);

        var me = await _botClient.GetMeAsync();

        Console.WriteLine($"Start listening for @{me.Username}");
        Console.ReadLine();
    }

    private static async Task AtualizarAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message && update.Message.Text != null)
        {
            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;

            try
            {
                if (messageText.StartsWith("/AtualizarSenha"))
                {
                    if (messageText.Split(" ").Count() != 4)
                    {
                        throw new Exception("❌ O comando foi digitado em um formato incorreto!\n\n" +
                                            "Formato correto: /AtualizarSenha NomeRobo Usuario Senha");
                    }

                    _nomeRobo = messageText.Split(" ")[1].Trim();
                    _usuario = messageText.Split(" ")[2].Trim();
                    _senha = messageText.Split(" ")[3].Trim();

                    if (!Enum.TryParse(_nomeRobo, out RoboEnum robo))
                    {
                        throw new Exception("❌ O nome do robô não foi identificado!\n\n" +
                                            "Opções de robô:\n" +
                                            "- P3\n" +
                                            "- Autorizador\n" +
                                            "- FrontEnd");
                    }

                    string caminhoAutenticacao = ObterCaminho(robo);

                    AtualizarSenha(caminhoAutenticacao, _usuario, _senha);

                    string mensagemRetorno = "✅ Login alterado com sucesso!\n\n" +
                                            $"Novo usuário: {_usuario}\n" +
                                            $"Nova senha: {_senha}\n";

                    await botClient.SendTextMessageAsync(chatId, mensagemRetorno, cancellationToken: cancellationToken);

                    return;
                }
                else
                {
                    string mensagemRetorno = "❌ Comando não identificado!\n\n" +
                                            $"Comando existente: /AtualizarSenha";

                    await botClient.SendTextMessageAsync(chatId, mensagemRetorno, cancellationToken: cancellationToken);
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Erro: {ex.Message}", cancellationToken: cancellationToken);
            }
        }
    }

    private static string ObterCaminho(RoboEnum nomeRobo)
    {
        switch (nomeRobo)
        {
            case RoboEnum.P3:
                return _caminhoAutenticacaoP3;
            case RoboEnum.Autorizador:
                return _caminhoAutenticacaoAutorizador;
            case RoboEnum.FrontEnd:
                return _caminhoAutenticacaoFrontEnd;
            default:
                throw new Exception("Nenhum robô identificado");
        }
    }

    private static void AtualizarSenha(string caminho, string usuario, string senha)
    {
        var json = System.IO.File.ReadAllText(caminho);
        var jsonObj = JObject.Parse(json);

        jsonObj["Usuario"] = usuario;
        jsonObj["Senha"] = senha;
        jsonObj["IsAtivo"] = true;

        string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);

        System.IO.File.WriteAllText(caminho, output);
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
