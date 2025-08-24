namespace Domain.Interfaces.Infrastructure;

public interface IEmailService
{
    Task<bool> SendAsync(string email, string toName, string subject, string text);
}