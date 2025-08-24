using Mopere.Configurations.Secrets;

namespace Mopere.Configurations;

public static class AwsConfig
{
    public static void AddAmazonSecretsManager(this IConfigurationBuilder configurationBuilder,
        string region,
        string secretName
    )
    {
        var configurationSource = new AmazonSecretsManagerConfigurationSource(region, secretName);
        configurationBuilder.Add(configurationSource);
    }
}