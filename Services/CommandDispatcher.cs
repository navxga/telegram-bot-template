using TelegramBot.Interfaces;

namespace TelegramBot.Services;

public class CommandDispatcher(IEnumerable<ICommand> commands) : ICommandDispatcher
{
    public Task<string> DispatchAsync(string messageText)
    {
        var typedCommand = messageText.Split(' ')[0].ToLower();

        var command = commands.FirstOrDefault(c => c.Trigger == typedCommand)
            ?? throw new NotSupportedException();

        return Task.FromResult(command.Execute(messageText));
    }
}