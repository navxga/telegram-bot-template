using TelegramBot.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace TelegramBot.Commands
{
    public class HelpCommand(IServiceProvider serviceProvider) : ICommand
    {
        public string Trigger => "/help";
        public string Description => "Mostra esta mensagem de ajuda";

        public string Execute(string messageText)
        {
            var commands = serviceProvider.GetServices<ICommand>();

            var lista = commands.Select(c => $"{c.Trigger} - {c.Description}");

            return "📌 Central de Ajuda - Bot CPA SMS!\n\nLista de comandos:\n\n" +
                   string.Join("\n", lista);
        }
    }
}
