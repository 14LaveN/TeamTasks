namespace TeamTasks.Application.Core.Settings;

public sealed class MongoSettings
{
    public static string MongoSettingsKey = "MongoConnection";
    
    public string ConnectionString { get; set; } = null!;

    public string Database { get; set; } = null!;

    public string RabbitMessagesCollectionName { get; set; } = null!;

    public string MetricsCollectionName { get; set; } = null!;
}