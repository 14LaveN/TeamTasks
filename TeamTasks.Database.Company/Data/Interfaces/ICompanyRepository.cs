using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Core.Primitives.Result;
using TeamTasks.Domain.DTO.Tasks;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Database.Company.Data.Interfaces;

/// <summary>
/// Represents the company repository interface.
/// </summary>
public interface ICompanyRepository
{
    /// <summary>
    /// Gets the company with the specified identifier.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <returns>The maybe instance that may contain the company entity with the specified identifier.</returns>
    Task<Maybe<Domain.Entities.Company>> GetByIdAsync(Guid companyId);

    /// <summary>
    /// Inserts the specified company entity to the database.
    /// </summary>
    /// <param name="company">The company to be inserted to the database.</param>
    Task Insert(Domain.Entities.Company company);

    /// <summary>
    /// Remove the specified company entity to the database.
    /// </summary>
    /// <param name="company">The company to be removed from the database.</param>
    void Remove(Domain.Entities.Company company);

    /// <summary>
    /// Update the specified company entity to the database.
    /// </summary>
    /// <param name="company">The company to be inserted to the database.</param>
    /// <returns>The result instance that may contain the company entity with the specified company class.</returns>
    Task<Result<Domain.Entities.Company>> UpdateCompany(Domain.Entities.Company company);
}