namespace TeamTasks.Application.Core.Settings;

public class MongoSettings
{
    public static string MongoSettingsKey = "MongoSettings";
    
    public string ConnectionString { get; set; } = null!;

    public string Database { get; set; } = null!;

    public string RabbitMessagesCollectionName { get; set; } = null!;

    public string MetricsCollectionName { get; set; } = null!;
}