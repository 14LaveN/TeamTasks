using System.Diagnostics;
using System.Globalization;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TeamTasks.Application.Core.Settings;
using TeamTasks.Domain.Entities;
using Prometheus;

namespace TeamTasks.Application.Core.Helpers.Metric;

/// <summary>
/// Represents the create metrics helper.
/// </summary>
public sealed class CreateMetricsHelper
{
    private static readonly Counter RequestCounter =
        Metrics.CreateCounter("TeamTasks_requests_total", "Total number of requests.");
    private readonly IMongoCollection<MetricEntity> _metricsCollection;

    /// <summary>
    /// Initialize a new instance of the <see cref="CreateMetricsHelper"/>
    /// </summary>
    /// <param name="dbSettings"></param>
    public CreateMetricsHelper(IOptions<MongoSettings> dbSettings)
    {
        var mongoClient = new MongoClient(
            dbSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            dbSettings.Value.Database);

        _metricsCollection = mongoDatabase.GetCollection<MetricEntity>(
            dbSettings.Value.MetricsCollectionName);
    }

    /// <summary>
    /// Create metrics method.
    /// </summary>
    /// <param name="stopwatch">The <see cref="Stopwatch"/> class.</param>
    public async Task CreateMetrics(Stopwatch stopwatch)
    {
        RequestCounter.Inc();

        Metrics.CreateHistogram("TeamTasks_request_duration_seconds", "Request duration in seconds.")
            .Observe(stopwatch.Elapsed.TotalMilliseconds);

        var metrics = new List<MetricEntity>()
        { 
            new("TeamTasks_request_duration_seconds",
                stopwatch.Elapsed.TotalMilliseconds.ToString(CultureInfo.CurrentCulture)),
            new(RequestCounter.Name,
                RequestCounter.Value.ToString(CultureInfo.CurrentCulture))
        };
            
        await _metricsCollection.InsertManyAsync(metrics);
    }
}