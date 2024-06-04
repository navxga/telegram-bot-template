namespace BotTelegram.Interfaces
{
    public interface ICommand
    {
        string Execute(string message);
    }
}
