namespace TelegramBot.Interfaces;

public interface ICommandDispatcher
{
    Task<string> DispatchAsync(string messageText);
}
