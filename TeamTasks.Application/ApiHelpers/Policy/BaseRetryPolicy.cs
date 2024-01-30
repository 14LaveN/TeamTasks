using Polly;
using Polly.Retry;
using Polly.Timeout;

namespace TeamTasks.Application.ApiHelpers.Policy;

public static class BaseRetryPolicy
{
    public static readonly ResiliencePipeline Policy = new ResiliencePipelineBuilder()
        .AddRetry(new RetryStrategyOptions
        {
            ShouldHandle =
                new PredicateBuilder()
                    .Handle<Exception>(),
            MaxRetryAttempts = 3,
            Delay = TimeSpan.FromSeconds(1),
            BackoffType =
                DelayBackoffType.Constant
        })
        .AddTimeout(new TimeoutStrategyOptions
        {
            Timeout = TimeSpan.FromSeconds(10)
        })
        .Build();
}