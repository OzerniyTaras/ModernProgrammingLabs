namespace OnlineStore.Domain;

public class DomainRuleViolationException : InvalidOperationException
{
    public DomainRuleViolationException(string message)
        : base(message)
    {
    }
}