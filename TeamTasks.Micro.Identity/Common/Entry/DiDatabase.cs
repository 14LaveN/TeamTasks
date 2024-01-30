using TeamTasks.Application.Core.Settings;
using TeamTasks.Database.Attendee;
using TeamTasks.Database.GroupEvent;
using TeamTasks.Database.Identity;
using TeamTasks.Database.Invitation;
using TeamTasks.Database.Notification;
using TeamTasks.Database.PersonalEvent;

namespace TeamTasks.Micro.Identity.Common.Entry;

public static class DiDatabase
{
    public static IServiceCollection AddDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddUserDatabase(configuration);
        services.AddAttendeesDatabase(configuration);
        services.AddPersonalEventDatabase(configuration);
        services.AddGroupEventDatabase(configuration);
        services.AddInvitationsDatabase(configuration);
        services.AddNotificationsDatabase(configuration);
        
        services.Configure<MongoSettings>(
            configuration.GetSection(MongoSettings.MongoSettingsKey));
        
        return services;
    }
}