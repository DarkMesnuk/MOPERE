namespace Infrastructure.Emails.Configs;

public class EmailConfiguration
{
    public const string ConfigSectionName = "Email";
    
    public string Host { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public int Port { get; set; }
    public bool UseSsl { get; set; }
    public string SupportName { get; set; }
    public string SupportEmail { get; set; }
}