namespace Base.Infrastructure.Database.PostgreSQL.Configs;

public class PostgreSQLConfigurations
{
    public const string ConfigSectionName = "PostgreSQL";
    
    public string Host { get; set; }
    public int Port { get; set; }
    public string Name { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public bool IntegratedSecurity { get; set; }
    public int CommandTimeout { get; set; }
}