using Dapper;
using Microsoft.Data.SqlClient;
using TeamTasks.Database.Common;
using TeamTasks.Database.Company.Data.Interfaces;
using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Core.Primitives.Result;
using TeamTasks.Domain.DTO.Tasks;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Database.Company.Data.Repositories;

/// <summary>
/// Represents the tasks repository.
/// </summary>
internal sealed class CompanyRepository : GenericRepository<Domain.Entities.Company>, ICompanyRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CompanyRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public CompanyRepository(BaseDbContext<Domain.Entities.Company> dbContext)
        : base(dbContext) {}

    /// <inheritdoc />
    public async Task<Result<Domain.Entities.Company>> UpdateCompany(Domain.Entities.Company company)
    {
        const string sql = """
                           
                                           UPDATE companies
                                           SET ModifiedOnUtc= @ModifiedOnUtc,
                                               CompanyName = @CompanyName,
                                               Description = @Description
                                           WHERE Id = @Id AND Deleted = 0
                           """;
        
        SqlParameter[] parameters =
        {
            new("@ModifiedOnUtc", DateTime.UtcNow),
            new("@CompanyName", company.CompanyName.Value),
            new("@Id", company.Id),
            new("@Description", company.Description)
        };
        var result = await DbContext.ExecuteSqlAsync(sql, parameters);
        
        return result is not 0 ? company : throw new ArgumentException();
    }
}