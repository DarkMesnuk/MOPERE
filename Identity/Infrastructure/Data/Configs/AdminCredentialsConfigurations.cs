namespace Infrastructure.Data.Configs;

public class AdminCredentialsConfigurations
{
    public const string ConfigSectionName = "AdminCredentials";

    public string Email { get; set; }
    public string Password { get; set; }
}