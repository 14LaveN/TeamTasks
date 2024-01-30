using TeamTasks.Micro.Identity.Commands.Login;
using TeamTasks.Micro.Identity.Commands.Register;
using FluentValidation;
using TeamTasks.Micro.Identity.Commands.Login;

namespace TeamTasks.Micro.Identity.Common.Entry;

public static class DiValidator
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.AddScoped<IValidator<LoginCommand>, LoginCommandValidator>();
        services.AddScoped<IValidator<RegisterCommand>, RegisterCommandValidator>();
        
        return services;
    }
}