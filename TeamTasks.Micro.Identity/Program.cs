using HealthChecks.UI.Client;
using MediatR;
using TeamTasks.Micro.Identity.Common.Entry;
using TeamTasks.Micro.Identity.Configurations;
using TeamTasks.Micro.Identity.Middlewares;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using TeamTasks.Application;
using TeamTasks.Application.Core.Behaviours;
using TeamTasks.Email;
using TeamTasks.QuartZ;
using TeamTasks.QuartZ.Jobs;
using TeamTasks.RabbitMq;
using Prometheus;
using Prometheus.Client.AspNetCore;
using Prometheus.Client.HttpRequestDurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddRabbitMq(builder.Configuration);

builder.Configuration
    .AddJsonFile("appsetting.json")
    .Build();

builder.Services.AddEmailService(builder.Configuration);

builder.Services.AddQuartzExtensions();

builder.Services.AddSwachbackleService()
    .AddValidators();

builder.Services.AddMediatR(x =>
{
    x.RegisterServicesFromAssemblyContaining<Program>();

    x.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    x.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UserTransactionBehaviour<,>));
});

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddHelpers();

//TODO: builder.Services.AddBackgroundTasks(builder.Configuration); - Add Background Tasks

builder.Services.AddApplication();

builder.Services.AddAuthorizationExtension(builder.Configuration);

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => options.AddDefaultPolicy(corsPolicyBuilder =>
    corsPolicyBuilder.WithOrigins("https://localhost:44460", "http://localhost:44460", "http://localhost:44460/")
        .AllowAnyHeader()
        .AllowAnyMethod()));

builder.Services.AddLoggingExtension(builder.Host);

AbstractScheduler<UserDbTask>.Start(builder.Services);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerApp();
}

app.UseCors();

app.UseMetricServer();

app.UseHttpMetrics();

app.UsePrometheusServer();

app.UsePrometheusRequestDurations();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseIdentityServer();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
});

app.MapControllers();

app.UseCustomMiddlewares();

app.Run();