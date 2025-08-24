using System.Text.Json;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace Mopere.Configurations.Secrets;

public class AmazonSecretsManagerConfigurationProvider(
    string region, string secretName
) : ConfigurationProvider
{
    public override void Load()
    {
        var secret = GetSecret();

        if (secret != null)
        {
            Data = JsonSerializer.Deserialize<Dictionary<string, string>>(secret)!;
        }
    }

    private string? GetSecret()
    {
        var request = new GetSecretValueRequest
        {
            SecretId = secretName,
            VersionStage = "AWSCURRENT" // VersionStage defaults to AWSCURRENT if unspecified.
        };

        using var client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));
        
        var response = client.GetSecretValueAsync(request).Result;
        string secretString;
            
        if (response.SecretString == null)
        {
            var memoryStream = response.SecretBinary;
            var reader = new StreamReader(memoryStream);
            secretString = System.Text.Encoding.UTF8
                .GetString(Convert.FromBase64String(reader.ReadToEnd()));
        }
        else
        {
            secretString = response.SecretString;
        }

        return secretString;
    }
}