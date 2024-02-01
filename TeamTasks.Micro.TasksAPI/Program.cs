using TeamTasks.Micro.TasksAPI.Common.DependencyInjection;
using TeamTasks.Micro.TasksAPI.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwachbackleService("Tasks API");

builder.Services.AddLogs(builder.Configuration);

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddCaching();

builder.Services.AddAuthenticationService();

builder.Services.AddMediatrExtension();

builder.Services.AddValidators();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();