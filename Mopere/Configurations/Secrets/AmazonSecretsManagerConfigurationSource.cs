namespace Mopere.Configurations.Secrets;

public class AmazonSecretsManagerConfigurationSource(
    string region, string secretName
) : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new AmazonSecretsManagerConfigurationProvider(region, secretName);
    }
}