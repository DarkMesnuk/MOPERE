using System.Diagnostics;
using System.Text.RegularExpressions;
using Domain.Interfaces.Infrastructure;
using Infrastructure.Emails.Configs;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Emails;

public class EmailService(
    IOptions<EmailConfiguration> emailConfigurationOption
) : IEmailService
{
    private readonly EmailConfiguration _emailConfiguration = emailConfigurationOption.Value;

    public async Task<bool> SendAsync(string email, string toName, string subject, string text)
    {
        var message = new MimeMessage();

        var from = new MailboxAddress(_emailConfiguration.SupportName, _emailConfiguration.SupportEmail);
        message.From.Add(from);

        var to = new MailboxAddress(toName, email);
        message.To.Add(to);

        message.Subject = subject;

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = text;
        bodyBuilder.TextBody = Regex.Replace(text, "<.*?>", string.Empty);

        message.Body = bodyBuilder.ToMessageBody();
        
        using var client = new SmtpClient();

        try
        {

            if (!client.IsConnected)
            {
                await client.ConnectAsync(_emailConfiguration.Host, _emailConfiguration.Port, _emailConfiguration.UseSsl);
                await client.AuthenticateAsync(_emailConfiguration.UserName, _emailConfiguration.Password);
            }

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[EmailService]: send exception. Message: {ex.Message}, inner exception: {ex.InnerException}");
            return false;
        }

        return true;
    }
}