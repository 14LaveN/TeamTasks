using System.Diagnostics;
using System.Globalization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TeamTasks.Application.Core.Settings;
using TeamTasks.Domain.Entities;
using Prometheus;
using TeamTasks.Cache.Service;

namespace TeamTasks.Application.Core.Helpers.Metric;

/// <summary>
/// Represents the create metrics helper.
/// </summary>
public sealed class CreateMetricsHelper
{
    private static readonly Counter RequestCounter =
        Metrics.CreateCounter("TeamTasks_requests_total", "Total number of requests.");
    private readonly IMongoCollection<MetricEntity> _metricsCollection;

    private readonly IDistributedCache _distributedCache;

    /// <summary>
    /// Initialize a new instance of the <see cref="CreateMetricsHelper"/>
    /// </summary>
    /// <param name="dbSettings">The mongo db settings.</param>
    /// <param name="distributedCache">The distributed cache.</param>
    public CreateMetricsHelper(IOptions<MongoSettings> dbSettings,
        IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
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

        var metrics = new List<MetricEntity>
        { 
            new("TeamTasks_request_duration_seconds",
                stopwatch.Elapsed.TotalMilliseconds.ToString(CultureInfo.CurrentCulture)),
            new(RequestCounter.Name,
                RequestCounter.Value.ToString(CultureInfo.CurrentCulture))
        };

        //TODO Create the bg task where save to MongoDb metrics every 5 minutes and get them from Redis.
        
        await _distributedCache.SetRecordAsync(
            "metrics_counter-key",
            RequestCounter,
            TimeSpan.FromMinutes(5),
            TimeSpan.FromMinutes(5));
        
        await _metricsCollection.InsertManyAsync(metrics);
    }
}