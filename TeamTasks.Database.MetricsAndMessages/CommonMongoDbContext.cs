using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TeamTasks.Application.Core.Settings;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Database.MetricsAndMessages;

/// <summary>
/// The metrics database context.
/// </summary>
public sealed class CommonMongoDbContext
{
    private readonly IMongoDatabase _database = null!;

    public CommonMongoDbContext(IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        
        if (client is not null)
            _database = client.GetDatabase(settings.Value.Database);
    }
    
    public IMongoCollection<MetricEntity> Metrics =>
        _database.GetCollection<MetricEntity>("Metrics");

    public IMongoCollection<RabbitMessage> RabbitMessages =>
        _database.GetCollection<RabbitMessage>("RabbitMessages");
}