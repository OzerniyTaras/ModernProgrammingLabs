namespace OnlineStore.Domain;

public class Customer
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required Address Address { get; init; }
    public string? PhoneNumber { get; init; }
}
