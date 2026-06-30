namespace TelegramBot.Interfaces;

public interface ICommand
{
    string Trigger { get; }
    string Description { get; }
    string Execute(string message);
}
