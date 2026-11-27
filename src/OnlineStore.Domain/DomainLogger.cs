
namespace OnlineStore.Domain;

public static class DomainLogger
{
    public static Action<string> Logger { get; set; } =
        message => Console.WriteLine(
            $"[{DateTime.Now:HH:mm:ss}] {message}");
}
