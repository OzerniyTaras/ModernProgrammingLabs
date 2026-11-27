using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace OnlineStore.App;

public static class JsonReportStorage
{
    public static async Task SaveAsync(
        List<OrderReport> reports,
        string filePath)
    {
        ArgumentNullException.ThrowIfNull(reports);
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        string configFile = Path.Combine(
            AppContext.BaseDirectory, "appsettings.json");

        var config = new ConfigurationBuilder()
            .AddJsonFile(configFile, optional: false)
            .Build();

        bool writeIndented = bool.TryParse(
            config["Storage:WriteIndented"],
            out bool value) && value;

        var options = new JsonSerializerOptions
        {
            WriteIndented = writeIndented
        };

        string json = JsonSerializer.Serialize(
            reports, options);

        string fullPath = Path.GetFullPath(filePath);

        string directory = Path.GetDirectoryName(fullPath)!;

        Directory.CreateDirectory(directory);

        await File.WriteAllTextAsync(fullPath, json);

        Console.WriteLine(
            $"Storage:WriteIndented = {writeIndented}");
    }

    public static async Task<List<OrderReport>> LoadAsync(
        string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        string json = await File.ReadAllTextAsync(filePath);

        return JsonSerializer.Deserialize<List<OrderReport>>(json)
            ?? throw new JsonException("Report data is null.");
    }
}