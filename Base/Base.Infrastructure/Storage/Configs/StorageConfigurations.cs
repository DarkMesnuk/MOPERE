namespace Base.Infrastructure.Storage.Configs;

public class StorageConfigurations
{
	public const string ConfigSectionName = "Storage";
	public string AWSAccessKey { get; set; }
	public string AWSSecretKey { get; set; }
	public string S3PathTemplate { get; set; }
}