using System.Text.Json;

namespace OnlineStore.App;

public static class JsonReportStorage
{
    public static async Task SaveAsync(
        List<OrderReport> reports,
        string filePath)
    {
        ArgumentNullException.ThrowIfNull(reports);
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(
            reports, options);

        string fullPath = Path.GetFullPath(filePath);

        string directory = Path.GetDirectoryName(fullPath)!;

        Directory.CreateDirectory(directory);

        await File.WriteAllTextAsync(fullPath, json);
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