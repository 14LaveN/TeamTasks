using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TeamTasks.Email.Emails;
using TeamTasks.Email.Emails.Settings;

namespace TeamTasks.Email;

public static class DependencyInjection
{
    public static IServiceCollection AddEmailService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
            throw new ArgumentException();
    
        // services.ConfigureOptions<MailSettings>(configuration.GetSection(MailSettings.SettingsKey));
        
        services.AddScoped<IEmailService, EmailService>();
        
        return services;
    }
}