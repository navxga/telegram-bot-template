using TelegramBot.Interfaces;

namespace TelegramBot.Commands
{
    public class TestCommand : ICommand
    {
        public string Trigger => "/test";

        public string Execute(string messageText)
        {
            return $"✅ Command typed: \"{messageText}\"" +
                $"\n\nThe bot is working perfectly! Time to start building your bot!";
        }
    }
}
