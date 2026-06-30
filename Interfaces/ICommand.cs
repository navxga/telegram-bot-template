namespace TelegramBot.Interfaces;

public interface ICommand
{
    string Trigger { get; }
    string Execute(string message);
}
