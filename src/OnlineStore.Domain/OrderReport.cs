namespace OnlineStore.App;

public sealed record OrderReport(
    int OrderId,
    string CustomerName,
    decimal Total,
    string Status,
    int ItemsCount);