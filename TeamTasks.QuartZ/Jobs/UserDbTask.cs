using TeamTasks.Database.Identity;
using Quartz;
using static System.Console;

namespace TeamTasks.QuartZ.Jobs;

public sealed class UserDbTask : IJob
{
    private readonly UserDbContext _appDbContext = new();

    public async Task Execute(IJobExecutionContext context)
    {
        await _appDbContext.SaveChangesAsync();
        WriteLine("SaveChanges");
    }
}